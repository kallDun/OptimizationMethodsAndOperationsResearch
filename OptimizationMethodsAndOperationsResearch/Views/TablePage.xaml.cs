using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OptimizationMethodsAndOperationsResearch.Views
{
    /// <summary>
    /// Interaction logic for TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        public TablePage(Table table)
        {
            InitializeComponent();
            InitializeTable(table);
        }
        private void InitializeTable(Table table)
        {

        }
    }
}
