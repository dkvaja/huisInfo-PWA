using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using System.Security.Cryptography.X509Certificates;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public PersonRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string GetPersonPhotoLocation(string personId)
        {
            return _dbContext.Persoon.Find(personId).Foto;
        }

        public string GetPersonPhotoLocationByChatParticipantId(string chatParticipantId)
        {
            return _dbContext.ChatDeelnemer.Include(x => x.LoginGu).ThenInclude(x => x.PersoonGu)
                .Where(x => x.Guid == chatParticipantId).SingleOrDefault()?.LoginGu?.PersoonGu?.Foto;
        }
    }
}
