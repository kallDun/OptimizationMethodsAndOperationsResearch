using System;

namespace FindFunctionExtreme.Logic
{
    static class VectorExtensions
    {
        public static double[] Minus(this double[] vector, double[] other_vector)
        {
            if (vector.Length != other_vector.Length) throw new Exception("Vectors size must be equal in minus method!");
            double[] result = new double[vector.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = vector[i] - other_vector[i];
            }
            return result;
        }

        public static double Norm(this double[] vector)
        {
            double sum = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                sum += Math.Pow(vector[i], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}