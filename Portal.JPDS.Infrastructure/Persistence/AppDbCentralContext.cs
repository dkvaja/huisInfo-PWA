using Portal.JPDS.Domain.CentralDBEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Portal.JPDS.Infrastructure.Persistence
{
    public class AppDbCentralContext : DbContext
    {

        public AppDbCentralContext(DbContextOptions<AppDbCentralContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<CentralLogin> CentralLogins { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
