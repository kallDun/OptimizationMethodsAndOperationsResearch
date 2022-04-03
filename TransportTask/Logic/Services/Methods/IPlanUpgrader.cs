using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    interface IPlanUpgrader
    {
        bool CanUpgrade(Table table);
        Table Upgrade(Table table);
    }
}