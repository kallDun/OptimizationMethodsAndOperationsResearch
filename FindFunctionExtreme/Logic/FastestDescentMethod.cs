using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFunctionExtreme.Logic
{
    class FastestDescentMethod : IExtremumCalculator
    {
        public double[] GetExtremum(CustomFunc func, double[] x0, double epsilon)
        {
            double h0 = 100;


            return Array.Empty<double>();
        }

        private double FindH(CustomFunc func, double[] x, double[] grad, double h0, double epsilon)
        {

            // vectors minus ->
            double[] z = x.Minus(x);

            // vectors norm ||x|| ->
            double c = x.Norm();

            // get gradient ->
            double[] g = func.Gradient(x);

            // calc function ->
            double f = func.Solve(x);

            return double.NaN;
        }
    }
}