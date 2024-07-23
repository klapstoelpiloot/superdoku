namespace Superdoku.Data
{
    /// <summary>
    /// The various sizes of a sudoku puzzle we support
    /// </summary>
    internal enum PuzzleSize
    {
        // A 4x4 sudoku.
        Size4 = 4,

        // A 9x9 sudoku (most common).
        Size9 = 9,

        // A 16x16 sudoku.
        Size16 = 16,

        // A 25x25 sudoku. Good luck.
        Size25 = 25
    }
}
