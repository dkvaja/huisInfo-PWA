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
    public class OrganisationRepository : BaseRepository, IOrganisationRepository
    {
        public OrganisationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string GetOrganisationLogoPath(string organisationId)
        {
            return _dbContext.Organisatie.Find(organisationId)?.Logo;
        }

        public IEnumerable<OrganisationApiModel> GetOrganisationsByProject(string projectId)
        {
            var subQuery = _dbContext.Betrokkene.Where(x => x.WerkGuid == projectId && x.OrganisatieGuid != null).Select(x => x.OrganisatieGuid).Distinct();
            var lstOrgs = _dbContext.Organisatie.Include(x => x.Relatie).Where(x => subQuery.Contains(x.Guid)).Select(x => new OrganisationApiModel(x));

            return lstOrgs;
        }

        public IEnumerable<OrganisationApiModel> GetOrganisations(string projectId, string productServiceId, string methodName, string searchText, int maxCount)
        {
            var result = _dbContext.Organisatie.AsQueryable();
            var maxRecords = maxCount > 0 ? maxCount : 20;
            if (productServiceId != null)
            {
                var subQuery = _dbContext.OrganisatieProducts.Where(x => x.ProductDienstGuid == productServiceId).Select(x => x.OrganisatieGuid).Distinct();
                result = result.Where(x => subQuery.Contains(x.Guid));
            }
            OrganisationSearchMethod organisationMethod = methodName != null ? (OrganisationSearchMethod)Enum.Parse(typeof(OrganisationSearchMethod), methodName) : OrganisationSearchMethod.InvolvedInTheProject;
            switch (organisationMethod)
            {
                case OrganisationSearchMethod.Own:
                    result = result.Where(x => x.EigenOrganisatie == true);
                    break;
                case OrganisationSearchMethod.All:
                    result = result.OrderBy(x => x.NaamOnderdeel);
                    break;
                default:
                    if (projectId != null)
                    {
                        var subQuery = _dbContext.Betrokkene.Where(x => x.WerkGuid == projectId && x.OrganisatieGuid != null).Select(x => x.OrganisatieGuid).Distinct();
                        result = result.Where(x => subQuery.Contains(x.Guid));
                    }
                    break;
            }
            if (searchText != null)
            {
                result = result.Where(x => x.NaamOnderdeel.Contains(searchText));
            }
            return result.OrderBy(x => x.NaamOnderdeel).Take(maxRecords).Select(x => new OrganisationApiModel(x));
        }

        public IEnumerable<OrganisationEmployeeApiModel> GetRelationsbyOrganisationId(string organisationId)
        {
            return _dbContext.Relatie.Where(x => x.OrganisatieGuid == organisationId)
                .Select(x => new OrganisationEmployeeApiModel(x));
        }
    }
}
