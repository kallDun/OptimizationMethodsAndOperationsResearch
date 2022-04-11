using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransportTask.Logic.Models;
using TransportTask.Views;

namespace TransportTask
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

            StartPage startPage = new StartPage();
            PageFrame.Content = startPage;
            pages.Add(startPage);

        }        

        private void CalculateClick()
        {

        }

        private void ToRightButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.NavigationService.RemoveBackEntry();
            if (page_index == pages.Count - 1) page_index = 0;
            else page_index++;
            PageFrame.Content = pages[page_index];
        }

        private void ToLeftButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.NavigationService.RemoveBackEntry();
            if (page_index == 0) page_index = pages.Count - 1;
            else page_index--;
            PageFrame.Content = pages[page_index];
        }
    }
}