namespace Superdoku.Data
{
    internal class Puzzle
    {
        public static readonly int[] VALID_RANGES = [4, 9, 16, 25, 36];

        /// <summary>
        /// The length of an entire row or column,
        /// the number of cells in a region, or the number of regions.
        /// </summary>
        public int Range { get; }

        /// <summary>
        /// Square root of Range. This is the number of regions
        /// in one row or column or the number of cells within
        /// a region row or column.
        /// </summary>
        public int RegionRange { get; }

        /// <summary>
        /// All cells in the puzzle.
        /// </summary>
        public Cell[,] Cells { get; }

        public Puzzle(int range)
        {
            if (!VALID_RANGES.Contains(range))
                throw new ArgumentException($"Invalid puzzel range '{range}'");

            // Setup field
            Range = range;
            RegionRange = (int)Math.Sqrt(range);
            Cells = new Cell[range, range];
            for (int x = 0; x < range; x++)
            {
                for (int y = 0; y < range; y++)
                    Cells[x, y] = new Cell(range);
            }
        }
    }
}
