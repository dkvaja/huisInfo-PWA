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
    public class ActionRepository : BaseRepository, IActionRepository
    {
        public ActionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        private IQueryable<DashboardActionApiModel> GetActionsQuery()
        {
            return _dbContext.Actie
                .Include(x => x.GebouwGu).ThenInclude(x => x.KoperHuurderGu)
                .Include(x => x.GebouwGu.KoperHuurderGu.Persoon1Gu)
                .Include(x => x.GebouwGu.KoperHuurderGu.Persoon2Gu)
                .Include(x => x.GebouwGu.KoperHuurderGu.OrganisatieGu)
                .Include(x => x.GebouwGu.KoperHuurderGu.RelatieGu.PersoonGu)
                .OrderBy(x => x.ActieDatum).ThenBy(x => x.ActieStarttijd)
                .Where(x => x.AfgehandeldMedewerkerGuid == null && x.AfgehandeldOp == null)
                .Select(x => new DashboardActionApiModel
                {
                    ActionId = x.Guid,
                    ProjectId = x.WerkGuid,
                    BuildingId = x.GebouwGuid,
                    EmployeeId = x.ActieMedewerkerGuid,
                    Description = x.Omschrijving,
                    DescriptionExtended = x.OmschrijvingUitgebreid,
                    ActionDate = x.ActieDatum,
                    StartTime = x.ActieStarttijd,
                    BuildingNoIntern = x.GebouwGu.BouwnummerIntern,
                    BuildingNoExtern = x.GebouwGu.BouwnummerExtern,
                    BuyerRenterType = x.GebouwGu.KoperHuurderGu.Soort,
                    BuyerRenterP1Name = x.GebouwGu.KoperHuurderGu.Persoon1Gu.Naam,
                    BuyerRenterP2Name = x.GebouwGu.KoperHuurderGu.Persoon2Gu.Naam,
                    BuyerRenterOrganisationName = x.GebouwGu.KoperHuurderGu.OrganisatieGu.Naam,
                    BuyerRenterRelationName = x.GebouwGu.KoperHuurderGu.RelatieGu.PersoonGu.Naam,
                });
        }

        public IEnumerable<DashboardActionApiModel> GetActionsByBuildingId(string buildingId, string userId)
        {
            var query = GetActionsQuery();
            query = query.Where(x => x.BuildingId == buildingId && x.ActionDate.HasValue);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var employeeId = _dbContext.Login.Where(x => x.Guid == userId && !x.Verwijderd).Select(x => x.MedewerkerGuid).SingleOrDefault();
                query = query.Where(x => x.EmployeeId == employeeId);
            }

            return query;
        }

        public IEnumerable<DashboardActionApiModel> GetActionsByProjectId(string projectId, string userId)
        {
            var query = GetActionsQuery();
            query = query.Where(x => x.ProjectId == projectId && x.ActionDate.HasValue);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var employeeId = _dbContext.Login.Where(x => x.Guid == userId && !x.Verwijderd).Select(x => x.MedewerkerGuid).SingleOrDefault();
                query = query.Where(x => x.EmployeeId == employeeId);
            }

            return query;
        }

        public bool AddAction(ActionApiModel action)
        {
            var projectId = _dbContext.Gebouw.Find(action.BuildingId)?.WerkGuid;
            if (!string.IsNullOrEmpty(projectId))
            {
                Actie actie = new Actie
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    WerkGuid = projectId,
                    GebouwGuid = action.BuildingId,
                    ActieMedewerkerGuid = action.EmployeeId,
                    Omschrijving = action.Description,
                    OmschrijvingUitgebreid = action.DescriptionExtended,
                    ActieDatum = action.DateTime.Date,
                    ActieStarttijd = action.DateTime.TimeOfDay
                };
                _dbContext.Actie.Add(actie);

                return true;
            }

            return false;
        }
    }
}
