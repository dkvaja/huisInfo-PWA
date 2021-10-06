using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Portal.JPDS.Web.Controllers
{
    public class DossierController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IMimeMappingService _mimeMappingService;
        public DossierController(IOptions<AppSettings> appSettings, IRepoSupervisor repoSupervisor, IEmailService emailService, IUserService userService, IMimeMappingService mimeMappingService, IHostEnvironment hostEnvironment) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings?.Value;
            _repoSupervisor = repoSupervisor;
            _emailService = emailService;
            _userService = userService;
            _mimeMappingService = mimeMappingService;
        }

        [HttpGet("GetAllDossiersByProjectId/{projectId}")]
        public IActionResult GetAllDossiersByProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty) { return BadRequest("Project Id is empty"); }
            var result = _repoSupervisor.Dossiers.GetAllDossiersByProjectId(projectId.ToUpperString(), false);
            return Ok(result);
        }

        [HttpGet("GetDossiersListByProjectId/{projectId}")]
        public IActionResult GetDossiersListByProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty) { return BadRequest("Project Id is empty"); }
            var result = _repoSupervisor.Dossiers.GetDossiersListByProjectId(projectId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetDossiersListByBuildingId/{buildingId}")]
        public IActionResult GetDossiersListByBuildingId(Guid buildingId)
        {
            if (buildingId == Guid.Empty) { return BadRequest("building Id is empty"); }
            var result = _repoSupervisor.Dossiers.GetDossiersListByBuildingId(buildingId.ToUpperString(), false);
            return Ok(result);
        }

        [HttpGet("GetDossierBuildingInfo/{dossierId}/{buildingId}")]
        public IActionResult GetDossierBuildingInfo(Guid dossierId, Guid buildingId)
        {
            if (dossierId == Guid.Empty || buildingId == Guid.Empty) { return BadRequest("Dossier Id or Building Id is empty"); }

            var result = _repoSupervisor.Dossiers.GetDossierBuildingInfo(dossierId.ToUpperString(), buildingId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetDossierGeneralInfo/{dossierId}")]
        public IActionResult GetDossierGeneralInfo(Guid dossierId)
        {
            if (dossierId == Guid.Empty) { return BadRequest("Dossier Id is empty"); }

            var result = _repoSupervisor.Dossiers.GetDossierGeneralInfo(dossierId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetAvailableUsersAndRolesByProjectId/{projectId}")]
        public IActionResult GetAvailableUsersAndRolesByProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty) { return BadRequest("Project Id is empty"); }

            var result = _repoSupervisor.Dossiers.GetAvailableUsersAndRolesByProjectId(projectId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetObjectsForDossier/{projectId}")]
        public IActionResult GetObjectsForDossier(Guid projectId)
        {
            if (projectId == Guid.Empty) { return BadRequest("Project Id is empty"); }

            var result = _repoSupervisor.Dossiers.GetObjectsForDossier(projectId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetBackgroundImage/{dossierId}")]
        public async Task<IActionResult> GetBackgroundImage(Guid dossierId)
        {
            if (dossierId == Guid.Empty) return BadRequest("Dossier Id is Empty.");

            var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(dossierId.ToUpperString());
            if (dossierData != null) 
            {
                if (!string.IsNullOrWhiteSpace(dossierData.BackgroundImagePath))
                {
                    return await GetFileStream(dossierData.BackgroundImagePath).ConfigureAwait(false);
                }
                return BadRequest("Image not found");
            }
            return Unauthorized("Authorization failed");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CreateOrUpdateDossier")]
        public IActionResult CreateOrUpdateDossier(NewDossierApiModel dossierModel)
        {
            try
            {
                if (dossierModel != null)
                {
                    string dossierId = _repoSupervisor.Dossiers.CreateOrUpdateDossier(dossierModel);
                    if (!string.IsNullOrWhiteSpace(dossierId))
                    {
                        _repoSupervisor.Complete();
                        _repoSupervisor.Dossiers.MarkWholeDossierAsViewed(dossierId);
                        _repoSupervisor.Complete();
                        return Ok(dossierId);
                    }
                    return BadRequest("Can not create/update");
                }
                else
                    return BadRequest("Model is Empty.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [RequestFormLimits(MultipartBodyLengthLimit = 25 * 1024 * 1024)]//25MB
        [HttpPost("UploadDossierFiles")]
        public IActionResult UploadDossierFiles([FromForm] DossierFileApiModel fileApiModel, List<IFormFile> files)
        {
            if (fileApiModel == null) return BadRequest("Model is Empty.");

            if (files == null || !files.Any()) return BadRequest("No File");
            if (files.Any(x => x.Length == 0)) return BadRequest("File is empty");

            if (!string.IsNullOrWhiteSpace(fileApiModel.DossierId) && fileApiModel.DossierId.ToGuid() == null)
                return BadRequest("Invalid Dossier Id");

            if (!string.IsNullOrWhiteSpace(fileApiModel.BuildingId) && fileApiModel.BuildingId.ToGuid() == null)
                return BadRequest("Invalid Building Id");

            if (_repoSupervisor.Dossiers.IsUserNotOnlySpectator(fileApiModel.DossierId, fileApiModel.BuildingId))
            {
                var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(fileApiModel.DossierId);
                if (dossierData != null)
                {
                    if (string.IsNullOrWhiteSpace(fileApiModel.BuildingId))
                        fileApiModel.BuildingId = null;
                    //if (_repoSupervisor.Dossiers.CheckForRights(User.Identity.Name, User.Identity.Name, fileApiModel.DossierId, fileApiModel.BuildingId, dossierData.HasObjectBoundFiles, fileApiModel.IsInternal))
                    if (_repoSupervisor.Dossiers.HasEditRightsToSection(fileApiModel.DossierId, dossierData.ProjectId, fileApiModel.BuildingId, fileApiModel.IsInternal))
                    {
                        var dossierUploadedFiles = new List<DossierFileModel>();
                        string attachmentId = null;
                        var uploadLocation = _repoSupervisor.Dossiers.GetUploadLocationForDossierFiles(fileApiModel.DossierId, fileApiModel.BuildingId);
                        if (!Directory.Exists(uploadLocation))
                        {
                            Directory.CreateDirectory(uploadLocation);
                        }
                        foreach (var file in files.Where(x => x.Length != 0))
                        {
                            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                            {
                                var fileBytes = binaryReader.ReadBytes((int)file.Length);
                                var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                var duplicateFileId = _repoSupervisor.Dossiers.CheckDuplicateDossierFile(originalFileName, fileApiModel);
                                var filePath = _repoSupervisor.Attachments.AddDossierFileAttachment(
                                out attachmentId,
                                dossierData.ProjectId,
                                fileApiModel.BuildingId,
                                _appSettings.AttachmentHeaders.DocumentUpload,
                                originalFileName,
                                uploadLocation
                                );
                                System.IO.File.WriteAllBytes(filePath, fileBytes);
                            }
                            if (!string.IsNullOrWhiteSpace(attachmentId))
                            {
                                string dossierFileId = _repoSupervisor.Dossiers.CreateFile(fileApiModel, attachmentId);
                                _repoSupervisor.Complete();
                                var dossierUploadedFile = _repoSupervisor.Dossiers.GetUploadedDossierFile(dossierFileId);
                                dossierUploadedFile.Path = null;
                                dossierUploadedFiles.Add(dossierUploadedFile);
                            }
                        }
                        return Ok(new { uploadedFiles = dossierUploadedFiles });
                    }
                }
                return BadRequest("Invalid Dossier");
            }
            return BadRequest("Can not upload dossier files");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierDataByKey/{dossierId}")]
        public IActionResult UpdateDossierDataByKey(Guid dossierId, List<CommonKeyValueApiModel> lstKeyValue)
        {
            if (dossierId != Guid.Empty && lstKeyValue != null && lstKeyValue.Any())
            {
                foreach (var item in lstKeyValue)
                {
                    var success = _repoSupervisor.Dossiers.UpdateDossierDataKeyValue(dossierId.ToUpperString(), item);
                    if (success)
                    {
                        _repoSupervisor.Complete();
                    }
                    else
                    {
                        return Unauthorized("Unauthorized to update :" + item.Id);
                    }
                }
                return Ok(new { success = true });
            }
            else
                return BadRequest("Invalid input");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateBuildingDossierDataKeyValue/{dossierId}/{buildingId}")]
        public IActionResult UpdateBuildingDossierDataKeyValue(Guid dossierId, Guid buildingId, List<CommonKeyValueApiModel> lstKeyValue)
        {
            if (dossierId != Guid.Empty && buildingId != Guid.Empty && lstKeyValue != null && lstKeyValue.Any())
            {
                foreach (var item in lstKeyValue)
                {
                    var success = _repoSupervisor.Dossiers.UpdateBuildingDossierDataKeyValue(dossierId.ToUpperString(), buildingId.ToUpperString(), item);
                    if (success)
                    {
                        _repoSupervisor.Complete();
                    }
                    else
                    {
                        return Unauthorized("Unauthorized to update :" + item.Id);
                    }
                }
                return Ok(new { success = true });
            }
            else
                return BadRequest("Invalid input");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteDossier/{dossierId}")]
        public IActionResult DeleteDossier(Guid dossierId)
        {
            if (dossierId != Guid.Empty)
            {
                if (_repoSupervisor.Dossiers.IsUserBuyerGuideForDossier(dossierId.ToUpperString(), User.Identity.Name))
                {
                    var success = _repoSupervisor.Dossiers.DeleteDossier(dossierId.ToUpperString());
                    if (success)
                    {
                        _repoSupervisor.Complete();
                        return Ok(new { success = true });
                    }
                    return BadRequest("Can not delete");
                }
                return Unauthorized("Bad Authentication");
            }
            else
                return BadRequest("Dossier Id is empty");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierUserRights/{dossierId}")]
        public IActionResult UpdateDossierUserRights(Guid dossierId, IEnumerable<NewDossierUserRightApiModel> newUserApiModel)
        {
            try
            {
                if (dossierId != Guid.Empty)
                {
                    var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(dossierId.ToUpperString());
                    if (dossierData != null)
                    {
                        if (_repoSupervisor.Dossiers.UpdateDossierUserRights(dossierId.ToUpperString(), newUserApiModel))
                        {
                            _repoSupervisor.Complete();
                            return Ok(new { success = true });
                        }
                        else
                            return BadRequest("Failed to update Dossier User rights");
                    }
                    else
                        return BadRequest("Invalid Dossier id");
                }
                else
                    return BadRequest("Dossier Id is empty");
            }
            catch (Exception)
            {
                throw;
                //return StatusCode(500, "Some Error Occured.");
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("DossierNotifications")]
        public async Task<IActionResult> DossierNotifications(DossierNotificationModel dossierNotifications)
        {
            if (dossierNotifications == null)
                return BadRequest("Model is Empty.");

            foreach (var buildingId in dossierNotifications.BuildingIds.Union(dossierNotifications.BuyerBuildingIds).Distinct())
            {
                if (!string.IsNullOrWhiteSpace(buildingId))
                {
                    if (!_repoSupervisor.Dossiers.IsUserNotOnlySpectator(dossierNotifications.DossierId, buildingId))
                    {
                        return BadRequest("Spectator can not send dossier notifications");
                    }
                }
            }
            try
            {
                await SendDossierNotification(dossierNotifications).ConfigureAwait(false);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Can not send dossier notifications. " + ex.Message);
            }
        }

        private async Task SendDossierNotification(DossierNotificationModel dossierNotifications)
        {
            string employeeId = null;
            var loginUserData = _userService.GetById(User.Identity.Name);
            var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(dossierNotifications.DossierId);
            if (dossierData != null)
            {
                if (dossierNotifications != null && dossierNotifications.BuyerBuildingIds != null && dossierNotifications.BuyerBuildingIds.Any())
                {
                    foreach (var buildingId in dossierNotifications.BuyerBuildingIds)
                    {
                        if (!string.IsNullOrWhiteSpace(buildingId))
                        {
                            var isVisibleToBuyer = _repoSupervisor.Dossiers.BuildingIsVisibleToBuyer(dossierNotifications.DossierId, buildingId);
                            if (isVisibleToBuyer)
                            {
                                var building_link = _appSettings.SiteUrl + "dossier/" + dossierData.Id + "?buildingId=" + buildingId;
                                // Send chat to buyer 
                                var message = string.Format(CultureInfo.InvariantCulture, "Dossier: [**{0}**]({1}) \n {2}"
                                    , dossierData.Name, building_link, dossierNotifications.Message);
                                _repoSupervisor.Chats.SendMessageToDefaultChat(buildingId, message);
                            }
                        }
                    }
                }

                if (loginUserData != null && !string.IsNullOrWhiteSpace(loginUserData.EmployeeId))
                {
                    employeeId = loginUserData.EmployeeId;
                }
                else
                {
                    var defaultSettings = _repoSupervisor.Config.GetSmtpConfig();
                    if (defaultSettings != null && !string.IsNullOrWhiteSpace(defaultSettings.EmployeeId))
                    {
                        employeeId = defaultSettings.EmployeeId;
                    }
                }
                var emailTemplate = _repoSupervisor.Config.DossierNotificationEmailTemplate();
                using (MailMessage mail = new MailMessage())
                {
                    Dictionary<string, string> tokens = _repoSupervisor.Dossiers.GetDefaultNotificationTokens(dossierNotifications.DossierId, dossierNotifications.BuildingIds, employeeId);
                    if (dossierNotifications != null && dossierNotifications.ToUserIdList != null && dossierNotifications.ToUserIdList.Any())
                    {
                        tokens["[verplichte_tekst]"] = string.Empty;
                        if (!string.IsNullOrWhiteSpace(dossierNotifications.Message))
                        {
                            var markdown = new MarkdownSharp.Markdown();
                            string html_message = markdown.Transform(dossierNotifications.Message);
                            tokens["[verplichte_tekst]"] = html_message;
                        }
                        var toEmailsWithSalutation = _repoSupervisor.Dossiers.GetUserEmailAddress(dossierNotifications.ToUserIdList);
                        if (toEmailsWithSalutation != null && toEmailsWithSalutation.Any())
                        {
                            foreach (var toEmail in toEmailsWithSalutation.Distinct())
                            {
                                mail.To.Clear();
                                var salutation = toEmail.Value ?? "Geachte heer, mevrouw,";
                                emailTemplate.UpdateTokenValues(tokens);
                                mail.To.Add(new MailAddress(toEmail.Key));
                                mail.Subject = emailTemplate.Subject;
                                mail.Body = emailTemplate.TemplateHtml
                                             .Replace("[geachte]", salutation, StringComparison.InvariantCulture);
                                mail.IsBodyHtml = true;
                                try
                                {
                                    await _emailService.SendEmailAsync(mail, loginUserData != null && !string.IsNullOrWhiteSpace(loginUserData.EmployeeId) ? loginUserData.EmployeeId : null).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    throw ex.InnerException;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(dossierNotifications.DossierId);
            }
        }

        [HttpGet("GetExistingFilesForProject/{projectId}")]
        public IActionResult GetExistingFilesForProject(Guid projectId)
        {
            try
            {
                if (!projectId.Equals(Guid.Empty))
                {
                    if (_repoSupervisor.Dossiers.HasRolesForProject(projectId.ToUpperString(), User.Identity.Name, new List<string> { LoginRoles.BuyersGuide, LoginRoles.Spectator }))
                    {
                        var result = _repoSupervisor.Dossiers.GetExistingFilesForProject(projectId.ToUpperString());
                        if (result != null && result.Any())
                            return Ok(result);
                        else
                            return NoContent();
                    }
                    else
                        return Unauthorized();
                }
                else
                    return BadRequest("Project Id cannot be empty");
            }
            catch (Exception)
            {
                throw; //check which is better - throw or error code?
                       //return StatusCode(500, "Some Error Occured.");
            }

        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierBuildingStatus/{dossierId}")]
        public IActionResult UpdateDossierBuildingStatus(Guid dossierId, bool isClosed, Guid buildingId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(dossierId.ToUpperString()) && dossierId.ToUpperString().ToGuid() == null)
                    return BadRequest("Invalid Dossier Id");

                var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(dossierId.ToUpperString());
                if (dossierData != null)
                {
                    var result = _repoSupervisor.Dossiers.UpdateDossierBuildingStatus(dossierId.ToUpperString(), isClosed, buildingId.ToUpperString());
                    if (result)
                    {
                        _repoSupervisor.Complete();
                        return Ok(result);
                    }

                }
                return BadRequest("Failed to update status");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet("GetUploadedDossierFile/{dossierFileId}")]
        public async Task<IActionResult> GetUploadedDossierFile(Guid dossierFileId)
        {
            try
            {
                if (!dossierFileId.Equals(Guid.Empty))
                {
                    var dossierFile = _repoSupervisor.Dossiers.GetUploadedDossierFile(dossierFileId.ToUpperString(), true);
                    if (dossierFile != null && !string.IsNullOrEmpty(dossierFile.Path) && System.IO.File.Exists(dossierFile.Path))
                    {
                        //cache valid image response
                        Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = new TimeSpan(24, 0, 0)
                        };
                        return await GetFileStream(dossierFile.Path).ConfigureAwait(false);
                    }
                    else
                        return NotFound();
                }
                else
                    return BadRequest("Dossier image id cannot be null");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("MoveDossierBuildingsFiles")]
        public IActionResult MoveDossierBuildingsFiles(DossierFileApiModel dossierFileApiModel)
        {
            try
            {
                if (dossierFileApiModel != null)
                {
                    if (_repoSupervisor.Dossiers.IsUserNotOnlySpectator(dossierFileApiModel.DossierId, dossierFileApiModel.BuildingId))
                    {
                        if (string.IsNullOrWhiteSpace(dossierFileApiModel.BuildingId))
                        {
                            dossierFileApiModel.BuildingId = null;
                        }
                        var result = _repoSupervisor.Dossiers.MoveDossierBuildingsFiles(dossierFileApiModel);
                        if (result == DossierResponseTypes.Success)
                        {
                            _repoSupervisor.Complete();
                        }
                        return ResolveDossierResponse(result);
                    }
                    return BadRequest("Can not move dossier files");
                }
                else
                    return BadRequest("Input model cannot be null");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierBuildings/{dossierId}")]
        public IActionResult UpdateDossierBuildings(Guid dossierId, List<DossiersBuildingUpdateModel> dossiersBuildingUpdateModels)
        {
            try
            {
                if (dossierId != Guid.Empty && dossiersBuildingUpdateModels != null && dossiersBuildingUpdateModels.Any())
                {
                    var response = _repoSupervisor.Dossiers.UpdateDossierBuildings(dossierId.ToUpperString(), dossiersBuildingUpdateModels);
                    if (response == DossierResponseTypes.Success)
                    {
                        _repoSupervisor.Complete();
                    }
                    return ResolveDossierResponse(response);
                }
                else
                    return BadRequest("Dossier Id or model is empty");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UploadBackgroundImage/{dossierId}")]
        public IActionResult UploadBackgroundImage(Guid dossierId, AppCore.Models.FileModel fileModel)
        {
            if (fileModel == null) return BadRequest("File model is null");
            var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(dossierId.ToUpperString());
            if (dossierData != null)
            {
                if (_repoSupervisor.Dossiers.IsUserBuyerGuideForDossier(dossierData.Id))
                {
                    var backgroundImagePath = _repoSupervisor.Dossiers.UploadBackgroundImageforDossier(dossierId.ToUpperString(), dossierData.ProjectId, fileModel);
                    if (!string.IsNullOrWhiteSpace(backgroundImagePath))
                    {
                        var success = _repoSupervisor.Dossiers.UpdateBackgroundImage(dossierId.ToUpperString(), backgroundImagePath);
                        if (success)
                        {
                            _repoSupervisor.Complete();
                            return Ok(new { success = true });
                        }
                    }
                }
                else
                    return Unauthorized();
            }
            return BadRequest("Failed to upload Background Image");
        }

        private IActionResult ResolveDossierResponse(DossierResponseTypes dossierResponseTypes)
        {
            switch (dossierResponseTypes)
            {
                case DossierResponseTypes.Success:
                    return Ok();
                case DossierResponseTypes.Unauthorized:
                    return Unauthorized();
                case DossierResponseTypes.UpdateFailed:
                    return BadRequest();
                default:
                    return BadRequest("Some Error occured");
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("LinkFilesToDossier")]
        public IActionResult LinkFilesToDossier(DossierFileApiModel fileApiModel)
        {

            if (fileApiModel == null) return BadRequest("Model is Empty.");

            if (!string.IsNullOrWhiteSpace(fileApiModel.DossierId) && fileApiModel.DossierId.ToGuid() == null)
                return BadRequest("Invalid Dossier Id");

            if (!string.IsNullOrWhiteSpace(fileApiModel.BuildingId) && fileApiModel.BuildingId.ToGuid() == null)
                return BadRequest("Invalid Building Id");

            if (_repoSupervisor.Dossiers.IsUserNotOnlySpectator(fileApiModel.DossierId, fileApiModel.BuildingId))
            {
                if (fileApiModel.DossierFileList != null || fileApiModel.DossierFileList.Any(x => !string.IsNullOrWhiteSpace(x.AttachmentId)))
                {
                    var dossierData = _repoSupervisor.Dossiers.GetDossierInfo(fileApiModel.DossierId);
                    if (dossierData != null)
                    {
                        if (string.IsNullOrWhiteSpace(fileApiModel.BuildingId))
                            fileApiModel.BuildingId = null;

                        //if (_repoSupervisor.Dossiers.CheckForRights(User.Identity.Name, User.Identity.Name, fileApiModel.DossierId, fileApiModel.BuildingId, dossierData.HasObjectBoundFiles, fileApiModel.IsInternal))
                        if (_repoSupervisor.Dossiers.HasEditRightsToSection(fileApiModel.DossierId, dossierData.ProjectId, fileApiModel.BuildingId, fileApiModel.IsInternal))
                        {
                            var dossierLinkedFiles = new List<DossierFileModel>();
                            string dossierFileId = null;
                            foreach (var fileModel in fileApiModel.DossierFileList)
                            {
                                if (!string.IsNullOrWhiteSpace(fileModel.AttachmentId))
                                    dossierFileId = _repoSupervisor.Dossiers.CreateFile(fileApiModel, fileModel.AttachmentId);
                                _repoSupervisor.Complete();
                                if (!string.IsNullOrWhiteSpace(dossierFileId))
                                {
                                    var dossierLinkedFile = _repoSupervisor.Dossiers.GetUploadedDossierFile(dossierFileId);
                                    dossierLinkedFile.Path = null;
                                    dossierLinkedFiles.Add(dossierLinkedFile);
                                }
                            }
                            return Ok(new { linkedFiles = dossierLinkedFiles });
                        }
                        else
                            return Unauthorized();
                    }
                }
                return BadRequest("DossierFileList Model is Empty.");
            }
            return BadRequest("Can not link dossier files");
        }

        [HttpGet("GetBuildingListWithDossiers/{projectId}")]
        public IActionResult GetBuildingListWithDossiers(Guid projectId)
        {
            if (projectId == Guid.Empty) { return BadRequest("Project Id is empty"); }

            var result = _repoSupervisor.Dossiers.GetBuildingListWithDossiers(projectId.ToUpperString());
            return Ok(result);
        }

        [HttpGet("GetUsersForDossierDeadline/{dossierId}")]
        public IActionResult GetUsersForDossierDeadline(Guid dossierId, string buildingId)
        {
            if (dossierId == Guid.Empty) { return BadRequest("Dossier Id is empty"); }
            if (string.IsNullOrWhiteSpace(buildingId))
                buildingId = null;
            var result = _repoSupervisor.Dossiers.GetUsersByDossierId(dossierId.ToUpperString(), buildingId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetFileThumbnail/{dossierFileId}")]
        public async Task<IActionResult> GetFileThumbnail(Guid dossierFileId, int thumbnailSize)
        {
            try
            {
                if (!dossierFileId.Equals(Guid.Empty))
                {
                    var dossierFile = _repoSupervisor.Dossiers.GetUploadedDossierFile(dossierFileId.ToUpperString(), true);
                    if (dossierFile != null && !string.IsNullOrEmpty(dossierFile.AttachmentId))
                    {
                        return await GetFileThumbnail(dossierFile.AttachmentId, thumbnailSize).ConfigureAwait(false);
                    }
                    return NotFound();
                }
                return BadRequest("Dossier attachment id is empty");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierDeadline/{dossierId}")]
        public IActionResult UpdateDossierDeadline(Guid dossierId, DateTime? deadlineDate, bool isUpdateBuildings)
        {
            if (dossierId == Guid.Empty) { return BadRequest("Dossier Id is empty"); }

            var success = _repoSupervisor.Dossiers.UpdateDossierDeadline(dossierId.ToUpperString(), deadlineDate, isUpdateBuildings);
            if (success)
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Cannot update.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateDossierLastView/{dossierId}")]
        public IActionResult UpdateDossierLastView(Guid dossierId, DossierLastViewModel viewModel)
        {
            if (dossierId == Guid.Empty) { return BadRequest("Dossier Id is empty"); }

            var dossierViewId = _repoSupervisor.Dossiers.UpdateDossierLastView(dossierId.ToUpperString(), viewModel);
            if (!string.IsNullOrWhiteSpace(dossierViewId))
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true, dossierReadId = dossierViewId });
            }
            return BadRequest("Cannot create.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("OrderDossier/{dossierId}")]
        public IActionResult OrderDossier(Guid dossierId, Guid? previousDossierId)
        {
            _repoSupervisor.Dossiers.OrderDossier(dossierId.ToUpperString(), previousDossierId?.ToUpperString());
            _repoSupervisor.Complete();
            return Ok(new { success = true });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CreateZipForDownload")]
        public IActionResult CreateZipForDownload(DownloadFilesModel downloadDossierFiles)
        {
            if (downloadDossierFiles != null)
            {
                var filename = _repoSupervisor.Dossiers.CreateZipForDownload(downloadDossierFiles);
                return Ok(filename);
            }
            return BadRequest();
        }


        [AllowAnonymous]
        [HttpGet("DownloadZip/{fileName}")]
        public async Task<IActionResult> DownloadZip(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(Path.Combine("Temp", fileName)))
            {
                return await GetFileStream(Path.Combine("Temp", fileName)).ConfigureAwait(false);
            }
            else
                return NotFound();
        }
    }
}
