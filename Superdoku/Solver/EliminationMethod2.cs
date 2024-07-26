using Superdoku.Data;
using Superdoku.Tools;
using System.Drawing;

namespace Superdoku.Solver
{
    /// <summary>
    /// This checks each region if there is only a single location where a value can be within that region.
    /// </summary>
    public class EliminationMethod2 : ISolverMethod
    {
        /// <summary>
        /// Attempts to progress the puzzle one step further.
        /// Returns True when successful, returns False when this method failed.
        /// </summary>
        public bool SolveOneStep(Puzzle puzzle)
        {
            foreach(int rx in Enumerable.Range(0, puzzle.RegionRange).Shuffle())
            {
                foreach(int ry in Enumerable.Range(0, puzzle.RegionRange).Shuffle())
                {
                    if(DoRegion(puzzle, rx, ry))
                        return true;
                }
            }
            return false;
        }

        // Returns True when a definitive value has been found for the specified cell
        private bool DoRegion(Puzzle puzzle, int rx, int ry)
        {
            // Determine which values are not in this region yet
            List<int> missingvalues = Enumerable.Range(1, puzzle.Range).ToList();
            foreach(int x in Enumerable.Range(rx * puzzle.RegionRange, puzzle.RegionRange))
            {
                foreach(int y in Enumerable.Range(ry * puzzle.RegionRange, puzzle.RegionRange))
                {
                    Cell c = puzzle.Cells[x, y];
                    if(c.Value > 0)
                        missingvalues.Remove(c.Value);
                }
            }
            
            foreach(int v in missingvalues.Shuffle())
            {
                Point? p = null;
                bool failed = false;
                foreach(int x in Enumerable.Range(rx * puzzle.RegionRange, puzzle.RegionRange))
                {
                    foreach(int y in Enumerable.Range(ry * puzzle.RegionRange, puzzle.RegionRange))
                    {
                        if(CheckPosition(puzzle, v, x, y))
                        {
                            if(p.HasValue)
                            {
                                // This is the second position where this value could be,
                                // so forget about it...
                                failed = true;
                                break;
                            }
                            else
                            {
                                p = new Point(x, y);
                            }
                        }
                    }

                    if(failed)
                        break;
                }

                if(p.HasValue && !failed)
                {
                    // We have found a definitive value
                    Cell c = puzzle.Cells[p.Value.X, p.Value.Y];
                    c.Value = v;
                    c.ClearOptions();
                    return true;
                }
            }

            return false;
        }

        private bool CheckPosition(Puzzle puzzle, int value, int x, int y)
        {
            // Check the same row
            for(int x1 = 0; x1 < puzzle.Range; x1++)
            {
                int v = puzzle.Cells[x1, y].Value;
                if((x1 != x) && (v == value))
                    return false;
            }

            // Check the same column
            for(int y1 = 0; y1 < puzzle.Range; y1++)
            {
                int v = puzzle.Cells[x, y1].Value;
                if((y1 != y) && (v == value))
                    return false;
            }

            // Eliminate options from cells in the same region
            int rxs = (x / puzzle.RegionRange) * puzzle.RegionRange;
            int rys = (y / puzzle.RegionRange) * puzzle.RegionRange;
            for(int rx = rxs; rx < (rxs + puzzle.RegionRange); rx++)
            {
                for(int ry = rys; ry < (rys + puzzle.RegionRange); ry++)
                {
                    int v = puzzle.Cells[rx, ry].Value;
                    if((rx != x) && (ry != y) && (v == value))
                        return false;
                }
            }

            return true;
        }
    }
}
