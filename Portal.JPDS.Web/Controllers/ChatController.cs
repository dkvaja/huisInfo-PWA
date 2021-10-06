using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Web.Helpers;

namespace Portal.JPDS.Web.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly AppSettings _appSettings;
        private readonly IRepoSupervisor _repoSupervisor;
        public ChatController(IOptions<AppSettings> appSettings, IRepoSupervisor repoSupervisor, IMimeMappingService mimeMappingService, IHostEnvironment hostEnvironment) : base(hostEnvironment, repoSupervisor, mimeMappingService)
        {
            _appSettings = appSettings.Value;
            _repoSupervisor = repoSupervisor;
        }

        [HttpGet("GetChatsByBuilding/{buildingId}")]
        public IActionResult GetChatsByBuilding(Guid buildingId, DateTime? dateTime)
        {
            dateTime = dateTime?.ToLocalTime();
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetChatsByBuilding(userId, buildingId.ToUpperString(), dateTime).OrderByDescending(x => x.DateTime));
        }

        [HttpGet("GetChatsByProject/{projectId}")]
        public IActionResult GetChatsByProject(Guid projectId, DateTime? dateTime)
        {
            dateTime = dateTime?.ToLocalTime();
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetChatsByProject(userId, projectId.ToUpperString(), dateTime).OrderByDescending(x => x.DateTime));
        }

        [HttpGet("GetTopUnreadMessages")]
        public IActionResult GetTopUnreadMessages()
        {
            var userId = User.Identity.Name;
            var unreadChats = _repoSupervisor.Chats.GetTopUnreadChatMessagesPerProject(userId, 5).OrderByDescending(x => x.DateTime);

            return Ok(unreadChats);
        }

        [HttpGet("GetTopSavedMessages")]
        public IActionResult GetTopSavedMessages()
        {
            var userId = User.Identity.Name;
            var savedMessages = _repoSupervisor.Chats.GetTopSavedMessagesPerProject(userId, 5);

            return Ok(savedMessages);
        }

        [HttpGet("GetChatParticipants/{chatId}")]
        public IActionResult GetChatParticipants(Guid chatId)
        {
            return Ok(_repoSupervisor.Chats.GetChatParticipants(chatId.ToUpperString()));
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("GetParticipantPhoto/{participantId}")]
        public async Task<IActionResult> GetParticipantPhoto(Guid participantId)
        {
            var filePath = _repoSupervisor.Persons.GetPersonPhotoLocationByChatParticipantId(participantId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetChatItems/{chatId}")]
        public IActionResult GetChatItems(Guid chatId, DateTime? dateTime, bool newer)
        {
            return Ok(_repoSupervisor.Chats.GetChatMessages(chatId.ToUpperString(), 10, dateTime?.ToLocalTime(), newer));
        }

        [HttpGet("GetChatItemsByMessageId/{chatMessageId}")]
        public IActionResult GetChatItemsByMessageId(Guid chatMessageId)
        {
            var chatMessage = _repoSupervisor.Chats.GetChatMessageByMessageId(chatMessageId.ToUpperString());
            var olderMessages = _repoSupervisor.Chats.GetChatMessages(chatMessage.ChatId, 10, chatMessage.DateTime, false);
            var newerMessages = _repoSupervisor.Chats.GetChatMessages(chatMessage.ChatId, 10, chatMessage.DateTime, true);

            var result = olderMessages.Append(chatMessage).Union(newerMessages);

            return Ok(result);
        }

        [HttpGet("GetMessageAttachment/{chatMessageId}")]
        public async Task<IActionResult> GetMessageAttachment(Guid chatMessageId)
        {
            var filePath = _repoSupervisor.Attachments.GetChatAttachmentLocation(chatMessageId.ToUpperString());
            return await GetFileStream(filePath).ConfigureAwait(false);
        }

        [HttpGet("GetChatStartListByBuilding/{buildingId}")]
        public IActionResult GetChatStartListByBuilding(Guid buildingId)
        {
            return Ok(_repoSupervisor.Chats.GetChatStartListByBuilding(buildingId.ToUpperString()).OrderBy(x => x.Title));
        }

        [HttpGet("GetChatStartListByProject/{projectId}")]
        public IActionResult GetChatStartListByProject(Guid projectId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetChatStartListByProject(userId, projectId.ToUpperString()).OrderBy(x => x.Title));
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddNewChat")]
        public IActionResult AddNewChat([FromBody] AddNewChatApiModel newChat)
        {
            if (newChat == null)
                return BadRequest("Model is Empty.");

            var userId = User.Identity.Name;
            var projectId = _repoSupervisor.UserObjects.GetProjectIdForBuilding(newChat.BuildingId);
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, projectId, newChat.BuildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => (x.RoleName == LoginRoles.BuyersGuide || x.RoleName == LoginRoles.BuyerOrRenter)))
            {
                return Ok(_repoSupervisor.Chats.AddNewChat(userId, newChat));
            }
            return BadRequest("Can not add new chat");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddNewChatMessage")]
        public IActionResult AddNewChatMessage([FromBody] AddNewChatMessageApiModel newChatMessage)
        {
            if (newChatMessage == null)
                return BadRequest("Model is Empty.");

            var userId = User.Identity.Name;
            var chat = _repoSupervisor.Chats.GetChatById(userId, newChatMessage.ChatId);
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, chat.ProjectId, chat.BuildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => x.RoleName != LoginRoles.Spectator))
            {
                var chatMessageId = _repoSupervisor.Chats.AddNewChatMessage(userId, newChatMessage);

                _repoSupervisor.Complete();

                return Ok(new { chatMessageId });
            }
            return BadRequest("Can not add new chat message");
        }


        [Authorize(PolicyConstants.FullAccess)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]//10MB
        [HttpPost("UploadAttachment/{chatId}")]
        public IActionResult UploadAttachment(Guid chatId, IFormFile file)
        {
            if (file == null)
                return BadRequest("File is null");
            if (file.Length == 0)
                return BadRequest("File is empty");

            var userId = User.Identity.Name;
            var chat = _repoSupervisor.Chats.GetChatById(userId, chatId.ToUpperString());
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, chat.ProjectId, chat.BuildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => x.RoleName != LoginRoles.Spectator))
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileBytes = binaryReader.ReadBytes((int)file.Length);
                    var uploadLocation = _repoSupervisor.UserObjects.GetUploadLocationForBuilding(chat.BuildingId);
                    var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');


                    AddNewChatMessageApiModel newChatMessage = new AddNewChatMessageApiModel { ChatId = chat.ChatId, Message = originalFileName };
                    var chatMessageId = _repoSupervisor.Chats.AddNewChatMessage(userId, newChatMessage);
                    string filePath = _repoSupervisor.Attachments.AddNewChatMessageAttachment(
                        chatMessageId,
                        chat.BuildingId,
                        _appSettings.AttachmentHeaders.ChatMessageUpload,
                        originalFileName,
                        uploadLocation
                        );

                    if (!Directory.Exists(uploadLocation))
                    {
                        Directory.CreateDirectory(uploadLocation);
                    }
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    _repoSupervisor.Complete();
                }

                return Ok(true);
            }
            return BadRequest("Can not upload attachment");
        }


        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteChatMessage/{chatMessageId}")]
        public IActionResult DeleteChatMessage(Guid chatMessageId)
        {
            var userId = User.Identity.Name;
            var chatMessage = _repoSupervisor.Chats.GetChatMessageByMessageId(chatMessageId.ToUpperString());
            var chat = chatMessage != null ? _repoSupervisor.Chats.GetChatById(userId, chatMessage.ChatId) : null;
            var IsUserBuyerGuide = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, chat.ProjectId, chat.BuildingId).Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.BuyersGuide);
            if (IsUserBuyerGuide)
            {
                _repoSupervisor.Chats.MarkChatMessageDeleted(userId, chatMessageId.ToUpperString());
                _repoSupervisor.Complete();
                return Ok(true);
            }
            return BadRequest("Can not delete chat message");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("MarkLastReadChatItem/{chatMessageId}")]
        public IActionResult MarkLastReadChatItem(Guid chatMessageId)
        {
            var userId = User.Identity.Name;
            var chatMessage = _repoSupervisor.Chats.GetChatMessageByMessageId(chatMessageId.ToUpperString());
            var chat = chatMessage != null ? _repoSupervisor.Chats.GetChatById(userId, chatMessage.ChatId) : null;
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, chat.ProjectId, chat.BuildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => x.RoleName != LoginRoles.Spectator))
            {
                _repoSupervisor.Chats.MarkChatMessageAsRead(userId, chatMessageId.ToUpperString());
                _repoSupervisor.Complete();
                return Ok(true);
            }
            return BadRequest("Can not mark last read chat item.");
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("MarkUnmarkChatMessageImportant/{chatMessageId}")]
        public IActionResult MarkUnmarkChatMessageImportant(Guid chatMessageId, bool isMark)
        {
            var userId = User.Identity.Name;
            var chatMessage = _repoSupervisor.Chats.GetChatMessageByMessageId(chatMessageId.ToUpperString());
            var chat = chatMessage != null ? _repoSupervisor.Chats.GetChatById(userId, chatMessage.ChatId) : null;
            var availbleModule = _repoSupervisor.Logins.GetAvailableModulesWithRolesForUser(userId, chat.ProjectId, chat.BuildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availbleModule.Any(x => x.RoleName != LoginRoles.Spectator))
            {
                if (isMark)
                    _repoSupervisor.Chats.MarkChatMessageAsImportant(userId, chatMessageId.ToUpperString());
                else
                    _repoSupervisor.Chats.UnMarkChatMessageAsImportant(userId, chatMessageId.ToUpperString());
                _repoSupervisor.Complete();
                return Ok(true);
            }
            return BadRequest("Can not mark unmark chat message important.");
        }

        [HttpGet("GetImportantMessagesByBuilding/{buildingId}")]
        public IActionResult GetImportantMessagesByBuilding(Guid buildingId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetImportantMessagesByBuilding(userId, buildingId.ToUpperString()));
        }

        [HttpGet("GetImportantMessagesByProject/{projectId}")]
        public IActionResult GetImportantMessagesByProject(Guid projectId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetImportantMessagesByProject(userId, projectId.ToUpperString()));
        }

        [HttpPost("SearchChats")]
        public IActionResult SearchChats(SearchChatApiModel searchChatApiModel)
        {
            if (searchChatApiModel == null)
                return BadRequest("Search model is null.");
            if (string.IsNullOrWhiteSpace(searchChatApiModel.SearchTerm) && !searchChatApiModel.Attachment)
                return BadRequest("Search term is empty.");

            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.SearchChatMessages(userId, searchChatApiModel));
        }

        [HttpGet("GetStandardTextsByProject/{projectId}")]
        public IActionResult GetStandardTextsByProject(Guid projectId)
        {
            var userId = User.Identity.Name;
            return Ok(_repoSupervisor.Chats.GetStandardTextsByProject(userId, projectId.ToUpperString()));
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("AddStandardTextForProject")]
        public IActionResult AddStandardTextForProject(StandardTextApiModel standardText)
        {
            if (standardText == null)
                return BadRequest("Model is empty");
            Guid projectId;
            if (!Guid.TryParse(standardText.ProjectId, out projectId) || projectId == Guid.Empty)
                return BadRequest("projectId can not be NULL.");
            if (string.IsNullOrWhiteSpace(standardText.Hashtag))
                return BadRequest("Hashtag can not be empty.");
            if (string.IsNullOrWhiteSpace(standardText.TextBlock))
                return BadRequest("Text can not be empty.");

            var userId = User.Identity.Name;
            _repoSupervisor.Chats.AddStandardText(userId, standardText);
            _repoSupervisor.Complete();
            return Ok(true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPatch("UpdateStandardText")]
        public IActionResult UpdateStandardText(StandardTextApiModel standardText)
        {
            if (standardText == null)
                return BadRequest("Model is empty");
            Guid textId;
            if (!Guid.TryParse(standardText.TextId, out textId) || textId == Guid.Empty)
                return BadRequest("textId can not be NULL.");
            if (string.IsNullOrWhiteSpace(standardText.Hashtag))
                return BadRequest("Hashtag can not be empty.");
            if (string.IsNullOrWhiteSpace(standardText.TextBlock))
                return BadRequest("Text can not be empty.");

            var userId = User.Identity.Name;
            _repoSupervisor.Chats.UpdateStandardText(userId, standardText);
            _repoSupervisor.Complete();
            return Ok(true);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpDelete("DeleteStandardText/{textId}")]
        public IActionResult DeleteStandardText(Guid textId)
        {
            var userId = User.Identity.Name;
            _repoSupervisor.Chats.DeleteStandardText(userId, textId.ToUpperString());
            _repoSupervisor.Complete();
            return Ok(true);
        }
    }
}