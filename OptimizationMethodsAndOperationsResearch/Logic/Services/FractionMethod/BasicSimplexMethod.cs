using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
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
            XMethod(table, indexOfInputingVector, indexOfDeletingVector, keyElem, out Table newTable, out VisualDataModel visualData);
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
    }
}