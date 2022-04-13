using OptimizationMethodsAndOperationsResearch.Logic.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TransportTask.Views;

namespace TransportTask
{
    public partial class MainWindow : Window
    {
        List<Page> pages = new List<Page>();
        StartPage startPage;
        int page_index = 0;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();

            startPage = new StartPage();
            PageFrame.Content = startPage;
            pages.Add(startPage);
        }

        private void SolveButton_Click(object sender, RoutedEventArgs args)
        {
            pages.Clear();
            pages.Add(startPage);
            try
            {
                var start_table = startPage.GetTable();
                pages.Add(new PrepTablePage(start_table));



            }
            catch (Exception e)
            {
                MessageBox.Show("Something gone wrong! Exception msg: " + e.Message);
            }
            ToLeftButton.IsEnabled = true;
            ToRightButton.IsEnabled = true;
        }

        private void ToPdfButton_Click(object sender, RoutedEventArgs e) => PdfService.SaveToPdf(pages);
        private void ToRightButton_Click(object sender, RoutedEventArgs e) => ChangePage(1);
        private void ToLeftButton_Click(object sender, RoutedEventArgs e) => ChangePage(-1);
        private void ChangePage(int plus)
        {
            PageFrame.NavigationService.RemoveBackEntry();
            page_index += plus;
            if (page_index > pages.Count - 1) page_index = 0;
            else if (page_index < 0) page_index = pages.Count - 1;
            PageFrame.Content = pages[page_index];

            SolvePanel.Visibility = page_index != 0 ? Visibility.Hidden : Visibility.Visible;
            PageCounterTextBlock.Text = page_index != 0 ? page_index.ToString() : "";
        }        
    }
}