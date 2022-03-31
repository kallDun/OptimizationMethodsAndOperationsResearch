using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    class SimplexMethodCalculator
    {
        public bool hasSolution { get; private set; } = true;

        public bool IsOptimizated(Table table)
        {
            if (HasSolutions(table))
            {
                return table.Matrix[table.Matrix.Length - 1].Skip(1).Where(x => x <= 0).Count() == 0;
            }
            else
            {
                hasSolution = false;
                return true;
            }
            throw new NotImplementedException();
        }

        private bool HasSolutions(Table table)
        {
            for (int i = 0; i < table.Matrix[0].Length - 1; i++)
            {
                if (table.Matrix[table.Matrix.Length - 1].Skip(1).ToArray()[i] > 0 &&
                    table.Matrix
                    .Take(table.Matrix.Length - 1)
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
        
        public Table GetNextTable(Table table)
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
                    table.Matrix[i][indexOfInputingVector] = 0;
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

            return newTable;
        }

        public Dictionary<int, Fraction> GetResults(Table table)
        {
            Dictionary<int, Fraction> results = new Dictionary<int, Fraction>();
            for (int i = 0; i < table.RowBasises.Length; i++)
            {
                if (table.ColumnBasises.Select(elem => elem.Index).ToArray().Contains(i))
                {
                    results.Add(i, table.RowBasises[i].SumValue.ValueNumber);
                }
                else
                {
                    results.Add(i, 0);
                }
            }
            return results;
        }

        private int GetIndexOfInputingVector(Table table)
        { 
            return Array.IndexOf(
                table.Matrix[table.Matrix.Length - 1],
                table.Matrix[table.Matrix.Length - 1]
                    .Skip(1)
                    .Where(x => x > 0)
                    .Min());
        }

        private int GetIndexOfDeletingVector(Table table, int indexOfInputingVector)
        {
            int index = -1;
            Fraction min = Fraction.Zero;
            for (int i = 0; i < table.Matrix.Length - 1; i++)
            {
                Fraction fraction = table.Matrix[i][indexOfInputingVector];
                if (fraction > 0)
                {
                    index = i;
                    min = table.Matrix[i][0] / fraction;
                    break;
                }
            }

            for (int i = 0; i < table.Matrix.Length - 1; i++)
            {
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

        public static SumValue[] GenerateLastRow(Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises)
        {
            SumValue[] lastRow = new SumValue[matrix[0].Length];
            lastRow[0] = CalculateFunction(matrix, columnBasises.ToArray(), rowBasises);
            for (int i = 1; i < matrix[0].Length; i++)
            {
                lastRow[i] = CalculateDelta(i, matrix, columnBasises, rowBasises);
            }
            return lastRow;
        }
        public static SumValue CalculateFunction(Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises)
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