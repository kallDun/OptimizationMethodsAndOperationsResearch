using System.Linq;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    class LeftUpperCornerMethod : IInitialRefPlanBuilder
    {
        public PrepTable Build(PrepTable table)
        {
            var cells = table.Cells;
            var needs = table.Need;
            var reserves = table.Reserves;

            int row = 0, col = 0;
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (!cells[i][j].IsChecked)
                    {
                        row = i;
                        col = j;
                        goto Break;
                    }
                }
            }
        Break:
            InitialRefPlanBuilderService.CalculateCell(ref cells, ref needs, ref reserves, row, col);
            return new PrepTable(cells, reserves, needs);
        }

        public bool IsBuilt(PrepTable table)
        {
            return table.Cells.Select(x => x.Where(s => !s.IsChecked).Count()).Sum() == 0;
        }
    }
}