namespace FindFunctionExtreme.Logic.ExtremumMethods
{
    interface IExtremumCalculator
    {
        ExtremumResult GetExtremum(CustomFunc func, double[] x0, double epsilon);
    }
}