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

namespace BhCalcApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Generates MainWindow
        public MainWindow()
        {
            InitializeComponent();
        }
        //Asynchronous function to run program when the Start button is clicked
        private async void StartButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await Program.GenerateSolution();
        }
        //Function to open theory file when Theory button is clicked
        private void TheoryButtonClick(object sender, RoutedEventArgs e)
        {
            string theoryPath = @"src\theory.pdf";
            if (System.IO.File.Exists(theoryPath))
            {
                Process.Start(new ProcessStartInfo(theoryPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show($"The file '{theoryPath}' was not found.");
            }
        }
        //Function to open help file when Help button is clicked
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {
            string helpPath = @"src\help.pdf";
            if (System.IO.File.Exists(helpPath))
            {
                Process.Start(new ProcessStartInfo(helpPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show($"The file '{helpPath}' was not found.");
            }
        }
        //Function to close the program when Exit button is clicked
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}