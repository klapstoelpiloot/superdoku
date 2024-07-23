using Superdoku.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace Superdoku.Windows
{
    internal partial class PuzzlePanel : UserControl
    {
        private static readonly Pen CellLine = new Pen(Brushes.Gray, 1);
        private static readonly Pen RegionLine = new Pen(Brushes.DimGray, 3);
        private static readonly Pen BorderLine = new Pen(Brushes.DimGray, 3);
        private static readonly Typeface ValueFont = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
        private static readonly Typeface OptionsFont = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        private static readonly Brush ValueFontBrush = Brushes.Black;
        private static readonly Brush OptionsFontBrush = Brushes.DimGray;
        private const double ValueFontSizeFactor = 0.7;
        private const double OptionsFontSizeFactor = 0.2;

        private Puzzle? puzzle;

        // Constructor
        public PuzzlePanel()
        {
            InitializeComponent();

            // TEST: Remove this
            puzzle = new Puzzle(9);
            puzzle.Cells[0, 0].Value = 2;
            puzzle.Cells[2, 0].Value = 1;
            puzzle.Cells[7, 0].Value = 6;
            puzzle.Cells[1, 4].Value = 9;
            puzzle.Cells[0, 7].Value = 3;
            puzzle.Cells[4, 3].Value = 4;
            puzzle.Cells[5, 1].Value = 5;
            puzzle.Cells[6, 8].Value = 7;
            puzzle.Cells[8, 6].Value = 8;
        }

        public void SetPuzzle(Puzzle? puzzle)
        {
            this.puzzle = puzzle;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if(puzzle == null)
                return;

            // Determine where to draw the puzzle
            double paddedwidth = ActualWidth - Padding.Left - Padding.Right;
            double paddedheight = ActualHeight - Padding.Top - Padding.Bottom;
            double psize = (paddedwidth > paddedheight) ? paddedheight : paddedwidth;
            double px = Math.Round((paddedwidth - psize) / 2 + Padding.Left);
            double py = Math.Round((paddedheight - psize) / 2 + Padding.Top);
            double cellsize = psize / puzzle.Range;
            double pixelsperdip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            // Draw the values and options
            double valuesize = cellsize * ValueFontSizeFactor;
            for(int x = 0; x < puzzle.Range; x++)
            {
                for(int y = 0; y < puzzle.Range; y++)
                {
                    Cell c = puzzle.Cells[x, y];
                    if(c.Value > 0)
                    {
                        // Draw a definite value
                        FormattedText valueft = new FormattedText(c.Value.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            ValueFont, valuesize, ValueFontBrush, pixelsperdip);
                        valueft.TextAlignment = TextAlignment.Center;
                        valueft.Trimming = TextTrimming.None;
                        valueft.MaxTextWidth = cellsize * pixelsperdip;
                        dc.DrawText(valueft, new Point(px + x * cellsize, py + y * cellsize));
                    }
                    else
                    {
                        // Draw options

                    }
                }
            }

            // Draw the cell lines
            DrawSquareGrid(dc, puzzle.Range, px, py, cellsize, CellLine);

            // Draw the region lines
            DrawSquareGrid(dc, puzzle.RegionRange, px, py, psize / puzzle.RegionRange, RegionLine);

            // Outer border rectangle
            dc.DrawRectangle(null, BorderLine, new Rect(px, py, psize, psize));
        }

        private void DrawSquareGrid(DrawingContext dc, int cells, double offsetx, double offsety, double cellsize, Pen pen)
        {
            for(int x = 1; x < cells; x++)
            {
                double lx = offsetx + x * cellsize;
                dc.DrawLine(pen, new Point(lx, offsety), new Point(lx, offsety + (cellsize * cells)));
            }
            for(int y = 1; y < cells; y++)
            {
                double ly = offsety + y * cellsize;
                dc.DrawLine(pen, new Point(offsetx, ly), new Point(offsetx + (cellsize * cells), ly));
            }
        }
    }
}
