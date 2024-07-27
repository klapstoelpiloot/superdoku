using Superdoku.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PointI = System.Drawing.Point;

namespace Superdoku.Windows
{
	public partial class PuzzlePanel : UserControl
	{
		private static readonly Pen CellLine = new Pen(Brushes.Gray, 1);
		private static readonly Pen RegionLine = new Pen(Brushes.DimGray, 4);
		private static readonly Pen BorderLine = new Pen(Brushes.DimGray, 4);
		private static readonly Pen CellHighlightLine = new Pen(Brushes.CornflowerBlue, 4);
		private static readonly Typeface ValueFont = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
		private static readonly Typeface OptionsFont = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
		private static readonly Brush ValueFontBrush = Brushes.Black;
		private static readonly Brush OptionsFontBrush = Brushes.DimGray;
		private static readonly Brush FixedCellBrush = new SolidColorBrush(Color.FromRgb(240, 240, 240));
		private static readonly Brush BackgroundBrush = Brushes.White;
		private const double ValueFontSizeFactor = 0.7;
		private const double OptionsFontSizeFactor = 0.25;
		private const double CellHighlightInset = 2.0;

		private Puzzle? puzzle;
		private double psize;               // Puzzle size in DIPs
		private Point plefttop;             // Puzzle left-top position within the panel
		private double pcellsize;           // Size of a single cell in DIPs
		private double optionscellsize;     // Size of options within a single cell in DIPs;

		/// <summary>
		/// Cell in which the mouse is located or null when mouse is not inside the puzzle area.
		/// </summary>
		public PointI? MouseCell { get; private set; }

		// Constructor
		public PuzzlePanel()
		{
			InitializeComponent();
		}

		public void SetPuzzle(Puzzle? puzzle)
		{
			this.puzzle = puzzle;
			if(puzzle == null)
			{
				MouseCell = null;
			}
			DeterminePuzzleDimensions();
			InvalidateVisual();
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if(DetermineMouseCellPosition(e.GetPosition(this)))
				InvalidateVisual();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			if(DetermineMouseCellPosition(e.GetPosition(this)))
				InvalidateVisual();
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(DetermineMouseCellPosition(e.GetPosition(this)))
				InvalidateVisual();
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if(DetermineMouseCellPosition(null))
				InvalidateVisual();
			base.OnMouseLeave(e);
		}

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			dc.DrawRectangle(BackgroundBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));

			if(puzzle == null)
				return;

			DeterminePuzzleDimensions();
			double pixelsperdip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

			// Draw a darker background for fixed cells
			for(int x = 0; x < puzzle.Range; x++)
			{
				for(int y = 0; y < puzzle.Range; y++)
				{
					if(puzzle.Cells[x, y].Fixed)
					{
						Rect r = new Rect(plefttop.X + x * pcellsize, plefttop.Y + y * pcellsize, pcellsize, pcellsize);
						dc.DrawRectangle(FixedCellBrush, null, r);
					}
				}
			}

			// Draw the values and options
			double valuesize = pcellsize * ValueFontSizeFactor;
			double optionssize = pcellsize * OptionsFontSizeFactor;
			for(int x = 0; x < puzzle.Range; x++)
			{
				for(int y = 0; y < puzzle.Range; y++)
				{
					Cell c = puzzle.Cells[x, y];
					if(c.Value > 0)
					{
						// Draw a definite value
						string element = Cell.ELEMENTS[c.Value].ToString();
						FormattedText valueft = new FormattedText(element, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
							ValueFont, valuesize, ValueFontBrush, pixelsperdip);
						valueft.TextAlignment = TextAlignment.Center;
						valueft.Trimming = TextTrimming.None;
						valueft.MaxTextWidth = pcellsize;
						dc.DrawText(valueft, new Point(plefttop.X + x * pcellsize, plefttop.Y + y * pcellsize));
					}
					else
					{
						int index = 0;
						for(int y1 = 0; y1 < 3; y1++)
						{
							if(index >= c.Options.Count)
								break;

							for(int x1 = 0; x1 < 3; x1++)
							{
								if(index >= c.Options.Count)
									break;

								// If there are more options than we can fit, we draw dots in the last place
								string element = Cell.ELEMENTS[c.Options[index]].ToString();
								if((y1 == 2) && (x1 == 2) && (c.Options.Count > (index + 1)))
								{
									element = "...";
								}

								// Draw an option
								FormattedText optionft = new FormattedText(element, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
									OptionsFont, optionssize, OptionsFontBrush, pixelsperdip);
								optionft.TextAlignment = TextAlignment.Center;
								optionft.Trimming = TextTrimming.None;
								optionft.MaxTextWidth = optionscellsize;
								dc.DrawText(optionft, new Point(plefttop.X + x * pcellsize + x1 * optionscellsize, plefttop.Y + y * pcellsize + y1 * optionscellsize));

								index++;
							}
						}
					}
				}
			}

			// Draw the cell lines
			DrawSquareGrid(dc, puzzle.Range, plefttop.X, plefttop.Y, pcellsize, CellLine);

			// Draw the region lines
			DrawSquareGrid(dc, puzzle.RegionRange, plefttop.X, plefttop.Y, Math.Round(psize / puzzle.RegionRange), RegionLine);

			// Outer border rectangle
			dc.DrawRectangle(null, BorderLine, new Rect(plefttop.X, plefttop.Y, psize, psize));

			// If the mouse is in a non-fixed cell, draw a highlight
			if(MouseCell.HasValue && !puzzle.Cells[MouseCell.Value.X, MouseCell.Value.Y].Fixed)
			{
				Rect hr = new Rect(plefttop.X + MouseCell.Value.X * pcellsize + CellHighlightInset, plefttop.Y + MouseCell.Value.Y * pcellsize + CellHighlightInset,
					pcellsize - CellHighlightInset * 2.0, pcellsize - CellHighlightInset * 2.0);
				dc.DrawRectangle(null, CellHighlightLine, hr);
			}
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

		private void DeterminePuzzleDimensions()
		{
			if(puzzle == null)
				return;

			double paddedwidth = ActualWidth - Padding.Left - Padding.Right;
			double paddedheight = ActualHeight - Padding.Top - Padding.Bottom;
			psize = (paddedwidth > paddedheight) ? paddedheight : paddedwidth;
			double pleft = Math.Round((paddedwidth - psize) / 2 + Padding.Left);
			double ptop = Math.Round((paddedheight - psize) / 2 + Padding.Top);
			plefttop = new Point(pleft, ptop);
			pcellsize = Math.Round(psize / puzzle.Range);
			// Calculate this again, because we want to align this with screen pixels
			// (otherwise it gets blurry from anti-aliasing)
			psize = pcellsize * puzzle.Range;
			optionscellsize = pcellsize / 3;
		}

		private bool DetermineMouseCellPosition(Point? canvaspos)
		{
			PointI? oldcell = MouseCell;
			MouseCell = null;
			if((puzzle != null) && canvaspos.HasValue)
			{
				Rect prect = new Rect(plefttop.X, plefttop.Y, psize, psize);
				if(prect.Contains(canvaspos.Value))
				{
					int cx = (int)Math.Floor((canvaspos.Value.X - plefttop.X) / pcellsize);
					int cy = (int)Math.Floor((canvaspos.Value.Y - plefttop.Y) / pcellsize);
					cx = Math.Clamp(cx, 0, puzzle.Range - 1);
					cy = Math.Clamp(cy, 0, puzzle.Range - 1);
					MouseCell = new PointI(cx, cy);
				}
			}
			return MouseCell != oldcell;
		}
	}
}
