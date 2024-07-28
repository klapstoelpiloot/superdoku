using Superdoku.Data;
using Superdoku.Tools;

namespace Superdoku.Solver
{
    /// <summary>
    /// This checks the row, column and region of each cell to see if a definitive
    /// value can be chosen for that cell and otherwise lists the options.
    /// </summary>
    public class SingleRemainderMethod : ISolverMethod
    {
        /// <summary>
        /// Attempts to progress the puzzle one step further.
        /// Returns True when successful, returns False when this method failed.
        /// </summary>
        public bool SolveOneStep(Puzzle puzzle)
        {
            foreach(int x in Enumerable.Range(0, puzzle.Range).Shuffle())
            {
                foreach(int y in Enumerable.Range(0, puzzle.Range).Shuffle())
                {
                    if(!puzzle.Cells[x, y].HasValue)
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
            int possiblevalue = 0;
			foreach(int v in Enumerable.Range(1, puzzle.Range))
			{
				if(puzzle.CheckConstraints(v, x, y))
				{
					if(possiblevalue > 0)
					{
						// This is the second value possible,
						// so we don't have a definitive value.
						return false;
					}
					else
					{
						possiblevalue = v;
					}
				}
			}

            // Check if we have found a definitive value
            if(possiblevalue > 0)
            {
                puzzle.Cells[x, y].SetValue(possiblevalue);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
