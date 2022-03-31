using Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    struct SumValue
    {
        public SumValue(Fraction value, Fraction value_m)
        {
            ValueNumber = value;
            ValueM = value_m;
        }

        public Fraction ValueNumber { get; set; }
        public Fraction ValueM { get; set; }

        public static SumValue operation +(SumValue a, SumValue b)
        {
            return new SumValue(a.ValueNumber + b.ValueNumber, a.ValueM + b.ValueM);
        }
    }
}