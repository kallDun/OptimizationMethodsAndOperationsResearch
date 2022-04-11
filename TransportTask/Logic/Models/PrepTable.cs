namespace TransportTask.Logic.Models
{
    public class PrepTable
    {
        public PrepTable(PrepCell[][] cells, int[] reserves, int[] need)
        {
            Cells = cells;
            Reserves = reserves;
            Need = need;
        }
        public PrepCell[][] Cells { get; private set; }
        public int[] Reserves { get; private set; }
        public int[] Need { get; private set; }
    }
}