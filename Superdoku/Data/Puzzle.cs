namespace Superdoku.Data
{
    public class Puzzle
    {
        /// <summary>
        /// The size of this puzzle.
        /// </summary>
        public PuzzleSize Size { get; }

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

        public Puzzle(PuzzleSize size)
        {
            // Setup field
            Size = size;
            Range = (int)size;
            RegionRange = (int)Math.Sqrt(Range);
            Cells = new Cell[Range, Range];
            for (int x = 0; x < Range; x++)
            {
                for (int y = 0; y < Range; y++)
                    Cells[x, y] = new Cell(Range);
            }

            // Check our code compatability
            if(Cell.ELEMENTS.Length < Range)
                throw new NotSupportedException("Cell.ELEMENTS array does not support a puzzle of this size.");
        }
    }
}
