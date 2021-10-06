using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Infrastructure;
using Portal.JPDS.Web.Filters;
using Portal.JPDS.Web.Helpers;
using Portal.JPDS.Web.Services;
using System;
using System.IO;
using System.Text;

namespace Portal.JPDS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.AddScoped<ScriveIpCheckFilter>();
            services.AddCors();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HuisInfo API", Version = "v1" });
            });


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ClientApp/build");
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.FullAccess,
                    policy => policy.RequireClaim(PolicyClaimType.Access, PolicyClaimValue.FullAccess));
                options.AddPolicy(PolicyConstants.ViewOnlyAccess,
                    policy => policy.RequireClaim(PolicyClaimType.Access, PolicyClaimValue.ViewOnly, PolicyClaimValue.FullAccess));
                options.AddPolicy(PolicyConstants.ResetPasswordOnly,
                    policy => policy.RequireClaim(PolicyClaimType.Access, PolicyClaimValue.ResetPasswordOnly));
            });

            var provider = new FileExtensionContentTypeProvider();
            //add new MIME mappings below like ===>>  provider.Mappings.Add(".newExt", "application/newExtensionContentType");
            services.AddSingleton<IMimeMappingService>(new MimeMappingService(provider));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserService, UserService>();
            services.AddRepositories(Configuration);
            services.AddHostedService<TempFileCleanUpService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HuisInfo API V1");
            });


            app.UseRouting();

            
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            if (appSettings.CorsAllowAll)
            {
                app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            }

            //Add User session
            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken) && !context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                else if (string.IsNullOrEmpty(JWToken) && context.Request.Headers.ContainsKey("Authorization")) {
                    var headerValue = context.Request.Headers["Authorization"].ToString() ?? string.Empty;
                    if (headerValue.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                        context.Session.SetString("JWToken", headerValue.Replace("Bearer ", "", StringComparison.InvariantCultureIgnoreCase));
                }
                await next();
            });
            //Add JWToken Authentication service
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //below command will prevent multiple node.exe instances. TO IMPROVE SYSTEM PERFORMANCE WHILE DEBUGGING
                    //DELETE the below command when there is a fix from microsoft for this issue.
                    System.Diagnostics.Process.Start("CMD.exe", "cmd \"/C TASKKILL /IM node.exe /F\"");

                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
