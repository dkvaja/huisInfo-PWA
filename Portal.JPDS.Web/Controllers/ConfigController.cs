using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Dtos;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class ConfigController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        public ConfigController(
            IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IMimeMappingService mimeMappingService,
            IHostEnvironment hostEnvironment)
            :
            base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
        }

        [AllowAnonymous]
        [HttpGet("CheckApiStatus")]
        public IActionResult CheckApiStatus()
        {
            return Ok(new
            {
                AppName = "Huisinfo",
                Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                PlayStoreAppVersion = "1.8"
            });
        }

        [AllowAnonymous]
        [HttpGet("manifest.json")]
        public IActionResult GetAppManifest()
        {
            var manifest = new
            {
                name = string.IsNullOrWhiteSpace(_appSettings.AppName) ? "Huisinfo App" : _appSettings.AppName,
                short_name = string.IsNullOrWhiteSpace(_appSettings.AppShortName) ? "Huisinfo" : _appSettings.AppShortName,
                start_url = "/",
                display = "standalone",
                background_color = "#fff",
                theme_color = "#fff",
                scope = "/",
                icons = new object[] {
                    new
                    {
                        src= "/Content/Images/Favicons/android-chrome-192x192.png",
                        sizes="192x192",
                        type="image/png"
                    },
                    new
                    {
                        src="/Content/Images/Favicons/android-chrome-512x512.png",
                        sizes="512x512",
                        type="image/png",
                        purpose="any maskable"
                    }
                }
            };

            return Ok(manifest);
        }

        [AllowAnonymous]
        [HttpGet("GetPageTitle")]
        public IActionResult GetPageTitle()
        {
            var type = User.FindFirstValue(ClaimTypes.Role);
            if (type != null && type == ((int)AccountType.Employee).ToString(CultureInfo.InvariantCulture))
            {
                return Ok(_repoSupervisor.Config.GetBrowserTabNameInternal());
            }

            return Ok(_repoSupervisor.Config.GetBrowserTabNameExternal());
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 3600)]
        [HttpGet("WebLogo")]
        public async Task<IActionResult> WebLogo()
        {
            string filePath = _repoSupervisor.Config.GetWebPortalLogo();
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 3600)]
        [HttpGet("WebBackground")]
        public async Task<IActionResult> WebBackground()
        {
            string filePath = _repoSupervisor.Config.GetWebPortalLoginBackground();
            return await GetFileStream(filePath).ConfigureAwait(false);
        }
    }
}