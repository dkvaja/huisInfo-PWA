using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Repositories
{
    /// <summary>
    /// Author : Abhishek Saini
    /// This is interface which should be implemented in outer layer.
    /// </summary>
    public interface IPlanningRepository
    {
        IEnumerable<PlanningApiModel> GetPlanningsByProjectId(string projectId);
        IEnumerable<PlanningApiModel> GetPlanningsByBuildingId(string buildingId);
    }
}
