namespace FindFunctionExtreme.Logic
{
    interface IExtremumCalculator
    {
        double[] GetExtremum(CustomFunc func, double[] x0, double epsilon);
    }
}