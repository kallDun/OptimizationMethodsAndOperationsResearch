using FindFunctionExtreme.Logic;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace FindFunctionExtreme
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PaletteHelper helper = new PaletteHelper();
            ITheme theme = helper.GetTheme();
            theme.SetPrimaryColor(Colors.MidnightBlue);
            theme.SetSecondaryColor(Colors.Firebrick);
            helper.SetTheme(theme);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                string[] variables = VariablesTextBox.Text
                    .Replace("(", "")
                    .Replace(")", "")
                    .Split(',');
                string func_right_part = FunctionTextBox.Text.Contains('=') ? FunctionTextBox.Text.Split('=')[1] : FunctionTextBox.Text;
                string function = $"f({string.Join(",", variables)}) = {func_right_part}";

                double[] zero_vector = ZeroVectorTextBox.Text
                    .Replace("(", "")
                    .Replace(")", "")
                    .Split(';')
                    .Select(double.Parse)
                    .ToArray();

                double epsilon = double.Parse(EpsilonTextBox.Text);

                if (variables.Length != zero_vector.Length)
                {
                    throw new Exception("Variables and zero vector must be one length!");
                }

                CustomFunc func = new CustomFunc(function, variables);
                IExtremumCalculator extremumCalculator = new FastestDescentMethod();
                double[] result = extremumCalculator.GetExtremum(func, zero_vector, epsilon);

                int signs_count = (int)Math.Round(Math.Log(1 / epsilon, 10));
                ResultTextBox.Text = string.Join("\n", result.Select(x => Math.Round(x, signs_count)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There is nothing here yet!");
        }
    }
}