namespace Superdoku.Data
{
    internal class Cell
    {
        /// <summary>
        /// Definite value for this cell.
        /// When this is 0, then no definite value has been assigned.
        /// </summary>
        public int Value { get; set; } = 0;

        /// <summary>
        /// Value options to consider for this cell.
        /// The size of this array is range-1 and the first item (0) represents value 1.
        /// </summary>
        public bool[] Options { get; }

        public Cell(int range)
        {
            Options = new bool[range];
        }
    }
}
