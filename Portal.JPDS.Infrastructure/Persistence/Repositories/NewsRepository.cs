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
    public class NewsRepository : BaseRepository, INewsRepository
    {
        public NewsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<NewsApiModel> GetNewsByProjectId(string projectId)
        {
            return _dbContext.Nieuws.Where(x => x.WerkGuid == projectId && x.Publiceren == true && x.Datum <= DateTime.Now).Select(x => new NewsApiModel(x));
        }

        public string GetNewsImagePath(string newsId)
        {
            return _dbContext.Nieuws.Find(newsId).Afbeelding;
        }
    }
}
