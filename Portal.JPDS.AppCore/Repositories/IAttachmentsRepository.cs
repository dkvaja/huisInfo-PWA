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
    public interface IAttachmentsRepository
    {
        IEnumerable<AttachmentWithHeaderApiModel> GetAttachmentsByBuildingId(string buildingId);
        IEnumerable<AttachmentWithHeaderApiModel> GetCommonAttachmentsForProject(string projectId);
        string GetAttachmentLocation(string attachmentId, bool updateModifyTimeStamp = false);
        string GetChatAttachmentLocation(string chatMessageId);
        /// <summary>
        /// Adds new Bijlage(Attachment) row in current repo context and returns file location to be saved.
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="attachmentHeaderId"></param>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="storeLocation"></param>
        /// <returns></returns>
        string AddAttachmentForBuilding(string buildingId, string attachmentHeaderId, string fileName, string storeLocation);
        IEnumerable<AttachmentApiModel> GetQuotationDocuments(string quoteId, string attachmentHeaderId);

        /// <summary>
        /// Adds new Bijlage(Attachment) row in current repo context and returns file location to be saved.
        /// </summary>
        /// <param name="chatMessageId"></param>
        /// <param name="buildingId"></param>
        /// <param name="attachmentHeaderId"></param>
        /// <param name="fileName"></param>
        /// <param name="storeLocation"></param>
        /// <returns></returns>
        string AddNewChatMessageAttachment(string chatMessageId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation);
        string AddNewSignedDocumentAttachment(out string attachmentId, string quoteId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation, bool appendTimestampToFileName = true);
        string AddNewStandardOptionImage(string optionStandardId, string attachmentHeaderId, string fileName, string storeLocation);
        string AddNewRepairRequestAttachment(string repairRequestId, string attachmentHeaderId, string fileName, string storeLocation, string resolverId);
        IEnumerable<AttachmentApiModel> GetSelectedOptionAttachments(string selectedOptionId, bool imagesOnly = false);
        IEnumerable<AttachmentApiModel> GetStandardOptionAttachments(string standardOptionId, bool imagesOnly = false);
        bool DeleteAttachment(string attachmentId, out string fileToDelete);
        void SortAttachments(List<Guid> lstIds);
        string AddDossierFileAttachment(out string attachmentId, string projectId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation);
        IEnumerable<AttachmentWithHeaderApiModel> GetDossierAttachmentsForBuilding(string buildingId);
        string GetStandardOptionId(string attachmentId);
        string GetStandardOptionId(List<string> attachmentIds);
    }
}
