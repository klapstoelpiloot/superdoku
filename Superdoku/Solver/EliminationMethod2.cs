using Superdoku.Data;
using Superdoku.Tools;
using PointI = System.Drawing.Point;

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
                    if(c.HasValue)
                        missingvalues.Remove(c.Value);
                }
            }
            
            foreach(int v in missingvalues.Shuffle())
            {
                PointI? p = null;
                bool failed = false;
                foreach(int x in Enumerable.Range(rx * puzzle.RegionRange, puzzle.RegionRange))
                {
                    foreach(int y in Enumerable.Range(ry * puzzle.RegionRange, puzzle.RegionRange))
                    {
                        if(!puzzle.Cells[x, y].HasValue && puzzle.CheckConstraints(v, x, y))
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
                                p = new PointI(x, y);
                            }
                        }
                    }

                    if(failed)
                        break;
                }

                if(p.HasValue && !failed)
                {
                    // We have found a definitive value
                    puzzle.Cells[p.Value.X, p.Value.Y].SetValue(v);
                    return true;
                }
            }

            return false;
        }
    }
}
