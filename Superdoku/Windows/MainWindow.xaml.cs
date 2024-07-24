using Microsoft.Win32;
using Superdoku.Data;
using Superdoku.IO;
using System.Windows;
using System.Windows.Controls;

namespace Superdoku.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The puzzle we are playing, if any
        private Puzzle? puzzle;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text files with Sudoku puzzles (*.txt)|*.txt|Sudoku puzzle files (*.sdk)|*.sdk|All files|*.*";
            dlg.Title = "Open Sudoku puzzle";
            bool? result = dlg.ShowDialog(this);
            if (result == true)
            {
                puzzle = PuzzleFileReader.Read(dlg.FileName);
                canvas.SetPuzzle(puzzle);
            }
        }
    }
}