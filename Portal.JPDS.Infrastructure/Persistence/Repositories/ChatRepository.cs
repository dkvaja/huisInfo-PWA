using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public ChatMessageApiModel GetChatMessageByMessageId(string messageId)
        {
            return new ChatMessageApiModel(_dbContext.ViewPortalChatBericht.SingleOrDefault(x => x.ChatBerichtGuid == messageId));
        }

        public IEnumerable<ChatMessageApiModel> GetChatMessages(string chatId, int count, DateTime? dateTime, bool newer)
        {
            var queryBuilder = _dbContext.ViewPortalChatBericht.Where(x => x.ChatGuid == chatId);
            if (dateTime.HasValue)
            {
                if (newer)
                {
                    queryBuilder = queryBuilder.Where(x => x.DatumEnTijd > dateTime).OrderBy(x => x.DatumEnTijd);
                }
                else
                {
                    queryBuilder = queryBuilder.Where(x => x.DatumEnTijd < dateTime).OrderByDescending(x => x.DatumEnTijd);
                }
            }
            else
            {
                queryBuilder = queryBuilder.OrderByDescending(x => x.DatumEnTijd);
            }
            queryBuilder = queryBuilder.Take(count);

            return queryBuilder.Select(x => new ChatMessageApiModel(x));
        }

        public IEnumerable<ChatParticipantApiModel> GetChatParticipants(string chatId)
        {
            return _dbContext.ChatDeelnemer
                .Include(x => x.LoginGu)
                .ThenInclude(x => x.PersoonGu)
                .Where(x => x.ChatGuid == chatId)
                .Select(x => new ChatParticipantApiModel(x));
        }

        public ChatModel GetChatById(string loginId, string chatId)
        {
            var chat = _dbContext.ViewPortalChat
                .SingleOrDefault(x => x.LoginGuid == loginId
                && x.ChatGuid == chatId);

            return new ChatModel
            {
                ChatId = chat.ChatGuid,
                ProjectId = chat.WerkGuid,
                BuildingId = chat.GebouwGuid
            };
        }

        public IEnumerable<ChatApiModel> GetChatsByBuilding(string loginId, string buildingId, DateTime? dateTime)
        {
            IQueryable<ViewPortalChat> queryBuilder = null;
            var projectId = _dbContext.Gebouw.Find(buildingId).WerkGuid;
            var availableModuleRole = GetAvailableModulesWithRoles(loginId, projectId, buildingId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availableModuleRole.Any(x => x.RoleName == LoginRoles.Spectator))
            {
                queryBuilder = _dbContext.ViewPortalChat.Where(x => x.GebouwGuid == buildingId)
                    .Select(x => new
                    {
                        ChatId = x.ChatGuid,
                        BuildingId = x.GebouwGuid,
                        OrganisationId = x.OrganisatieGuid,
                        OrganisationName = x.OrganisatieNaam,
                        ChatParticipantId = (string)null,
                        BuildingNoExtern = x.BouwnummerExtern,
                        DateAndTimeLastSentChatMessage = x.DatumEnTijdLaatstVerzondenChatBericht,
                        ChatStartedOn = x.ChatBegonnenOp,
                        LastChatMessagePartialText = x.EersteRegelLaatsteChatBericht,
                        UnreadMessagesCount = 0,
                        SenderName = x.LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam,
                        LastMessageAttachment = x.LaatsteChatBerichtBijlage,
                        ChatSubject = x.ChatOnderwerp,
                        ProjectId = x.WerkGuid,
                        LoginId = (string)null,
                        BuildingNointern = x.BouwnummerIntern,
                        ChatStartedByLoginGuid = x.ChatBegonnenDoorLoginGuid,
                        LastChatMessageSentBy = x.LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid,
                        LastReadChatMessage = (DateTime?)null
                    })
                    .Distinct()
                    .Select(x => new ViewPortalChat
                    {
                        ChatGuid = x.ChatId,
                        GebouwGuid = x.BuildingId,
                        OrganisatieGuid = x.OrganisationId,
                        OrganisatieNaam = x.OrganisationName,
                        ChatDeelnemerGuid = x.ChatParticipantId,
                        BouwnummerExtern = x.BuildingNoExtern,
                        AantalOngelezenBerichten = x.UnreadMessagesCount,
                        LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam = x.SenderName,
                        EersteRegelLaatsteChatBericht = x.LastChatMessagePartialText,
                        LaatsteChatBerichtBijlage = x.LastMessageAttachment,
                        ChatOnderwerp = x.ChatSubject,
                        ChatBegonnenOp = x.ChatStartedOn,
                        DatumEnTijdLaatstVerzondenChatBericht = x.DateAndTimeLastSentChatMessage,
                        WerkGuid = x.ProjectId,
                        LoginGuid = x.LoginId,
                        BouwnummerIntern = x.BuildingNointern,
                        ChatBegonnenDoorLoginGuid = x.ChatStartedByLoginGuid,
                        LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid = x.LastChatMessageSentBy,
                        LaatstGelezenChatBericht = x.LastReadChatMessage
                    })
                    .AsQueryable();
            }
            else
            {
                queryBuilder = _dbContext.ViewPortalChat.Where(x => x.LoginGuid == loginId && x.GebouwGuid == buildingId);
            }
            if (dateTime.HasValue)
                queryBuilder = queryBuilder.Where(x => x.DatumEnTijdLaatstVerzondenChatBericht > dateTime || x.ChatBegonnenOp > dateTime);

            return queryBuilder.Select(x => new ChatApiModel(x));
        }

        public IEnumerable<ChatApiModel> GetChatsByProject(string loginId, string projectId, DateTime? dateTime)
        {
            IQueryable<ViewPortalChat> queryBuilder = null;
            var availableModuleRole = GetAvailableModulesWithRoles(loginId, projectId).Where(x => x.ModuleName == LoginModules.BuyerGuide);
            if (availableModuleRole.Any(x => x.RoleName == LoginRoles.Spectator))
            {
                var buildings = availableModuleRole.Where(x => x.RoleName == LoginRoles.Spectator).Select(x => x.BuildingId).Distinct();
                queryBuilder = _dbContext.ViewPortalChat.Where(x => x.WerkGuid == projectId)
                   .Select(x => new
                   {
                       ChatId = x.ChatGuid,
                       BuildingId = x.GebouwGuid,
                       OrganisationId = x.OrganisatieGuid,
                       OrganisationName = x.OrganisatieNaam,
                       ChatParticipantId = (string)null,
                       BuildingNoExtern = x.BouwnummerExtern,
                       DateAndTimeLastSentChatMessage = x.DatumEnTijdLaatstVerzondenChatBericht,
                       ChatStartedOn = x.ChatBegonnenOp,
                       LastChatMessagePartialText = x.EersteRegelLaatsteChatBericht,
                       UnreadMessagesCount = 0,
                       SenderName = x.LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam,
                       LastMessageAttachment = x.LaatsteChatBerichtBijlage,
                       ChatSubject = x.ChatOnderwerp,
                       ProjectId = x.WerkGuid,
                       LoginId = (string)null,
                       BuildingNointern = x.BouwnummerIntern,
                       ChatStartedByLoginGuid = x.ChatBegonnenDoorLoginGuid,
                       LastChatMessageSentBy = x.LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid,
                       LastReadChatMessage = (DateTime?)null
                   })
                   .Distinct()
                   .Select(x => new ViewPortalChat
                   {
                       ChatGuid = x.ChatId,
                       GebouwGuid = x.BuildingId,
                       OrganisatieGuid = x.OrganisationId,
                       OrganisatieNaam = x.OrganisationName,
                       ChatDeelnemerGuid = x.ChatParticipantId,
                       BouwnummerExtern = x.BuildingNoExtern,
                       AantalOngelezenBerichten = x.UnreadMessagesCount,
                       LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam = x.SenderName,
                       EersteRegelLaatsteChatBericht = x.LastChatMessagePartialText,
                       LaatsteChatBerichtBijlage = x.LastMessageAttachment,
                       ChatOnderwerp = x.ChatSubject,
                       ChatBegonnenOp = x.ChatStartedOn,
                       DatumEnTijdLaatstVerzondenChatBericht = x.DateAndTimeLastSentChatMessage,
                       WerkGuid = x.ProjectId,
                       LoginGuid = x.LoginId,
                       BouwnummerIntern = x.BuildingNointern,
                       ChatBegonnenDoorLoginGuid = x.ChatStartedByLoginGuid,
                       LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid = x.LastChatMessageSentBy,
                       LaatstGelezenChatBericht = x.LastReadChatMessage
                   })
                   .Where(y => buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid))
                   .AsQueryable();
            }
            else
            {
                queryBuilder = _dbContext.ViewPortalChat.Where(x => x.LoginGuid == loginId && x.WerkGuid == projectId);
            }
            if (dateTime.HasValue)
                queryBuilder = queryBuilder.Where(x => x.DatumEnTijdLaatstVerzondenChatBericht > dateTime || x.ChatBegonnenOp > dateTime);

            return queryBuilder.Select(x => new ChatApiModel(x));
        }

        public IEnumerable<ChatMessageDetailsApiModel> GetTopUnreadChatMessagesPerProject(string loginId, int resultsPerProject)
        {
            var queryBuilder = _dbContext.ViewPortalChat
                .Join(_dbContext.ViewPortalChatBericht, x => x.ChatGuid, y => y.ChatGuid, (chat, chatMessage) =>
                new
                {
                    chat,
                    chatMessage,
                })
                .Where(x => x.chat.LoginGuid == loginId && x.chat.AantalOngelezenBerichten > 0
                && x.chatMessage.VerzenderChatDeelnemerGuid != x.chat.ChatDeelnemerGuid
                && (!x.chat.LaatstGelezenChatBericht.HasValue || x.chatMessage.DatumEnTijd > x.chat.LaatstGelezenChatBericht)
                && x.chatMessage.Verwijderd != true);

            var grouped = _dbContext.ViewPortalChat
                .Where(x => x.LoginGuid == loginId && x.AantalOngelezenBerichten > 0)
                .Select(x => x.WerkGuid).Distinct()
                .SelectMany(x => queryBuilder.Where(y => y.chat.WerkGuid == x).OrderByDescending(x => x.chatMessage.DatumEnTijd).Take(resultsPerProject));


            return grouped.Select(x => new ChatMessageDetailsApiModel(x.chat, x.chatMessage));
        }

        public IEnumerable<ChatStartApiModel> GetChatStartListByBuilding(string buildingId)
        {
            return _dbContext.ViewPortalChatGebouwStart.Where(x => x.GebouwGuid == buildingId).Select(x => new ChatStartApiModel(x));
        }
        public IEnumerable<ChatStartApiModel> GetChatStartListByProject(string loginId, string projectId)
        {
            var relationId = _dbContext.Login.Where(x => x.Guid == loginId && !x.Verwijderd).Select(x => x.RelatieGuid).SingleOrDefault();

            return _dbContext.ViewPortalChatRelatieStart.Where(x => x.RelatieGuid == relationId && x.WerkGuid == projectId).Select(x => new ChatStartApiModel(x));
        }

        /// <summary>
        /// Checks if chat already exists for userId, buildingId and organisationId then returns chatId,
        /// otherwise creates new and returns chatId
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddNewChat(string loginId, AddNewChatApiModel model)
        {
            string chatId = GetDefaultChatIdForBuildingAndOrganisationForUser(loginId.ToUpperInvariant(), model.BuildingId.ToUpperInvariant(), model.OrganisationId.ToUpperInvariant());
            if (string.IsNullOrEmpty(chatId))
            {

                var sql = "chat_toevoegen @onderwerp, @gebouw_guid, @organisatie_guid, @chat_begonnen_door_login_guid, @chat_guid OUTPUT";
                var outputParameter = new SqlParameter
                {
                    ParameterName = "@chat_guid",
                    DbType = System.Data.DbType.String,
                    Size = 40,
                    Direction = System.Data.ParameterDirection.Output
                };

                _dbContext.Database.ExecuteSqlRaw(sql,
                    new SqlParameter("@onderwerp", model.Subject),
                    new SqlParameter("@gebouw_guid", model.BuildingId),
                    new SqlParameter("@organisatie_guid", model.OrganisationId),
                    new SqlParameter("@chat_begonnen_door_login_guid", loginId),
                    outputParameter
                    );

                chatId = outputParameter.Value.ToString();
            }

            return chatId;
        }

        public string AddNewChatMessage(string loginId, AddNewChatMessageApiModel model)
        {
            var chatParticipant = _dbContext.ChatDeelnemer.SingleOrDefault(x => x.ChatGuid == model.ChatId && x.LoginGuid == loginId);

            ChatBericht chatBericht = new ChatBericht
            {
                Guid = Guid.NewGuid().ToUpperString(),
                ChatGuid = model.ChatId,
                VerzenderChatDeelnemerGuid = chatParticipant.Guid,
                ReactieOpChatBerichtGuid = model.ReplyToChatMessageId,
                Belangrijk = model.Important,
                Bericht = model.Message,
                DatumEnTijd = DateTime.Now,
                ActieGuid = null
            };

            _dbContext.ChatBericht.Add(chatBericht);

            return chatBericht.Guid;
        }

        public void MarkChatMessageDeleted(string loginId, string messageId)
        {
            var chatMessage = _dbContext.ChatBericht.Find(messageId);
            if (!string.IsNullOrWhiteSpace(chatMessage.VerzenderChatDeelnemerGuid))
            {
                var chatParticipant = _dbContext.ChatDeelnemer.SingleOrDefault(x => x.ChatGuid == chatMessage.ChatGuid && x.LoginGuid == loginId);

                chatMessage.Verwijderd = true;
                chatMessage.VerwijderdDoorDeelnemerGuid = chatParticipant.Guid;
            }
        }

        public void MarkChatMessageAsRead(string loginId, string messageId)
        {
            var chatMessage = _dbContext.ChatBericht.Find(messageId);
            var chatParticipant = _dbContext.ChatDeelnemer.SingleOrDefault(x => x.ChatGuid == chatMessage.ChatGuid && x.LoginGuid == loginId);
            //Update only when the existing datetime value is null or less than the one is being updated with.
            if (!chatParticipant.LaatstGelezenChatBericht.HasValue || chatParticipant.LaatstGelezenChatBericht < chatMessage.DatumEnTijd)
            {
                chatParticipant.LaatstGelezenChatBericht = chatMessage.DatumEnTijd;
            }
        }

        public void MarkChatMessageAsImportant(string loginId, string messageId)
        {
            var chatMessage = _dbContext.ChatBericht.Find(messageId);
            var chatParticipant = _dbContext.ChatDeelnemer.SingleOrDefault(x => x.ChatGuid == chatMessage.ChatGuid && x.LoginGuid == loginId);
            var resultCount = _dbContext.ChatBerichtBelangrijk.Where(x => x.ChatDeelnemerGuid == chatParticipant.Guid && x.ChatBerichtGuid == messageId).Count();
            if (resultCount == 0)
            {
                ChatBerichtBelangrijk chatBerichtBelangrijk = new ChatBerichtBelangrijk
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    ChatBerichtGuid = chatMessage.Guid,
                    ChatDeelnemerGuid = chatParticipant.Guid
                };

                _dbContext.ChatBerichtBelangrijk.Add(chatBerichtBelangrijk);
            }
        }

        public void UnMarkChatMessageAsImportant(string loginId, string messageId)
        {
            var chatMessage = _dbContext.ChatBericht.Find(messageId);
            var chatParticipant = _dbContext.ChatDeelnemer.SingleOrDefault(x => x.ChatGuid == chatMessage.ChatGuid && x.LoginGuid == loginId);
            var itemToDelete = _dbContext.ChatBerichtBelangrijk.Where(x => x.ChatDeelnemerGuid == chatParticipant.Guid && x.ChatBerichtGuid == messageId).FirstOrDefault();
            if (itemToDelete != null)
            {
                _dbContext.ChatBerichtBelangrijk.Remove(itemToDelete);
            }
        }

        public IEnumerable<ImportantChatsApiModel> GetImportantMessagesByBuilding(string userId, string buildingId)
        {
            var queryBuilder = _dbContext.ViewPortalChatBerichtBelangrijk.Where(x => x.LoginGuid == userId && x.GebouwGuid == buildingId);
            return ImportantChatsApiModel.GroupImprotantChatMessagesByChat(queryBuilder);
        }

        public IEnumerable<ImportantChatsApiModel> GetImportantMessagesByProject(string userId, string projectId)
        {
            var queryBuilder = _dbContext.ViewPortalChatBerichtBelangrijk.Where(x => x.LoginGuid == userId && x.WerkGuid == projectId);
            return ImportantChatsApiModel.GroupImprotantChatMessagesByChat(queryBuilder);
        }

        public IEnumerable<ChatMessageDetailsApiModel> GetTopSavedMessagesPerProject(string userId, int resultsPerProject)
        {
            var subQuery = _dbContext.ChatBericht.Include(x => x.ChatGu).ThenInclude(x => x.GebouwGu)
                .Where(x => x.ChatBerichtBelangrijk.Any(y => y.ChatDeelnemerGu.LoginGuid == userId)).Select(x => x.ChatGu.GebouwGu.WerkGuid).Distinct();
            var queryBuilder = _dbContext.ChatBericht.Include(y => y.ChatGu).ThenInclude(y => y.GebouwGu)
                                .Include(y => y.VerzenderChatDeelnemerGu).ThenInclude(y => y.LoginGu)
                                .Include(y => y.Bijlage)
                                .Where(y => subQuery.Contains(y.ChatGu.GebouwGu.WerkGuid) && y.ChatBerichtBelangrijk.Any(z => z.ChatDeelnemerGu.LoginGuid == userId) && !y.Verwijderd)
                                .OrderByDescending(x => x.DatumEnTijd)
                                .Take(resultsPerProject);

            return queryBuilder.Select(x => new ChatMessageDetailsApiModel(x));
        }

        public IEnumerable<SearchChatResultApiModel> SearchChatMessages(string userId, SearchChatApiModel model)
        {
            IQueryable<ViewPortalChatZoeken> queryBuilder = null;
            if (model.ProjectId.HasValue || model.BuildingId.HasValue || model.ChatId.HasValue)
            {
                IEnumerable<UserModuleModel> availableModule = null;
                if (model.ProjectId.HasValue)
                {
                    if (model.BuildingId.HasValue)
                    {
                        availableModule = GetAvailableModulesWithRoles(userId, model.ProjectId.Value.ToUpperString(), model.BuildingId.Value.ToUpperString());
                    }
                    else
                    {
                        availableModule = GetAvailableModulesWithRoles(userId, model.ProjectId.Value.ToUpperString());
                    }
                }
                else if (model.BuildingId.HasValue)
                {
                    var projectId = _dbContext.Gebouw.Find(model.BuildingId.Value.ToUpperString()).WerkGuid;
                    availableModule = GetAvailableModulesWithRoles(userId, projectId, model.BuildingId.Value.ToUpperString());
                }
                else if (model.ChatId.HasValue)
                {
                    var chat = _dbContext.Chat.Find(model.ChatId.Value.ToUpperString());
                    availableModule = GetAvailableModulesWithRoles(userId, chat.WerkGuid, chat.GebouwGuid);
                }
                if (availableModule.Any(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.Spectator))
                {
                    var buildings = availableModule.Where(x => x.ModuleName == LoginModules.BuyerGuide &&  x.RoleName == LoginRoles.Spectator).Select(x => x.BuildingId).Distinct();
                    queryBuilder = _dbContext.ViewPortalChatZoeken.Select(x => new
                    {
                        ChatId = x.ChatGuid,
                        ChatMessageId = x.ChatBerichtGuid,
                        BuildingId = x.GebouwGuid,
                        OrganisationId = x.OrganisatieGuid,
                        ChatParticipantId = (string)null,
                        OrganisationName = x.OrganisatieNaam,
                        BuildingNoExtern = x.BouwnummerExtern,
                        DateTime = x.DatumEnTijd,
                        Message = x.Bericht,
                        SenderChatParticipantGuid = x.VerzenderChatDeelnemerGuid,
                        SenderName = x.VerzenderNaam,
                        AttachmentPresent = x.BijlageAanwezig,
                        ChatSubject = x.ChatOnderwerp,
                        ProjectId = x.WerkGuid,
                        LoginId = (string)null,
                        BuildingNoIntern = x.BouwnummerIntern
                    })
                    .Distinct()
                    .Select(x => new ViewPortalChatZoeken
                    {
                        ChatGuid = x.ChatId,
                        ChatBerichtGuid = x.ChatMessageId,
                        GebouwGuid = x.BuildingId,
                        OrganisatieGuid = x.OrganisationId,
                        ChatDeelnemerGuid = x.ChatParticipantId,
                        OrganisatieNaam = x.OrganisationName,
                        BouwnummerExtern = x.BuildingNoExtern,
                        DatumEnTijd = x.DateTime,
                        Bericht = x.Message,
                        BijlageAanwezig = x.AttachmentPresent,
                        VerzenderChatDeelnemerGuid = x.SenderChatParticipantGuid,
                        VerzenderNaam = x.SenderName,
                        ChatOnderwerp = x.ChatSubject,
                        WerkGuid = x.ProjectId,
                        LoginGuid = x.LoginId,
                        BouwnummerIntern = x.BuildingNoIntern
                    })
                    .Where(y => buildings.Any(z => z == null) || buildings.Contains(y.GebouwGuid))
                    .AsQueryable();
                }
                else
                {
                    queryBuilder = _dbContext.ViewPortalChatZoeken.Where(x => x.LoginGuid == userId);
                }

                if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                    queryBuilder = queryBuilder.Where(x => x.Bericht.Contains(model.SearchTerm));

                if (model.ChatId.HasValue)
                    queryBuilder = queryBuilder.Where(x => x.ChatGuid == model.ChatId.Value.ToUpperString());

                if (model.BuildingId.HasValue)
                    queryBuilder = queryBuilder.Where(x => x.GebouwGuid == model.BuildingId.Value.ToUpperString());

                if (model.ProjectId.HasValue)
                    queryBuilder = queryBuilder.Where(x => x.WerkGuid == model.ProjectId.Value.ToUpperString());

                if (model.OrganisationId.HasValue)
                    queryBuilder = queryBuilder.Where(x => x.OrganisatieGuid == model.OrganisationId.Value.ToUpperString());

                if (model.Attachment)
                    queryBuilder = queryBuilder.Where(x => x.BijlageAanwezig != 0);

                if (model.DateTime.HasValue)
                    queryBuilder = queryBuilder.Where(x => x.DatumEnTijd < model.DateTime);

                queryBuilder = queryBuilder.OrderByDescending(x => x.DatumEnTijd).Take(model.Count);

                return queryBuilder.Select(x => new SearchChatResultApiModel(x));
            }
            return (new List<SearchChatResultApiModel>()).AsQueryable();
        }

        public void SendMessageToDefaultChat(string buildingId, string message, string email)
        {
            var sql = "chat_en_chat_bericht_toevoegen @gebouw_guid, @verzender_email, @bericht";
            _dbContext.Database.ExecuteSqlRaw(sql,
                new SqlParameter("@gebouw_guid", buildingId),
                new SqlParameter("@verzender_email", email ?? string.Empty),//if email is empty then it will be set as system message
                new SqlParameter("@bericht", message)
                );
        }

        public int GetCountOfChatsWithNewMessagesByBuilding(string userId, string buildingId)
        {
            return _dbContext.ViewPortalChat.Count(x => x.LoginGuid == userId && x.GebouwGuid == buildingId && x.AantalOngelezenBerichten > 0);
        }

        public int GetCountOfChatsWithNewMessagesByProject(string userId, string projectId)
        {
            return _dbContext.ViewPortalChat.Count(x => x.LoginGuid == userId && x.WerkGuid == projectId && x.AantalOngelezenBerichten > 0);
        }

        public IEnumerable<object> GetCountOfNewMessagesPerBuilding(string userId)
        {
            var spectatorData = GetAvailableModulesWithRoles(userId)
                .Where(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.Spectator)
                .Select(x => new
                {
                    x.BuildingId,
                    x.ProjectId
                }).ToList();
            var spectatorForProject = spectatorData.Where(x => x.BuildingId == null).Select(x=>x.ProjectId).Distinct().ToList();
            var spectatorForBuilding = spectatorData.Where(x => x.BuildingId != null).Select(x => x.BuildingId).Distinct().ToList();

            return _dbContext.ViewPortalChat
                .Where(x => x.LoginGuid == userId && x.AantalOngelezenBerichten > 0 && !spectatorForProject.Contains(x.WerkGuid) && !spectatorForBuilding.Contains(x.GebouwGuid))
                .GroupBy(x => new { x.GebouwGuid, x.WerkGuid })
                .Select(x => new
                {
                    BuildingId = x.Key.GebouwGuid,
                    ProjectId = x.Key.WerkGuid,
                    NoOfChats = x.Count(),
                    Count = x.Sum(y => y.AantalOngelezenBerichten)
                });
        }

        public int GetCountOfImportantMessagesByBuilding(string userId, string buildingId)
        {
            return _dbContext.ViewPortalChatBerichtBelangrijk.Where(x => x.LoginGuid == userId && x.GebouwGuid == buildingId).Count();
        }

        public IEnumerable<object> GetCountOfImportantMessagesPerBuilding(string userId)
        {
            var spectatorData = GetAvailableModulesWithRoles(userId)
                .Where(x => x.ModuleName == LoginModules.BuyerGuide && x.RoleName == LoginRoles.Spectator)
                .Select(x => new
                {
                    x.BuildingId,
                    x.ProjectId
                }).ToList();
            var spectatorForProject = spectatorData.Where(x => x.BuildingId == null).Select(x => x.ProjectId).Distinct().ToList();
            var spectatorForBuilding = spectatorData.Where(x => x.BuildingId != null).Select(x => x.BuildingId).Distinct().ToList();

            return _dbContext.ViewPortalChatBerichtBelangrijk
                .Where(x => x.LoginGuid == userId && !spectatorForProject.Contains(x.WerkGuid) && !spectatorForBuilding.Contains(x.GebouwGuid))
                .GroupBy(x => new { x.GebouwGuid, x.WerkGuid })
                .Select(x => new
                {
                    BuildingId = x.Key.GebouwGuid,
                    ProjectId = x.Key.WerkGuid,
                    Count = x.Count()
                });
        }

        public int GetCountOfImportantMessagesByProject(string userId, string projectId)
        {
            return _dbContext.ViewPortalChatBerichtBelangrijk.Where(x => x.LoginGuid == userId && x.WerkGuid == projectId).Count();
        }

        public string GetDefaultChatIdForBuildingAndOrganisationForUser(string userId, string buildingId, string organisationId)
        {
            return _dbContext.ViewPortalChat.Where(x => x.LoginGuid == userId && x.GebouwGuid == buildingId && x.OrganisatieGuid == organisationId)
                //to get the latest one
                .OrderByDescending(x => x.DatumEnTijdLaatstVerzondenChatBericht ?? x.ChatBegonnenOp)
                .Select(x => x.ChatGuid).FirstOrDefault();
        }

        public IEnumerable<StandardTextApiModel> GetStandardTextsByProject(string userId, string projectId)
        {
            return _dbContext.Tekstblok
                .Where(x => x.LoginGuid == userId && x.WerkGuid == projectId)
                .OrderBy(x => x.Volgorde)
                .Select(x => new StandardTextApiModel(x));
        }

        public void AddStandardText(string userId, StandardTextApiModel model)
        {
            var maxOrder = _dbContext.Tekstblok.Where(x => x.LoginGuid == userId && x.WerkGuid == model.ProjectId).Max(x => x.Volgorde) ?? 0;
            maxOrder++;
            var textBlock = new Tekstblok
            {
                Guid = Guid.NewGuid().ToUpperString(),
                LoginGuid = userId,
                WerkGuid = model.ProjectId,
                Zoekterm = model.Hashtag,
                Tekstblok1 = model.TextBlock,
                TekstblokIsHandtekening = model.IsSignature,
                Volgorde = maxOrder
            };

            _dbContext.Tekstblok.Add(textBlock);
        }

        public bool UpdateStandardText(string userId, StandardTextApiModel model)
        {
            var textBlock = _dbContext.Tekstblok.Find(model.TextId);
            if (textBlock != null && textBlock.LoginGuid == userId)
            {
                if (model.IsSignature && !string.IsNullOrWhiteSpace(textBlock.WerkGuid))
                {
                    var removeSignatureBlocks = _dbContext.Tekstblok.Where(x => x.Guid != textBlock.Guid && x.LoginGuid == userId && x.WerkGuid == textBlock.WerkGuid && x.TekstblokIsHandtekening);
                    foreach (var block in removeSignatureBlocks)
                    {
                        block.TekstblokIsHandtekening = false;
                    }
                }

                textBlock.Zoekterm = model.Hashtag;
                textBlock.Tekstblok1 = model.TextBlock;
                textBlock.TekstblokIsHandtekening = model.IsSignature;

                return true;
            }
            return false;
        }

        public void DeleteStandardText(string userId, string textId)
        {
            var textBlock = _dbContext.Tekstblok.Find(textId);
            if (textBlock != null && textBlock.LoginGuid == userId)
            {
                _dbContext.Tekstblok.Remove(textBlock);
            }
        }
    }
}
