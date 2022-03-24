using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using OptimizationMethodsAndOperationsResearch.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            var page = new TablePage(GetTable());
            PageFrame.NavigationService.RemoveBackEntry();
            PageFrame.Content = page;
        }

        private Table GetTable()
        {
            Fraction[][] matrix =
            {
                new Fraction[]
                {
                    new Fraction(2), new Fraction(5, 3), new Fraction(0), new Fraction(1), new Fraction(1, 3), new Fraction(0),
                },
                new Fraction[]
                {
                    new Fraction(1), new Fraction(-1, 3), new Fraction(1), new Fraction(0), new Fraction(1, 3), new Fraction(0),
                },
                new Fraction[]
                {
                    new Fraction(8, 3), new Fraction(5, 3), new Fraction(0), new Fraction(0), new Fraction(-2, 3), new Fraction(1),
                },
                new Fraction[]
                {
                    new Fraction(3), new Fraction(-13, 3), new Fraction(0), new Fraction(0), new Fraction(-2, 3), new Fraction(0),
                },
            };
            Basis P1 = new Basis(1, new Fraction(5));
            Basis P2 = new Basis(2, new Fraction(-2));
            Basis P3 = new Basis(3, new Fraction(0));
            Basis P4 = new Basis(4, new Fraction(0));
            Basis P5 = new Basis(5, new Fraction(0));
            Basis[] column_basis =
            {
                P1, P2, P3, P4, P5
            };
            Basis[] row_basis =
            {
                P3, P4, P5
            };
            Fraction[] big_num_row = new Fraction[]
            {
                new Fraction(0), new Fraction(0), new Fraction(0), new Fraction(0), new Fraction(0), new Fraction(0)
            };
            return new Table(4, 6, matrix, column_basis, row_basis, big_num_row);
        }
    }
}
