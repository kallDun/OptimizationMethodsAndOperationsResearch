using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System.Collections.Generic;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    abstract class AbstractSimplexMethod
    {
        public bool HasSolution { get; protected set; } = true;
        public abstract bool IsOptimized(Table table);
        public abstract (Table, VisualDataModel) GetNextTable(Table table);
        public Dictionary<int, Fraction> GetResults(Table table)
        {
            Dictionary<int, Fraction> results = new();
            for (int i = 0; i < table.RowBasises.Length; i++)
            {
                results.Add(i + 1, 0);
            }
            for (int i = 0; i < table.RowBasises.Length; i++)
            {
                for (int j = 0; j < table.ColumnBasises.Length; j++)
                {
                    if (table.ColumnBasises[j].Index == i + 1)
                    {
                        results[i + 1] = table.Matrix[j][0];
                    }
                }
            }
            return results;
        }

        public static void ChangeHasBigNumbers(Table table)
        {
            for (int i = 1; i < table.LastRow.Length; i++)
            {
                if (table.LastRow[i].HasBigNum && !table.RowBasises[i - 1].SumValue.HasBigNum)
                {
                    table.HasBigNumbers = true;
                    return;
                }
            }
            table.HasBigNumbers = false;
        }
        protected static IEnumerable<Fraction> GetLastRowValues(Table table)
        {
            return table.LastRow
                .Skip(1)
                .Take(GetNotArtificialVariablesCount(table.RowBasises))
                .Select(x => table.HasBigNumbers ? x.ValueM : x.ValueNumber)
                .Where(x => table.IsMin ? x > 0 : x < 0);
        }
        protected static void XMethod(Table table, int indexOfInputingVector, int indexOfDeletingVector, Fraction keyElem, out Table newTable, out VisualDataModel visualData)
        {
            newTable = (Table)table.Clone();
            newTable.ColumnBasises[indexOfDeletingVector] = table.RowBasises[indexOfInputingVector - 1];

            table.Matrix[indexOfDeletingVector]
                .Select(x => x /= keyElem)
                .ToArray()
                .CopyTo(newTable.Matrix[indexOfDeletingVector], 0);

            for (int i = 0; i < table.Matrix.Length; i++)
            {
                if (i != indexOfDeletingVector)
                {
                    newTable.Matrix[i][indexOfInputingVector] = 0;
                }
            }

            for (int i = 0; i < table.Matrix.Length; i++)
            {
                if (i != indexOfDeletingVector)
                {
                    table.Matrix[i]
                        .Select((x, ind) => x = (x * keyElem - table.Matrix[indexOfDeletingVector][ind] * table.Matrix[i][indexOfInputingVector]) / keyElem)
                        .ToArray()
                        .CopyTo(newTable.Matrix[i], 0);
                }
            }

            var newLastRow = GenerateLastRow(newTable.Matrix, newTable.ColumnBasises, newTable.RowBasises);
            for (int i = 0; i < newTable.LastRow.Length; i++)
            {
                newTable.LastRow[i] = newLastRow[i];
            }

            visualData = new VisualDataModel(new VisualSelectedColsRowsModel(indexOfDeletingVector, indexOfInputingVector));
            ChangeHasBigNumbers(newTable);
        }
        protected static int GetNotArtificialVariablesCount(Basis[] rowBasises)
            => rowBasises.Where(x => !x.SumValue.HasBigNum).Count() - 1;
        public static SumValue[] GenerateLastRow(Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises)
        {
            SumValue[] lastRow = new SumValue[matrix[0].Length];
            lastRow[0] = CalculateFunction(matrix, columnBasises);
            for (int i = 1; i < matrix[0].Length; i++)
            {
                lastRow[i] = CalculateDelta(i, matrix, columnBasises, rowBasises);
            }
            return lastRow;
        }
        public static SumValue CalculateFunction(Fraction[][] matrix, Basis[] columnBasises)
        {
            var num = GetFractionSum(columnBasises.Select((x, i) => x.SumValue.ValueNumber * matrix[i][0]));
            var m_num = GetFractionSum(columnBasises.Select((x, i) => x.SumValue.ValueM * matrix[i][0]));
            return new SumValue(num, m_num);
        }
        public static SumValue CalculateDelta(int j, Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises)
        {
            var num = GetFractionSum(columnBasises.Select((x, i) => x.SumValue.ValueNumber * matrix[i][j])) - rowBasises[j - 1].SumValue.ValueNumber;
            var m_num = GetFractionSum(columnBasises.Select((x, i) => x.SumValue.ValueM * matrix[i][j])) - rowBasises[j - 1].SumValue.ValueM;
            return new SumValue(num, m_num);
        }
        protected static Fraction GetFractionSum(IEnumerable<Fraction> enumerable)
        {
            Fraction sum = 0;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            return sum;
        }
    }
}