using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services
{
    class GeneratePotentialService
    {
        public Cell[][] GeneratePotentials(Cell[][] cells)
        {
            Coord[] matrix_based_array = GenerateArray(cells, cells.Length, cells[0].Length);
            int[] results = GenerateAndSolveMatrixFromArray(matrix_based_array, cells, cells.Length, cells[0].Length);
            return GenerateNewCellsWithPotential(results, cells, cells.Length, cells[0].Length);
        }
        private Cell[][] GenerateNewCellsWithPotential(int[] results, Cell[][] cells, int cols, int rows)
        {
            Cell[][] new_cells = cells.Select(x => x.Clone() as Cell[]).ToArray().Clone() as Cell[][];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {                    
                    if (new_cells[i][j].IsProductsNone is true)
                    {
                        new_cells[i][j].IsPotentialNone = false;
                        new_cells[i][j].Potential = results[cols + j] - results[i] - new_cells[i][j].Coefficient;
                    }
                    else
                    {
                        new_cells[i][j].IsPotentialNone = true;
                    }
                }
            }
            return new_cells;
        }
        private Coord[] GenerateArray(Cell[][] cells, int cols, int rows)
        {
            var matrix_based_array = new Coord[cols + rows - 1];
            for (int i = 0, counter = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (cells[i][j].IsProductsNone is false)
                    {
                        matrix_based_array[counter++] = new(i, j);
                    }
                }
            }
            return matrix_based_array;
        }
        private int[] GenerateAndSolveMatrixFromArray(Coord[] array, Cell[][] cells, int cols, int rows)
        {
            var A = new double[cols + rows, cols + rows];
            var B = new double[cols + rows];

            A[0, 0] = 1;
            B[0] = 0;

            for (int i = 1; i < array.Length + 1; i++)
            {
                Coord item = array[i - 1];
                A[i, item.I] = -1;
                A[i, item.J + cols] = 1;
                B[i] = cells[item.I][item.J].Coefficient;
            }

            var mx = DenseMatrix.OfArray(A);
            var results = mx.Solve(DenseVector.OfArray(B));
            return results.Select(x => (int)Math.Round(x)).ToArray();
        }
    }
}