using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using OptimizationMethodsAndOperationsResearch.Logic.Services;
using OptimizationMethodsAndOperationsResearch.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OptimizationMethodsAndOperationsResearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Page> pages = new List<Page>();
        int page_index = 0;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
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

            var (func, matrix) = page.GetFunctionData;
            var curr_table = new FunctionParser().ToTable(func, matrix);
            pages.Add(new TablePage(curr_table));
            var simplex_calculator = new SimplexMethodCalculator();

            var counter = 0;
            while (!simplex_calculator.IsOptimizated(curr_table) && counter < 1000)
            {
                var prev_table = curr_table.Clone() as Table;
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
            }
            else
            {
                page.OutputTextBox.Text = "No Solutions";
            }

            ToLeftButton.IsEnabled = true;
            ToRightButton.IsEnabled = true;
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