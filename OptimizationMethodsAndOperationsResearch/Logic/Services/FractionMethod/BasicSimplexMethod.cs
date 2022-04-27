using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System.Collections.Generic;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    class BasicSimplexMethod : AbstractSimplexMethod
    {
        public override bool IsOptimized(Table table)
        {
            if (HasSolutions(table))
            {
                return GetLastRowValues(table).Count() == 0;
            }
            else
            {
                HasSolution = false;
                return true;
            }
        }

        private bool HasSolutions(Table table)
        {
            for (int i = 0; i < table.Matrix[0].Length - 1; i++)
            {
                if (table.LastRow
                    .Skip(1)
                    .Select(x => table.HasBigNumbers ? x.ValueM : x.ValueNumber)
                    .ToArray()[i] > 0 
                    &&
                    table.Matrix
                    .Where(x => x[i] > 0)
                    .Count() == 0)
                {
                    return false;
                }
            }
            for (int i = 0; i < table.ColumnBasises.Length; i++)
            {
                if (table.ColumnBasises[i].SumValue.HasBigNum && table.Matrix[0][i].IsNegative)
                {
                    return false;
                }
            }
            return true;
        }
        
        public override (Table, VisualDataModel) GetNextTable(Table table)
        {
            int indexOfInputingVector = GetIndexOfInputingVector(table);
            int indexOfDeletingVector = GetIndexOfDeletingVector(table, indexOfInputingVector);
            Fraction keyElem = table.Matrix[indexOfDeletingVector][indexOfInputingVector];
            Table newTable = (Table)table.Clone();
            newTable.ColumnBasises[indexOfDeletingVector] = table.RowBasises[indexOfInputingVector - 1];
            
            table.Matrix[indexOfDeletingVector]
                .Select(x => x = x / keyElem)
                .ToArray()
                .CopyTo(newTable.Matrix[indexOfDeletingVector],0);

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

            var visualData = new VisualDataModel(new VisualSelectedColsRowsModel(indexOfDeletingVector, indexOfInputingVector));
            ChangeHasBigNumbers(newTable);
            return (newTable, visualData);
        }
        private int GetIndexOfInputingVector(Table table)
        {
            var founded = GetLastRowValues(table).Min();
            return table.LastRow
                .Select(x => table.HasBigNumbers ? x.ValueM : x.ValueNumber)
                .Select((x, i) => new { item = x, index = i })
                .First(x => x.item == founded && x.index != 0).index;
        }

        private static IEnumerable<Fraction> GetLastRowValues(Table table)
        {
            return table.LastRow
                .Skip(1)
                .Take(GetNotArtificialVariablesCount(table.RowBasises))
                .Select(x => table.HasBigNumbers ? x.ValueM : x.ValueNumber)
                .Where(x => table.IsMin ? x > 0 : x < 0);
        }

        private int GetIndexOfDeletingVector(Table table, int indexOfInputingVector)
        {
            int index = -1;
            Fraction min = int.MaxValue;

            for (int i = 0; i < table.Matrix.Length; i++)
            {
                if (table.ColumnBasises[i].SumValue.HasBigNum) return i; // added M priority
                Fraction fraction = table.Matrix[i][indexOfInputingVector];
                Fraction relation = table.Matrix[i][0] / fraction;
                if (fraction > 0 && relation < min)
                {
                    index = i;
                    min = relation;
                }
            }

            return index;
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
        private static int GetNotArtificialVariablesCount(Basis[] rowBasises)
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
        private static Fraction GetFractionSum(IEnumerable<Fraction> enumerable)
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