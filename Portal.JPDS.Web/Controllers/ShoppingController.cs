using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ShoppingController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ShoppingController(IOptions<AppSettings> appSettings,
            IRepoSupervisor repoSupervisor,
            IServiceScopeFactory serviceScopeFactory,
            IMimeMappingService mimeMappingService,
            IHostEnvironment hostEnvironment) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet("GetStandardOptionsPerProject/{projectId}")]
        public IActionResult GetStandardOptionsPerProject(Guid projectId)
        {
            var options = _repoSupervisor.BuildingOptions.GetStandardOptionsByProjectId(projectId.ToUpperString());
            return Ok(options);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UploadStandardOptionImages/{optionStandardId}")]
        public IActionResult UploadStandardOptionImages(Guid optionStandardId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return BadRequest("No File");
            if (files.Any(x => x.Length == 0)) return BadRequest("File is empty");

            var userId = User.Identity.Name;
            var projectId = _repoSupervisor.BuildingOptions.GetProjectIdForStandardOption(optionStandardId.ToUpperString());
            var isUserBuyerGuide = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId).Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide);
            if (isUserBuyerGuide)
            {
                string uploadLocation = Path.Combine(_appSettings.FileStoreSettings.BasePath, _appSettings.FileStoreSettings.StandardOptionAttachmentsLocation);

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


                        string filePath = _repoSupervisor.Attachments.AddNewStandardOptionImage(
                            optionStandardId.ToUpperString(),
                            _appSettings.AttachmentHeaders.PictureUpload,
                            originalFileName,
                            uploadLocation
                            );
                        System.IO.File.WriteAllBytes(filePath, fileBytes);
                    }
                }

                _repoSupervisor.Complete();

                return Ok(true);
            }
            return BadRequest("Can not upload standard option images");
        }

        [HttpGet("GetStandardOptions/{buildingId}")]
        public IActionResult GetStandardOptions(Guid buildingId)
        {
            var standardOptions = _repoSupervisor.BuildingOptions.GetStandardOptionsByBuildingId(buildingId.ToUpperString());
            var categories = _repoSupervisor.BuildingOptions.GetStandardOptionCategoriesByBuildingId(buildingId.ToUpperString());

            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingId.ToUpperString());
            var headers = _repoSupervisor.BuildingOptions.GetStandardOptionHeadersByProjectId(projectId);

            foreach (var category in categories)
            {
                category.Headers = headers
                    .Where(x => standardOptions.Any(y => y.OptionCategoryProjectId == category.OptionCategoryProjectId && y.OptionHeaderProjectId == x.OptionHeaderProjectId))
                    .OrderBy(x => x.Order).ThenBy(x => x.Header);
            }

            OptionGroupedCategoryHeaderApiModel<StandardOptionApiModel> apiModel = new OptionGroupedCategoryHeaderApiModel<StandardOptionApiModel>
            {
                Categories = categories.OrderBy(x => x.Order).ThenBy(x => x.Category),
                Options = standardOptions
            };

            return Ok(apiModel);
        }

        /// <summary>
        /// Get selected options which are in shopping cart as well as available individual options
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        [HttpGet("GetSelectedOptionsWithIndividualOptions/{buildingId}")]
        public IActionResult GetSelectedOptionsWithIndividualOptions(Guid buildingId)
        {
            var selectedOptions = _repoSupervisor.BuildingOptions.GetSelectedOptionsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Provisional, SelectedOptionSubStatus.InShoppingCart);
            var individualOptions = _repoSupervisor.BuildingOptions.GetAvailableIndividualOptions(buildingId.ToUpperString());

            return Ok(selectedOptions.Union(individualOptions));
        }

        [HttpGet("GetStandardOptionImageList/{standardOptionId}")]
        public IActionResult GetStandardOptionImageList(Guid standardOptionId)
        {
            return Ok(_repoSupervisor.Attachments.GetStandardOptionAttachments(standardOptionId.ToUpperString(), true));
        }

        [HttpGet("GetSelectedOptionImageList/{selectedOptionId}")]
        public IActionResult GetSelectedOptionImageList(Guid selectedOptionId)
        {
            return Ok(_repoSupervisor.Attachments.GetSelectedOptionAttachments(selectedOptionId.ToUpperString(), true));
        }


        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddToCartIndividualOption")]
        public IActionResult AddToCartIndividualOption([FromBody] AddToCartApiModel model)
        {
            if (model == null)
                return BadRequest("Model is empty");

            var userId = User.Identity.Name;
            _repoSupervisor.BuildingOptions.AddOrUpdateToCartIndividualOption(model, userId);
            _repoSupervisor.Complete();

            return GetSelectedOptionsWithIndividualOptions(Guid.Parse(model.BuildingId));
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddToCartStandardOption")]
        public IActionResult AddToCartStandardOption([FromBody] AddToCartApiModel model)
        {
            if (model == null)
                return BadRequest("Model is empty");

            var userId = User.Identity.Name;
            _repoSupervisor.BuildingOptions.AddOrUpdateToCartStandardOption(model, userId.ToUpperInvariant());

            return GetSelectedOptionsWithIndividualOptions(Guid.Parse(model.BuildingId));
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteSelectedOption/{selectedOptionId}")]
        public IActionResult DeleteSelectedOption(Guid selectedOptionId)
        {
            _repoSupervisor.BuildingOptions.DeleteSelectedOption(selectedOptionId.ToUpperString());
            _repoSupervisor.Complete();
            return Ok(true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("RequestSelectedOptions/{buildingId}")]
        public IActionResult RequestSelectedOptions(Guid buildingId)
        {
            var buildingIdUpperCase = buildingId.ToUpperString();
            var hasAdditionalDescription = _repoSupervisor.BuildingOptions.RequestSelectedOptions(buildingIdUpperCase);
            _repoSupervisor.Complete();

            var buildingInfo = _repoSupervisor.UserObjects.GetBuildingInfo(buildingIdUpperCase);
            var objectName = _repoSupervisor.UserObjects.GetNameForObjectBasedOnConstructionType(buildingIdUpperCase);
            var message = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} heeft opties geselecteerd en verzoekt deze te beoordelen en een offerte te sturen.{2}",
                objectName,
                buildingInfo.BuildingNoExtern,
                hasAdditionalDescription ? "\nLET OP: Er zijn aanvullende omschrijvingen ingevuld!" : ""
                );
            _repoSupervisor.Chats.SendMessageToDefaultChat(buildingIdUpperCase, message);
            return Ok(true);
        }

        [HttpGet("GetQuotations/{buildingId}")]
        public IActionResult GetQuotations(Guid buildingId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.BuildingOptions.GetQuotationsForBuilding(buildingId.ToUpperString()));
        }

        [HttpGet("GetQuotationDocuments/{quoteId}")]
        public IActionResult GetQuotationDocuments(Guid quoteId)
        {
            var documents = _repoSupervisor.Attachments.GetQuotationDocuments(quoteId.ToUpperString(), _appSettings.AttachmentHeaders.Quotations);
            var drawings = _repoSupervisor.Attachments.GetQuotationDocuments(quoteId.ToUpperString(), _appSettings.AttachmentHeaders.QuotationDrawing);

            return Ok(new { documents, drawings });
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("OrderQuotationAndUploadSignedDocuments/{quoteId}")]
        public IActionResult OrderQuotationAndUploadSignedDocuments(Guid quoteId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return BadRequest("No File");
            if (files.Any(x => x.Length == 0)) return BadRequest("File is empty");

            var quotation = _repoSupervisor.BuildingOptions.GetQuotationById(quoteId.ToUpperString());
            var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(quotation.BuildingId);
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

                    string attachmentId;
                    string filePath = _repoSupervisor.Attachments.AddNewSignedDocumentAttachment(
                        out attachmentId,
                        quotation.QuoteId,
                        quotation.BuildingId,
                        _appSettings.AttachmentHeaders.SignedDocumentUpload,
                        originalFileName,
                        uploadLocation
                        );
                    System.IO.File.WriteAllBytes(filePath, fileBytes);
                }
            }

            _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.SignedDocumentsToBeReviewed);
            _repoSupervisor.Complete();

            return Ok(true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("OrderAndSendQuotationForDigitalSigning/{quoteId}")]
        public IActionResult OrderAndSendQuotationForDigitalSigning(Guid quoteId)
        {
            _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.OrderedOnline);
            _repoSupervisor.Complete();
            var userId = User.Identity.Name;

            var callbackUrl = new Uri(_appSettings.SiteUrl + "scrive/updatedocumentstatus/" + quoteId);

            Task.Run(
                () =>
                {
                    //Creating a new scope here because when we run it in background than it looses scope and disposes the dbContext...
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _signingService = scope.ServiceProvider.GetRequiredService<IDigitalSigningService>();
                        _signingService.SendDocumentToBeDigitallySigned(
                            quoteId.ToUpperString(),
                            userId,
                            _appSettings.AttachmentHeaders.Quotations,
                            _appSettings.AttachmentHeaders.QuotationDrawing,
                            callbackUrl
                            );
                    }
                }
            );

            return Ok(true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("CancelQuotation/{quoteId}")]
        public IActionResult CancelQuotation(Guid quoteId, [FromBody] string message)
        {
            if (quoteId == Guid.Empty || string.IsNullOrWhiteSpace(message))
                return BadRequest("Please check the request parameters and try again.");

            var quotation = _repoSupervisor.BuildingOptions.GetQuotationById(quoteId.ToUpperString());

            _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quotation.QuoteId, SelectedOptionStatus.Cancelled, SelectedOptionSubStatus.Denied, ScriveDocumentStatus.Rejected);
            _repoSupervisor.Complete();

            var name = User.GetClaim("FullName");
            message = "Offerte-" + quotation.QuoteNo + " is afgewezen door " + name + ": " + message;
            _repoSupervisor.Chats.SendMessageToDefaultChat(quotation.BuildingId, message);

            return Ok(true);
        }

        [HttpGet("GetMyRequestedOptions/{buildingId}")]
        public IActionResult GetMyRequestedOptions(Guid buildingId)
        {
            var requestedOptions = _repoSupervisor.BuildingOptions.GetRequestedOptionsByBuildingId(buildingId.ToUpperString());
            var categories = _repoSupervisor.BuildingOptions.GetStandardOptionCategoriesByBuildingId(buildingId.ToUpperString());

            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingId.ToUpperString());
            var headers = _repoSupervisor.BuildingOptions.GetStandardOptionHeadersByProjectId(projectId);

            foreach (var category in categories)
            {
                category.Headers = headers
                    .Where(x => requestedOptions.Any(y => y.OptionCategoryProjectId == category.OptionCategoryProjectId && y.OptionHeaderProjectId == x.OptionHeaderProjectId))
                    .OrderBy(x => x.Order).ThenBy(x => x.Header);
            }

            OptionGroupedCategoryHeaderApiModel<SelectedOptionApiModel> apiModel = new OptionGroupedCategoryHeaderApiModel<SelectedOptionApiModel>
            {
                Categories = categories.OrderBy(x => x.Order).ThenBy(x => x.Category),
                Options = requestedOptions
            };

            return Ok(apiModel);
        }

        [HttpGet("GetMyDefiniteOptions/{buildingId}")]
        public IActionResult GetMyDefiniteOptions(Guid buildingId)
        {
            var definiteOptions = _repoSupervisor.BuildingOptions.GetSeletedDefiniteOptionsByBuildingId(buildingId.ToUpperString());
            var categories = _repoSupervisor.BuildingOptions.GetStandardOptionCategoriesByBuildingId(buildingId.ToUpperString());

            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingId.ToUpperString());
            var headers = _repoSupervisor.BuildingOptions.GetStandardOptionHeadersByProjectId(projectId);

            foreach (var category in categories)
            {
                category.Headers = headers
                    .Where(x => definiteOptions.Any(y => y.OptionCategoryProjectId == category.OptionCategoryProjectId && y.OptionHeaderProjectId == x.OptionHeaderProjectId))
                    .OrderBy(x => x.Order).ThenBy(x => x.Header);
            }

            OptionGroupedCategoryHeaderApiModel<SelectedOptionApiModel> apiModel = new OptionGroupedCategoryHeaderApiModel<SelectedOptionApiModel>
            {
                Categories = categories.OrderBy(x => x.Order).ThenBy(x => x.Category),
                Options = definiteOptions
            };

            return Ok(apiModel);
        }

        [HttpGet("GetOptionsOverview/{buildingId}")]
        public IActionResult GetOptionsOverview(Guid buildingId)
        {
            SelectedOptionsOverviewApiModel result = new SelectedOptionsOverviewApiModel
            {
                AvailableIndividualOptions = _repoSupervisor.BuildingOptions.GetAvailableIndividualOptions(buildingId.ToUpperString()).OrderBy(x => x.Category).ThenBy(x => x.Header),
                AvailableQuotations = _repoSupervisor.BuildingOptions.GetQuotationsForBuilding(buildingId.ToUpperString()),
                Cancelled = _repoSupervisor.BuildingOptions.GetQuotationsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Quotation, null),
                Definite = _repoSupervisor.BuildingOptions.GetSeletedDefiniteOptionsByBuildingId(buildingId.ToUpperString()).OrderBy(x => x.Category).ThenBy(x => x.Header),
                OptionsInShoppingCart = _repoSupervisor.BuildingOptions.GetSelectedOptionsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Provisional, SelectedOptionSubStatus.InShoppingCart).OrderBy(x => x.Category).ThenBy(x => x.Header),
                OrderedOnlineAndSentToBeSigned = _repoSupervisor.BuildingOptions.GetQuotationsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.SentToBeDigitallySigned),
                OrderedOnlineButNotSentToBeSigned = _repoSupervisor.BuildingOptions.GetQuotationsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.OrderedOnline),
                RequestedToBeJudged = _repoSupervisor.BuildingOptions.GetSelectedOptionsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.New, SelectedOptionSubStatus.NewOrToBeJudged).OrderBy(x => x.Category).ThenBy(x => x.Header),
                SignedToBeReviewed = _repoSupervisor.BuildingOptions.GetQuotationsByBuildingIdAndStatuses(buildingId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.SignedDocumentsToBeReviewed)
            };

            return Ok(result);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("RequestIndividualOption/{buildingId}")]
        public IActionResult RequestIndividualOption(Guid buildingId, [FromForm] string desc, List<IFormFile> files)
        {
            if (string.IsNullOrWhiteSpace(desc)) return BadRequest("No Description");

            var userId = User.Identity.Name;
            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(buildingId.ToUpperString());
            var organisationId = _repoSupervisor.UserObjects.GetOrganisationIdForProject(projectId.ToUpperInvariant());
            string chatId = _repoSupervisor.Chats.AddNewChat(userId, new AddNewChatApiModel { BuildingId = buildingId.ToUpperString(), OrganisationId = organisationId.ToUpperInvariant(), Subject = string.Empty });

            var message = "**Aanvraag individuele optie:**\n" + desc;
            _repoSupervisor.Chats.AddNewChatMessage(userId, new AddNewChatMessageApiModel { ChatId = chatId.ToUpperInvariant(), Message = message });

            if (files != null && files.Count > 0)
            {
                var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(buildingId.ToUpperString());
                if (!Directory.Exists(uploadLocation))
                {
                    Directory.CreateDirectory(uploadLocation);
                }

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                        {
                            var fileBytes = binaryReader.ReadBytes((int)file.Length);
                            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                            AddNewChatMessageApiModel newChatMessage = new AddNewChatMessageApiModel { ChatId = chatId, Message = originalFileName };
                            var chatMessageId = _repoSupervisor.Chats.AddNewChatMessage(userId, newChatMessage);
                            string filePath = _repoSupervisor.Attachments.AddNewChatMessageAttachment(
                                chatMessageId,
                                buildingId.ToUpperString(),
                                _appSettings.AttachmentHeaders.ChatMessageUpload,
                                originalFileName,
                                uploadLocation
                                );

                            System.IO.File.WriteAllBytes(filePath, fileBytes);
                        }
                    }
                }
            }
            _repoSupervisor.Complete();

            return Ok(new { chatId });
        }

        //[HttpGet("ResetMyTestQuotation")]
        //public IActionResult ResetMyTestQuotation()
        //{
        //    Guid quoteId = Guid.Parse("ADBE64C4-90B1-41F8-B42E-1CE088A5E881");
        //    _repoSupervisor.BuildingOptions.UpdateQuotationStatus(quoteId.ToUpperString(), SelectedOptionStatus.Quotation, SelectedOptionSubStatus.Quotation, ScriveDocumentStatus.Pending, null);
        //    _repoSupervisor.Complete();
        //    return Ok();
        //}

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("SortStandardOptionAttachments")]
        public IActionResult SortStandardOptionAttachments([FromBody] List<Guid> lstIds)
        {
            if (lstIds == null) return BadRequest("List is empty");

            var userId = User.Identity.Name;
            var lstAttachmentId = lstIds.Select(x => x.ToUpperString()).ToList();
            var optionStandardId = _repoSupervisor.Attachments.GetStandardOptionId(lstAttachmentId);
            if (!string.IsNullOrWhiteSpace(optionStandardId))
            {
                var projectId = _repoSupervisor.BuildingOptions.GetProjectIdForStandardOption(optionStandardId);
                var isUserBuyerGuide = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId).Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide);
                if (isUserBuyerGuide)
                {
                    _repoSupervisor.Attachments.SortAttachments(lstIds);
                    _repoSupervisor.Complete();
                    return Ok(lstIds);   
                }
            }
            return BadRequest("Can not sort attachment");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteStandardOptionAttachment/{attachmentId}")]
        public async Task<IActionResult> DeleteStandardOptionAttachment(Guid attachmentId)
        {
            string fileToDelete;
            var userId = User.Identity.Name;
            var optionStandardId = _repoSupervisor.Attachments.GetStandardOptionId(attachmentId.ToUpperString());
            if (!string.IsNullOrWhiteSpace(optionStandardId))
            {
                var projectId = _repoSupervisor.BuildingOptions.GetProjectIdForStandardOption(optionStandardId);
                var isUserBuyerGuide = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId).Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide && x.ProjectId == projectId);
                if (isUserBuyerGuide)
                {
                    var success = _repoSupervisor.Attachments.DeleteAttachment(attachmentId.ToUpperString(), out fileToDelete);
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
                }
            }
            return BadRequest("Can not delete");
        }
    }
}