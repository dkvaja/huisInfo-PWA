using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string GetProjectBackgroundPath(string projectId)
        {
            return _dbContext.Werk.Find(projectId).AchtergrondWebPortal;
        }

        public string GetProjectLogoPath(string projectId)
        {
            return _dbContext.Werk.Find(projectId).LogoWebPortal;
        }

        public List<string> GetResponsibleRelationsEmails(string projectId)
        {
            var result = new List<string>();

            var project = _dbContext.Werk
                .Include(x => x.MeldingVerantwoordelijkeManagementRelatieGu)
                .Include(x => x.MeldingVerantwoordelijkeUitvoeringRelatieGu)
                .FirstOrDefault(x => x.Guid == projectId);

            var email = project.MeldingVerantwoordelijkeManagementRelatieGu?.EmailZakelijk;
            if (!string.IsNullOrWhiteSpace(email))
                result.Add(email);
            
            email = project.MeldingVerantwoordelijkeUitvoeringRelatieGu?.EmailZakelijk;
            if (!string.IsNullOrWhiteSpace(email))
                result.Add(email);

            return result;
        }
    }
}
