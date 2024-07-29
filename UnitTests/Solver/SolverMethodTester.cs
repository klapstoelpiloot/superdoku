using Superdoku.Data;
using Superdoku.IO;
using Superdoku.Solver;
using System.Text;

namespace UnitTests.Solver
{
	public abstract class SolverMethodTester : TestBase
	{
		protected void TestMethod(ISolverMethod method, string puzzlestr, string? expected)
		{
			MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(puzzlestr));
			Puzzle p = PuzzleFileReader.Read(stream);
			bool result = method.SolveOneStep(p);


		}
	}
}
