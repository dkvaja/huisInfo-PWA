using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Portal.JPDS.Domain.CentralDBEntities;

namespace Portal.JPDS.Infrastructure.Persistence
{
    public class RepoSupervisor : IRepoSupervisor
    {
        private readonly AppDbContext _dbContext;
        private readonly AppDbCentralContext _dbContextCentral;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _appSettingConfig;
        public IConfigRepository Config { get; private set; }
        public ILoginRepository Logins { get; private set; }
        public IProjectRepository Project { get; private set; }
        public IUserObjectRepository UserObjects { get; private set; }
        public IActionRepository Actions { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IAttachmentsRepository Attachments { get; private set; }
        public IChatRepository Chats { get; private set; }
        public IBuildingOptionsRepository BuildingOptions { get; private set; }
        public IFAQRepository FAQs { get; private set; }
        public IPersonRepository Persons { get; private set; }
        public IPlanningRepository Plannings { get; private set; }
        public INewsRepository News { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IRepairRequestRepository RepairRequest { get; private set; }
        public ISurveyRepository Survey { get; private set; }
        public IOrganisationRepository Organisations { get; private set; }
        public IDossierRepository Dossiers { get; private set; }

        public RepoSupervisor(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor,IConfiguration appSettingConfig, AppDbCentralContext dbContextCentral)
        {
            _dbContext = dbContext;
            _dbContextCentral = dbContextCentral;
            _httpContextAccessor = httpContextAccessor;
            _appSettingConfig = appSettingConfig;
            Actions = new ActionRepository(_dbContext);
            Attachments = new AttachmentsRepository(_dbContext, _httpContextAccessor);
            BuildingOptions = new BuildingOptionsRepository(_dbContext);
            Chats = new ChatRepository(_dbContext);
            Config = new ConfigRepository(_dbContext);
            Employees = new EmployeeRepository(_dbContext);
            FAQs = new FAQRepository(_dbContext);
            Logins = new LoginRepository(_dbContext, _dbContextCentral);
            News = new NewsRepository(_dbContext);
            Notifications = new NotificationRepository(_dbContext);
            Persons = new PersonRepository(_dbContext);
            Plannings = new PlanningRepository(_dbContext);
            Project = new ProjectRepository(_dbContext);
            UserObjects = new UserObjectRepository(_dbContext);
            RepairRequest = new RepairRequestRepository(_dbContext, _httpContextAccessor, _appSettingConfig);
            Survey = new SurveyRepository(_dbContext);
            Organisations = new OrganisationRepository(_dbContext);
            Dossiers = new DossierRepository(_dbContext, _httpContextAccessor, _appSettingConfig);
        }

        public int Complete(string username)
        {
            AddTimestamps(username);
            return _dbContext.SaveChanges();
        }

        private void AddTimestamps(string username)
        {
            var entities = _dbContext.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            username = username ?? "System";//Default Name

            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                var name = _dbContext.ViewLogins.Where(x => x.Guid == currentUserId).Select(x => x.Naam).SingleOrDefault();
                username = name ?? username;
            }

            username = username.TrimToMaxLength(40);
            foreach (var entity in entities)
            {
                var currentDateTime = DateTime.Now;
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).IngevoerdOp = currentDateTime;
                    ((BaseEntity)entity.Entity).IngevoerdDoor = username;
                }

                ((BaseEntity)entity.Entity).GewijzigdOp = currentDateTime;
                ((BaseEntity)entity.Entity).GewijzigdDoor = username;
            }
        }

        public int CentralComplete()
        {
            AddCentralTimeStamps();
            return _dbContextCentral.SaveChanges();
        }

        private void AddCentralTimeStamps()
        {
            var entities = _dbContextCentral.ChangeTracker.Entries().Where(x => x.Entity is CentralBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var centralLoginId = string.Empty;
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                centralLoginId = _dbContext.Login.Where(x => x.Guid == currentUserId && !x.Verwijderd)?.Select(x => x.CentralLoginGuid).SingleOrDefault();
            }

            foreach (var entity in entities)
            {
                var currentDateTime = DateTime.Now;
                if (entity.State == EntityState.Added)
                {
                    ((CentralBaseEntity)entity.Entity).CreatedOn = currentDateTime;
                    ((CentralBaseEntity)entity.Entity).CreatedBy = !string.IsNullOrWhiteSpace(centralLoginId) ? Guid.Parse(centralLoginId) : null;
                }

                ((CentralBaseEntity)entity.Entity).ModifiedOn  = currentDateTime;
                ((CentralBaseEntity)entity.Entity).ModifiedBy = !string.IsNullOrWhiteSpace(centralLoginId) ? Guid.Parse(centralLoginId) : null;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _dbContextCentral.Dispose();
        }
    }
}
