using Superdoku.Data;
using Superdoku.Tools;
using System.Drawing;

namespace Superdoku.Solver
{
    /// <summary>
    /// This checks each row if there is only a single location where a value can be within that row.
    /// </summary>
    public class EliminationMethod3 : ISolverMethod
    {
        /// <summary>
        /// Attempts to progress the puzzle one step further.
        /// Returns True when successful, returns False when this method failed.
        /// </summary>
        public bool SolveOneStep(Puzzle puzzle)
        {
            foreach(int ry in Enumerable.Range(0, puzzle.Range).Shuffle())
            {
                if(DoRow(puzzle, ry))
                    return true;
            }
            return false;
        }

        // Returns True when a definitive value has been found for the specified cell
        private bool DoRow(Puzzle puzzle, int y)
        {
            // Determine which values are not in this row yet
            List<int> missingvalues = Enumerable.Range(1, puzzle.Range).ToList();
            foreach(int x in Enumerable.Range(0, puzzle.RegionRange))
            {
                Cell c = puzzle.Cells[x, y];
                if(c.HasValue)
                    missingvalues.Remove(c.Value);
            }
            
            foreach(int v in missingvalues.Shuffle())
            {
                Point? p = null;
                bool failed = false;
                foreach(int x in Enumerable.Range(0, puzzle.RegionRange))
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
                            p = new Point(x, y);
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
