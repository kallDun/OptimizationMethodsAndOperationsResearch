using Fractions;
using System;
using System.Linq;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Table : ICloneable
    {
        public Table(Fraction[][] matrix, Basis[] columnBasises, Basis[] rowBasises, SumValue[] lastRow, bool isMin, bool hasBigNumbers)
        {
            Matrix = matrix;
            ColumnBasises = columnBasises;
            RowBasises = rowBasises;
            LastRow = lastRow;
            IsMin = isMin;
            HasBigNumbers = hasBigNumbers;
        }
        public Fraction this[int i, int j]
        {
            get => Matrix[i][j];
            set => Matrix[i][j] = value;
        }
        public int Rows => Matrix.Length;
        public int Columns => Matrix[0].Length;
        public Fraction[][] Matrix { get; private set; }
        public Basis[] ColumnBasises { get; private set; }
        public Basis[] RowBasises { get; private set; }
        public SumValue[] LastRow { get; private set; }
        public bool HasBigNumbers { get; set; }
        public bool IsMin { get; private set; }
        public VisualDataModel VisualData { get; set; } = new();

        public object Clone() => new Table(Matrix.Select(x => x.ToArray()).ToArray(),
            ColumnBasises.Select(x => x.Clone() as Basis).ToArray(),
            RowBasises.Select(x => x.Clone() as Basis).ToArray(),
            LastRow.ToArray(), IsMin, HasBigNumbers)
        { VisualData = new() };
    }
}