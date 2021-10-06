using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class SurveyController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IReportingService _reportingService;
        private readonly IEmailService _emailService;
        private readonly IMimeMappingService _mimeMappingService;
        private readonly ILogger _logger;
        public SurveyController(IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IReportingService reportingService,
            IEmailService emailService,
            IMimeMappingService mimeMappingService,
            ILogger<HomeController> logger,
            IHostEnvironment hostEnvironment
            ) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
            _reportingService = reportingService;
            _emailService = emailService;
            _mimeMappingService = mimeMappingService;
            _logger = logger;
        }

        [HttpGet("GetSurveys/{surveyType}")]
        public IActionResult GetSurveys(SurveyType surveyType, int? noOfDaysForOffline, bool? isSecondSignature)
        {
            var userId = User.Identity.Name;
            var result = _repoSupervisor.Survey.GetSurveys(userId, surveyType, noOfDaysForOffline, isSecondSignature).ToList();

            if (isSecondSignature == true)
            {
                //Remove delivery signatures
                result = result.Select(x => { x.ExecutorSignature = null; x.BuyerRenter1Signature = null; x.BuyerRenter2Signature = null; return x; }).ToList();
            }

            return Ok(result);
        }

        [HttpGet("GetSurvey/{surveyId}")]
        public IActionResult GetSurvey(Guid surveyId)
        {
            var result = _repoSupervisor.Survey.GetSurveyDetails(surveyId.ToUpperString());

            return Ok(result);
        }

        [HttpGet("GetSignatureForSurvey/{surveyId}/{surveySignatoryType}")]
        public IActionResult GetSignatureForSurvey(Guid surveyId, SurveySignatoryType surveySignatoryType)
        {
            var fileModel = _repoSupervisor.Survey.GetSurveySignature(surveyId.ToUpperString(), surveySignatoryType);
            if (fileModel != null)
            {
                var contentType = _mimeMappingService.Map(fileModel.Name);

                return File(fileModel.Content, contentType, fileModel.Name);
            }
            return NotFound();
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddOrUpdateSurvey")]
        public async Task<IActionResult> AddOrUpdateSurvey(AddOrUpdateSurveyApiModel surveyApiModel)
        {
            if (surveyApiModel == null)
                return BadRequest("Model is Empty.");
            if (surveyApiModel.Status == SurveyStatus.Sent)
                return BadRequest("Can not update to status sent");

            bool isAdd = surveyApiModel.SurveyId == Guid.Empty;
            SurveyApiModel existingSurvey = null;
            if (!isAdd)
            {
                existingSurvey = _repoSupervisor.Survey.GetSurveyDetails(surveyApiModel.SurveyId.ToUpperString());
                if (existingSurvey == null)
                    return BadRequest("No Survey Found to Update");
                if (existingSurvey.Status == SurveyStatus.Completed || existingSurvey.Status == SurveyStatus.Sent)
                    return BadRequest("Updating the Survey is not allowed");
            }

            if (!surveyApiModel.ExecutorEmployeeId.HasValue || surveyApiModel.ExecutorEmployeeId.Value == Guid.Empty)
            {
                var login = _repoSupervisor.Logins.GetLoginById(User.Identity.Name);
                if (!string.IsNullOrWhiteSpace(login.EmployeeId))
                {
                    surveyApiModel.ExecutorEmployeeId = Guid.Parse(login.EmployeeId);
                }
            }
            var surveyId = _repoSupervisor.Survey.AddOrUpdateSurvey(surveyApiModel);
            _repoSupervisor.Complete();

            var updatedSurvey = _repoSupervisor.Survey.GetSurveyDetails(surveyId);
            if (isAdd)
            {
                return Ok(updatedSurvey);
            }
            else
            {
                if (surveyApiModel.Status != updatedSurvey.Status && surveyApiModel.Status == SurveyStatus.Completed && updatedSurvey.SurveyType == SurveyType.Delivery)
                {
                    if (updatedSurvey.ExecutorSignature?.Content != null && updatedSurvey.ExecutorSignature.Content.Length > 0
                        && updatedSurvey.BuyerRenter1Signature?.Content != null && updatedSurvey.BuyerRenter1Signature.Content.Length > 0
                        && (string.IsNullOrWhiteSpace(updatedSurvey.BuyerRenter2) || updatedSurvey.BuyerRenter2Signature?.Content != null && updatedSurvey.BuyerRenter2Signature.Content.Length > 0))
                    {
                        _repoSupervisor.Survey.MarkSurveyComplete(updatedSurvey.SurveyId);

                        byte[] content;
                        if (surveyApiModel.OfflineReport?.Content != null && surveyApiModel.OfflineReport.Content.Length > 0)
                        {
                            content = surveyApiModel.OfflineReport.Content;
                        }
                        else
                        {
                            content = await _reportingService.GetSurveyReportAsync(updatedSurvey.SurveyId);
                        }

                        var filename = "PvO_" + DateTime.Now.ToString("dd-MM-yyyy_hhmmss", CultureInfo.InvariantCulture) + ".pdf";

                        var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(updatedSurvey.BuildingId);

                        string attachmentId;
                        var filePath = _repoSupervisor.Attachments.AddNewSignedDocumentAttachment(
                            out attachmentId,
                            null,
                            updatedSurvey.BuildingId,
                            _appSettings.AttachmentHeaders.SignedDocumentUpload,
                            filename,
                            uploadLocation,
                            false
                            );

                        _repoSupervisor.Survey.UpdateDeliveryReport(updatedSurvey.SurveyId, attachmentId);

                        System.IO.File.WriteAllBytes(filePath, content);
                        _repoSupervisor.Complete();
                    }
                    else
                    {
                        return BadRequest("No signatures");
                    }
                }

                return Ok(new { success = true });
            }
        }

        [HttpGet("GetSurveyReport/{surveyId}")]
        public async Task<IActionResult> GetSurveyReport(Guid surveyId)
        {
            try
            {
                var survey = _repoSupervisor.Survey.GetSurveyDetails(surveyId.ToUpperString());
                if (survey != null && (survey.Status == SurveyStatus.Completed || survey.Status == SurveyStatus.Sent))
                {
                    var filePath = _repoSupervisor.Survey.GetOfficialReportOfCompletion(surveyId.ToUpperString(), _appSettings.AttachmentHeaders.SignedDocumentUpload);
                    if (!string.IsNullOrWhiteSpace(filePath))
                        return await GetFileStream(filePath).ConfigureAwait(false);
                }

                var content = await _reportingService.GetSurveyReportAsync(surveyId.ToUpperString());

                return File(content, "application/pdf", "PvO_Preview_" + DateTime.Now.ToShortDateString() + ".pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSurveyReport: {0}\n{1}\n{2}", surveyId, ex.Message, ex.StackTrace ?? string.Empty);
                return BadRequest();
            }
        }

        [HttpGet("GetSurveyReportForSecondSignature/{surveyId}")]
        public async Task<IActionResult> GetSurveyReportForSecondSignature(Guid surveyId)
        {
            try
            {
                var survey = _repoSupervisor.Survey.GetSurveyDetails(surveyId.ToUpperString());
                if (survey.IsSecondSignatureInitiated)
                {
                    if (survey != null && survey.SecondSignatureDate.HasValue)
                    {
                        var filePath = _repoSupervisor.Survey.GetOfficialReportOfCompletionSecondSignature(surveyId.ToUpperString(), _appSettings.AttachmentHeaders.SignedDocumentUpload);
                        if (!string.IsNullOrWhiteSpace(filePath))
                            return await GetFileStream(filePath).ConfigureAwait(false);
                    }

                    var content = await _reportingService.GetSurveyReportAsync(surveyId.ToUpperString());

                    return File(content, "application/pdf", "PvO_met_tweede_handtekening_preview_" + DateTime.Now.ToShortDateString() + ".pdf");
                }
                else
                {
                    return BadRequest("Second signature process not started.");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAvailableObjects")]
        public IActionResult GetAvailableObjects(Guid? projectId)
        {
            if (projectId.HasValue)
                return Ok(_repoSupervisor.UserObjects.GetBuildingsForSelectionByProject(projectId.Value.ToUpperString()));

            var phases = new string[] { "uitvoering" };
            return Ok(_repoSupervisor.UserObjects.GetProjectsForSelectionByPhases(phases));
        }


        [HttpGet("GetRecipientsForDeliveryEmail/{surveyId}")]
        public IActionResult GetRecipientsForDeliveryEmail(Guid surveyId)
        {
            var survey = _repoSupervisor.Survey.GetSurveyDetails(surveyId.ToUpperString());

            if (survey == null)
                return BadRequest("No survey!");

            var emailRecipients = getEmailRecipientsModel(survey);

            return Ok(emailRecipients);
        }

        private EmailRecipientsModel getEmailRecipientsModel(SurveyApiModel survey)
        {
            EmailRecipientsModel emailRecipients = new EmailRecipientsModel();

            var emailAddresses = _repoSupervisor.UserObjects.GetBuyerEmails(survey.BuildingId);
            foreach (var emailAddress in emailAddresses)
            {
                emailRecipients.To.Add(emailAddress);
            }

            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(survey.BuildingId);
            var responsibleRelationsEmails = _repoSupervisor.Project.GetResponsibleRelationsEmails(projectId);
            foreach (var email in responsibleRelationsEmails)
            {
                emailRecipients.Bcc.Add(email);
            }

            var user = _repoSupervisor.Logins.GetLoginById(User.Identity.Name);
            if (!responsibleRelationsEmails.Any(x => x.ToUpperInvariant() == user.Email.ToUpperInvariant()))
            {
                emailRecipients.Bcc.Add(user.Email);
            }

            return emailRecipients;
        }

        private async Task<IActionResult> sendDeliveryEmail(Guid surveyId, EmailRecipientsModel emailRecipients, bool secondSignature = false)
        {
            var survey = _repoSupervisor.Survey.GetSurveyDetails(surveyId.ToUpperString());

            if (survey == null)
                return BadRequest("No survey!");

            if (survey.SurveyType != SurveyType.Delivery)
            {
                return BadRequest("This is not delivery!");
            }

            if (survey.Status != SurveyStatus.Completed && survey.Status != SurveyStatus.Sent)
            {
                return BadRequest("Survey is not completed. Email can be sent only after status is Completed or already Sent");
            }

            string filePath = null;
            if (secondSignature)
            {
                if (!survey.SecondSignatureDate.HasValue)
                {
                    return BadRequest("Second Signature is not completed. Email can be sent only after it is signed and completed");
                }

                filePath = _repoSupervisor.Survey.GetOfficialReportOfCompletionSecondSignature(surveyId.ToUpperString(), _appSettings.AttachmentHeaders.SignedDocumentUpload);
            }
            else
            {
                filePath = _repoSupervisor.Survey.GetOfficialReportOfCompletion(surveyId.ToUpperString(), _appSettings.AttachmentHeaders.SignedDocumentUpload);
            }
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("Report not found!");

            //Send file to email
            if (emailRecipients == null)
            {
                emailRecipients = getEmailRecipientsModel(survey);
            }
            var emailTemplate = _repoSupervisor.Config.GetOfficialReportOfCompletionEmailTemplate();
            Dictionary<string, string> tokens = _repoSupervisor.UserObjects.GetDefaultEmailTokensForBuyers(survey.BuildingId);
            emailTemplate.UpdateTokenValues(tokens);

            using (MailMessage mail = new MailMessage())
            {
                foreach (var emailAddress in emailRecipients.To)
                {
                    mail.To.Add(new MailAddress(emailAddress));
                }

                foreach (var emailAddress in emailRecipients.Cc)
                {
                    mail.CC.Add(new MailAddress(emailAddress));
                }

                foreach (var emailAddress in emailRecipients.Bcc)
                {
                    mail.Bcc.Add(new MailAddress(emailAddress));
                }

                mail.Subject = emailTemplate.Subject;
                mail.Body = emailTemplate.TemplateHtml;
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(filePath));
                try
                {
                    await _emailService.SendEmailAsync(mail).ConfigureAwait(false);

                    if (survey.Status != SurveyStatus.Sent)
                    {
                        _repoSupervisor.Survey.MarkSurveySent(surveyId.ToUpperString());
                        _repoSupervisor.Complete();
                    }
                    return Ok(new { surveryStatus = SurveyStatus.Sent });
                }
                catch (Exception ex)
                {
                    //add log here
                    return BadRequest(ex.Message + " " + (ex.InnerException?.Message ?? string.Empty));
                }
            }
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("SendDeliveryEmail/{surveyId}")]
        public async Task<IActionResult> SendDeliveryEmail(Guid surveyId, EmailRecipientsModel emailRecipients)
        {
            if (emailRecipients?.IsModelValid() != true)
                return BadRequest("Incorrect Email Address");

            return await sendDeliveryEmail(surveyId, emailRecipients);
        }


        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("SendSecondSignatureEmail/{surveyId}")]
        public async Task<IActionResult> SendSecondSignatureEmail(Guid surveyId, EmailRecipientsModel emailRecipients)
        {
            if (emailRecipients?.IsModelValid() != true)
                return BadRequest("Incorrect Email Address");

            return await sendDeliveryEmail(surveyId, emailRecipients, true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("StartSecondSignature/{surveyId}")]
        public IActionResult StartSecondSignature(Guid surveyId)
        {
            bool result = _repoSupervisor.Survey.SetSecondSignatureAsInitiated(surveyId.ToUpperString());
            if (result)
            {
                _repoSupervisor.Complete();
                return Ok(new { success = true });
            }
            return BadRequest("Can not update.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CompleteSecondSignature/{surveyId}")]
        public async Task<IActionResult> CompleteSecondSignature(CompleteSecondSignatureApiModel completeSecondSignature)
        {
            if (completeSecondSignature == null)
                return BadRequest("Model can not be empty");

            bool result = _repoSupervisor.Survey.UpdateSecondSignature(completeSecondSignature);
            if (result)
            {
                _repoSupervisor.Complete();
                var surveyId = completeSecondSignature.SurveyId.ToUpperString();
                var updatedSurvey = _repoSupervisor.Survey.GetSurveyDetails(surveyId);

                if (updatedSurvey.BuyerRenter1SecondSignature?.Content != null && updatedSurvey.BuyerRenter1SecondSignature.Content.Length > 0
                        && (string.IsNullOrWhiteSpace(updatedSurvey.BuyerRenter2) || updatedSurvey.BuyerRenter2SecondSignature?.Content != null && updatedSurvey.BuyerRenter2SecondSignature.Content.Length > 0))
                {
                    _repoSupervisor.Survey.CompleteSecondSignature(surveyId);

                    byte[] content;
                    if (completeSecondSignature.OfflineReport?.Content != null && completeSecondSignature.OfflineReport.Content.Length > 0)
                    {
                        content = completeSecondSignature.OfflineReport.Content;
                    }
                    else
                    {
                        content = await _reportingService.GetSurveyReportAsync(surveyId);
                    }

                    var filename = "PvO_met_tweede_handtekening_" + DateTime.Now.ToString("dd-MM-yyyy_hhmmss", CultureInfo.InvariantCulture) + ".pdf";

                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(updatedSurvey.BuildingId);

                    string attachmentId;
                    var filePath = _repoSupervisor.Attachments.AddNewSignedDocumentAttachment(
                        out attachmentId,
                        null,
                        updatedSurvey.BuildingId,
                        _appSettings.AttachmentHeaders.SignedDocumentUpload,
                        filename,
                        uploadLocation,
                        false
                        );

                    _repoSupervisor.Survey.UpdateSecondSignatureReport(surveyId, attachmentId);

                    System.IO.File.WriteAllBytes(filePath, content);
                    _repoSupervisor.Complete();
                }
                else
                {
                    return BadRequest("No signatures");
                }

                return Ok(new { success = true });
            }
            return BadRequest("Can not update.");
        }
    }
}