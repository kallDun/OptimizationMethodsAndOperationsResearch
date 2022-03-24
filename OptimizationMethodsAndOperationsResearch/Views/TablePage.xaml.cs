﻿using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OptimizationMethodsAndOperationsResearch.Views
{
    /// <summary>
    /// Interaction logic for TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        public TablePage(Table table)
        {
            InitializeComponent();
            InitializeTable(table);
        }
        private void InitializeTable(Table table)
        {
            int rows = 4 + table.RowBasises.Length;
            for (int i = 0; i < rows; i++)
            {
                TableGrid.RowDefinitions.Add(new RowDefinition());
            }
            int cols = 4 + table.ColumnBasises.Length;
            for (int i = 0; i < cols; i++)
            {
                TableGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            var text_boxes = new TextBox[rows][];
            for (int i = 0; i < rows; i++)
            {
                var arr = new TextBox[cols];
                for (int j = 0; j < cols; j++)
                {
                    if (i == 1 && j <= 3)
                    {
                        Grid.SetRowSpan(text_boxes[0][j].Parent as Border, 2);                        
                        continue;
                    }
                    var border = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.DimGray
                    };
                    var textbox = new TextBox
                    {
                        Style = FindResource("TextBoxTableStyle") as Style
                    };
                    border.Child = textbox;
                    arr[j] = textbox;
                    TableGrid.Children.Add(border);
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                }
                text_boxes[i] = arr;
            }

            text_boxes[0][0].Text = "i";
            text_boxes[0][1].Text = "Basis";
            text_boxes[0][2].Text = "c";
            text_boxes[0][3].Text = "P" + GetSmallNumber(0);
            for (int i = 2; i < rows; i++)
            {
                text_boxes[i][0].Text = $"{i - 1}";
            }
            for (int i = 2; i < rows - 2; i++)
            {
                var basis = table.RowBasises[i - 2];
                text_boxes[i][1].Text = $"P{GetSmallNumber(basis.Index)}";
                text_boxes[i][2].Text = basis.Value.ToString();
            }
            for (int j = 4; j < cols; j++)
            {
                var basis = table.ColumnBasises[j - 4];
                text_boxes[0][j].Text = basis.Value.ToString();
                text_boxes[1][j].Text = $"P{GetSmallNumber(basis.Index)}";
            }
            for (int i = 2; i < rows - 1; i++)
            {
                for (int j = 3; j < cols; j++)
                {
                    text_boxes[i][j].Text = table[i - 2, j - 3].ToString();
                }
            }
            for (int i = 3; i < cols; i++)
            {
                text_boxes[rows - 1][i].Text = table.BigNumRow[i - 3].ToString();
            }
        }
        private char GetSmallNumber(int num)
        {
            return (char)(8320 + num);
        }
    }
}