using Superdoku.Data;

namespace Superdoku.Solver
{
    public class PuzzleSolver
    {
        // These are the different methods in order in which we will try to use them
        private readonly ISolverMethod[] methods = [
                new EliminationMethod()
            ];

        private readonly Puzzle puzzle;

        // Constructor
        public PuzzleSolver(Puzzle p)
        {
            puzzle = p;
        }

        /// <summary>
        /// This progresses the puzzle one step further.
        /// </summary>
        public void SolveOneStep()
        {
            foreach (ISolverMethod m in methods)
            {
                if (m.SolveOneStep(puzzle))
                    break;
            }
        }

        /// <summary>
        /// This solves the entire puzzle.
        /// </summary>
        public void SolveComplete()
        {
        }
    }
}
