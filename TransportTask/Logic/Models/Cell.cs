namespace TransportTask.Logic.Models
{
    public struct Cell
    {
        public Cell(int coefficient, bool isProductsNone = true, bool isPotentialNone = true, int products = 0, int potential = 0)
        {
            Coefficient = coefficient;
            Products = products;
            IsProductsNone = isProductsNone;
            IsPotentialNone = isPotentialNone;
            Potential = potential;
        }

        public int Coefficient { get; set; }
        public bool IsProductsNone { get; set; }
        public int Products { get; set; }
        public bool IsPotentialNone { get; set; }
        public int Potential { get; set; }
    }
}