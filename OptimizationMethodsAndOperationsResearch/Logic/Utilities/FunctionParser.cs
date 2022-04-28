using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    class FunctionParser
    {
        int GetElementsInRow(int elements_count, int equations_count) => 1 + elements_count + equations_count;

        private struct LastColValue
        {
            public Fraction Value;
            public bool IsLess;
        }

        public Table ToTable(string function, string equations)
        {
            var function_parts = function.Split(new string[] { "=", "->" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            bool if_func_min = function_parts[2] is "min";

            var arguments_parts = function_parts[1].Split(new string[] { "x1", "x2" }, StringSplitOptions.None).Select(x => x.Replace(" ", "")).ToArray();
            Fraction x1 = GetFracFromString(arguments_parts[0]), x2 = GetFracFromString(arguments_parts[1]);
            var C = new Fraction[] { x1, x2, 0, 0, 0 };

            var eqs = equations.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var equation_count = eqs.Length - 1;
            var orig_matrix = new Fraction[equation_count, 2];
            var last_column = new LastColValue[equation_count];
            var ps = new List<int[]>();
            for (int i = 0; i < equation_count; i++)
            {
                string[] values = eqs[i].Split(new string[] { "x1", "x2" }, StringSplitOptions.None).Select(x => x.Replace(" ", "")).ToArray();
                orig_matrix[i, 0] = GetFracFromString(values[0]);
                orig_matrix[i, 1] = GetFracFromString(values[1]);
                last_column[i] = GetLastColValue(values[2]);
            }

            Fraction[][] main_matrix = FormMatrix(orig_matrix, last_column);
            var X0 = new Fraction[main_matrix[0].Length - 1];
            for (int i = 0; i < X0.Length; i++)
            {
                X0[i] = i < 2 
                    ? 0 
                    : GetSimpleBasisResult(main_matrix, i + 1);
            }

            return FormTableFromData(main_matrix, X0, C, if_func_min);
        }

        private Table FormTableFromData(Fraction[][] matrix, Fraction[] X0, Fraction[] C, bool if_min)
        {
            var equations_count = matrix.Length;
            bool has_big_num = false;
            Basis[] rowBasises = new Basis[matrix[0].Length - 1];
            for (int i = 0; i < rowBasises.Length; i++)
            {
                rowBasises[i] = i + 1 < GetElementsInRow(2, equations_count) ? new Basis(i + 1, new SumValue(C[i], 0)) : new Basis(i + 1, new SumValue(0, if_min ? 1 : -1));
                if (i + 1 >= GetElementsInRow(2, equations_count)) has_big_num = true;
            }
            for (int i = 0; i < 2; i++) // make basises 1 & 2 start variable
            {
                rowBasises[i].IsStartVariable = true;
            }
            var temp_basises = new List<Basis>();
            foreach (var basis in rowBasises)
            {
                if (IsSimpleBasis(matrix, basis.Index)) temp_basises.Add(basis);
            }
            var columnBasises = new Basis[temp_basises.Count()];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < temp_basises.Count; j++)
                {
                    if (matrix[i][temp_basises[j].Index] == 1)
                    {
                        columnBasises[i] = temp_basises[j];
                        break;
                    }
                }
            }
            return new Table(matrix, columnBasises, rowBasises, AbstractSimplexMethod.GenerateLastRow(matrix, columnBasises, rowBasises), if_min, has_big_num);
        }
        private Fraction GetSimpleBasisResult(Fraction[][] matrix, int index)
        {
            var find_one = false;
            Fraction value = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                var item = matrix[i][index];
                if (item == 0) continue;
                else if (item == 1 && !find_one)
                {
                    find_one = true;
                    value = matrix[i][0];
                }
                else return 0;
            }
            return value;
        }
        private bool IsSimpleBasis(Fraction[][] matrix, int index) => GetSimpleBasisResult(matrix, index) != 0;
        private Fraction[][] FormMatrix(Fraction[,] orig, LastColValue[] cols)
        {
            var equations_count = orig.GetLength(0);
            var matrix = new Fraction[equations_count][];
            var row_length = GetElementsInRow(2, equations_count) + cols.Where(x => !x.IsLess).Count();

            for (int i = 0; i < matrix.Length; i++) matrix[i] = new Fraction[row_length]; // initialize array
            for (int i = 0; i < equations_count; i++) matrix[i][0] = cols[i].Value; // initialize P[0]

            for (int j = 0; j < orig.GetLength(1); j++)
            {
                for (int i = 0; i < equations_count; i++)
                {
                    matrix[i][j + 1] = orig[i, j];
                }
            }
            for (int j = 0; j < cols.Length; j++)
            {
                for (int i = 0; i < equations_count; i++)
                {
                    matrix[i][j + 1 + 2/*count of x*/] = i == j ? (cols[i].IsLess ? 1 : -1) : 0;
                }
            }
            for (int i = 0, counter = 0; i < cols.Length; i++)
            {
                var col = cols[i];
                if (col.IsLess) continue;
                for (int j = 0; j < equations_count; j++)
                {
                    matrix[j][GetElementsInRow(2, equations_count) + counter] = i == j ? 1 : 0;
                }
                counter++;
            }

            return matrix;
        }
        private Fraction GetFracFromString(string num)
        {
            if (num == "" || num == "+") return 1;
            else if (num == "-") return -1;
            else return Fraction.FromDoubleRounded(double.Parse(num));
        }
        private LastColValue GetLastColValue(string value)
        {
            var sign = value.Substring(0, 2);
            var num = value.Substring(2);
            return new LastColValue
            {
                Value = GetFracFromString(num),
                IsLess = sign == "<="
            };
        }
    }
}