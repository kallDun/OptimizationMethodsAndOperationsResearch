using FindFunctionExtreme.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FindFunctionExtreme
{
    public partial class ExtremumResultView : Window
    {
        public ExtremumResultView(ExtremumResult result)
        {
            InitializeComponent();
            MainDataGrid.ItemsSource
                = new List<ExtremumResult> { result }
                .Select(x => new { x.Itterations, x.FunctionCalls, X = string.Join(", ", x.MinX), x.Function, x.Time });
        }
    }
}