using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            InputFuncTextBox.Text = "f(x1,x2) = 5x1 - 2x2 -> min";
            InputMatrixTextBox.Text = 
                "2x1 - x2 <= 6\n" +
                "-x1 + 3x2 <= 3\n" +
                "x1 + 2x2 <= 8\n" +
                "x1 >= 0, x2 >= 0";
        }
        public (string, string) GetFunctionData => (InputFuncTextBox.Text, InputMatrixTextBox.Text);
    }
}
