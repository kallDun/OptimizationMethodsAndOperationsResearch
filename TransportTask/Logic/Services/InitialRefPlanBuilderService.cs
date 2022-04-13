using System;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services
{
    static class InitialRefPlanBuilderService
    {
        public static void CalculateCell(ref PrepCell[][] cells, ref int[] needs, ref int[] reserves, int i, int j)
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
                bool added_zero = false;
                for (int k = 0; k < cells[0].Length; k++)
                {
                    if (!cells[i][k].IsChecked)
                    {
                        if (k != j)
                        {
                            if (!added_zero)
                            {
                                cells[i][k].Products = 0;
                                added_zero = true;
                            }
                            else
                            {
                                cells[i][k].IsProductsNone = true;
                            }
                        }                            
                        cells[i][k].IsChecked = true;                        
                    }
                }
                for (int k = 0; k < cells.Length; k++)
                {
                    if (!cells[k][j].IsChecked)
                    {
                        if (k != i)
                        {
                            if (!added_zero)
                            {
                                cells[k][j].Products = 0;
                                added_zero = true;
                            }
                            else
                            {
                                cells[k][j].IsProductsNone = true;
                            }
                        }
                        cells[k][j].IsChecked = true;
                    }
                }
            }
        }
    }
}