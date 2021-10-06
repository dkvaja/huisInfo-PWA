using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class RepairRequestController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public RepairRequestController(IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IMimeMappingService mimeMappingService,
            IEmailService emailService, IUserService userService, IHostEnvironment hostEnvironment) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpGet("GetRepairRequests/{buildingId}")]
        public IActionResult GetRepairRequests(Guid buildingId)
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestsByBuildingId(buildingId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetRepairRequestsByProject/{projectId}")]
        public IActionResult GetRepairRequestsByProject(Guid projectId)
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestsByProjectId(projectId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetRepairRequestDetails/{repairRequestId}")]
        public IActionResult GetRepairRequestDetails(Guid repairRequestId)
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestDetails(repairRequestId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetRepairRequestAttachments/{repairRequestId}")]
        public IActionResult GetRepairRequestAttachments(Guid repairRequestId)
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestAttachments(repairRequestId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetRepairRequestsForSurvey/{surveyId}")]
        public IActionResult GetRepairRequestsForSurvey(Guid surveyId)
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestsForSurvey(surveyId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetRepairRequestLocations")]
        public IActionResult GetRepairRequestLocations()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestLocations();

            return Ok(result);
        }

        [HttpGet("GetRepairRequestAddWarningText")]
        public IActionResult GetRepairRequestAddWarningText()
        {
            string warningText = _repoSupervisor.Config.GetRepairRequestAddWarningText();

            return Ok(new { warningText });
        }

        [HttpGet("GetSettlementTerm")]
        public IActionResult GetSettlementTerm()
        {
            var result = _repoSupervisor.Config.GetSettlementTerms();

            return Ok(result);
        }


        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddRepairRequest")]
        public IActionResult AddRepairRequest([FromForm] NewRepairRequestApiModel newRepairRequest, List<IFormFile> files)
        {
            if (newRepairRequest == null)
                return BadRequest("Model is Empty.");

            Microsoft.Extensions.Primitives.StringValues jsonString = string.Empty;
            if (Request.Form.TryGetValue("ResolverOrganisations", out jsonString))
            {
                try
                {
                    newRepairRequest.ResolverOrganisations = JsonConvert.DeserializeObject<List<AddRepairRequestResolverApiModel>>(jsonString);
                }
                catch
                {
                    return BadRequest("ResolverOrganisations JSON value cannot be parsed.");
                }
            }

            newRepairRequest.ReporterLoginId = Guid.Parse(User.Identity.Name);
            var repairRequest = _repoSupervisor.RepairRequest.AddRepairRequest(newRepairRequest);

            if (!string.IsNullOrWhiteSpace(repairRequest.RequestId))
            {
                if (newRepairRequest.ResolverOrganisations != null)
                {
                    _repoSupervisor.RepairRequest.AddRepairRequestResolvers(repairRequest.RequestId, newRepairRequest.ResolverOrganisations);
                }

                if (files != null && files.Any())
                {
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(newRepairRequest.BuildingId.ToUpperString(), repairRequest.Number);

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


                            string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                                repairRequest.RequestId,
                                _appSettings.AttachmentHeaders.PictureUpload,
                                originalFileName,
                                uploadLocation,
                                null
                                );
                            System.IO.File.WriteAllBytes(filePath, fileBytes);
                        }
                    }
                }
                _repoSupervisor.Complete();

                if (!newRepairRequest.SurveyId.HasValue)
                {
                    SendConfirmationEmail(repairRequest.RequestId, newRepairRequest.SendMailToReporter, newRepairRequest);
                }
            }

            return Ok(new { success = true, requestId = repairRequest.RequestId });
        }

        private async Task SendConfirmationEmail(string repairRequestId, bool sendToReporter, NewRepairRequestApiModel newRepairRequest)
        {
            var settings = _repoSupervisor.Config.GetConfigSettings();
            var emailTemplate = _repoSupervisor.Config.RepairRequestConfirmationEmailTemplate();
            Dictionary<string, string> tokens = _repoSupervisor.RepairRequest.GetDefaultEmailTokens(repairRequestId);
            tokens["[bedrijf_naam]"] = settings.CompanyName;
            tokens["[melding_voorkeurstijdstip_afspraak]"] = newRepairRequest.PreferredAppointmentTime ?? string.Empty;
            emailTemplate.UpdateTokenValues(tokens);

            using (MailMessage mail = new MailMessage { From = new MailAddress(settings.EmailRepairRequest, "Afdeling Nazorg van " + settings.CompanyName) })
            {
                if (sendToReporter)
                {
                    var user = _repoSupervisor.Logins.GetLoginById(User.Identity.Name);
                    mail.To.Add(new MailAddress(user.Email));
                }

                //send to support
                mail.To.Add(new MailAddress(settings.EmailRepairRequest));

                mail.Subject = emailTemplate.Subject;
                mail.Body = emailTemplate.TemplateHtml;
                mail.IsBodyHtml = true;
                try
                {
                    await _emailService.SendEmailAsync(mail).ConfigureAwait(false);
                }
                catch
                {
                    //add log here
                }
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateRepairRequest")]
        public IActionResult UpdateRepairRequest([FromForm] UpdateRepairRequestApiModel updateRepairRequest, List<IFormFile> files)
        {
            if (updateRepairRequest == null)
                return BadRequest("Model is empty.");

            var result = _repoSupervisor.RepairRequest.UpdateRepairRequest(updateRepairRequest);
            if (result)
            {
                if (files != null && files.Any())
                {
                    var repairRequestDetails = _repoSupervisor.RepairRequest.GetRepairRequestById(updateRepairRequest.RepairRequestId.ToUpperString());
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(repairRequestDetails.BuildingId, repairRequestDetails.Number);

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


                            string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                                repairRequestDetails.RequestId,
                                _appSettings.AttachmentHeaders.PictureUpload,
                                originalFileName,
                                uploadLocation,
                                null
                                );
                            System.IO.File.WriteAllBytes(filePath, fileBytes);
                        }
                    }
                }
                _repoSupervisor.Complete();
                return Ok(new { status = true });
            }

            return BadRequest("Can not update.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("MarkCompletedByBuyer")]
        public IActionResult MarkCompletedByBuyer([FromForm] BuyerAgreementApiModel buyerAgreement)
        {
            if (buyerAgreement == null)
                return BadRequest("Model is empty.");

            var result = _repoSupervisor.RepairRequest.MarkCompletedByBuyer(buyerAgreement);
            if (result)
            {
                _repoSupervisor.Complete();
                return Ok(new { status = true });
            }

            return BadRequest("Can not update.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateRework")]
        public IActionResult UpdateRework([FromForm] ReWorkApiModel reWork, List<IFormFile> files)
        {
            if (reWork == null)
                return BadRequest("Model is empty.");

            var result = _repoSupervisor.RepairRequest.UpdateRework(reWork);
            if (result)
            {
                if (reWork.ResolverOrganisations != null || reWork.ResolverOrganisations.Count > 0)
                {
                    _repoSupervisor.RepairRequest.AddRepairRequestResolvers(reWork.RepairRequestId.ToUpperString(), reWork.ResolverOrganisations);
                }

                if (files != null && files.Any())
                {
                    var repairRequestDetails = _repoSupervisor.RepairRequest.GetRepairRequestById(reWork.RepairRequestId.ToUpperString());
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(repairRequestDetails.BuildingId, repairRequestDetails.Number);

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


                            string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                                repairRequestDetails.RequestId,
                                _appSettings.AttachmentHeaders.PictureUpload,
                                originalFileName,
                                uploadLocation,
                                null
                                );
                            System.IO.File.WriteAllBytes(filePath, fileBytes);
                        }
                    }
                }
                _repoSupervisor.Complete();
                return Ok(new { status = true });
            }

            return BadRequest("Can not update.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteRepairRequest/{repairRequestId}")]
        public IActionResult DeleteRepairRequest(Guid repairRequestId)
        {
            string[] filesToDelete = null;
            var repairRequest = _repoSupervisor.RepairRequest.GetRepairRequestById(repairRequestId.ToUpperString());
            var success = _repoSupervisor.RepairRequest.DeleteRepairRequest(repairRequestId.ToUpperString(), out filesToDelete);
            if (success)
            {
                _repoSupervisor.Complete();

                //Delete only files for survey.. otherwise keep in harddisk
                if (filesToDelete != null && !string.IsNullOrWhiteSpace(repairRequest.SurveyId))
                {
                    foreach (var file in filesToDelete)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch
                            {
                                //log exception here
                            }
                        }
                    }
                }
                return Ok(new { success = true });
            }
            return BadRequest("Can not delete");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddRepairRequestResolvers/{repairRequestId}")]
        public IActionResult AddRepairRequestResolvers(Guid repairRequestId, [FromBody] List<AddRepairRequestResolverApiModel> organisations)
        {
            if (organisations == null || organisations.Count == 0)
                return BadRequest("Model is Empty.");

            _repoSupervisor.RepairRequest.AddRepairRequestResolvers(repairRequestId.ToUpperString(), organisations);
            _repoSupervisor.Complete();

            return Ok(new { success = true });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPatch("UpdateRepairRequestResolverRelation/{resolverId}")]
        public IActionResult UpdateRepairRequestResolverRelation(Guid resolverId, Guid relationId)
        {
            if (resolverId == Guid.Empty || relationId == Guid.Empty)
                return BadRequest("Model is Empty.");

            var success = _repoSupervisor.RepairRequest.UpdateRepairRequestResolverRelation(resolverId.ToUpperString(), relationId.ToUpperString());
            if (success)
            {
                _repoSupervisor.Complete();

                return Ok(new { success = true });
            }
            return BadRequest("Failed to update");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteRepairRequestResolver/{resolverId}")]
        public IActionResult DeleteRepairRequestResolver(Guid resolverId)
        {
            var success = _repoSupervisor.RepairRequest.DeleteRepairRequestResolver(resolverId.ToUpperString());
            if (success)
            {
                _repoSupervisor.Complete();

                return Ok(new { success = true });
            }
            return BadRequest("Can not delete");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddRepairRequestAttachments/{repairRequestId}")]
        public IActionResult AddRepairRequestAttachments(Guid repairRequestId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return BadRequest("No File");
            if (files.Any(x => x.Length == 0)) return BadRequest("File is empty");

            var repairRequest = _repoSupervisor.RepairRequest.GetRepairRequestById(repairRequestId.ToUpperString());

            if (repairRequest == null)
                return BadRequest("Repair Request Not Found");

            var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(repairRequest.BuildingId, repairRequest.Number);

            if (!Directory.Exists(uploadLocation))
            {
                Directory.CreateDirectory(uploadLocation);
            }

            foreach (var file in files)
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileBytes = binaryReader.ReadBytes((int)file.Length);
                    var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');


                    string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                        repairRequest.RequestId,
                        _appSettings.AttachmentHeaders.PictureUpload,
                        originalFileName,
                        uploadLocation,
                        null
                        );
                    System.IO.File.WriteAllBytes(filePath, fileBytes);
                }
            }
            _repoSupervisor.Complete();
            return Ok(new { success = true });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateRepairRequestAttachment/{repairRequestId}/{attachmentId}")]
        public IActionResult UpdateRepairRequestAttachment(Guid repairRequestId, Guid attachmentId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var repairRequest = _repoSupervisor.RepairRequest.GetRepairRequestDetails(repairRequestId.ToUpperString());

            if (repairRequest != null && repairRequest.Completed != true && repairRequest.Attachments.Any(x => x.AttachmentId == attachmentId.ToUpperString()))
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileBytes = binaryReader.ReadBytes((int)file.Length);
                    var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    string filePath = _repoSupervisor.Attachments.GetAttachmentLocation(attachmentId.ToUpperString(), true);

                    //Overwrite existing file
                    System.IO.File.WriteAllBytes(filePath, fileBytes);
                }
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Can not update");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteRepairRequestAttachment/{repairRequestId}/{attachmentId}")]
        public IActionResult DeleteRepairRequestAttachment(Guid repairRequestId, Guid attachmentId)
        {
            var repairRequest = _repoSupervisor.RepairRequest.GetRepairRequestById(repairRequestId.ToUpperString());
            string fileToDelete;
            var success = _repoSupervisor.RepairRequest.DeleteAttachment(repairRequestId.ToUpperString(), attachmentId.ToUpperString(), out fileToDelete);
            if (success)
            {
                _repoSupervisor.Complete();
                if (!string.IsNullOrWhiteSpace(fileToDelete))
                {
                    if (System.IO.File.Exists(fileToDelete))
                    {
                        try
                        {
                            System.IO.File.Delete(fileToDelete);
                        }
                        catch
                        {
                            //log exception here
                        }
                    }
                }
                return Ok(new { success = true });
            }
            return BadRequest("Can not delete");
        }

        [AllowAnonymous]
        [HttpGet("GetResolverDetailsForWorkOrder/{resolverId}")]
        public IActionResult GetResolverDetailsForWorkOrder(Guid resolverId)
        {
            var result = _repoSupervisor.RepairRequest.GetResolverDetailsForWorkOrder(resolverId.ToUpperString());

            if (result == null)
            {
                return BadRequest("Not found");
            }
            else if (result.Status == ResolverStatus.Completed || result.Status == ResolverStatus.TurnedDown)
            {
                return BadRequest("Link expired");
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("UpdateWorkOrderByDirectLink")]
        public IActionResult UpdateWorkOrderByDirectLink([FromForm] WorkOrderApiModel model, List<IFormFile> files)
        {
            if (model == null)
                return BadRequest("Model is Empty.");

            var result = _repoSupervisor.RepairRequest.UpdateWorkOrderByDirectLink(model);
            if (result != null)
            {
                if (files != null && files.Any())
                {
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(result.BuildingId, result.RepairRequestNo);

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

                            string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                                result.RepairRequestId,
                                _appSettings.AttachmentHeaders.PictureUpload,
                                originalFileName,
                                uploadLocation,
                                model.ResolverId.ToUpperString()
                                );
                            System.IO.File.WriteAllBytes(filePath, fileBytes);
                        }
                    }
                }
                _repoSupervisor.Complete(result.OrganisationName);
                return Ok(new { status = true });
            }

            return BadRequest("Object not found");
        }

        [HttpGet("GetRepairRequestTypeList")]
        public IActionResult GetRepairRequestTypeList()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestTypeList();

            return Ok(result);
        }

        [HttpGet("GetRepairRequestNatureList")]
        public IActionResult GetRepairRequestNatureList()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestNatureList();

            return Ok(result);
        }

        [HttpGet("GetRepairRequestCauseList")]
        public IActionResult GetRepairRequestCauseList()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestCauseList();
            return Ok(result);
        }

        [HttpGet("GetRepairRequestCauserList")]
        public IActionResult GetRepairRequestCauserList()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestCauserList();

            return Ok(result);
        }

        [HttpGet("GetRepairRequestCarryOutAsTypeList")]
        public IActionResult GetRepairRequestCarryOutAsTypeList()
        {
            var result = _repoSupervisor.RepairRequest.GetRepairRequestCarryOutAsTypeList();

            return Ok(result);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateRepairRequestByKey/{repairRequestId}")]
        public IActionResult UpdateRepairRequestKeyValue(Guid repairRequestId, List<CommonKeyValueApiModel> lstKeyValue)
        {
            var success = _repoSupervisor.RepairRequest.UpdateRepairRequestKeyValue(repairRequestId.ToUpperString(), lstKeyValue);
            if (success)
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Can not update");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddRepairRequestResolver/{repairRequestId}")]
        public IActionResult AddRepairRequestResolver(Guid repairRequestId, [FromForm] Guid organisationId)
        {
            var resolverId = _repoSupervisor.RepairRequest.AddRepairRequestResolver(repairRequestId.ToUpperString(), organisationId.ToUpperString());
            _repoSupervisor.Complete();
            return Ok(new { success = true, resolverId = resolverId });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPatch("UpdateWorkOrder/{resolverId}")]
        public IActionResult UpdateWorkOrder(Guid resolverId, List<CommonKeyValueApiModel> lstKeyValue)
        {
            if (resolverId == Guid.Empty)
                return BadRequest("Model is Empty.");

            var success = _repoSupervisor.RepairRequest.UpdateWorkOrder(resolverId.ToUpperString(), lstKeyValue);
            if (success)
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Can not update");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteResolvers")]
        public IActionResult DeleteResolvers(List<Guid> lstResolverId)
        {
            if (lstResolverId == null)
                return BadRequest("Resolver list is Empty.");

            foreach (var item in lstResolverId)
            {
                if (item != Guid.Empty)
                {
                    var success = _repoSupervisor.RepairRequest.DeleteRepairRequestResolver(item.ToUpperString());
                    if (success)
                        _repoSupervisor.Complete();
                    else
                        return BadRequest("Can not delete");
                }
            }
            return Ok(new { success = true });

        }

        [HttpGet("GetWorkOrderDetails/{resolverId}")]
        public IActionResult GetWorkOrderDetails(Guid resolverId)
        {
            var result = _repoSupervisor.RepairRequest.GetWorkOrderDetails(resolverId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetWorkOrdersByProjectId/{projectId}")]
        public IActionResult GetWorkOrdersByProjectId(Guid projectId)
        {
            var result = _repoSupervisor.RepairRequest.GetWorkOrdersByProjectId(projectId.ToUpperString());

            return Ok(result);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CreateWorkOrder/{resolverId}")]
        public IActionResult CreateWorkOrder(Guid resolverId, RepairRequestNotificationModel notification)
        {
            if (resolverId == Guid.Empty)
                return BadRequest("Model is Empty.");
            var success = _repoSupervisor.RepairRequest.CreateWorkOrder(resolverId.ToUpperString());
            if (success)
            {
                _repoSupervisor.Complete();
                if (notification != null)
                {
                    if (notification.IsNotify)
                    {
                        Task result = SendWorkOrderNotification(resolverId.ToUpperString(), notification, ResolverStatus.New);
                        result.Wait();
                    }
                }
                return Ok(new { success = true });
            }
            return BadRequest("Can not create work order");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateWorkOrderStatus")]
        public IActionResult UpdateWorkOrderStatus(UpdateWorkOrderApiModel workOrder)
        {
            if (workOrder == null)
                return BadRequest("Model is Empty.");
            var resolverGuid = _repoSupervisor.RepairRequest.UpdateWorkOrderStatus(workOrder);
            if (resolverGuid != null)
            {
                _repoSupervisor.Complete();
                if (workOrder.Notification != null)
                {
                    if (workOrder.Notification.IsNotify)
                    {
                        var resolverData = _repoSupervisor.RepairRequest.GetWorkOrderDetails(workOrder.ResolverId.ToUpperString());
                        var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(User.Identity.Name, resolverData.ProjectId, resolverData.BuildingId);
                        if (resolverData != null && availbleModule.Any(x => x.ModuleName == LoginModules.ConstructionQuality && x.RoleName == LoginRoles.SubContractor))
                        {
                            workOrder.Notification = null;
                        }
                        ResolverStatus? status = workOrder.ContinuedWorkOrder ? ResolverStatus.Pending : workOrder.IsComplete ? ResolverStatus.Completed : ResolverStatus.TurnedDown;
                        Task notification = SendWorkOrderNotification(workOrder.ResolverId.ToUpperString(), workOrder.Notification, status);
                        notification.Wait();
                    }
                }
                return Ok(new { success = true, reworkResolverId = workOrder.ContinuedWorkOrder ? resolverGuid : null });
            }
            return BadRequest("Can not update");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateRepairRequestStatus/{repairRequestId}")]
        public IActionResult UpdateRepairRequestStatus(Guid repairRequestId, RepairRequestUpdateStatus repairRequestUpdateStatus)
        {
            if (repairRequestId == Guid.Empty)
                return BadRequest("Model is Empty.");

            var success = _repoSupervisor.RepairRequest.UpdateRepairRequestStatus(repairRequestId.ToUpperString(),
                repairRequestUpdateStatus != null && repairRequestUpdateStatus.IsComplete,
                repairRequestUpdateStatus != null ? repairRequestUpdateStatus.CompleteOrRejectionText : null);
            if (success)
            {
                _repoSupervisor.Complete();
                if (repairRequestUpdateStatus != null)
                {
                    if (repairRequestUpdateStatus.Notification != null)
                    {
                        if (repairRequestUpdateStatus.Notification.IsNotify)
                        {
                            ResolverStatus status = repairRequestUpdateStatus.IsComplete ? ResolverStatus.Completed : ResolverStatus.TurnedDown;
                            Task result = SendRepairRequestNotification(repairRequestId.ToUpperString(), repairRequestUpdateStatus.Notification, status);
                            result.Wait();
                        }
                    }
                }
                return Ok(new { success = true });
            }
            return BadRequest("Can not update");
        }

        [HttpGet("GetEmailsForWorkOrderResolver/{resolverId}")]
        public IActionResult GetEmailsForWorkOrderResolver(Guid resolverId)
        {
            var result = _repoSupervisor.RepairRequest.GetEmailsForWorkOrderResolver(resolverId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetEmailsForRepairRequest/{repairRequestId}")]
        public IActionResult GetEmailsForRepairRequest(Guid repairRequestId)
        {
            var result = _repoSupervisor.RepairRequest.GetEmailsForRepairRequest(repairRequestId.ToUpperString());

            return Ok(result);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("SendReminder")]
        public IActionResult SendReminder(ResolverReminderModel resolverReminderModel)
        {
            if (resolverReminderModel?.ResolverIdList == null)
                return BadRequest("Resolver list is Empty.");

            List<string> lstResolverId = new List<string>();
            foreach (var item in resolverReminderModel.ResolverIdList)
            {
                if (item != Guid.Empty)
                {
                    var notification = new RepairRequestNotificationModel
                    {
                        ToEmails = new RepairRequestEmailModel { ResolverEmails = new List<string> { _repoSupervisor.RepairRequest.GetResolverEmail(item.ToUpperString()) } },
                        CCEmails = new RepairRequestEmailModel { CustomEmails = resolverReminderModel.CCEmails }
                    };
                    Task result = SendWorkOrderNotification(item.ToUpperString(), notification, ResolverStatus.Informed);
                    result.Wait();
                    if (result.IsCompletedSuccessfully)
                    {
                        lstResolverId.Add(item.ToUpperString());
                    }
                }
            }
            return Ok(new { success = true, resolverIds = lstResolverId });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UploadWorkOrderImages/{resolverId}")]
        public IActionResult UploadWorkOrderImages(Guid resolverId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return BadRequest("No File");
            if (files.Any(x => x.Length == 0)) return BadRequest("File is empty");

            var resolver = _repoSupervisor.RepairRequest.GetWorkOrderDetails(resolverId.ToUpperString());

            if (resolver == null)
                return BadRequest("Repair Request Not Found");

            var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(resolver.BuildingId, resolver.RepairRequestNo);

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

                    string filePath = _repoSupervisor.Attachments.AddNewRepairRequestAttachment(
                        resolver.RepairRequestId,
                        _appSettings.AttachmentHeaders.PictureUpload,
                        originalFileName,
                        uploadLocation,
                        resolver.ResolverId
                        );
                    System.IO.File.WriteAllBytes(filePath, fileBytes);
                }
            }
            _repoSupervisor.Complete();
            return Ok(new { success = true });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateResolverStatusAsInformed/{resolverId}")]
        public IActionResult UpdateResolverStatusAsInformed(Guid resolverId)
        {
            if (resolverId == Guid.Empty)
                return BadRequest("Model is Empty.");

            var success = _repoSupervisor.RepairRequest.MarkResolverStatusAsInformed(resolverId.ToUpperString());
            if (success)
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Can not update");
        }

        [AllowAnonymous]
        [HttpGet("GetImage/{repairRequestId}/{attachmentId}")]
        public async Task<IActionResult> GetImage(Guid repairRequestId, Guid attachmentId)
        {
            if (repairRequestId == Guid.Empty && attachmentId == Guid.Empty)
                return BadRequest("Model is Empty.");

            var repairRequestAttachments = _repoSupervisor.RepairRequest.GetRepairRequestAttachments(repairRequestId.ToUpperString());
            if (repairRequestAttachments != null && repairRequestAttachments.Any(x => x.AttachmentId == attachmentId.ToUpperString()))
            {
                var filePath = _repoSupervisor.Attachments.GetAttachmentLocation(attachmentId.ToUpperString());
                if (filePath.IsImage())
                {
                    return await GetFileStream(filePath).ConfigureAwait(false);
                }
            }
            return BadRequest("Image not found");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CopyWorkOrder/{organisationId}/{resolverId}")]
        public IActionResult CopyWorkOrder(Guid organisationId, Guid resolverId)
        {
            if (resolverId == Guid.Empty || organisationId == Guid.Empty)
                return BadRequest("Model is Empty.");

            string newResolverId = _repoSupervisor.RepairRequest.CopyWorkOrder(organisationId.ToUpperString(), resolverId.ToUpperString());
            if (!string.IsNullOrWhiteSpace(newResolverId))
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true, resolverId = newResolverId });
            }
            return BadRequest("Can not copy work order");
        }

        private async Task SendWorkOrderNotification(string resolverId, RepairRequestNotificationModel notification, ResolverStatus? resolverStatus)
        {
            string employeeId = null;
            var loginUserData = _userService.GetById(User.Identity.Name);
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
            var emailTemplate = _repoSupervisor.Config.WorkOrderEmailTemplate();
            Dictionary<string, string> tokens = _repoSupervisor.RepairRequest.GetDefaultEmailTokensForWorkOrder(resolverId, employeeId);

            if (resolverStatus == ResolverStatus.Completed)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.Complete;
            }
            if (resolverStatus == ResolverStatus.TurnedDown)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.TurnedDown;
            }
            if (resolverStatus == ResolverStatus.Pending)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.Rework;
            }
            if (resolverStatus == ResolverStatus.New)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.Create;
            }
            if (resolverStatus == ResolverStatus.Informed)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.Reminder;
                if (string.IsNullOrWhiteSpace(tokens["[datum_ingelicht]"]))
                {
                    tokens["[datum_ingelicht]"] = DateTime.Now.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            using (MailMessage mail = new MailMessage())
            {
                if (notification != null)
                {
                    string toEmailManagerRole = notification.ToEmails != null ? GetEmailsWithBuildingRole(notification.ToEmails).Select(d => d.Key).FirstOrDefault() : null;
                    var toEmails = notification.ToEmails != null ? GetEmailsWithBuildingRole(notification.ToEmails).SelectMany(d => d.Value).ToList() : null;
                    if (toEmails != null && toEmails.Any())
                    {
                        var ccEmailsList = notification.CCEmails != null ? GetEmailsWithBuildingRole(notification.CCEmails).SelectMany(d => d.Value).ToList() : null;
                        var ccEmails = ccEmailsList != null && ccEmailsList.Any() ? ccEmailsList.Except(toEmails).ToList() : null;
                        var receiverContactDetails = _repoSupervisor.RepairRequest.GetReceiverContactDetails(resolverId, toEmailManagerRole);
                        tokens["[geachte]"] = receiverContactDetails != null ? receiverContactDetails.FormalSalutation : "Geachte heer, mevrouw,";
                        foreach (string toEmail in toEmails.Distinct())
                        {
                            mail.To.Add(new MailAddress(toEmail));
                        }
                        var emailTemplateWithoutToken = emailTemplate.TemplateHtml;
                        if (resolverStatus == ResolverStatus.Completed || resolverStatus == ResolverStatus.TurnedDown)
                        {
                            tokens["[workorder_direct_link]"] = string.Empty;
                        }
                        emailTemplate.UpdateTokenValues(tokens);
                        if (ccEmails != null && ccEmails.Any())
                        {
                            foreach (var ccEmail in ccEmails.Distinct())
                            {
                                mail.CC.Add(new MailAddress(ccEmail));
                            }
                        }
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailTemplate.TemplateHtml;
                        mail.IsBodyHtml = true;
                        try
                        {
                            var senderEmployeeData = await _emailService.SendEmailAsync(mail, loginUserData != null && !string.IsNullOrWhiteSpace(loginUserData.EmployeeId) ? loginUserData.EmployeeId : null).ConfigureAwait(false);
                            if (resolverStatus == ResolverStatus.New || resolverStatus == ResolverStatus.Informed)
                            {
                                _repoSupervisor.RepairRequest.MarkResolverStatusAsInformed(resolverId);
                                var communicationId = _repoSupervisor.RepairRequest.CreateCommunicationForWorkOrder(resolverId, emailTemplateWithoutToken, senderEmployeeData.EmployeeId, senderEmployeeData.EmailAccountId);
                                _repoSupervisor.RepairRequest.CreateContactsForWorkorder(resolverId, communicationId, emailTemplate.TemplateHtml,
                                 toEmails != null ? string.Join(";", toEmails) : null, ccEmails != null ? string.Join(";", ccEmails) : null
                                 , receiverContactDetails);
                                _repoSupervisor.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                throw ex.InnerException;
                            else
                                throw ex;
                        }
                    }
                }
                if (notification == null && (resolverStatus == ResolverStatus.Completed || resolverStatus == ResolverStatus.TurnedDown))
                {
                    string signerName = !string.IsNullOrWhiteSpace(loginUserData.RelationId) ? _repoSupervisor.RepairRequest.GetOrgRelationName(loginUserData.RelationId, loginUserData.OrganisationId) : string.Empty;
                    tokens["[ondertekenaar_naam]"] = signerName;
                    var siteManagerEmails = _repoSupervisor.RepairRequest.GetSiteManagerEmails(resolverId);
                    if (siteManagerEmails != null)
                    {
                        foreach (var siteManagerEmail in siteManagerEmails.Distinct())
                        {
                            mail.To.Clear();
                            tokens["[geachte]"] = _repoSupervisor.RepairRequest.GetSalutationForEmail(resolverId, "sitemanager", siteManagerEmail) ?? "Geachte heer, mevrouw,";
                            emailTemplate.UpdateTokenValues(tokens);
                            mail.To.Add(new MailAddress(siteManagerEmail));
                            mail.Subject = emailTemplate.Subject;
                            mail.Body = emailTemplate.TemplateHtml;
                            mail.IsBodyHtml = true;
                            try
                            {
                                await _emailService.SendEmailAsync(mail).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                    throw ex.InnerException;
                                else
                                    throw ex;
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<string, List<string>> GetEmailsWithBuildingRole(RepairRequestEmailModel emailsList)
        {
            Dictionary<string, List<string>> buildingRoleWithEmailList = new Dictionary<string, List<string>>();
            List<string> emailList = new List<string>();

            if (emailsList != null)
            {
                if (emailsList.ClientEmails != null && emailsList.ClientEmails.Any())
                {
                    foreach (string clientEmail in emailsList.ClientEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(clientEmail) && clientEmail.IsValidEmail())
                        {
                            emailList.Add(clientEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("client", emailList);
                    }
                }
                if (emailsList.VvEEmails != null && emailsList.VvEEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string vvEEmail in emailsList.VvEEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(vvEEmail) && vvEEmail.IsValidEmail())
                        {
                            emailList.Add(vvEEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("vve", emailList);
                    }
                }
                if (emailsList.VvEAdministratorEmails != null && emailsList.VvEAdministratorEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string vvEAdministratorEmail in emailsList.VvEAdministratorEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(vvEAdministratorEmail) && vvEAdministratorEmail.IsValidEmail())
                        {
                            emailList.Add(vvEAdministratorEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("vveadministrator", emailList);
                    }
                }
                if (emailsList.PropertyManagerEmails != null && emailsList.PropertyManagerEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string propertyManagerEmail in emailsList.PropertyManagerEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(propertyManagerEmail) && propertyManagerEmail.IsValidEmail())
                        {
                            emailList.Add(propertyManagerEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("propertymanager", emailList);
                    }
                }
                if (emailsList.ResolverEmails != null && emailsList.ResolverEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string resolverEmail in emailsList.ResolverEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(resolverEmail) && resolverEmail.IsValidEmail())
                        {
                            emailList.Add(resolverEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("resolver", emailList);
                    }
                }
                if (emailsList.ReporterEmails != null && emailsList.ReporterEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string reporterEmail in emailsList.ReporterEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(reporterEmail) && reporterEmail.IsValidEmail())
                        {
                            emailList.Add(reporterEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("reporter", emailList);
                    }
                }
                if (emailsList.BuyerEmails != null && emailsList.BuyerEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string buyerEmail in emailsList.BuyerEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(buyerEmail) && buyerEmail.IsValidEmail())
                        {
                            emailList.Add(buyerEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("buyer", emailList);
                    }
                }
                if (emailsList.CustomEmails != null && emailsList.CustomEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string customEmail in emailsList.CustomEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(customEmail) && customEmail.IsValidEmail())
                        {
                            emailList.Add(customEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        buildingRoleWithEmailList.Add("custom", emailList);
                    }
                }
            }
            return buildingRoleWithEmailList;
        }

        private async Task SendRepairRequestNotification(string repairRequestId, RepairRequestNotificationModel notification, ResolverStatus? resolverStatus)
        {
            string employeeId = null;
            var loginUserData = _userService.GetById(User.Identity.Name);
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
            var emailTemplate = _repoSupervisor.Config.RepairRequestEmailTemplate();
            Dictionary<string, string> tokens = _repoSupervisor.RepairRequest.GetDefaultEmailTokens(repairRequestId, employeeId);
            if (resolverStatus == ResolverStatus.Completed)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.Complete;
            }
            if (resolverStatus == ResolverStatus.TurnedDown)
            {
                tokens["[belangrijk_tekst]"] = WorkOrderEmailNotificationText.TurnedDown;
            }

            using (MailMessage mail = new MailMessage())
            {
                if (notification != null)
                {
                    var toEmailsSalutation = notification.ToEmails != null ? _repoSupervisor.RepairRequest.GetRepairRequestToEmailsAndSalutation(repairRequestId, notification.ToEmails) : null;
                    var salutation = toEmailsSalutation?.Select(d => d.Key).FirstOrDefault();
                    var toEmails = toEmailsSalutation?.SelectMany(d => d.Value).ToList();
                    if (toEmails != null && toEmails.Any())
                    {
                        var ccEmailsList = notification.CCEmails != null ? GetEmailsWithBuildingRole(notification.CCEmails).SelectMany(d => d.Value).ToList() : null;
                        var ccEmails = ccEmailsList != null && ccEmailsList.Any() ? ccEmailsList.Except(toEmails).ToList() : null;
                        tokens["[geachte]"] = salutation ?? "Geachte heer, mevrouw,";
                        emailTemplate.UpdateTokenValues(tokens);
                        foreach (var toEmail in toEmails.Distinct())
                        {
                            mail.To.Add(new MailAddress(toEmail));
                        }
                        if (ccEmails != null && ccEmails.Any())
                        {
                            foreach (string ccEmail in ccEmails.Distinct())
                            {
                                mail.CC.Add(new MailAddress(ccEmail));
                            }
                        }
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailTemplate.TemplateHtml;
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
}