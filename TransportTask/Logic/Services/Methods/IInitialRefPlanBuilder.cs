using TransportTask.Logic.Models;

namespace TransportTask.Logic.Services.Methods
{
    interface IInitialRefPlanBuilder
    {
        bool IsBuilt(PrepTable table);
        PrepTable Build(PrepTable table);
    }
}