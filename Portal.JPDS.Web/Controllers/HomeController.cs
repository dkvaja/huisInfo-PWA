using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Infrastructure.Persistence.Repositories;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        public HomeController(
            IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IEmailService emailService,
            IMimeMappingService mimeMappingService,
            ILogger<HomeController> logger,
            IHostEnvironment hostEnvironment
            ) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet("GetUserObjects/{app}")]
        public IActionResult GetUserObjects(Apps app)
        {
            var userId = User.Identity.Name;

            var userObjects = _repoSupervisor.UserObjects.GetUserObjectsForApp(userId, app);

            return Ok(userObjects);
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("ProjectLogo/{id}")]
        public async Task<IActionResult> ProjectLogo(Guid id)
        {
            var filePath = _repoSupervisor.Project.GetProjectLogoPath(id.ToUpperString());
            if (filePath == null || !System.IO.File.Exists(filePath))
                filePath = _repoSupervisor.Config.GetWebPortalLogo();

            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("ProjectBackground/{id}")]
        public async Task<IActionResult> ProjectBackground(Guid id)
        {
            var filePath = _repoSupervisor.Project.GetProjectBackgroundPath(id.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetDashboardCount")]
        public IActionResult GetDashboardCount(Apps app, Guid buildingId, Guid projectId)
        {
            switch (app)
            {
                case Apps.BuyerGuide:
                    int messageCount, systemMessageCount;
                    var userId = User.Identity.Name;
                    var user = _repoSupervisor.Logins.GetLoginById(userId);

                    if (user.Type == (int)AccountType.Buyer)
                    {
                        messageCount = _repoSupervisor.Chats.GetCountOfChatsWithNewMessagesByBuilding(userId, buildingId.ToUpperString());
                        systemMessageCount = _repoSupervisor.Chats.GetCountOfImportantMessagesByBuilding(userId, buildingId.ToUpperString());

                        int selectedOptionsCount = _repoSupervisor.BuildingOptions.GetCountOfItemsInShoppingCart(buildingId.ToUpperString());
                        int quotationsCount = _repoSupervisor.BuildingOptions.GetQuotationsCountForBuilding(buildingId.ToUpperString());

                        return Ok(new { messageCount, systemMessageCount, selectedOptionsCount, quotationsCount });
                    }
                    else
                    {
                        var messageCountPerBuilding = _repoSupervisor.Chats.GetCountOfNewMessagesPerBuilding(userId);
                        var savedMessagesCountPerBuilding = _repoSupervisor.Chats.GetCountOfImportantMessagesPerBuilding(userId);

                        return Ok(new { messageCountPerBuilding, savedMessagesCountPerBuilding });
                    }
                default:
                    return Ok(new { notifications = 0 });//To be implemented in future for different apps based on different dashboard count needs.
            }
        }

        [HttpGet("GetActionsByBuildingId/{buildingId}")]
        public IActionResult GetActionsByBuildingId(Guid buildingId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Actions.GetActionsByBuildingId(buildingId.ToUpperString(), userId));
        }

        [HttpGet("GetActionsByProjectId/{projectId}")]
        public IActionResult GetActionsByProjectId(Guid projectId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Actions.GetActionsByProjectId(projectId.ToUpperString(), userId));
        }

        [HttpGet("GetPlanningsByBuildingId/{buildingId}")]
        public IActionResult GetPlanningsByBuildingId(Guid buildingId)
        {
            var result = _repoSupervisor.Plannings.GetPlanningsByBuildingId(buildingId.ToUpperString()).ToList();

            var dossiers = _repoSupervisor.Dossiers.GetDossiersListByBuildingId(buildingId.ToUpperString(), true)?
                    .Select(x => new PlanningApiModel { DossierId = x.Id, Date = x.Deadline, Description = x.Name, IsDossierExternal = x.IsExternal });
            if (dossiers != null)
                result.AddRange(dossiers);

            return Ok(result.OrderBy(x => x.Date).ThenBy(x => x.StartTime));
        }

        [HttpGet("GetPlanningsByProjectId/{projectId}")]
        public IActionResult GetPlanningsByProjectId(Guid projectId)
        {
            var result = _repoSupervisor.Plannings.GetPlanningsByProjectId(projectId.ToUpperString()).ToList();
            var dossiers = _repoSupervisor.Dossiers.GetAllDossiersByProjectId(projectId.ToUpperString(), true)?
                    .Select(x => new PlanningApiModel { DossierId = x.Id, Date = x.Deadline, Description = x.Name, IsDossierExternal = x.IsExternal });
            if (dossiers != null)
                result.AddRange(dossiers);

            return Ok(result.OrderBy(x => x.Date).ThenBy(x => x.StartTime));
        }

        [HttpGet("GetNewsByProjectId/{projectId}")]
        public IActionResult GetNewsByProjectId(Guid projectId)
        {
            return Ok(_repoSupervisor.News.GetNewsByProjectId(projectId.ToUpperString()).OrderByDescending(x => x.Date));
        }

        [HttpGet("GetNewsImage/{newsId}")]
        public async Task<IActionResult> GetNewsImage(Guid newsId)
        {
            var filePath = _repoSupervisor.News.GetNewsImagePath(newsId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetBuyerInfo/{buyerRenterId}")]
        public IActionResult GetBuyerInfo(Guid buyerRenterId)
        {
            return Ok(_repoSupervisor.UserObjects.GetBuyerInfo(buyerRenterId.ToUpperString()));
        }

        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            return Ok(_repoSupervisor.Employees.GetEmployees());
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddNewAction")]
        public IActionResult AddNewAction([FromBody] ActionApiModel model)
        {
            if (model == null) return BadRequest("Model is Empty.");

            var userId = User.Identity.Name;
            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(model.BuildingId);
            var isUserBuyerGuide = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId, model.BuildingId).Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide);
            if (isUserBuyerGuide)
            {
                var result = _repoSupervisor.Actions.AddAction(model);
                _repoSupervisor.Complete();
                return Ok(result);
            }
            return BadRequest("Can not add new action");
        }

        [HttpGet("GetBuildingDocuments/{buildingId}")]
        public IActionResult GetBuildingDocuments(Guid buildingId)
        {
            var buildingIdStr = buildingId.ToUpperString();
            var attachmentsByBuilding = _repoSupervisor.Attachments.GetAttachmentsByBuildingId(buildingIdStr);
            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingIdStr);
            var attachmentsByProject = _repoSupervisor.Attachments.GetCommonAttachmentsForProject(projectId);
            var dossierAttachmentsByBuilding = _repoSupervisor.Attachments.GetDossierAttachmentsForBuilding(buildingIdStr);
            var attachments = attachmentsByBuilding.Union(attachmentsByProject).Union(dossierAttachmentsByBuilding).Distinct();

            var results = AttachmentWithHeaderApiModel.GroupByHeader(attachments);
            results = results.Select(
                x => new AttachmentGroupByHeaderApiModel()
                {
                    Header = x.Header,
                    HeaderId = x.HeaderId,
                    Attachments = x.Attachments.OrderByDescending(x => x.DateTime)
                }
                ).OrderBy(x => x.Header);

            return Ok(results);
        }

        [AllowAnonymous]
        [HttpGet("GetAttachment/{attachmentId}")]
        public async Task<IActionResult> GetAttachment(Guid attachmentId)
        {
            var filePath = _repoSupervisor.Attachments.GetAttachmentLocation(attachmentId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [AllowAnonymous]
        [HttpGet("GetAttachmentThumbnail/{attachmentId}")]
        public async Task<IActionResult> GetAttachmentThumbnail(Guid attachmentId, int thumbnailSize)
        {
            if (!attachmentId.Equals(Guid.Empty))
            {
                return await GetFileThumbnail(attachmentId.ToUpperString(), thumbnailSize).ConfigureAwait(false);
            }
            return BadRequest("Attachment id is empty");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]//10MB
        [HttpPost("UploadDocument/{buildingId}")]
        public IActionResult UploadDocument(Guid buildingId, IFormFile file)
        {
            if (file == null)
                return BadRequest("File is null");
            if (file.Length == 0)
                return BadRequest("File is empty");
            var userId = User.Identity.Name;
            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingId.ToUpperString());
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId, buildingId.ToUpperString()).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => (x.RoleName == LoginRoles.BuyersGuide || x.RoleName == LoginRoles.BuyerOrRenter)))
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileBytes = binaryReader.ReadBytes((int)file.Length);
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(buildingId.ToUpperString());
                    var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var filePath = _repoSupervisor.Attachments.AddAttachmentForBuilding(
                        buildingId.ToUpperString(),
                        _appSettings.AttachmentHeaders.DocumentUpload,
                        originalFileName,
                        uploadLocation
                        );

                    if (!Directory.Exists(uploadLocation))
                    {
                        Directory.CreateDirectory(uploadLocation);
                    }
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    _repoSupervisor.Complete();

                    var userName = User.GetClaim("FullName");

                    string message = string.Format(
                        CultureInfo.InvariantCulture,
                        "“{0}” heeft “{1}” geüpload. Deze kunt u terugvinden in het menu Documenten.",
                        userName,
                        originalFileName
                        );
                    _repoSupervisor.Chats.SendMessageToDefaultChat(buildingId.ToUpperString(), message);
                }

                return Ok(true);
            }
            return BadRequest("Can not upload document");
        }

        [HttpGet("GetFAQ/{projectId}")]
        public IActionResult GetFAQ(Guid projectId)
        {
            return Ok(_repoSupervisor.FAQs.GetFaqsByProjectId(projectId.ToUpperString()));
        }

        [HttpGet("GetBuildingInfo/{buildingId}")]
        public IActionResult GetBuildingInfo(Guid buildingId)
        {
            return Ok(_repoSupervisor.UserObjects.GetBuildingInfo(buildingId.ToUpperString()));
        }

        [HttpGet("GetProjectInfo/{projectId}")]
        public IActionResult GetProject(Guid projectId)
        {
            return Ok(_repoSupervisor.UserObjects.GetProjectInfo(projectId.ToUpperString()));
        }

        [HttpGet("GetRelationsByProject/{projectId}")]
        public IActionResult GetRelationsByProject(Guid projectId)
        {
            return Ok(_repoSupervisor.UserObjects.GetProjectRelations(projectId.ToUpperString()));
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("GetPersonPhoto/{personId}")]
        public async Task<IActionResult> GetPersonPhoto(Guid personId)
        {
            var filePath = _repoSupervisor.Persons.GetPersonPhotoLocation(personId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetInvolvedParties/{projectId}")]
        public IActionResult GetInvolvedParties(Guid projectId)
        {
            return Ok(_repoSupervisor.UserObjects.GetInvolvedPartiesByProject(projectId.ToUpperString()).OrderBy(x => x.ProductCode).ThenBy(x => x.ProductDescription));
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("SortAttachments")]
        public IActionResult SortAttachments([FromBody] List<Guid> lstIds)
        {

            _repoSupervisor.Attachments.SortAttachments(lstIds);
            _repoSupervisor.Complete();
            return Ok(lstIds);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddFeedback")]
        public IActionResult AddFeedback([FromBody] string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return BadRequest("Comment is empty.");

            var userId = User.Identity.Name;
            var userName = User.GetClaim("FullName");
            _repoSupervisor.UserObjects.AddFeedback(userId, comment);
            _repoSupervisor.Complete();

            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                EmailTemplateModel emailTemplate = new EmailTemplateModel
                {
                    Subject = "Feedback from [username]",
                    Template = string.Empty,
                    TemplateHtml = @"Feedback from
                                    <br />
                                    <b>Site:</b> [siteurl]
                                    <br />
                                    <b>User:</b> [username] ([userid])
                                    <br />
                                    <b>Comment:</b>
                                    <pre>[comment]<pre>"
                };

                Dictionary<string, string> tokens = new Dictionary<string, string>();
                tokens["[userid]"] = userId;
                tokens["[username]"] = userName;
                tokens["[siteurl]"] = _appSettings.SiteUrl;
                tokens["[comment]"] = comment;

                emailTemplate.UpdateTokenValues(tokens);

                var email = "support@jpds.nl";
                if (_appSettings.FeedbackEmail.IsValidEmail())
                    email = _appSettings.FeedbackEmail;
                mail.To.Add(email);
                mail.Subject = emailTemplate.Subject;
                mail.Body = emailTemplate.TemplateHtml;
                mail.IsBodyHtml = true;

                try
                {
                    var task = _emailService.SendEmailAsync(mail);
                    task.Wait();

                    if (task.Exception != null)
                        return BadRequest(task.Exception.Message);

                }
                catch (Exception ex)
                {
                    return BadRequest("Error: " + ex.Message);
                }
            }

            return Ok(new { success = true });
        }

        //[AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[HttpGet("TestNotifications")]
        //public IActionResult TestNotifications()
        //{
        //    return Ok(_repoSupervisor.Notifications.GetEmailNotifications());
        //}

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("TestEmail")]
        public IActionResult TestEmail(string email)
        {
            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                mail.To.Add(email);
                mail.Subject = "Test subject for test Email";
                mail.Body = "Test email body for test Email";
                mail.IsBodyHtml = true;

                try
                {
                    var task = _emailService.SendEmailAsync(mail);
                    task.Wait();

                    if (task.Exception != null)
                    {
                        _logger.LogError("TestEmail: {0}\n{1}", task.Exception.Message, task.Exception.StackTrace ?? string.Empty);
                        return BadRequest(task.Exception.Message);
                    }
                    return Ok(task.Status);
                }
                catch (Exception ex)
                {
                    _logger.LogError("TestEmail: {0}\n{1}", ex.Message, ex.StackTrace ?? string.Empty);
                    return BadRequest("Error: " + ex.Message);
                }
            }
        }

        [HttpGet("GetProductServices")]
        public IActionResult GetProductServices()
        {
            return Ok(_repoSupervisor.UserObjects.GetProductServices());
        }
    }
}