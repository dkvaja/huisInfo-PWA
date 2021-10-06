using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.JPDS.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    if (args.Length > 0 && File.Exists(args.First()))
                    {
                        configuration = new ConfigurationBuilder()
                            .AddJsonFile(args.First())
                            .Build();
                    }

                    services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddRepositories(configuration);
                    services.AddHostedService<DailyEmailNotification>();
                    services.AddHostedService<DailyEmailNotificationConstructionQuality>();
                });
    }
}
