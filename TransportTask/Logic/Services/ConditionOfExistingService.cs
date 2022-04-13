using System;
using System.Linq;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services
{
    class ConditionOfExistingService
    {
        public PrepTable Check(PrepTable table)
        {
            var reserves_sum = table.Reserves.Sum();
            var need_sum = table.Need.Sum();
            if (reserves_sum == need_sum) return new PrepTable(table.Cells, table.Reserves, table.Need);
            if (reserves_sum > need_sum)
            {
                var cells = table.Cells;
                for (int i = 0; i < cells.Length; i++)
                {
                    Array.Resize(ref cells[i], cells[i].Length + 1);
                    cells[i][cells[i].Length - 1] = 0;
                }
                var need = table.Need;
                Array.Resize(ref need, need.Length + 1);
                need[need.Length - 1] = reserves_sum - need_sum;
                return new PrepTable(cells, table.Reserves, need);
            }
            else
            {
                var cells = table.Cells;
                Array.Resize(ref cells, cells.Length + 1);
                cells[cells.Length - 1] = new PrepCell[cells[0].Length];
                for (int j = 0; j < cells[0].Length; j++)
                {
                    cells[cells.Length - 1][j] = 0;
                }
                var reserves = table.Reserves;
                Array.Resize(ref reserves, reserves.Length + 1);
                reserves[reserves.Length - 1] = need_sum - reserves_sum;
                return new PrepTable(cells, reserves, table.Need);
            }
        }
    }
}