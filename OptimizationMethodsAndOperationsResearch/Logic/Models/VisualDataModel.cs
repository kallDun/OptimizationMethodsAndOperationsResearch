using System;

namespace OptimizationMethodsAndOperationsResearch.Logic.Models
{
    public class VisualDataModel
    {
        public VisualDataModel(VisualSelectedColsRowsModel selectedColsRows)
        {
            Enabled = true;
            SelectedColsRows = selectedColsRows;
        }
        public VisualDataModel() { }
        public bool Enabled { get; set; }
        public VisualSelectedColsRowsModel SelectedColsRows { get; set; }
    }

    public struct VisualSelectedColsRowsModel
    {
        public VisualSelectedColsRowsModel(int selectedRow, int selectedCol)
        {
            SelectedRow = selectedRow;
            SelectedCol = selectedCol;
        }
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }
    }
}