using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<EmployeeApiModel> GetEmployees()
        {
            return _dbContext.ViewPortalMedewerker.OrderBy(x => x.PersoonVolledigeNaam).Select(x => new EmployeeApiModel(x));
        }
    }
}
