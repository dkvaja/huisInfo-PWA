using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class OrganisationController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        public OrganisationController(IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IMimeMappingService mimeMappingService,
            IHostEnvironment hostEnvironment) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
        }

        [HttpGet("GetOrganisationsByProject/{projectId}")]
        public IActionResult GetOrganisationsByProject(Guid projectId)
        {
            var result = _repoSupervisor.Organisations.GetOrganisationsByProject(projectId.ToUpperString());

            return Ok(result);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 3600)]
        [HttpGet("GetOrganisationLogo/{organisationId}")]
        public async Task<IActionResult> GetOrganisationLogo(Guid organisationId)
        {
            var filePath = _repoSupervisor.Organisations.GetOrganisationLogoPath(organisationId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetOrganisations/{projectId}")]
        public IActionResult GetAllOrganisations(Guid projectId, Guid? productServiceId, string methodName, string searchText, int maxCount)
        {
            var result = _repoSupervisor.Organisations.GetOrganisations(projectId.ToUpperString(), productServiceId?.ToUpperString(), methodName, searchText, maxCount);

            return Ok(result);
        }

        [HttpGet("GetRelationsbyOrganisationId/{organisationId}")]
        public IActionResult GetRelationsbyOrganisationId(Guid organisationId)
        {
            var result = _repoSupervisor.Organisations.GetRelationsbyOrganisationId(organisationId.ToUpperString());

            return Ok(result);
        }
    }
}