using Superdoku.Data;
using System.IO;

namespace Superdoku.IO
{
    public class PuzzleFileReader
    {
        private static SingleLinePuzzleFormatter singlelineformatter = new SingleLinePuzzleFormatter();

        public static Puzzle Read(string filename)
        {
            string str = File.ReadAllText(filename);
            return singlelineformatter.Deserialize(str);
        }

        public static Puzzle Read(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            string str = sr.ReadToEnd();
            return singlelineformatter.Deserialize(str);
        }
    }
}
