using System.Windows.Controls;
using TransportTask.Logic.Models;

namespace TransportTask.Views
{
    /// <summary>
    /// Interaction logic for PrepTablePage.xaml
    /// </summary>
    public partial class PrepTablePage : Page
    {       

        public PrepTablePage(PrepTable table)
        {
            InitializeComponent();
            var grid = PrepTableGenerator.InitTable(table);
            ViewGrid.Children.Add(grid);
        }
    }
}