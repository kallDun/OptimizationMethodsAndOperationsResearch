using System;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class Basis : ICloneable
    {
        public Basis(int index, SumValue value)
        {
            Index = index;
            SumValue = value;
        }
        public int Index { get; set; }
        public SumValue SumValue { get; set; }

        public object Clone() => new Basis(Index, SumValue);
    }
}