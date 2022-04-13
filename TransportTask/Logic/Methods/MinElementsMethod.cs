using System;
using System.Linq;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    class MinElementsMethod : IInitialRefPlanBuilder
    {
        public PrepTable Build(PrepTable table)
        {
            var cells = table.Cells;
            var needs = table.Need;
            var reserves = table.Reserves;

            var coeffs = cells.SelectMany(x => x).Where(x => !x.IsChecked);
            var coeffs_without_zeros = coeffs.Where(x => x.Coefficient > 0);
            if (coeffs_without_zeros.Any()) coeffs = coeffs_without_zeros;
            var min = coeffs.Min(x => x.Coefficient);
            
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (!cells[i][j].IsChecked && cells[i][j].Coefficient == min)
                    {
                        var need = needs[j];
                        var reserve = reserves[i];
                        cells[i][j].Products = Math.Min(need, reserve);
                        if (need > reserve)
                        {
                            needs[j] -= reserve;
                            reserves[i] = 0;
                            for (int k = 0; k < cells[0].Length; k++)
                            {
                                if (!cells[i][k].IsChecked)
                                {
                                    cells[i][k].IsChecked = true;
                                    if (k != j) cells[i][k].IsProductsNone = true;
                                }
                            }
                        }
                        else if (reserve > need)
                        {
                            reserves[i] -= need;
                            needs[j] = 0;
                            for (int k = 0; k < cells.Length; k++)
                            {
                                if (!cells[k][j].IsChecked)
                                {
                                    cells[k][j].IsChecked = true;
                                    if (k != i) cells[k][j].IsProductsNone = true;
                                }
                            }
                        }
                        else // if equals
                        {
                            needs[j] = 0;
                            reserves[i] = 0;
                            for (int k = 0; k < cells[0].Length; k++)
                            {
                                if (!cells[i][k].IsChecked)
                                {
                                    cells[i][k].IsChecked = true;
                                    if (k != j) cells[i][k].IsProductsNone = true;
                                }
                            }
                            for (int k = 0; k < cells.Length; k++)
                            {
                                if (!cells[k][j].IsChecked)
                                {
                                    cells[k][j].IsChecked = true;
                                    if (k != i) cells[k][j].IsProductsNone = true;
                                }
                            }
                        }
                        break;
                    }
                }                
            }
            return new PrepTable(cells, reserves, needs);
        }

        public bool IsBuilt(PrepTable table)
        {
            return table.Cells.Select(x => x.Where(s => !s.IsChecked).Count()).Sum() == 0;
        }
    }
}