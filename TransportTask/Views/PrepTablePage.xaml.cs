using System.Windows.Controls;
using TransportTask.Logic.Models;

namespace TransportTask.Views
{
    public partial class PrepTablePage : Page
    {       

        public PrepTablePage(PrepTable table)
        {
            InitializeComponent();
            var grid = TablesGenerator.InitPrepReadonlyTable(table);
            ViewGrid.Children.Add(grid);
        }
    }
}