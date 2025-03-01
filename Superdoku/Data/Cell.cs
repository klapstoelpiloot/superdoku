﻿namespace Superdoku.Data
{
    public class Cell
    {
        public static readonly char[] ELEMENTS = ['.', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W'];

        /// <summary>
        /// Definite value for this cell.
        /// When this is 0, then no definite value has been assigned.
        /// </summary>
        public int Value { get; private set; } = 0;

        /// <summary>
        /// Returns True when a definitive value has been set for this cell.
        /// </summary>
        public bool HasValue => Value > 0;

        /// <summary>
        /// True when this cell is fixed (given as part of the puzzle),
        /// False when this cell can be changed by the user.
        /// </summary>
        public bool IsFixed { get; set; } = false;

        /// <summary>
        /// Value options to consider for this cell.
        /// </summary>
        public IReadOnlyList<int> Options => options;
        private readonly List<int> options = new List<int>();

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

        public void SetValue(int value)
        {
            Value = value;
            ClearOptions();
        }

        public static int ValueOfElement(char e) { return Array.IndexOf(ELEMENTS, e); }
    }
}
