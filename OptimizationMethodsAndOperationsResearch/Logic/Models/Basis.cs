using Fractions;
using System;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Basis : ICloneable
    {
        public Basis(int index, Fraction value, bool isHugeNumber = false)
        {
            Index = index;
            Value = value;
            IsHugeNumber = isHugeNumber;
        }
        public int Index { get; set; }
        public Fraction Value { get; set; }
        public bool IsHugeNumber { get; set; }

        public object Clone() => new Basis(Index, Value, IsHugeNumber);
    }
}