using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Convertors
{
    class TableConvertor : IConvertor<Table, PrepTable>
    {
        public Table Convert(PrepTable data)
        {
            var convertor = new CellConvertor();
            var cells = new Cell[data.Cells.Length][];
            for (int i = 0; i < data.Cells.Length; i++)
            {
                var arr = new Cell[data.Cells[i].Length];
                for (int j = 0; j < data.Cells[i].Length; j++)
                {
                    arr[j] = convertor.Convert(data.Cells[i][j]);
                }
                cells[i] = arr;
            }
            return new Table() { Cells = cells };
        }
    }
}