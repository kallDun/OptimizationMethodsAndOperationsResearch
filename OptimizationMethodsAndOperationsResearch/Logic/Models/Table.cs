using Fractions;
using System;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Table : ICloneable
    {
        public Table(int rows, int columns, Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises, Fraction[] bigNumRow)
        {
            Rows = rows;
            Columns = columns;
            Matrix = matrix;
            ColumnBasises = columnBasises;
            RowBasises = rowBasises;
            BigNumRow = bigNumRow;
        }
        public Fraction this[int i, int j]
        {
            get => Matrix[i][j];
            set => Matrix[i][j] = value;
        }
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public Fraction[][] Matrix { get; private set; }
        public Basis[] ColumnBasises { get; private set; }
        public Basis[] RowBasises { get; private set; }
        public Fraction[] BigNumRow { get; private set; }

        public object Clone() => new Table(Columns, Rows,
            Matrix.Select(x => x.ToArray()).ToArray(),
            ColumnBasises.Select(x => x.Clone() as Basis).ToArray(),
            RowBasises.Select(x => x.Clone() as Basis).ToArray(),
            BigNumRow.ToArray());
    }
}