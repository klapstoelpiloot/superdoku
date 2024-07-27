using Superdoku.Data;
using Superdoku.IO;
using System.Text;

namespace UnitTests.Data
{
    [TestFixture]
    public class PuzzleTests : TestBase
    {
        //[TestCase("...........7.4.8........27..6..15.3....7.3.4.8....6..2...5...2...4..13...18.7.45.", 4, 1, 2, true)]
        public void TestCheckConstraints(string puzzlestr, int v, int x, int y, bool expected)
        {
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(puzzlestr));
            Puzzle p = PuzzleFileReader.Read(stream);
            bool result = p.CheckConstraints(v, x, y);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
