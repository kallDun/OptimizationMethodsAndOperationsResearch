using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Convertors
{
    class CellConvertor : IConvertor<Cell, PrepCell>
    {
        public Cell Convert(PrepCell data)
        {
            return new Cell(data.Coefficient, data.IsProductsNone, products: data.Products);
        }
    }
}