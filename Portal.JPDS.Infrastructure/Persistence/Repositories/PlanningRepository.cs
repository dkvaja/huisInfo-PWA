using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using Microsoft.Data.SqlClient;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class PlanningRepository : BaseRepository, IPlanningRepository
    {
        public PlanningRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<PlanningApiModel> GetPlanningsByBuildingId(string buildingId)
        {
            return _dbContext.ViewPortalDashboardPlanningKoperHuurder.Where(x => x.GebouwGuid == buildingId && x.ActieDatum.HasValue).Select(x => new PlanningApiModel(x));
        }

        public IEnumerable<PlanningApiModel> GetPlanningsByProjectId(string projectId)
        {
            return _dbContext.ViewPortalDashboardPlanningKopersbegeleider.Where(x => x.WerkGuid == projectId && x.ActieDatum.HasValue).Select(x => new PlanningApiModel(x));
        }
    }
}
