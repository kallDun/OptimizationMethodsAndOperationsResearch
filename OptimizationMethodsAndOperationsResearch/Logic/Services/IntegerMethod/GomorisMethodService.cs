using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services.IntegerMethod
{
    class GomorisMethodService
    {
        private bool AllResultElemIsInteger = false;

        public Table GomorisMethod(Table table)
        {            
            int notWholeResultElemIndex = GetFractionalResultElemIndex(table);
            if (AllResultElemIsInteger)
            {
                return null;
            }
            Fraction[] fractions = GetFractionalPartsOfRowElem(notWholeResultElemIndex, table);
            table = ResizeTable(table);
            FillTableWithNewValues(table, fractions);
            return table;
        }

        private void FillTableWithNewValues(Table table, Fraction[] fractions)
        {
            table.Matrix[table.Matrix.Length - 1][table.Matrix[table.Matrix.Length - 1].Length - 1] = 1; // set right bottom element to zero

            Basis new_basis = new(table.RowBasises.Length, new SumValue(0, 0)); // create basis
            table.RowBasises[table.RowBasises.Length - 1] = new_basis;
            table.ColumnBasises[table.ColumnBasises.Length - 1] = new_basis;

            for (int i = 0; i < fractions.Length; i++)
            {
                table.Matrix[table.Matrix.Length - 1][i] = -1 * fractions[i];
            }
        }

        private Table ResizeTable(Table table)
        {
            var rowBasises = ResizeArray(table.RowBasises);
            var columnBasises = ResizeArray(table.ColumnBasises);
            var last_row = ResizeArray(table.LastRow);
            var matrix = table.Matrix;            
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = ResizeArray(matrix[i]);
            }
            matrix = ResizeArray(matrix);
            matrix[matrix.Length - 1] = new Fraction[matrix[0].Length];
            return new Table(matrix, columnBasises, rowBasises, last_row, table.IsMin, false);
        }
        private T[] ResizeArray<T>(T[] array)
        {
            Array.Resize(ref array, array.Length + 1);
            return array;
        }


        private Fraction[] GetFractionalPartsOfRowElem(int notWholeResultElemIndex, Table table)
        {
            Fraction[] fractions = new Fraction[table.Matrix[0].Length];
            return fractions.Select((x, index) => GetFractionalPart(table.Matrix[notWholeResultElemIndex][index])).ToArray();
        }
        private Fraction GetFractionalPart(Fraction fraction)
        {
            if (fraction == 0) return 0;
            return new Fraction(fraction.Numerator % fraction.Denominator, fraction.Denominator);
        }

        private int GetFractionalResultElemIndex(Table table)
        {
            for (int i = 0; i < table.Matrix.Length; i++)
            {
                Fraction fraction = GetFractionalPart(table.Matrix[i][0]);
                if (!fraction.IsZero)
                {
                    return i;
                }
            }
            AllResultElemIsInteger = true;
            return 0;
        }

    }
}