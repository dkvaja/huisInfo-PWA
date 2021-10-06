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
    public interface IOrganisationRepository
    {
        IEnumerable<OrganisationApiModel> GetOrganisationsByProject(string projectId);
        string GetOrganisationLogoPath(string organisationId);
        IEnumerable<OrganisationApiModel> GetOrganisations(string projectId, string productServiceId, string methodName, string searchText, int maxCount);
        IEnumerable<OrganisationEmployeeApiModel> GetRelationsbyOrganisationId(string organisationId);
    }
}
