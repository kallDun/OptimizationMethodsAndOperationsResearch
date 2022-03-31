using Fractions;
using System;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Basis : ICloneable
    {
        public Basis(int index, SumValue value, bool isHugeNumber = false)
        {
            Index = index;
            Value = value;
            IsHugeNumber = isHugeNumber;
        }
        public int Index { get; set; }
        public SumValue Value { get; set; }

        public object Clone() => new Basis(Index, Value, IsHugeNumber);
    }
}