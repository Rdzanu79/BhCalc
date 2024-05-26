using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RhCalcApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await Program.GenerateSolution();
        }

        private void TheoryButtonClick(object sender, RoutedEventArgs e)
        {
            string theoryPath = @"src\theory.txt";
            if (System.IO.File.Exists(theoryPath))
            {
                Process.Start(new ProcessStartInfo(theoryPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show($"The file '{theoryPath}' was not found.");
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {
            string helpPath = @"src\help.txt";
            if (System.IO.File.Exists(helpPath))
            {
                Process.Start(new ProcessStartInfo(helpPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show($"The file '{helpPath}' was not found.");
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}