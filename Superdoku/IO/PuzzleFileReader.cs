using Superdoku.Data;
using System.IO;

namespace Superdoku.IO
{
    public class PuzzleFileReader
    {
        public static Puzzle Read(string filename)
        {
            using FileStream fs = File.OpenRead(filename);
            return Read(fs);
        }

        public static Puzzle Read(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            string text = sr.ReadToEnd();

            // For now only single-line compact format is supported
            // Example: ...........7.4.8........27..6..15.3....7.3.4.8....6..2...5...2...4..13...18.7.45.

            // Determine puzzle size
            text = text.Trim();
            double sized = Math.Sqrt(text.Length);
            int sizei = (int)sized;
            if(sized - sizei > 0.0001)
            {
                throw new FileFormatException("Unable to determine puzzle size from line length.");
            }

            Puzzle p = new Puzzle((PuzzleSize)sizei);
            
            // Parse each character
            for(int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // This is an empty cell
                if((c == '0') || (c == '.') || (c == 'X') || (c == 'x'))
                    continue;

                int v = Cell.ValueOfElement(c);
                if((v > -1) && (v <= p.Range))
                {
                    // Valid element
                    int x = i % p.Range;
                    int y = i / p.Range;
                    Cell cell = p.Cells[x, y];
                    cell.Value = Cell.ValueOfElement(c);
                    cell.Fixed = true;
                }
                else
                {
                    throw new FileFormatException("Puzzle contains invalid elements.");
                }
            }

            return p;
        }
    }
}
