using Superdoku.Data;

namespace Superdoku.Solver
{
    /// <summary>
    /// Elimination checks the row, column and region of each cell to see if a definitive
    /// value can be chosen for that cell and otherwise lists the options.
    /// </summary>
    public class EliminationMethod : ISolverMethod
    {
        /// <summary>
        /// Attempts to progress the puzzle one step further.
        /// Returns True when successful, returns False when this method failed.
        /// </summary>
        public bool SolveOneStep(Puzzle puzzle)
        {
            for(int x = 0; x < puzzle.Range; x++)
            {
                for(int y = 0; y < puzzle.Range; y++)
                {
                    if(puzzle.Cells[x, y].Value == 0)
                    {
                        if(TryCell(puzzle, x, y))
                            return true;
                    }
                }
            }
            return false;
        }

        // Returns True when a definitive value has been found for the specified cell
        private bool TryCell(Puzzle puzzle, int x, int y)
        {
            Cell c = puzzle.Cells[x, y];
            List<int> options = new List<int>(Enumerable.Range(1, puzzle.Range));

            // Eliminate options from cells in the same row
            for(int x1 = 0; x1 < puzzle.Range; x1++)
            {
                int v = puzzle.Cells[x1, y].Value;
                if((x1 != x) && (v > 0))
                {
                    options.Remove(v);
                }
            }

            // Eliminate options from cells in the same column
            for(int y1 = 0; y1 < puzzle.Range; y1++)
            {
                int v = puzzle.Cells[x, y1].Value;
                if((y1 != y) && (v > 0))
                {
                    options.Remove(v);
                }
            }

            // Eliminate options from cells in the same region
            int rxs = (x / puzzle.RegionRange) * puzzle.RegionRange;
            int rys = (y / puzzle.RegionRange) * puzzle.RegionRange;
            for(int rx = rxs; rx < (rxs + puzzle.RegionRange); rx++)
            {
                for(int ry = rys; ry < (rys + puzzle.RegionRange); ry++)
                {
                    int v = puzzle.Cells[rx, ry].Value;
                    if((rx != x) && (ry != y) && (v > 0))
                    {
                        options.Remove(v);
                    }
                }
            }

            // CHeck if we have found a definitive value
            if(options.Count == 1)
            {
                c.Value = options[0];
                c.ClearOptions();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
