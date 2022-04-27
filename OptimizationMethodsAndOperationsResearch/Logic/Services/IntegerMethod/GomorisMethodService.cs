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
            throw new NotImplementedException();
            
            int notWholeResultElemIndex = GetFractionalResultElemIndex(table);

            if (AllResultElemIsInteger)
            {
                return null;  
            }

            Fraction[] fractions = GetFractionalPartsOfRowElem(notWholeResultElemIndex, table);

            table = ResizeTable(table)
        }

        private Table ResizeTable(Table table)
        {
            // scadcasdcasdcascasdcasdcasd
            var rowBasises = ResizeArray(table.RowBasises);
            var columnBasises = ResizeArray(table.ColumnBasises);
            var matrix = table.Matrix;
            var last_row = table.LastRow;
            
            Array.Resize(ref columnBasises, columnBasises.Length + 1);
            Array.Resize(ref matrix, matrix.Length + 1);
            for (int i = 0; i < matrix.Length; i++)
            {
                Array.Resize(ref matrix[i], matrix[i].Length + 1);
            }

            Array.Resize(ref Array[], size);

            return new Table(matrix, columnBasises, rowBasises, last_row, table.IsMin, false);
            // dcsdvasdfasdcasdcasdcadscasd
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
