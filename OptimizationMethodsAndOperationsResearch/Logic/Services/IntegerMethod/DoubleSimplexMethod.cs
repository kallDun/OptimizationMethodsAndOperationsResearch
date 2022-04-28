using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services.FractionMethod
{
    class DoubleSimplexMethod : AbstractSimplexMethod
    {
        public override bool IsOptimized(Table table)
        {
            if (HasSolutions(table))
            {
                return IsSolved(table);
            }
            else
            {
                HasSolution = false;
                return true;
            }
        }
        private bool HasSolutions(Table table)
        {
            return !table.Matrix
                .Select(P => P[0])
                .Select((x, i) => (item: x, index: i))
                .Any(x => x.item < 0 && !table.Matrix[x.index].Any(s => s < 0));
        }

        private bool IsSolved(Table table)
        {
            for (int i = 0; i < table.Matrix.Length; i++)
            {
                Fraction fraction = GetFractionalPart(table.Matrix[i][0]);
                if (!fraction.IsZero) return false;
            }
            return true;
        }
        private Fraction GetFractionalPart(Fraction fraction)
        {
            if (fraction == 0) return 0;
            return new Fraction(fraction.Numerator % fraction.Denominator, fraction.Denominator);
        }

        public override (Table, VisualDataModel) GetNextTable(Table table)
        {
            int indexOfRow = GetIndexOfInputingRow(table);
            int indexOfCol = GetIndexOfInputingCol(table, indexOfRow);
            Fraction keyElem = table.Matrix[indexOfRow][indexOfCol];
            XMethod(table, indexOfCol, indexOfRow, keyElem, out Table newTable, out VisualDataModel visualData);
            return (newTable, visualData);
        }

        private int GetIndexOfInputingRow(Table table)
        {
            return table.Matrix
                .Select(P => P[0])
                .Select((x, i) => (item: x, index: i))
                .OrderBy(x => x.item)
                .First()
                .index;
        }
        private int GetIndexOfInputingCol(Table table, int indexOfInputingRow)
        {
            var last_row = table.LastRow.Select(x => x.ValueNumber).ToList();
            return table.Matrix[indexOfInputingRow]                                         // get need row
                .Select((x, i) => (item: x, index: i))
                .Where(x => x.item < 0)                                                     // select items < 0
                .Select(x => (item: (last_row[x.index] / x.item).Abs(), index: x.index))    // divide delta on item and get abs
                .OrderBy(x => x.item)                                                       // get minimum
                .First(x => x.index is not 0)
                .index;
        }
    }
}