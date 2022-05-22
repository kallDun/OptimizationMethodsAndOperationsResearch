using System;
using System.Diagnostics;
using System.Linq;

namespace FindFunctionExtreme.Logic.ExtremumMethods
{
    class CoordinateDescentMethod : IExtremumCalculator
    {
        public ExtremumResult GetExtremum(CustomFunc func, double[] x0, double epsilon)
        {
            Stopwatch timer = new();
            timer.Start();
            ExtremumResult result = new();

            double h0 = 10, l = 0.5;
            double[] h = new double[func.GetParamsCount];
            for (int i = 0; i < func.GetParamsCount; i++)
            {
                h[i] = h0;
            }
            double[] x_int = x0.ToArray();
            double[] x_ext;

            do
            {
                x_ext = x_int.ToArray();
                for (int i = 0; i < func.GetParamsCount; i++)
                {
                    double[] x = x_int.ToArray();
                    double fx = func.Solve(x);

                    double[] y1 = x.ToArray();
                    y1[i] += 3 * epsilon;
                    double[] y2 = x.ToArray();
                    y2[i] -= 3 * epsilon;
                    double f1 = func.Solve(y1), f2 = func.Solve(y2);
                    double sign = f2 - f1 > 0 ? 1 : f2 < f1 ? -1 : 0;

                    result.FunctionCalls += 3;

                    double fx1;
                    do
                    {
                        x_int[i] = x[i] + h[i] * sign;
                        fx1 = func.Solve(x_int);
                        result.FunctionCalls++;
                        if (fx1 >= fx) h[i] *= l;
                    } 
                    while (!(fx1 < fx || h[i] < epsilon / 2));
                }
                result.Itterations++;
            }
            while (!(x_int.Minus(x_ext).Norm() < epsilon));

            timer.Stop();
            result.Time = timer.Elapsed;
            result.Function = func.Solve(x_int);
            result.MinX = x_int;
            return result;
        }
    }
}