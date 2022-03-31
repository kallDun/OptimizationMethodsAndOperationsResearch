using System.Windows.Controls;

namespace OptimizationMethodsAndOperationsResearch.Views
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            /*InputFuncTextBox.Text = "f(x1,x2) = 5x1 - 2x2 -> min";
            InputMatrixTextBox.Text =
                "2x1 - x2 <= 6\n" +
                "-x1 + 3x2 <= 3\n" +
                "x1 + 2x2 <= 8\n" +
                "x1 >= 0, x2 >= 0";*/
            InputFuncTextBox.Text = "f(x1,x2) = 2x1 + 3x2 -> max";
            InputMatrixTextBox.Text =
                "x1 + 3x2 >= 10\n" +
                "-x1 + 5x2 <= 25\n" +
                "3x1 + 2x2 <= 18\n" +
                "x1 >= 0, x2 >= 0";
        }
        public (string, string) GetFunctionData => (InputFuncTextBox.Text, InputMatrixTextBox.Text);
    }
}