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
        /// </summary>
        public List<int> Options { get; }

        public Cell(int range)
        {
            Options = new List<int>(range);
        }
    }
}
