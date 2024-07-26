using Superdoku.Data;

namespace Superdoku.Solver
{
    public interface ISolverMethod
    {
        /// <summary>
        /// Attempts to progress the puzzle one step further.
        /// Returns True when successful, returns False when this method failed.
        /// </summary>
        bool SolveOneStep(Puzzle puzzle);
    }
}
