using org.mariuszgromada.math.mxparser;
using System;
using System.Linq;

namespace FindFunctionExtreme.Logic
{
    class CustomFunc
    {
        string[] parameters;
        Function function;
        public int GetParamsCount => parameters.Length;

        public CustomFunc(string function, string[] parameters)
        {
            this.parameters = parameters;
            this.function = new Function(function);
            if (!this.function.checkSyntax())
            {
                throw new Exception("Bad function syntax!");
            }
        }

        public double Solve(double[] x)
        {
            return function.calculate(x);
        }

        public double[] Gradient(double[] x)
        {
            if (x.Length != parameters.Length)
            {
                throw new Exception("X vector in gradient method is invalid!");
            }

            double[] grad = new double[x.Length];
            Argument[] arguments = x.Select((value, index) => new Argument($"{parameters[index]} = {value}")).ToArray();
            string fy = $"f({string.Join(",", parameters)})";

            for (int i = 0; i < x.Length; i++)
            {
                Expression e = new Expression($"der({fy}, {parameters[i]})", new PrimitiveElement[] { function }.Concat(arguments).ToArray());
                grad[i] = e.calculate();
            }

            return grad;
        }
    }
}