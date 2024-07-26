using Superdoku.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool result = false;
            for(int x = 0; x < puzzle.Range; x++)
            {
                for(int y = 0; y < puzzle.Range; y++)
                {
                    result |= TryCell(puzzle, x, y);
                }
            }
            return result;
        }

        // Returns True when progress has been made
        private bool TryCell(Puzzle puzzle, int x, int y)
        {
            Cell c = puzzle.Cells[x, y];

            // Begin with all options open
            c.ClearOptions();
            c.AddOptionsRange(Enumerable.Range(1, puzzle.Range));

            // Eliminate options from cells in the same row
            for(int x1 = 0; x1 < puzzle.Range; x1++)
            {
                int v = puzzle.Cells[x1, y].Value;
                if((x1 != x) && (v > 0))
                {
                    c.RemoveOption(v);
                }
            }

            // Eliminate options from cells in the same column
            for(int y1 = 0; y1 < puzzle.Range; y1++)
            {
                int v = puzzle.Cells[x, y1].Value;
                if((y1 != y) && (v > 0))
                {
                    c.RemoveOption(v);
                }
            }

            // Eliminate options from cells in the same region
            int rxs = x % puzzle.RegionRange;
            int rys = y % puzzle.RegionRange;
            for(int rx = rxs; rx < (rxs + puzzle.RegionRange); rx++)
            {
                for(int ry = rys; ry < (rys + puzzle.RegionRange); ry++)
                {
                    int v = puzzle.Cells[rx, ry].Value;
                    if((rx != x) && (ry != y) && (v > 0))
                    {
                        c.RemoveOption(v);
                    }
                }
            }

            // If there is only one option left, then that is the definitive value
            if(c.Options.Count == 1)
            {
                c.Value = c.Options[0];
                c.ClearOptions();
            }
        }
    }
}
