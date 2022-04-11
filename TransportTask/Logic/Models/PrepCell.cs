namespace TransportTask.Logic.Models
{
    public struct PrepCell
    {
        public PrepCell(int coefficient, int products = 0, bool isProductsNone = false, bool isChecked = false)
        {
            Coefficient = coefficient;
            Products = products;
            IsProductsNone = isProductsNone;
            IsChecked = isChecked;
        }

        public int Coefficient { get; set; }
        public int Products { get; set; }
        public bool IsProductsNone { get; set; }
        public bool IsChecked { get; set; }

        public static implicit operator PrepCell(int n) => new PrepCell(n);
    }
}