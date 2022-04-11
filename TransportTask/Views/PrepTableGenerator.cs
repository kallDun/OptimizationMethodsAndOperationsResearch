using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TransportTask.Logic.Models;

namespace TransportTask.Views
{
    static class PrepTableGenerator
    {
        public delegate PrepTable GetPrepTableDelegate();

        struct TextBoxes
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
            public TextBoxes[][] text_boxes;
            public Grid[][] background_boxes;
            public Border[][] border_boxes;

            public TableDesign(int rows, int cols)
            {
                text_boxes = new TextBoxes[rows][];
                for (int i = 0; i < text_boxes.Length; i++) text_boxes[i] = new TextBoxes[cols];
                background_boxes = new Grid[rows][];
                for (int i = 0; i < background_boxes.Length; i++) background_boxes[i] = new Grid[cols];
                border_boxes = new Border[rows][];
                for (int i = 0; i < border_boxes.Length; i++) border_boxes[i] = new Border[cols];
            }

            public void Resize(int rows, int cols)
            {
                if (rows < 3 || cols < 2) return;
                int prev_rows = text_boxes.Length;
                int prev_cols = text_boxes[0].Length;



            }
        }

        public static Grid InitTable(PrepTable table)
        {
            var (grid, del) = InitReturnTable(table, true);
            return grid;
        }

        public static (Grid, GetPrepTableDelegate) InitReturnTable(PrepTable table, bool IsReadonly = false)
        {
            Grid main_grid = new Grid();
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
            TableDesign tableDesign = new TableDesign(rows, cols);

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
                    tableDesign.text_boxes[i][j] = new TextBoxes(textbox_center, textbox_left_top, textbox_left_bottom, textbox_right_top, textbox_right_bottom);
                    tableDesign.
                    background_boxes[i][j] = background;
                    tableDesign.border_boxes[i][j] = border;
                    Grid.SetRow(background, i);
                    Grid.SetColumn(background, j);
                }
            }
            Grid.SetColumnSpan(tableDesign.background_boxes[0][1], cols - 2);

            for (int i = 2; i < 5; i++) tableDesign.text_boxes[i][0].center.Text = $"A{GetSmallNumber(i - 1)}";
            for (int j = 1; j < 4; j++) tableDesign.text_boxes[1][j].center.Text = $"B{GetSmallNumber(j)}";
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
            var return_table = new GetPrepTableDelegate(() =>
            {


                /*var table_ = new PrepTable(cells, reserves, need);
                return table_;*/
                return null;
            });

            return (main_grid, return_table);
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
                    Foreground = Brushes.IndianRed
                },
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