using Microsoft.Win32;
using Superdoku.Data;
using Superdoku.IO;
using Superdoku.Solver;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void SolveStepButton_Click(object sender, RoutedEventArgs e)
        {
            if(puzzle == null)
                return;

            PuzzleSolver solver = new PuzzleSolver(puzzle);
            bool result = solver.SolveOneStep();
            canvas.InvalidateVisual();
            if(!result)
                SystemSounds.Beep.Play();
        }

        private void SolveCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(puzzle == null)
                return;

            PuzzleSolver solver = new PuzzleSolver(puzzle);
            bool result = solver.SolveComplete();
            canvas.InvalidateVisual();
            if(!result)
                SystemSounds.Beep.Play();
        }

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			puzzle = new Puzzle(PuzzleSize.Size9);
            canvas.SetPuzzle(puzzle);
        }

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if((puzzle != null) && canvas.MouseCell.HasValue)
			{
				Cell c = puzzle.Cells[canvas.MouseCell.Value.X, canvas.MouseCell.Value.Y];
				if(!c.Fixed)
				{
					switch(e.Key)
					{
						case Key.D1: c.SetValue(1); break;
						case Key.D2: c.SetValue(2); break;
						case Key.D3: c.SetValue(3); break;
						case Key.D4: c.SetValue(4); break;
						case Key.D5: c.SetValue(5); break;
						case Key.D6: c.SetValue(6); break;
						case Key.D7: c.SetValue(7); break;
						case Key.D8: c.SetValue(8); break;
						case Key.D9: c.SetValue(9); break;
						case Key.Delete: c.SetValue(0); break;
						case Key.Back: c.SetValue(0); break;
						case Key.Space: c.SetValue(0); break;
					}
					canvas.InvalidateVisual();
				}
			}
        }

		private void CopyButton_Click(object sender, RoutedEventArgs e)
		{
            if(puzzle == null)
                return;

			string puzzlestr = "";
			foreach(int y in Enumerable.Range(0, puzzle.Range))
			{
				foreach(int x in Enumerable.Range(0, puzzle.Range))
				{
					Cell c = puzzle.Cells[x, y];
					if(c.HasValue)
						puzzlestr += c.Value.ToString();
					else
						puzzlestr += ".";
				}
			}

			Clipboard.SetText(puzzlestr);
        }
    }
}