using FindFunctionExtreme.Logic;
using FindFunctionExtreme.Logic.ExtremumMethods;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FindFunctionExtreme
{
    public partial class MainWindow : Window
    {
        ExtremumResult calculation_result;

        public MainWindow()
        {
            InitializeComponent();
            SetMaterialDesignColor();
            InitializeMethodsComboBox();
        }

        private void InitializeMethodsComboBox()
        {
            MethodComboBox.ItemsSource = Enum.GetNames(typeof(ExtremumMethodTypes)).Select(x => new ComboBoxItem() { Content = x });
            MethodComboBox.SelectedIndex = 0;
        }

        private static void SetMaterialDesignColor()
        {
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

                ExtremumMethodTypes methodType = (ExtremumMethodTypes)Enum.Parse(typeof(ExtremumMethodTypes), (MethodComboBox.SelectedItem as ComboBoxItem).Content.ToString());

                CustomFunc func = new(function, variables);
                IExtremumCalculator extremumCalculator = methodType switch
                {
                    ExtremumMethodTypes.FastestDescent => new FastestDescentMethod(),
                    ExtremumMethodTypes.DivideStep => new DivideStepMethod(),
                    ExtremumMethodTypes.CoordinateDescent => new CoordinateDescentMethod(),
                    _ => throw new NotImplementedException(),
                };
                calculation_result = extremumCalculator.GetExtremum(func, zero_vector, epsilon);

                int signs_count = (int)Math.Round(Math.Log(1 / epsilon, 10));
                ResultTextBox.Text = string.Join("\n", calculation_result.MinX.Select(x => Math.Round(x, signs_count)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (calculation_result is null) return;
            Window view = new ExtremumResultView(calculation_result);
            view.Show();
        }
    }
}