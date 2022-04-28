using OptimizationMethodsAndOperationsResearch.Logic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OptimizationMethodsAndOperationsResearch.Views
{
    public partial class StartPage : Page
    {
        public SimplexMethodType simplexMethodType { get; private set; } = SimplexMethodType.Fraction;
        public StartPage()
        {
            InitializeComponent();
            /*InputFuncTextBox.Text = "f(x1,x2) = 2x1 + 3x2 -> max";
            InputMatrixTextBox.Text =
                "x1 + 3x2 >= 10\n" +
                "-x1 + 5x2 <= 25\n" +
                "3x1 + 2x2 <= 18\n" +
                "x1 >= 0, x2 >= 0";*/
            InputFuncTextBox.Text = "f(x1,x2) = x1 + 4x2 -> max";
            InputMatrixTextBox.Text =
                "6x1 + 3x2 <= 19\n" +
                "x1 + 3x2 <= 4\n" +
                "x1 >= 0, x2 >= 0";
        }
        public (string, string) GetFunctionData => (InputFuncTextBox.Text, InputMatrixTextBox.Text);

        private void ChangeMethod_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            simplexMethodType = button.Tag switch
            {
                "Fraction" => SimplexMethodType.Fraction,
                "Integer" => SimplexMethodType.Integer,
                _ => SimplexMethodType.Fraction
            };
            List<Button> buttons = new()
            {
                FractionMethodButton,
                IntegerMethodButton
            };
            buttons.ForEach(x => x.Background = Brushes.Transparent);
            button.Background = Brushes.Black;
        }
    }
}