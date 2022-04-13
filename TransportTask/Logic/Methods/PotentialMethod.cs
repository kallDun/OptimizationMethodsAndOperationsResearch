using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
                Cells = cells, Cycle = new Cycle()
            };
        }

        private Table CalcTableByCycle(Table cycle_table)
        {
            throw new NotImplementedException("Need to write this shit");
        }

        private Table GetCycleTable(Table table)
        {
            var max_potential = table.Cells.SelectMany(x => x).Max(x => x.Potential);
            Cycle cycle = new();
            for (int i = 0; i < table.Cells.Length; i++)
            {
                for (int j = 0; j < table.Cells[0].Length; j++)
                {
                    if (!table.Cells[i][j].IsPotentialNone && table.Cells[i][j].Potential == max_potential)
                    {
                        cycle.StartVertex = new(i, j);
                        goto Break;
                    }
                }
            }
        Break:
            var points = GetCycleVerticesRecursive(table.Cells, cycle.StartVertex);
            var vertices = new List<Vertex>();
            for (int i = 0; i < points.Count; i++)
            {
                vertices.Add(new Vertex
                {
                    Point = points[i],
                    NegativeSign = i % 2 == 0
                });
            }
            var min_value = int.MaxValue;
            Vertex min_vertex;
            foreach (var item in vertices)
            {
                if (!item.NegativeSign) continue;
                var value = table.Cells[item.Point.I][item.Point.J].Potential;
                if (value < min_value)
                {
                    min_vertex = item;
                    min_value = value;
                }
            }
            return new Table
            {
                Cells = table.Cells,
                Cycle = cycle
            };
        }

        private List<Coord> GetCycleVerticesRecursive(Cell[][] cells, Coord start)
        {
            throw new NotImplementedException("Need to write this shit");
        }
    }
}