using Superdoku.Solver;

namespace UnitTests.Solver
{
	[TestFixture]
	public class SingleRemainderMethodTests : SolverMethodTester
	{
		[TestCase("....45.........1..3.......7...........2................8.......9..............3..", "....45.........1..3.......7...........2................8.......9..............3..")]
		public void TestSingleRemainderMethod(string puzzlestr, string? expected)
		{
			SingleRemainderMethod m = new SingleRemainderMethod();
			TestMethod(m, puzzlestr, expected);
		}
	}
}
