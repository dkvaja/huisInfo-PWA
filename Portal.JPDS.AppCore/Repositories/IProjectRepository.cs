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
    public interface IProjectRepository
    {
        string GetProjectLogoPath(string projectId);
        string GetProjectBackgroundPath(string projectId);
        List<string> GetResponsibleRelationsEmails(string projectId);
    }
}
