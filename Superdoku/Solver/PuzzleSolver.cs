﻿using Superdoku.Data;
using System.Diagnostics;

namespace Superdoku.Solver
{
    public class PuzzleSolver
    {
        // These are the different methods in order in which we will try to use them
        private readonly ISolverMethod[] methods = [
                new EliminationMethod1(),
                new EliminationMethod2()
            ];

        private readonly Puzzle puzzle;

        // Constructor
        public PuzzleSolver(Puzzle p)
        {
            puzzle = p;
        }

        /// <summary>
        /// This progresses the puzzle one step further.
        /// Returns True when progress was made or False when no progress was made.
        /// </summary>
        public bool SolveOneStep()
        {
            foreach (ISolverMethod m in methods)
            {
                if (m.SolveOneStep(puzzle))
                {
                    Trace.WriteLine($"Used method {m.GetType().Name}");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This solves the entire puzzle.
        /// Returns True when the puzzle was solved or False when the puzzle could not be solved.
        /// </summary>
        public void SolveComplete()
        {
            // This needs work, but just nice to test this here
            while(SolveOneStep());
        }
    }
}
