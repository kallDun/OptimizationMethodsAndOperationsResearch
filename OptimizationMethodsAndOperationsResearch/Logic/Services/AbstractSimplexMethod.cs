using Fractions;
using OptimizationMethodsAndOperationsResearch.Logic.Models;
using System.Collections.Generic;

namespace OptimizationMethodsAndOperationsResearch.Logic.Services
{
    abstract class AbstractSimplexMethod
    {
        public bool HasSolution { get; protected set; } = true;
        public abstract bool IsOptimized(Table table);
        public abstract (Table, VisualDataModel) GetNextTable(Table table);
        public Dictionary<int, Fraction> GetResults(Table table)
        {
            Dictionary<int, Fraction> results = new Dictionary<int, Fraction>();
            for (int i = 0; i < table.RowBasises.Length; i++)
            {
                results.Add(i + 1, 0);
            }
            for (int i = 0; i < table.RowBasises.Length; i++)
            {
                for (int j = 0; j < table.ColumnBasises.Length; j++)
                {
                    if (table.ColumnBasises[j].Index == i + 1)
                    {
                        results[i + 1] = table.Matrix[j][0];
                    }
                }
            }
            return results;
        }
    }
}