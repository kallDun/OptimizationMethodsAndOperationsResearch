using Fractions;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public struct SumValue
    {
        public Fraction ValueNumber { get; set; }
        public Fraction ValueM { get; set; }
        public bool HasBigNum => ValueM != 0;
        public SumValue(Fraction value, Fraction value_m)
        {
            ValueNumber = value;
            ValueM = value_m;
        }
        public override string ToString()
        {
            if (HasBigNum)
            {
                if (ValueM == 1) return "M";
                else if (ValueM == -1) return "-M";
                else return $"{ValueM}M";
            }
            else return ValueNumber.ToString();
        }

        public static SumValue operator +(SumValue a, SumValue b)
        {
            return new SumValue(a.ValueNumber + b.ValueNumber, a.ValueM + b.ValueM);
        }
    }
}