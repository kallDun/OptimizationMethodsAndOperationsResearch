using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            for (int i = 0; i < fractions.Length; i++)
            {
                table.Matrix[table.Matrix.Length - 1][i] = -1 * fractions[i];
            }

            table.Matrix[table.Matrix.Length - 1][table.Matrix[0].Length - 1] = 1;

            return table;
        }

        private Table ResizeTable(Table table)
        {
            Basis[] rowBasises = ResizeArray(table.RowBasises);
            Basis[] columnBasises = ResizeArray(table.ColumnBasises);
            SumValue[] last_row = ResizeArray(table.LastRow);
            Fraction[][] matrix = ResizeMatrix(table.Matrix);

            return new Table(matrix, columnBasises, rowBasises, last_row, table.IsMin, false);
        }

        private Fraction[][] ResizeMatrix(Fraction[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Array.Resize(ref matrix[i], matrix[i].Length + 1);
            }
            Array.Resize(ref matrix, matrix.Length + 1);

            matrix[matrix.Length - 1] = new Fraction[matrix[0].Length];

            return matrix;
        }

        private static T[] ResizeArray<T>(T[] array)
        {
            Array.Resize(ref array, array.Length + 1);
            return array;
        }

        private Fraction[] GetFractionalPartsOfRowElem(int notWholeResultElemIndex, Table table)
        {
            Fraction[] fractions = new Fraction[table.Matrix[0].Length];
            return fractions.Select((x, index) => GetFractionalPart(table.Matrix[notWholeResultElemIndex][index])).ToArray();
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

        private Fraction GetFractionalPart(Fraction fraction)
        {
            return new Fraction(fraction.Numerator % fraction.Denominator, fraction.Denominator);
        }
    }
}
