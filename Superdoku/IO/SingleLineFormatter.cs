using Superdoku.Data;
using System.IO;

namespace Superdoku.IO
{
    /// <summary>
    /// Serializes a puzzle to single line of text. This does NOT include cell options.
    /// The length of the string indicates the size of the puzzle and each character
    /// is either an element (from Cell.ELEMENTS) or a 'whitespace' character.
    /// Example: ...........7.4.8........27..6..15.3....7.3.4.8....6..2...5...2...4..13...18.7.45.
    /// </summary>
	public class SingleLineFormatter
    {
        private static readonly char[] WHITESPACE_CHARS = ['.', ' ', 'X', 'x', '0'];

        public enum WhitespaceChar
        {
            Dot = 0,
            Space = 1,
            LargeX = 2,
            SmallX = 3,
            Zero = 4
        }

        /// <summary>
        /// Which character to use during serialization for a cell that has no definitive value set.
        /// </summary>
        public WhitespaceChar Whitespace { get; set; } = WhitespaceChar.Dot;

        /// <summary>
        /// Sets the IsFixed property on cells with definitive values on deserialization.
        /// </summary>
        public bool SetFixed { get; set; } = true;

        /// <summary>
        /// Deserializes a single line string into a puzzle.
        /// </summary>
		public Puzzle Deserialize(string str)
        {
            // Determine puzzle size
            str = str.Trim();
            double sized = Math.Sqrt(str.Length);
            int sizei = (int)sized;
            if (sized - sizei > 0.0001)
            {
                throw new FileFormatException("Unable to determine puzzle size from line length.");
            }

            Puzzle p = new Puzzle((PuzzleSize)sizei);

            // Parse each character
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                // This is an empty cell
                if (WHITESPACE_CHARS.Contains(c))
                    continue;

                int v = Cell.ValueOfElement(c);
                if ((v > -1) && (v <= p.Range))
                {
                    // Valid element
                    int x = i % p.Range;
                    int y = i / p.Range;
                    Cell cell = p.Cells[x, y];
                    cell.SetValue(Cell.ValueOfElement(c));
                    cell.IsFixed = SetFixed;
                }
                else
                {
                    throw new FileFormatException("Puzzle contains invalid elements.");
                }
            }

            return p;
        }

        /// <summary>
        /// Serializes a puzzle into a single text line.
        /// </summary>
        public string Serialize(Puzzle puzzle)
        {
            string str = string.Empty;
            foreach (int y in Enumerable.Range(0, puzzle.Range))
            {
                foreach (int x in Enumerable.Range(0, puzzle.Range))
                {
                    Cell c = puzzle.Cells[x, y];
                    if (c.HasValue)
                    {
                        str += Cell.ELEMENTS[c.Value];
                    }
                    else
                    {
                        str += WHITESPACE_CHARS[(int)Whitespace];
                    }
                }
            }
            return str;
        }
    }
}
