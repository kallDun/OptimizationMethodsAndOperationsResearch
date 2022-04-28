using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Utilities
{
    static class TableUtility
    {
        public static Table GetWithoutMajorColumn(Table table)
        {
            var cut_count = table.RowBasises.Count(x => x.SumValue.HasBigNum);
            var new_size = table.RowBasises.Length - cut_count;

            var rowBasises = table.RowBasises;
            var matrix = table.Matrix;
            var last_row = table.LastRow;
            Array.Resize(ref last_row, new_size + 1);
            Array.Resize(ref rowBasises, new_size);
            for (int i = 0; i < matrix.Length; i++)
            {
                Array.Resize(ref matrix[i], new_size + 1);
            }

            return new Table(matrix, table.ColumnBasises, rowBasises, last_row, table.IsMin, false);
        }
    }
}