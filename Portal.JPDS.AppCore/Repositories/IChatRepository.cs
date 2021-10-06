using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Repositories
{
    /// <summary>
    /// Author : Abhishek Saini
    /// This is interface which should be implemented in outer layer.
    /// </summary>
    public interface IChatRepository
    {
        ChatModel GetChatById(string loginId, string chatId);
        IEnumerable<ChatApiModel> GetChatsByBuilding(string loginId, string buildingId, DateTime? dateTime);
        IEnumerable<ChatApiModel> GetChatsByProject(string loginId, string projectId, DateTime? dateTime);
        IEnumerable<ChatMessageDetailsApiModel> GetTopUnreadChatMessagesPerProject(string loginId, int resultsPerProject);
        IEnumerable<ChatParticipantApiModel> GetChatParticipants(string chatId);
        ChatMessageApiModel GetChatMessageByMessageId(string messageId);
        IEnumerable<ChatMessageApiModel> GetChatMessages(string chatId, int count, DateTime? dateTime, bool newer);
        IEnumerable<ChatStartApiModel> GetChatStartListByBuilding(string buildingId);
        IEnumerable<ChatStartApiModel> GetChatStartListByProject(string loginId, string projectId);
        string AddNewChat(string loginId, AddNewChatApiModel model);
        string AddNewChatMessage(string loginId, AddNewChatMessageApiModel model);
        void MarkChatMessageDeleted(string loginId, string messageId);
        void MarkChatMessageAsRead(string loginId, string messageId);
        void MarkChatMessageAsImportant(string loginId, string messageId);
        void UnMarkChatMessageAsImportant(string loginId, string messageId);
        IEnumerable<ImportantChatsApiModel> GetImportantMessagesByBuilding(string userId, string buildingId);
        IEnumerable<ImportantChatsApiModel> GetImportantMessagesByProject(string userId, string projectId);
        IEnumerable<ChatMessageDetailsApiModel> GetTopSavedMessagesPerProject(string userId, int resultsPerProject);
        IEnumerable<SearchChatResultApiModel> SearchChatMessages(string userId, SearchChatApiModel searchChatApiModel);
        string GetDefaultChatIdForBuildingAndOrganisationForUser(string userId, string buildingId, string organisationId);
        void SendMessageToDefaultChat(string buildingId, string message, string email = null);
        int GetCountOfChatsWithNewMessagesByBuilding(string userId, string buildingId);
        int GetCountOfChatsWithNewMessagesByProject(string userId, string projectId);
        IEnumerable<object> GetCountOfNewMessagesPerBuilding(string userId);
        IEnumerable<object> GetCountOfImportantMessagesPerBuilding(string userId);
        int GetCountOfImportantMessagesByBuilding(string userId, string buildingId);
        int GetCountOfImportantMessagesByProject(string userId, string projectId);
        IEnumerable<StandardTextApiModel> GetStandardTextsByProject(string userId, string projectId);
        void AddStandardText(string userId, StandardTextApiModel model);
        bool UpdateStandardText(string userId, StandardTextApiModel model);
        void DeleteStandardText(string userId, string textId);
    }
}
