namespace TransportTask.Logic.Services.Convertors
{
    interface IConvertor<T1, T2>
    {
        public T1 Convert(T2 data);
    }
}