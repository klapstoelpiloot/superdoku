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

        /// <summary>
        /// Checks if the specified value can be placed at the specified coordinates according to Sudoku constraints.
        /// Returns True when the value is valid at the specified coordinates or False when the value conflicts with other definitive values.
        /// </summary>
        public bool CheckConstraints(int value, int x, int y)
        {
            // Check the same row
            for(int x1 = 0; x1 < Range; x1++)
            {
                int v = Cells[x1, y].Value;
                if((x1 != x) && (v == value))
                    return false;
            }

            // Check the same column
            for(int y1 = 0; y1 < Range; y1++)
            {
                int v = Cells[x, y1].Value;
                if((y1 != y) && (v == value))
                    return false;
            }

            // Check the same region
            int rxs = (x / RegionRange) * RegionRange;
            int rys = (y / RegionRange) * RegionRange;
            for(int rx = rxs; rx < (rxs + RegionRange); rx++)
            {
                for(int ry = rys; ry < (rys + RegionRange); ry++)
                {
                    int v = Cells[rx, ry].Value;
                    if(((rx != x) || (ry != y)) && (v == value))
                        return false;
                }
            }

            return true;
        }
    }
}
