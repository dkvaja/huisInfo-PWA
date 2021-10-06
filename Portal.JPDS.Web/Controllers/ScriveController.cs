using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Filters;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScriveController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IDigitalSigningService _signingService;
        public ScriveController(IOptions<AppSettings> appSettings, IRepoSupervisor repoSupervisor, IDigitalSigningService signingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
            _signingService = signingService;
        }

        [ServiceFilter(typeof(ScriveIpCheckFilter))]
        [HttpPost("UpdateDocumentStatus/{quoteId}")]
        public IActionResult UpdateDocumentStatus(Guid quoteId)
        {
            string documentJson = Request.Form["document_json"];
            bool documentSignedAndSealed = bool.Parse(Request.Form["document_signed_and_sealed"]);
            var document = JsonConvert.DeserializeObject<ScriveSigningModel>(documentJson);
            if (document != null && document.id > 0)
            {
                var quotation = _repoSupervisor.BuildingOptions.GetQuotationById(quoteId.ToUpperString());
                string message;
                try
                {
                    if (string.IsNullOrWhiteSpace(quotation.DigitalDocumentStatus) || quotation.DigitalDocumentStatus == ScriveDocumentStatus.Pending)
                    {
                        if (documentSignedAndSealed)
                        {
                            var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(quotation.BuildingId);
                            byte[] signedFile = _signingService.GetDigitallySignedFile(document.id, document.sealed_file.id);

                            var originalFileName = Path.GetFileNameWithoutExtension(document.sealed_file.name) + "_ondertekend_op_"
                                + DateTime.Now.ToString("dd-MM-yyyy_hhmmss", CultureInfo.InvariantCulture)
                                + Path.GetExtension(document.sealed_file.name);


                            _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Definite, SelectedOptionSubStatus.Definite, document.status, document.id);

                            string attachmentId;
                            string filePath = _repoSupervisor.Attachments.AddNewSignedDocumentAttachment(
                                out attachmentId,
                                quotation.QuoteId,
                                quotation.BuildingId,
                                _appSettings.AttachmentHeaders.SignedDocumentUpload,
                                originalFileName,
                                uploadLocation,
                                false
                            );

                            System.IO.File.WriteAllBytes(filePath, signedFile);
                            _repoSupervisor.Complete("ScriveCallback");

                            message = string.Format(CultureInfo.InvariantCulture, "Offerte {0} is digitaal ondertekend", quotation.QuoteNo);
                            _repoSupervisor.Chats.SendMessageToDefaultChat(quotation.BuildingId, message);
                        }
                        else
                        {
                            switch (document.status)
                            {
                                case ScriveDocumentStatus.Pending:
                                    _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.SentToBeDigitallySigned, document.status, document.id);
                                    _repoSupervisor.Complete("ScriveCallback");

                                    if (quotation.DigitalDocumentStatus != ScriveDocumentStatus.Pending)
                                    {
                                        message = string.Format(CultureInfo.InvariantCulture, "Er is een e-mail verzonden met een link om offerte “{0}” digitaal te ondertekenen. Het kan zijn dat de e-mail in uw spambox is terechtgekomen. Stuurt u ons aub een berichtje als u niets hebt ontvangen.", quotation.QuoteNo);
                                        _repoSupervisor.Chats.SendMessageToDefaultChat(quotation.BuildingId, message);
                                    }
                                    return Ok();

                                case ScriveDocumentStatus.Canceled:
                                    _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Cancelled, SelectedOptionSubStatus.Cancelled, document.status, document.id);
                                    _repoSupervisor.Complete("ScriveCallback");
                                    return Ok();

                                case ScriveDocumentStatus.Timedout:
                                    _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Cancelled, SelectedOptionSubStatus.Cancelled, document.status, document.id);
                                    _repoSupervisor.Complete("ScriveCallback");
                                    message = string.Format(CultureInfo.InvariantCulture, "De geldigheidsdatum van offerte “{0}” is voorbij. Deze offerte kan niet meer ondertekend worden. Voor vragen kunt u contact opnemen met uw kopersbegeleider door een bericht achter te laten.", quotation.QuoteNo);
                                    _repoSupervisor.Chats.SendMessageToDefaultChat(quotation.BuildingId, message);
                                    return Ok();

                                case ScriveDocumentStatus.Rejected:
                                    _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Cancelled, SelectedOptionSubStatus.Denied, document.status, document.id);
                                    _repoSupervisor.Complete("ScriveCallback");
                                    var rejectedBy = document.parties.Where(x => x.is_signatory && x.rejected_time.HasValue).FirstOrDefault();
                                    if (rejectedBy != null)
                                    {
                                        var name = rejectedBy.fields
                                        .Where(x => x.type != null && string.Equals(x.type, "name", StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.value).FirstOrDefault();
                                        message = "Offerte-" + quotation.QuoteNo + " is afgewezen door " + name
                                            + (string.IsNullOrWhiteSpace(rejectedBy.rejection_reason) ? string.Empty : ": " + rejectedBy.rejection_reason);
                                        _repoSupervisor.Chats.SendMessageToDefaultChat(quotation.BuildingId, message);
                                    }
                                    return Ok();

                                case ScriveDocumentStatus.DocumentError:
                                    if (_repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Cancelled, SelectedOptionSubStatus.Cancelled, document.status, document.id))
                                    {
                                        return Ok();
                                    }
                                    else
                                        return BadRequest("Please check the request parameters and try again.");

                                default: break;
                            }
                        }

                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest("Please check the request parameters and try again.");
                }
            }
            return BadRequest("Please check the request parameters and try again.");
        }
    }
}