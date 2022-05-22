using System.Diagnostics;
using System.Linq;

namespace FindFunctionExtreme.Logic.ExtremumMethods
{
    class FastestDescentMethod : IExtremumCalculator
    {
        public ExtremumResult GetExtremum(CustomFunc func, double[] x0, double epsilon)
        {
            Stopwatch timer = new();
            timer.Start();
            ExtremumResult result = new();

            double h0 = 10;
            double[] x;

            double[] g = func.Gradient(x0);
            if (g.Norm() > epsilon)
            {
                do
                {
                    x = x0.ToArray();
                    double h = FindH(func, x.ToArray(), g, h0, epsilon, result);
                    for (int i = 0; i < func.GetParamsCount; i++)
                    {
                        x0[i] = x[i] - h * g[i];
                    }
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

        private double FindH(CustomFunc func, double[] x0, double[] g, double h0, double epsilon, ExtremumResult result)
        {
            double h = 0;
            double f1 = func.Solve(x0);
            result.FunctionCalls++;
            double f2;

            double[] x2 = new double[x0.Length];
            double[] x1;

            do
            {
                h0 /= 2;
                for (int i = 0; i < func.GetParamsCount; i++)
                {
                    x2[i] = x0[i] - (h0 * g[i]);
                }
                f2 = func.Solve(x2);
                result.FunctionCalls++;
            }
            while (!(f1 > f2 || h0 < epsilon));

            if (h0 > epsilon)
            {
                do
                {
                    x1 = x2.ToArray();
                    f1 = f2;
                    h += h0;

                    for (int i = 0; i < func.GetParamsCount; i++)
                    {
                        x2[i] = x1[i] - h * g[i];
                    }

                    f2 = func.Solve(x2);
                    result.FunctionCalls++;
                }
                while (!(f1 < f2));

                double ha = h - 2 * h0;
                double hb = h;

                double delta = epsilon / 3;

                do
                {
                    double h1 = (ha + hb - delta) / 2;
                    double h2 = (ha + hb + delta) / 2;

                    for (int i = 0; i < func.GetParamsCount; i++)
                    {
                        x1[i] = x0[i] - h1 * g[i];
                        x2[i] = x0[i] - h2 * g[i];
                    }

                    f1 = func.Solve(x1);
                    f2 = func.Solve(x2);
                    result.FunctionCalls += 2;

                    if (f1 <= f2)
                    {
                        hb = h2;
                    }
                    else
                    {
                        ha = h1;
                    }
                }
                while (!(hb - ha < epsilon));

                h = (ha + hb) / 2;
            }

            return h;
        }
    }
}