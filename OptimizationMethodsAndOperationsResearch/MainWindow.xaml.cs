using OptimizationMethodsAndOperationsResearch.Logic.Models;
using OptimizationMethodsAndOperationsResearch.Logic.Services;
using OptimizationMethodsAndOperationsResearch.Logic.Services.FractionMethod;
using OptimizationMethodsAndOperationsResearch.Logic.Services.IntegerMethod;
using OptimizationMethodsAndOperationsResearch.Logic.Utilities;
using OptimizationMethodsAndOperationsResearch.Views;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace OptimizationMethodsAndOperationsResearch
{
    public partial class MainWindow : Window
    {
        List<Page> pages = new();
        int page_index = 0;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();
            var start_page = new StartPage();
            PageFrame.Content = start_page;
            pages.Add(start_page);
            start_page.SolveButton.Click += (s, e) => CalculateClick(start_page);
            start_page.ToPdfButton.Click += (s, e) => PdfService.SaveToPdf(pages);
        }

        private void CalculateClick(StartPage page)
        {
            pages.Clear();
            pages.Add(page);

            var methodType = page.simplexMethodType;
            if (methodType is SimplexMethodType.Fraction)
            {
                CalcSimplexMethod(page);
            }
            else if (methodType is SimplexMethodType.Integer)
            {
                Table table = CalcSimplexMethod(page);
                if (table is not null) CalcIntegerMethod(page, table);
            }            

            ToLeftButton.IsEnabled = true;
            ToRightButton.IsEnabled = true;
        }

        private Table CalcSimplexMethod(StartPage page)
        {
            Table curr_table, prev_table;
            var (func, matrix) = page.GetFunctionData;
            curr_table = new FunctionParser().ToTable(func, matrix);
            pages.Add(new TablePage(curr_table));

            AbstractSimplexMethod simplex_calculator = new BasicSimplexMethod();
            int counter = 0;
            while (!simplex_calculator.IsOptimized(curr_table) && counter < 1000)
            {
                prev_table = curr_table.Clone() as Table;
                VisualDataModel data;
                (curr_table, data) = simplex_calculator.GetNextTable(curr_table);
                prev_table.VisualData = data;
                pages.Add(new TablePage(prev_table));
                pages.Add(new TablePage(curr_table));
                counter++;
            }
            if (simplex_calculator.HasSolution)
            {
                var results = simplex_calculator.GetResults(curr_table);
                page.OutputTextBox.Text = string.Join("  ", results.Select(x => $"x{x.Key} = {x.Value}"));
                return curr_table;
            }
            else
            {
                page.OutputTextBox.Text = "No Solutions";
                return null;
            }
        }
        private void CalcIntegerMethod(StartPage page, Table table)
        {
            if (AbstractSimplexMethod.HasBigNumbersInTable(table))
            {
                table = TableUtility.GetWithoutMajorColumn(table);
                pages.Add(new TablePage(table));
            }

            Table prev_table;
            GomorisMethodService gomoriService = new();
            AbstractSimplexMethod simplex_calculator = new DoubleSimplexMethod();

            int counter = 0;
            do
            {
                prev_table = gomoriService.GomorisMethod(table);
                if (prev_table is null) break;
                table = prev_table;
                pages.Add(new TablePage(table));

                prev_table = table.Clone() as Table;
                VisualDataModel data;
                (table, data) = simplex_calculator.GetNextTable(table);
                prev_table.VisualData = data;
                pages.Add(new TablePage(prev_table));
                pages.Add(new TablePage(table));

                counter++;
            } 
            while (!simplex_calculator.IsOptimized(table) && counter < 1000);

            if (simplex_calculator.HasSolution)
            {
                var results = simplex_calculator.GetResults(table);
                page.OutputTextBox.Text = string.Join("  ", results.Select(x => $"x{x.Key} = {x.Value}"));
            }
            else
            {
                page.OutputTextBox.Text = "No Solutions";
            }
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.NavigationService.RemoveBackEntry();
            if (page_index == pages.Count - 1) page_index = 0;
            else page_index++;
            PageFrame.Content = pages[page_index];
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.NavigationService.RemoveBackEntry();
            if (page_index == 0) page_index = pages.Count - 1;
            else page_index--;
            PageFrame.Content = pages[page_index];
        }
    }
}