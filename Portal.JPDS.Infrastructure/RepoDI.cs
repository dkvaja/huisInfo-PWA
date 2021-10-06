using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Infrastructure.Persistence;
using Portal.JPDS.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Portal.JPDS.Infrastructure.Services;

namespace Portal.JPDS.Infrastructure
{
    public static class RepoDI
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(360)));

            services.AddDbContext<AppDbCentralContext>(o => o.UseSqlServer(configuration.GetConnectionString("CentralDatabaseConnection"),
               sqlServerOptions => sqlServerOptions.CommandTimeout(360)));

            //I think only RepoSupervisor needed to register... need to confirm with Vikas, why he wanted to register them here---->Abhishek
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
            //services.AddScoped<ILoginRepository, LoginRepository>();
            //services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IRepoSupervisor, RepoSupervisor>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPdfHelperService, PdfHelperService>();
            services.AddScoped<IDigitalSigningService, DigitalSigningService>();
            services.AddScoped<IReportingService, ReportingService>();
            return services;
        }
    }
}
