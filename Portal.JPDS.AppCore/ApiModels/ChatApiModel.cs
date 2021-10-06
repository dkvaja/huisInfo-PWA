using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class ChatApiModel
    {
        public ChatApiModel() { }
        public ChatApiModel(ViewPortalChat entity)
        {
            ChatId = entity.ChatGuid;
            BuildingId = entity.GebouwGuid;
            OrganisationId = entity.OrganisatieGuid;
            OrganisationName = entity.OrganisatieNaam;
            ChatParticipantId = entity.ChatDeelnemerGuid;
            BuildingNoExtern = entity.BouwnummerExtern;
            DateTime = entity.DatumEnTijdLaatstVerzondenChatBericht ?? entity.ChatBegonnenOp;
            LastChatMessagePartialText = entity.EersteRegelLaatsteChatBericht;
            UnreadMessagesCount = entity.AantalOngelezenBerichten ?? 0;
            IsSender = entity.LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid == entity.ChatDeelnemerGuid;
            SenderName = entity.LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam;
            LastMessageIsAttachment = !string.IsNullOrWhiteSpace(entity.LaatsteChatBerichtBijlage);
            ChatSubject = entity.ChatOnderwerp;
        }

        public string ChatId { get; set; }
        public string BuildingId { get; set; }
        public string OrganisationId { get; set; }
        public string ChatParticipantId { get; set; }
        public string OrganisationName { get; set; }
        public string BuildingNoExtern { get; set; }
        public DateTime? DateTime { get; set; }
        public string LastChatMessagePartialText { get; set; }
        public int UnreadMessagesCount { get; set; }
        public bool IsSender { get; set; }
        public string SenderName { get; set; }
        public bool LastMessageIsAttachment { get; set; }
        public string ChatSubject { get; set; }
    }

    public class ChatParticipantApiModel
    {
        public ChatParticipantApiModel() { }
        public ChatParticipantApiModel(ChatDeelnemer entity)
        {
            ChatParticipantId = entity.Guid;
            LoginId = entity.LoginGuid;
            Name = entity.LoginGu?.Naam;
            PersonId = entity.LoginGu?.PersoonGuid;
            HasPhoto = File.Exists(entity.LoginGu?.PersoonGu?.Foto ?? string.Empty);
            IsBuyer = entity.LoginGu?.LoginAccountVoor == (int)AccountType.Buyer;
        }
        public string ChatParticipantId { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }
        public string PersonId { get; set; }
        public bool HasPhoto { get; set; }
        public bool IsBuyer { get; set; }
    }

    public class ChatMessageApiModel
    {
        public ChatMessageApiModel() { }
        public ChatMessageApiModel(ViewPortalChatBericht entity)
        {
            ChatMessageId = entity.ChatBerichtGuid;
            ChatId = entity.ChatGuid;
            SenderChatParticipantId = entity.VerzenderChatDeelnemerGuid;
            SenderLoginId = entity.VerzenderLoginGuid;
            SenderName = entity.VerzenderNaam;
            Message = entity.Bericht;
            DateTime = entity.DatumEnTijd;
            ActionId = entity.ActieGuid;
            Important = entity.Belangrijk;
            Deleted = entity.Verwijderd;
            DeletedByParticipant = entity.VerwijderdDoorDeelnemer;
            if (!string.IsNullOrEmpty(entity.Bijlage))
            {
                //Message = (Message ?? string.Empty) + Path.GetExtension(entity.Bijlage);
                IsFile = true;
                IsImage = entity.Bijlage.IsImage();
            }
            ReplyToChatMessageId = entity.ReactieOpChatBerichtGuid;
            ReplyToChatMessageMessage = entity.ReactieOpChatBerichtBericht;
            ReplyToChatMessageSenderChatParticipantId = entity.ReactieOpChatBerichtVerzenderChatDeelnemerGuid;
            ReplyToChatMessageDeleted = entity.ReactieOpChatBerichtVerwijderd == true;
            ReplyToChatMessageIsFile = !string.IsNullOrEmpty(entity.ReactieOpChatBerichtBijlage);
            if (!string.IsNullOrEmpty(entity.ReactieOpChatBerichtBijlage))
            {
                //ReplyToChatMessageMessage = (ReplyToChatMessageMessage ?? string.Empty) + Path.GetExtension(entity.ReactieOpChatBerichtBijlage);
                ReplyToChatMessageIsFile = true;
                ReplyToChatMessageIsImage = entity.ReactieOpChatBerichtBijlage.IsImage();
            }
        }


        public string ChatMessageId { get; set; }
        public string ChatId { get; set; }
        public string SenderChatParticipantId { get; set; }
        public string SenderLoginId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string ActionId { get; set; }
        public bool Important { get; set; }
        public bool Deleted { get; set; }
        public string DeletedByParticipant { get; set; }
        public bool IsFile { get; set; }
        public bool IsImage { get; set; }
        public string ReplyToChatMessageId { get; set; }
        public string ReplyToChatMessageMessage { get; set; }
        public string ReplyToChatMessageSenderChatParticipantId { get; set; }
        public bool ReplyToChatMessageDeleted { get; set; }
        public bool ReplyToChatMessageIsFile { get; set; }
        public bool ReplyToChatMessageIsImage { get; set; }
    }

    public class ChatMessageDetailsApiModel
    {
        public ChatMessageDetailsApiModel(ChatBericht chatBericht)
        {
            ChatId = chatBericht.ChatGuid;
            BuildingId = chatBericht.ChatGu?.GebouwGuid;
            ProjectId = chatBericht.ChatGu?.GebouwGu?.WerkGuid;
            BuildingNoIntern = chatBericht.ChatGu?.GebouwGu?.BouwnummerIntern;
            BuildingNoExtern = chatBericht.ChatGu?.GebouwGu?.BouwnummerExtern;
            ChatMessageId = chatBericht.Guid;
            Message = chatBericht.Bericht;
            IsImportant = chatBericht.Belangrijk;
            var bijlage = chatBericht.Bijlage.FirstOrDefault();
            if (bijlage != null && !string.IsNullOrEmpty(bijlage.Bijlage1))
            {
                IsFile = true;
                IsImage = bijlage.Bijlage1.IsImage();
            }
            SenderName = chatBericht.VerzenderChatDeelnemerGu?.LoginGu?.Naam;
            SenderChatParticipantId = chatBericht.VerzenderChatDeelnemerGuid;
            DateTime = chatBericht.DatumEnTijd;
        }

        public ChatMessageDetailsApiModel(ViewPortalChat chat, ViewPortalChatBericht chatMessage)
        {
            ChatId = chat.ChatGuid;
            BuildingId = chat.GebouwGuid;
            ProjectId = chat.WerkGuid;
            BuildingNoIntern = chat.BouwnummerIntern;
            BuildingNoExtern = chat.BouwnummerExtern;
            ChatMessageId = chatMessage.ChatBerichtGuid;
            Message = chatMessage.Bericht;
            IsImportant = chatMessage.Belangrijk;
            if (!string.IsNullOrEmpty(chatMessage.Bijlage))
            {
                IsFile = true;
                IsImage = chatMessage.Bijlage.IsImage();
            }
            SenderName = chatMessage.VerzenderNaam;
            SenderChatParticipantId = chatMessage.VerzenderChatDeelnemerGuid;
            DateTime = chatMessage.DatumEnTijd;
        }

        public string ChatId { get; set; }
        public string BuildingId { get; set; }
        public string ProjectId { get; set; }
        public string BuildingNoIntern { get; set; }
        public string BuildingNoExtern { get; set; }
        public string ChatMessageId { get; set; }
        public string Message { get; set; }
        public bool IsImportant { get; set; }
        public bool IsFile { get; set; }
        public bool IsImage { get; set; }
        public string SenderName { get; set; }
        public string SenderChatParticipantId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class ChatStartApiModel
    {
        public ChatStartApiModel() { }
        public ChatStartApiModel(ViewPortalChatGebouwStart entity)
        {
            Title = entity.OrganisatieNaam;
            BuildingId = entity.GebouwGuid;
            OrganisationId = entity.OrganisatieGuid;
        }
        public ChatStartApiModel(ViewPortalChatRelatieStart entity)
        {
            Title = entity.BouwnummerExtern;
            BuildingId = entity.GebouwGuid;
            OrganisationId = entity.OrganisatieGuid;
        }
        public string Title { get; set; }
        public string BuildingId { get; set; }
        public string OrganisationId { get; set; }
    }

    public class AddNewChatApiModel
    {
        public string BuildingId { get; set; }
        public string OrganisationId { get; set; }
        public string Subject { get; set; }
    }

    public class AddNewChatMessageApiModel
    {
        public string ChatId { get; set; }
        public string Message { get; set; }
        public bool Important { get; set; }
        public string ReplyToChatMessageId { get; set; }
    }

    public class ImportantChatMessagesApiModel
    {
        public ImportantChatMessagesApiModel() { }
        public ImportantChatMessagesApiModel(ViewPortalChatBerichtBelangrijk importantChatMessage)
        {
            ChatMessageId = importantChatMessage.ChatBerichtGuid;
            Message = importantChatMessage.Bericht;
            DateTime = importantChatMessage.DatumEnTijd;
            ChatParticipantId = importantChatMessage.ChatDeelnemerGuid;
            IsFile = !string.IsNullOrEmpty(importantChatMessage.Bijlage);
        }

        public string ChatMessageId { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string ChatParticipantId { get; set; }
        public bool IsFile { get; set; }
    }

    public class ImportantChatsApiModel
    {
        public static IEnumerable<ImportantChatsApiModel> GroupImprotantChatMessagesByChat(IEnumerable<ViewPortalChatBerichtBelangrijk> importantChatMessages)
        {
            var resultList = new List<ImportantChatsApiModel>();
            foreach (var groupItem in importantChatMessages.GroupBy(x => x.ChatGuid))
            {
                var firstItemForCommonInfo = groupItem.FirstOrDefault();
                var item = new ImportantChatsApiModel()
                {
                    ChatId = groupItem.Key,
                    BuildingId = firstItemForCommonInfo.GebouwGuid,
                    BuildingNoExtern = firstItemForCommonInfo.BouwnummerExtern,
                    OrganisationId = firstItemForCommonInfo.OrganisatieGuid,
                    OrganisationName = firstItemForCommonInfo.OrganisatieNaam,
                    DateTime = groupItem.Max(x => x.DatumEnTijd)
                };

                item.Messages = groupItem.Select(x => new ImportantChatMessagesApiModel(x)).OrderBy(x => x.DateTime);

                resultList.Add(item);
            }

            return resultList.OrderByDescending(x => x.DateTime);
        }

        public string ChatId { get; set; }
        public string BuildingId { get; set; }
        public string BuildingNoExtern { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<ImportantChatMessagesApiModel> Messages { get; set; }
    }

    public class SearchChatApiModel
    {
        public Guid? ChatId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? ProjectId { get; set; }
        public string SearchTerm { get; set; }
        public Guid? OrganisationId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool Attachment { get; set; }
        public int Count { get; set; }
    }

    public class SearchChatResultApiModel
    {
        public SearchChatResultApiModel() { }
        public SearchChatResultApiModel(ViewPortalChatZoeken viewResult)
        {
            ChatId = viewResult.ChatGuid;
            ChatMessageId = viewResult.ChatBerichtGuid;
            BuildingId = viewResult.GebouwGuid;
            OrganisationId = viewResult.OrganisatieGuid;
            ChatParticipantId = viewResult.ChatDeelnemerGuid;
            OrganisationName = viewResult.OrganisatieNaam;
            BuildingNoExtern = viewResult.BouwnummerExtern;
            DateTime = viewResult.DatumEnTijd;
            Message = viewResult.Bericht;
            IsSender = viewResult.VerzenderChatDeelnemerGuid == viewResult.ChatDeelnemerGuid;
            SenderName = viewResult.VerzenderNaam;
            IsFile = viewResult.BijlageAanwezig != 0;
            ChatSubject = viewResult.ChatOnderwerp;
        }
        public string ChatId { get; set; }
        public string ChatMessageId { get; set; }
        public string BuildingId { get; set; }
        public string OrganisationId { get; set; }
        public string ChatParticipantId { get; set; }
        public string OrganisationName { get; set; }
        public string BuildingNoExtern { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public bool IsSender { get; set; }
        public string SenderName { get; set; }
        public bool IsFile { get; set; }
        public string ChatSubject { get; set; }
    }

    public class ChatModel
    {
        public string ChatId { get; set; }
        public string ProjectId { get; set; }
        public string BuildingId { get; set; }
    }
}
