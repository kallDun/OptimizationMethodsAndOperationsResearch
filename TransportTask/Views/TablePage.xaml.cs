using System.Windows.Controls;
using TransportTask.Logic.Models;

namespace TransportTask.Views
{
    public partial class TablePage : Page
    {
        public TablePage(Table table)
        {
            InitializeComponent();
            var grid = TablesGenerator.InitTable(table);
            ViewGrid.Children.Add(grid);
        }
    }
}