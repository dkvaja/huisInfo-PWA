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
    public class FAQRepository : BaseRepository, IFAQRepository
    {
        public FAQRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<FaqGroupedByHeaderApiModel> GetFaqsByProjectId(string projectId)
        {
            return _dbContext.FaqRubriek
                .Where(x => x.FaqVraagAntwoordWerk.Any(x => x.WerkGuid == projectId))
                .OrderBy(x => x.Volgorde)
                .ThenBy(x => x.FaqRubriek1)
                .Select(x => new FaqGroupedByHeaderApiModel
                {
                    Header = x.FaqRubriek1,
                    Faqs = x.FaqVraagAntwoordWerk
                        .Where(x => x.WerkGuid == projectId)
                        .Select(y => new FaqApiModel
                        {
                            Ques = y.Vraag,
                            Ans = y.Antwoord
                        }).OrderBy(x => x.Ques).ToList()
                });
        }
    }
}
