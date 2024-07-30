using Superdoku.Data;
using Superdoku.IO;

namespace UnitTests.IO
{
    [TestFixture]
    public class SingleLineFormatterTests : TestBase
    {
        private const string COMMON_PUZZLE_STR = "....45.........1..3.......7...........2................8.......9..............3..";

        private Puzzle MakeCommonPuzzle()
        {
            Puzzle p = new Puzzle(PuzzleSize.Size9);
            p.Cells[0, 2].SetValue(3);
            p.Cells[4, 0].SetValue(4);
            p.Cells[5, 0].SetValue(5);
            p.Cells[6, 1].SetValue(1);
            p.Cells[8, 2].SetValue(7);
            p.Cells[2, 4].SetValue(2);
            p.Cells[0, 7].SetValue(9);
            p.Cells[1, 6].SetValue(8);
            p.Cells[6, 8].SetValue(3);
            return p;
        }

        private void ValidateCommonPuzzle(Puzzle p, bool fixedcells)
        {
            Assert.Multiple(() =>
            {
                Assert.That(p.Cells[0, 2].Value, Is.EqualTo(3));
                Assert.That(p.Cells[4, 0].Value, Is.EqualTo(4));
                Assert.That(p.Cells[5, 0].Value, Is.EqualTo(5));
                Assert.That(p.Cells[6, 1].Value, Is.EqualTo(1));
                Assert.That(p.Cells[8, 2].Value, Is.EqualTo(7));
                Assert.That(p.Cells[2, 4].Value, Is.EqualTo(2));
                Assert.That(p.Cells[0, 7].Value, Is.EqualTo(9));
                Assert.That(p.Cells[1, 6].Value, Is.EqualTo(8));
                Assert.That(p.Cells[6, 8].Value, Is.EqualTo(3));

                Assert.That(p.Cells[0, 0].Value, Is.EqualTo(0));
                Assert.That(p.Cells[1, 0].Value, Is.EqualTo(0));
                Assert.That(p.Cells[8, 8].Value, Is.EqualTo(0));

                Assert.That(p.Cells[0, 2].IsFixed, Is.EqualTo(fixedcells));
                Assert.That(p.Cells[4, 0].IsFixed, Is.EqualTo(fixedcells));
                Assert.That(p.Cells[5, 0].IsFixed, Is.EqualTo(fixedcells));
                Assert.That(p.Cells[0, 0].IsFixed, Is.EqualTo(false));
                Assert.That(p.Cells[1, 0].IsFixed, Is.EqualTo(false));
                Assert.That(p.Cells[8, 8].IsFixed, Is.EqualTo(false));
            });
        }

        [Test]
        public void SerializeWithDefaults()
        {
            Puzzle p = MakeCommonPuzzle();
            SingleLineFormatter serializer = new SingleLineFormatter();
            string str = serializer.Serialize(p);
            Assert.That(str, Is.EqualTo(COMMON_PUZZLE_STR));
        }

        [Test]
        public void SerializeWithWhitespaceX()
        {
            Puzzle p = MakeCommonPuzzle();
            SingleLineFormatter serializer = new SingleLineFormatter();
            serializer.Whitespace = SingleLineFormatter.WhitespaceChar.LargeX;
            string str = serializer.Serialize(p);
            Assert.That(str, Is.EqualTo(COMMON_PUZZLE_STR.Replace('.', 'X')));
        }

        [Test]
        public void SerializeWithWhitespaceZero()
        {
            Puzzle p = MakeCommonPuzzle();
            SingleLineFormatter serializer = new SingleLineFormatter();
            serializer.Whitespace = SingleLineFormatter.WhitespaceChar.Zero;
            string str = serializer.Serialize(p);
            Assert.That(str, Is.EqualTo(COMMON_PUZZLE_STR.Replace('.', '0')));
        }

        [Test]
        public void SerializeWithWhitespaceSpace()
        {
            Puzzle p = MakeCommonPuzzle();
            SingleLineFormatter serializer = new SingleLineFormatter();
            serializer.Whitespace = SingleLineFormatter.WhitespaceChar.Space;
            string str = serializer.Serialize(p);
            Assert.That(str, Is.EqualTo(COMMON_PUZZLE_STR.Replace('.', ' ')));
        }

