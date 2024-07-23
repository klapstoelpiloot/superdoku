using Superdoku.Data;
using System.Windows;
using System.Windows.Controls;

namespace Superdoku.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
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

        }
    }
}