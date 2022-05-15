using System;

namespace FindFunctionExtreme.Logic
{
    public class ExtremumResult
    {
        public int Itterations { get; set; }
        public int FunctionCalls { get; set; }
        public double[] MinX { get; set; }
        public double Function { get; set; }
        public TimeSpan Time { get; set; }
    }
}