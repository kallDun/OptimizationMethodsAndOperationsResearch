using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    interface IPlanUpgrader
    {
        /// <summary>
        /// In input get table with potentials
        /// </summary>
        /// <param name="table"></param>
        /// <returns>bool value that determines upgrade possibility</returns>
        bool CanUpgrade(Table table);

        /// <summary>
        /// In input get table with potentials
        /// </summary>
        /// <param name="table"></param>
        /// <returns>three tables: 1 table with cycle, 2 table - cycle solve, 3 table with potentials
        /// In input get the table with potentials</returns>
        (Table, Table, Table) Upgrade(Table table);
    }
}