        [Test]
        public void DeserializeWithDefaults()
        {
            string str = COMMON_PUZZLE_STR;
            SingleLineFormatter deserializer = new SingleLineFormatter();
            Puzzle p = deserializer.Deserialize(str);
            ValidateCommonPuzzle(p, true);
        }

        [Test]
        public void DeserializeWithWhitespaceX()
        {
            string str = COMMON_PUZZLE_STR.Replace('.', 'X');
            SingleLineFormatter deserializer = new SingleLineFormatter();
            Puzzle p = deserializer.Deserialize(str);
            ValidateCommonPuzzle(p, true);
        }

        [Test]
        public void DeserializeWithWhitespaceSpace()
        {
            string str = COMMON_PUZZLE_STR.Replace('.', ' ');
            SingleLineFormatter deserializer = new SingleLineFormatter();
            Puzzle p = deserializer.Deserialize(str);
            ValidateCommonPuzzle(p, true);
        }

        [Test]
        public void DeserializeWithoutFixedCells()
        {
            string str = COMMON_PUZZLE_STR;
            SingleLineFormatter deserializer = new SingleLineFormatter();
            deserializer.SetFixed = false;
            Puzzle p = deserializer.Deserialize(str);
            ValidateCommonPuzzle(p, false);
        }

        [Test]
        public void DeserializeInvalidLength()
        {
            string str = COMMON_PUZZLE_STR.Substring(0, COMMON_PUZZLE_STR.Length - 4);
            SingleLineFormatter deserializer = new SingleLineFormatter();
            deserializer.SetFixed = false;
            try
            {
                Puzzle p = deserializer.Deserialize(str);
            }
            catch(FileFormatException ex)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void RountripSize4()
        {
            Puzzle p = new Puzzle(PuzzleSize.Size4);
            p.Cells[0, 0].SetValue(1);
            p.Cells[1, 1].SetValue(2);
            p.Cells[2, 0].SetValue(3);
            p.Cells[3, 3].SetValue(4);
            RountripTest(p);

        }

        [Test]
        public void RountripSize9()
        {
            Puzzle p = new Puzzle(PuzzleSize.Size9);
            p.Cells[0, 0].SetValue(1);
            p.Cells[1, 1].SetValue(2);
            p.Cells[2, 0].SetValue(3);
            p.Cells[8, 8].SetValue(4);
            p.Cells[3, 7].SetValue(9);
            RountripTest(p);

        }

        [Test]
        public void RountripSize16()
        {
            Puzzle p = new Puzzle(PuzzleSize.Size16);
            p.Cells[0, 0].SetValue(1);
            p.Cells[1, 1].SetValue(2);
            p.Cells[2, 0].SetValue(3);
            p.Cells[15, 15].SetValue(16);
            p.Cells[12, 7].SetValue(9);
            RountripTest(p);

        }

        [Test]
        public void RountripSize25()
        {
            Puzzle p = new Puzzle(PuzzleSize.Size25);
            p.Cells[0, 0].SetValue(1);
            p.Cells[1, 1].SetValue(2);
            p.Cells[2, 0].SetValue(3);
            p.Cells[24, 24].SetValue(25);
            p.Cells[12, 7].SetValue(9);
            RountripTest(p);
        }

        private void RountripTest(Puzzle p)
        {
            SingleLineFormatter serializer = new SingleLineFormatter();
            string str1 = serializer.Serialize(p);

            SingleLineFormatter deserializer = new SingleLineFormatter();
            Puzzle r = deserializer.Deserialize(str1);

            Assert.Multiple(() =>
            {
                Assert.That(r.Size, Is.EqualTo(p.Size));
                Assert.That(r.Range, Is.EqualTo(p.Range));
                foreach (int x in Enumerable.Range(0, p.Range))
                {
                    foreach (int y in Enumerable.Range(0, p.Range))
                    {
                        Assert.That(r.Cells[x, y].Value, Is.EqualTo(p.Cells[x, y].Value));
                    }
                }
            });
        }
    }
}
