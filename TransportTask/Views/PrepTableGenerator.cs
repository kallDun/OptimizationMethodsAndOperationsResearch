using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TransportTask.Logic.Models;

namespace TransportTask.Views
{
    static class PrepTableGenerator
    {
        public delegate PrepTable GetPrepTableDelegate();

        class TextBoxes
        {
            public TextBox center;
            public TextBox left_top;
            public TextBox left_bottom;
            public TextBox right_top;
            public TextBox right_bottom;
            public TextBoxes(TextBox center, TextBox left_top, TextBox left_bottom, TextBox right_top, TextBox right_bottom)
            {
                this.center = center;
                this.left_top = left_top;
                this.left_bottom = left_bottom;
                this.right_top = right_top;
                this.right_bottom = right_bottom;
            }
        }
        class TableDesign
        {
            Grid MainGrid;
            public TextBoxes[][] text_boxes;
            public Grid[][] background_boxes;
            public Border[][] border_boxes;

            public TableDesign(Grid main_grid, int rows, int cols)
            {
                MainGrid = main_grid;
                text_boxes = new TextBoxes[rows][];
                for (int i = 0; i < text_boxes.Length; i++) text_boxes[i] = new TextBoxes[cols];
                background_boxes = new Grid[rows][];
                for (int i = 0; i < background_boxes.Length; i++) background_boxes[i] = new Grid[cols];
                border_boxes = new Border[rows][];
                for (int i = 0; i < border_boxes.Length; i++) border_boxes[i] = new Border[cols];
            }

            public void RemoveCol()
            {
                var cols = background_boxes[0].Length - 1;
                if (cols < 3) return;

                MainGrid.ColumnDefinitions.RemoveAt(cols);
                for (int i = 0; i < background_boxes.Length; i++)
                {
                    var item = background_boxes[i][cols];
                    if (item != null)
                    {
                        Grid.SetColumn(item, cols - 1);
                    }                    
                    MainGrid.Children.Remove(background_boxes[i][cols - 1]);
                    background_boxes[i][cols - 1] = item;
                    text_boxes[i][cols - 1] = text_boxes[i][cols];
                    border_boxes[i][cols - 1] = border_boxes[i][cols];       
                    
                    Array.Resize(ref background_boxes[i], cols);
                    Array.Resize(ref text_boxes[i], cols);
                    Array.Resize(ref border_boxes[i], cols);
                }
                Grid.SetColumnSpan(background_boxes[0][1], cols - 2);
            }
            public void AddCol()
            {
                var cols = background_boxes[0].Length + 1;
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int i = 0; i < background_boxes.Length; i++)
                {
                    Array.Resize(ref background_boxes[i], cols);
                    Array.Resize(ref text_boxes[i], cols);
                    Array.Resize(ref border_boxes[i], cols);

                    var item = background_boxes[i][cols - 2];
                    background_boxes[i][cols - 1] = item;
                    text_boxes[i][cols - 1] = text_boxes[i][cols - 2];
                    border_boxes[i][cols - 1] = border_boxes[i][cols - 2];

                    if (item != null)
                    {
                        Grid.SetColumn(item, cols - 1);
                    }
                    if (background_boxes[i][cols - 3] != null)
                    {
                        FillGridCell(MainGrid, this, i, cols - 2);

                        var textbox = text_boxes[i][cols - 2].center;
                        if (i == 1)
                        {
                            textbox.Text = $"B{GetSmallNumber(cols - 2)}";
                        }
                        else
                        {
                            textbox.Text = "0";
                            textbox.IsReadOnly = false;
                            textbox.Foreground = Brushes.DarkSlateBlue;
                        }
                    }
                    else
                    {
                        background_boxes[i][cols - 2] = null;
                        text_boxes[i][cols - 2] = null;
                        border_boxes[i][cols - 2] = null;
                    }
                }
                Grid.SetColumnSpan(background_boxes[0][1], cols - 2);
            }
            public void RemoveRow()
            {
                var rows = background_boxes.Length - 1;
                if (rows < 4) return;

                MainGrid.RowDefinitions.RemoveAt(rows);
                for (int j = 0; j < background_boxes[0].Length; j++)
                {
                    var item = background_boxes[rows][j];
                    if (item != null)
                    {
                        Grid.SetRow(item, rows - 1);
                    }
                    MainGrid.Children.Remove(background_boxes[rows - 1][j]);
                    background_boxes[rows - 1][j] = item;
                    text_boxes[rows - 1][j] = text_boxes[rows][j];
                    border_boxes[rows - 1][j] = border_boxes[rows][j];                    
                }
                Array.Resize(ref background_boxes, rows);
                Array.Resize(ref text_boxes, rows);
                Array.Resize(ref border_boxes, rows);
            }
            public void AddRow()
            {
                var rows = background_boxes.Length + 1;
                MainGrid.RowDefinitions.Add(new RowDefinition());

                var cols = background_boxes[0].Length;
                Array.Resize(ref background_boxes, rows);
                background_boxes[rows - 1] = new Grid[cols];
                Array.Resize(ref text_boxes, rows);
                text_boxes[rows - 1] = new TextBoxes[cols];
                Array.Resize(ref border_boxes, rows);
                border_boxes[rows - 1] = new Border[cols];

                for (int j = 0; j < background_boxes[0].Length; j++)
                {
                    var item = background_boxes[rows - 2][j];
                    background_boxes[rows - 1][j] = item;
                    text_boxes[rows - 1][j] = text_boxes[rows - 2][j];
                    border_boxes[rows - 1][j] = border_boxes[rows - 2][j];

                    Grid.SetRow(item, rows - 1);
                    FillGridCell(MainGrid, this, rows - 2, j);

                    var textbox = text_boxes[rows - 2][j].center;
                    if (j == 0)
                    {
                        textbox.Text = $"A{GetSmallNumber(rows - 3)}";
                    }
                    else
                    {
                        textbox.Text = "0";
                        textbox.IsReadOnly = false;
                        textbox.Foreground = Brushes.DarkSlateBlue;
                    }
                }
            }
        }

