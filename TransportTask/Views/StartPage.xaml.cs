using System.Windows.Controls;
using TransportTask.Logic.Models;
using static TransportTask.Views.PrepTableGenerator;

namespace TransportTask.Views
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public GetPrepTableDelegate GetTable { get; private set; }

        public StartPage()
        {
            InitializeComponent();
            var (grid, deleg) = InitReturnTable(GetPrepTablePreview());
            MainGrid.Children.Add(grid);
            Grid.SetRow(grid, 1);
            GetTable = deleg;
        }

        private PrepTable GetPrepTablePreview()
        {
            var cells = new PrepCell[][]
            {
                new PrepCell[] { 7, 6, 3 },
                new PrepCell[] { 2, 5, 4 },
                new PrepCell[] { 9, 10, 11 },
            };
            var reserves = new int[] { 175, 225, 230 };
            var need = new int[] { 205, 200, 250 };

            PrepTable table = new(cells, reserves, need);
            return table;
        }
    }
}