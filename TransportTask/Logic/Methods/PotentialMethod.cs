using System.Collections.Generic;
using System.Linq;
using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    class PotentialMethod : IPlanUpgrader
    {
        public bool CanUpgrade(Table table)
        {
            return table.Cells.Select(x => x.Where(x => !x.IsPotentialNone && x.Potential > 0).Count()).Sum() != 0;
        }
        
        public (Table, Table, Table) Upgrade(Table table)
        {
            var cycle_table = GetCycleTable(table);
            var calc_table = CalcTableByCycle(cycle_table);
            var potential_table = GetPotentialTable(calc_table);
            return (cycle_table, calc_table, potential_table);
        }

        private Table GetPotentialTable(Table empty_table)
        {
            GeneratePotentialService potentialService = new();
            var cells = potentialService.GeneratePotentials(empty_table.Cells);
            return new Table
            {
                Cells = cells
            };
        }
        private Table CalcTableByCycle(Table cycle_table)
        {
            var cells = cycle_table.Cells.Select(x => x.Clone() as Cell[]).ToArray().Clone() as Cell[][];
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[i].Length; j++)
                {
                    cells[i][j].IsPotentialNone = true;
                    cells[i][j].Potential = 0;
                }
            }

            var vertex_selected = cycle_table.Cycle.SelectedMinimumVertex;
            var products = cells[vertex_selected.Point.I][vertex_selected.Point.J].Products;
            foreach (var item in cycle_table.Cycle.Vertices)
            {
                if (item.NegativeSign)
                {
                    cells[item.Point.I][item.Point.J].Products -= products;
                }
                else
                {
                    cells[item.Point.I][item.Point.J].Products += products;
                }
            }
            cells[vertex_selected.Point.I][vertex_selected.Point.J].IsProductsNone = true;
            cells[cycle_table.Cycle.StartVertex.I][cycle_table.Cycle.StartVertex.J].IsProductsNone = false;

            return new Table
            {
                Cells = cells
            };
        }
        private Table GetCycleTable(Table table)
        {
            var cells = table.Cells.Select(x => x.Clone() as Cell[]).ToArray().Clone() as Cell[][];

            var max_potential = cells.SelectMany(x => x).Max(x => x.Potential);
            Cycle cycle = new();
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (!cells[i][j].IsPotentialNone && cells[i][j].Potential == max_potential)
                    {
                        cycle.StartVertex = new(i, j);
                        goto Break;
                    }
                }
            }
        Break:
            var points = GetCycleVerticesRecursive(cells, cycle.StartVertex, new List<Coord>() { }, Side.None).ToList();
            var vertices = new List<Vertex>();
            for (int i = 0; i < points.Count; i++)
            {
                vertices.Add(new Vertex
                {
                    Point = points[i],
                    NegativeSign = i % 2 != 0
                });
            }
            var min_value = int.MaxValue;
            Vertex min_vertex = new();
            foreach (var item in vertices)
            {
                if (!item.NegativeSign) continue;
                var value = cells[item.Point.I][item.Point.J].Products;
                if (value < min_value)
                {
                    min_vertex = item;
                    min_value = value;
                }
            }
            cycle.IsVisible = true;
            cycle.Vertices = vertices;
            cycle.SelectedMinimumVertex = min_vertex;
            return new Table
            {
                Cells = cells,
                Cycle = cycle
            };
        }
        private IEnumerable<Coord> GetCycleVerticesRecursive(Cell[][] cells, Coord cell_now, IEnumerable<Coord> verteces, Side side)
        {
            verteces = verteces.Concat(new List<Coord> { cell_now });
            List<IEnumerable<Coord>> list = new();
            if (side != Side.Vertical)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    Coord cell = new(i, cell_now.J);

                    if (verteces.First().CompareTo(cell) == 0 && side != Side.None) return verteces;
                    if (cell_now.I == i ||
                        cells[cell.I][cell.J].IsProductsNone ||
                        verteces.Contains(cell) || 
                        verteces.Count(x => x.J == cell.J) == 2) continue;                    
                    
                    list.Add(GetCycleVerticesRecursive(cells, cell, verteces, Side.Vertical));
                }
            }
            if (side != Side.Horizontal)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    Coord cell = new(cell_now.I, j);                    

                    if (verteces.First().CompareTo(cell) == 0 && side != Side.None) return verteces;
                    if (cell_now.J == j ||
                        cells[cell.I][cell.J].IsProductsNone ||
                        verteces.Contains(cell) ||
                        verteces.Count(x => x.I == cell.I) == 2) continue;

                    list.Add(GetCycleVerticesRecursive(cells, cell, verteces, Side.Horizontal));
                }
            }
            list = list.Where(x => x != null).ToList();
            if (list.Count > 0) return list.First();
            return null;
        }
        private enum Side
        {
            None, Horizontal, Vertical
        }
    }
}