        public static Grid InitTable(PrepTable table)
        {
            var (grid, del) = InitReturnTable(table, true);
            return grid;
        }

        public static (Grid, GetPrepTableDelegate) InitReturnTable(PrepTable table, bool IsReadonly = false)
        {
            Grid main_grid = new();
            int rows = 3 + table.Reserves.Length;
            int cols = 2 + table.Need.Length;
            for (int i = 0; i < rows; i++)
            {
                main_grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < cols; i++)
            {
                main_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            TableDesign tableDesign = new TableDesign(main_grid, rows, cols);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == 1 && (j == 0 || j == cols - 1))
                    {
                        Grid.SetRowSpan(tableDesign.background_boxes[i - 1][j], 2);
                        continue;
                    }
                    if (i == 0 && j > 1 && j < cols - 1) continue;
                    FillGridCell(main_grid, tableDesign, i, j);
                }
            }
            Grid.SetColumnSpan(tableDesign.background_boxes[0][1], cols - 2);

            for (int i = 2; i < rows - 1; i++) tableDesign.text_boxes[i][0].center.Text = $"A{GetSmallNumber(i - 1)}";
            for (int j = 1; j < cols - 1; j++) tableDesign.text_boxes[1][j].center.Text = $"B{GetSmallNumber(j)}";
            tableDesign.text_boxes[0][0].center.Text = "Supply";
            tableDesign.text_boxes[0][1].center.Text = "Consumption points";
            tableDesign.text_boxes[0][cols - 1].center.Text = "Stocks";
            tableDesign.text_boxes[rows - 1][0].center.Text = "Need";

            for (int i = 0; i < table.Reserves.Length; i++)
            {
                var item = tableDesign.text_boxes[i + 2][cols - 1];
                item.center.Text = table.Reserves[i].ToString();
                if (!IsReadonly)
                {
                    item.center.IsReadOnly = false;
                    item.center.Foreground = Brushes.DarkSlateBlue;
                }
            }
            for (int j = 0; j < table.Need.Length; j++)
            {
                var item = tableDesign.text_boxes[rows - 1][j + 1];
                item.center.Text = table.Need[j].ToString();
                if (!IsReadonly)
                {
                    item.center.IsReadOnly = false;
                    item.center.Foreground = Brushes.DarkSlateBlue;
                }
            }

            for (int i = 0; i < table.Cells.Length; i++)
            {
                for (int j = 0; j < table.Cells[i].Length; j++)
                {
                    var item = tableDesign.text_boxes[i + 2][j + 1];
                    var cell = table.Cells[i][j];

                    if (IsReadonly)
                    {
                        item.left_top.Text = cell.Coefficient.ToString();
                        if (cell.IsChecked)
                        {
                            item.right_bottom.Text = cell.IsProductsNone ? "x" : cell.Products.ToString();
                            item.right_bottom.FontWeight = FontWeights.Bold;
                        }
                    }
                    else
                    {
                        item.center.Text = cell.Coefficient.ToString();
                        item.center.IsReadOnly = false;
                        item.center.Foreground = Brushes.DarkSlateBlue;
                    }
                }
            }

            if (!IsReadonly) // add buttons
            {
                var parent = tableDesign.background_boxes[rows - 1][cols - 1];
                var add_col = GenerateButton("+");
                add_col.Click += (s, e) => tableDesign.AddCol();
                var remove_col = GenerateButton("-");
                remove_col.Click += (s, e) => tableDesign.RemoveCol();
                var col_stack = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                col_stack.Children.Add(add_col);
                col_stack.Children.Add(remove_col);
                
                var add_row = GenerateButton("+");
                add_row.Click += (s, e) => tableDesign.AddRow();
                var remove_row = GenerateButton("-");
                remove_row.Click += (s, e) => tableDesign.RemoveRow();
                var row_stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                row_stack.Children.Add(add_row);
                row_stack.Children.Add(remove_row);

                parent.Children.Add(col_stack);
                parent.Children.Add(row_stack);
            }

            var return_table = new GetPrepTableDelegate(() =>
            {
                /*var table_ = new PrepTable(cells, reserves, need);
                return table_;*/
                return null;
            });

            return (main_grid, return_table);
        }

        private static void FillGridCell(Grid main_grid, TableDesign tableDesign, int i, int j)
        {
            var background = new Grid();
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.DimGray
            };
            main_grid.Children.Add(background);
            background.Children.Add(border);

            TextBox textbox_center = GenerateTextBox(background, VerticalAlignment.Center, HorizontalAlignment.Center);
            TextBox textbox_left_top = GenerateTextBox(background, VerticalAlignment.Top, HorizontalAlignment.Left);
            TextBox textbox_left_bottom = GenerateTextBox(background, VerticalAlignment.Bottom, HorizontalAlignment.Left);
            TextBox textbox_right_top = GenerateTextBox(background, VerticalAlignment.Top, HorizontalAlignment.Right);
            TextBox textbox_right_bottom = GenerateTextBox(background, VerticalAlignment.Bottom, HorizontalAlignment.Right);
            
            tableDesign.text_boxes[i][j] = new TextBoxes(textbox_center, textbox_left_top, textbox_left_bottom, textbox_right_top, textbox_right_bottom); ;
            tableDesign.background_boxes[i][j] = background;
            tableDesign.border_boxes[i][j] = border;
            Grid.SetRow(background, i);
            Grid.SetColumn(background, j);
        }

        private static Button GenerateButton(string text)
        {
            return new Button
            {
                Content = new TextBlock
                {
                    Text = text,
                    FontSize = 16,
                    FontFamily = new FontFamily("DejaVu Sans"),
                    Foreground = Brushes.DarkSlateBlue,                    
                },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.DarkSlateBlue,
                Width = 20,
                Height = 20
            };
        }
        private static TextBox GenerateTextBox(Grid grid, VerticalAlignment verticalA, HorizontalAlignment horizontalA)
        {
            var textbox = new TextBox
            {
                Background = Brushes.Transparent,
                Foreground = Brushes.Black,
                VerticalAlignment = verticalA,
                HorizontalAlignment = horizontalA,
                Margin = new Thickness(7),
                TextAlignment = TextAlignment.Center,
                BorderThickness = new Thickness(0),
                IsReadOnly = true,
                FontSize = 20,
                FontFamily = new FontFamily("DejaVu Sans")
            };
            grid.Children.Add(textbox);
            return textbox;
        }
        static char GetSmallNumber(int num) => (char)(8320 + num);
    }
}