using System;
using System.Diagnostics;
using System.Linq;

namespace FindFunctionExtreme.Logic.ExtremumMethods
{
    class DivideStepMethod : IExtremumCalculator
    {
        public ExtremumResult GetExtremum(CustomFunc func, double[] x0, double epsilon)
        {
            Stopwatch timer = new();
            timer.Start();
            ExtremumResult result = new();

            double h0 = 10, l = 0.9;
            double[] x;
            double[] g = func.Gradient(x0);
            if (g.Norm() > epsilon)
            {
                double fx, f0;
                do
                {
                    x = x0.ToArray();
                    fx = func.Solve(x);
                    result.FunctionCalls++;
                    do
                    {
                        for (int i = 0; i < func.GetParamsCount; i++)
                        {
                            x0[i] = x[i] - h0 * g[i];
                        }
                        f0 = func.Solve(x0);
                        result.FunctionCalls++;

                        if (f0 - fx > -l * h0 * Math.Pow(g.Norm(), 2))
                        {
                            h0 *= l;
                        }
                    }
                    while (!((f0 - fx <= -l * h0 * Math.Pow(g.Norm(), 2)) || h0 < epsilon / 2));

                    g = func.Gradient(x0);
                    result.Itterations++;
                }
                while (!(x.Minus(x0).Norm() < epsilon || g.Norm() < epsilon));
            }

            timer.Stop();
            result.Time = timer.Elapsed;
            result.Function = func.Solve(x0);
            result.MinX = x0;
            return result;
        }
    }
}