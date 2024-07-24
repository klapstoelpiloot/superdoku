namespace Superdoku.Data
{
    public class Cell
    {
        public static readonly char[] ELEMENTS = ['.', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W'];

        /// <summary>
        /// Definite value for this cell.
        /// When this is 0, then no definite value has been assigned.
        /// </summary>
        public int Value { get; set; } = 0;

        /// <summary>
        /// Value options to consider for this cell.
        /// </summary>
        public IReadOnlyList<int> Options => options;
        private List<int> options = new List<int>();

        public Cell(int range)
        {
        }

        public void AddOption(int option)
        {
            if(!options.Contains(option))
            {
                options.Add(option);
                options.Sort();
            }
        }

        public void AddOptionsRange(IEnumerable<int> options)
        {
            foreach(int o in options)
            {
                if(!this.options.Contains(o))
                {
                    this.options.Add(o);
                }
            }
            this.options.Sort();
        }

        public void RemoveOption(int option)
        {
            options.Remove(option);
        }

        public void ClearOptions()
        {
            options.Clear();
        }

        public static int ValueOfElement(char e) { return Array.IndexOf(ELEMENTS, e); }
    }
}
