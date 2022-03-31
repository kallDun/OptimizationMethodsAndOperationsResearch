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
        const int basic_row_count = 6;

        private struct LastColValue
        {
            public int Value;
            public bool IsLess;
        }

        public Table ToTable(string function, string equations)
        {
            var function_parts = function.Split(new string[] { "=", "->" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            bool if_func_min = function_parts[2] is "min";

            var arguments_parts = function_parts[1].Split(new string[] { "x1", "x2" }, StringSplitOptions.None).Select(x => x.Replace(" ", "")).ToArray();
            int x1 = GetIntFromString(arguments_parts[0]), x2 = GetIntFromString(arguments_parts[1]);
            var C = new Fraction[] { x1, x2, 0, 0, 0 };

            var orig_matrix = new int[3, 2];
            var last_column = new LastColValue[3];
            var ps = new List<int[]>();
            var eqs = equations.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 3; i++)
            {
                var values = eqs[i].Split(new string[] { "x1", "x2" }, StringSplitOptions.None).Select(x => x.Replace(" ", "")).ToArray();
                orig_matrix[i, 0] = GetIntFromString(values[0]);
                orig_matrix[i, 1] = GetIntFromString(values[1]);
                last_column[i] = GetLastColValue(values[2]);
            }

            var main_matrix = FormMatrix(orig_matrix, last_column);
            var X0 = new Fraction[main_matrix[0].Length - 1];
            for (int i = 0; i < X0.Length; i++)
            {
                if (i < 2)
                {
                    X0[i] = 0;
                    continue;
                }
                X0[i] = GetSimpleBasisResult(main_matrix, i + 1);
            }

            return FormTableFromData(main_matrix, X0, C, if_func_min);
        }

        private Table FormTableFromData(Fraction[][] matrix, Fraction[] X0, Fraction[] C, bool if_min)
        {
            bool has_big_num = false;
            Basis[] rowBasises = new Basis[matrix[0].Length - 1];
            for (int i = 0; i < rowBasises.Length; i++)
            {
                rowBasises[i] = i + 1 < basic_row_count ? new Basis(i + 1, new SumValue(C[i], 0)) : new Basis(i + 1, new SumValue(0, -1));
                if (i + 1 >= basic_row_count) has_big_num = true;
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

            SumValue[] lastRow = new SumValue[matrix[0].Length];
            lastRow[0] = SimplexMethodCalculator.CalculateFunction(matrix, columnBasises.ToArray(), rowBasises);
            for (int i = 1; i < matrix[0].Length; i++)
            {
                lastRow[i] = SimplexMethodCalculator.CalculateDelta(i, matrix, columnBasises, rowBasises);
            }
            return new Table(matrix, columnBasises, rowBasises, lastRow, if_min, has_big_num);
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
        private Fraction[][] FormMatrix(int[,] orig, LastColValue[] cols)
        {
            var matrix = new Fraction[3][];
            var row_length = basic_row_count + cols.Where(x => !x.IsLess).Count();
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new Fraction[row_length];
            }
            for (int i = 0; i < 3; i++)
            {
                matrix[i][0] = cols[i].Value;
            }
            for (int j = 0; j < orig.GetLength(1); j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    matrix[i][j + 1] = orig[i, j];
                }
            }
            for (int j = 0; j < cols.Length; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    matrix[i][j + 3] = i == j ? (cols[i].IsLess ? 1 : -1) : 0;
                }
            }
            for (int i = 0, counter = 0; i < cols.Length; i++)
            {
                var col = cols[i];
                if (col.IsLess) continue;
                for (int j = 0; j < 3; j++)
                {
                    matrix[j][basic_row_count + counter] = i == j ? 1 : 0;
                }
                counter++;
            }

            return matrix;
        }
        private int GetIntFromString(string num)
        {
            if (num == "" || num == "+") return 1;
            else if (num == "-") return -1;
            else return int.Parse(num);
        }
        private LastColValue GetLastColValue(string value)
        {
            var sign = value.Substring(0, 2);
            var num = value.Substring(2);
            return new LastColValue
            {
                Value = GetIntFromString(num),
                IsLess = sign == "<="
            };
        }
    }
}