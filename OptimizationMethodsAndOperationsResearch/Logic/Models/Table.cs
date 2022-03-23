using Fractions;
using System;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Table : ICloneable
    {
        public Table(int columns, int rows, Fraction[][] matrix, Basis[] columnBasises, Basis[] rowsBasises)
        {
            Columns = columns;
            Rows = rows;
            Matrix = matrix;
            ColumnBasises = columnBasises;
            RowsBasises = rowsBasises;
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
        public Basis[] RowsBasises { get; private set; }

        public object Clone() => new Table(Columns, Rows,
            Matrix.Select(x => x.Clone() as Fraction[]).ToArray(),
            ColumnBasises.Select(x => x.Clone() as Basis).ToArray(),
            RowsBasises.Select(x => x.Clone() as Basis).ToArray());
    }
}