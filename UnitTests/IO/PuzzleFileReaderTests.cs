using Superdoku.IO;
using Superdoku.Data;

namespace UnitTests.IO
{
    [TestFixture]
    public class PuzzleFileReaderTests : TestBase
    {
        [TestCase("puzzle1.txt")]
        [TestCase("puzzle2.txt")]
        [TestCase("puzzle3.txt")]
        public void TestBasicPuzzles(string filename)
        {
            FileStream fs = StreamTestData(filename);
            Puzzle p = PuzzleFileReader.Read(fs);

            Assert.Pass();
        }

        [TestCase("invalidelement.txt")]
        [TestCase("charmissing.txt")]
        public void TestInvalidPuzzles(string filename)
        {
            FileStream fs = StreamTestData(filename);

            try
            {
                Puzzle p = PuzzleFileReader.Read(fs);
            }
            catch(FileFormatException ex)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}