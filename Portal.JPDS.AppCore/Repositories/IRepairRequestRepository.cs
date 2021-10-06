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
    public interface IRepairRequestRepository
    {
        IEnumerable<RepairRequestApiModel> GetRepairRequestsByBuildingId(string buildingId);
        IEnumerable<RepairRequestApiModel> GetRepairRequestsByProjectId(string projectId);
        IEnumerable<RepairRequestApiModel> GetRepairRequestsForSurvey(string surveyId);
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestLocations();
        RepairRequestApiModel AddRepairRequest(NewRepairRequestApiModel newRepairRequest);
        bool UpdateRepairRequest(UpdateRepairRequestApiModel updateRepairRequest);
        bool DeleteRepairRequest(string repairRequestId, out string[] filesToDelete);
        void AddRepairRequestResolvers(string repairRequestId, List<AddRepairRequestResolverApiModel> lstModel);
        bool UpdateRepairRequestResolverRelation(string resolverId, string relationId);
        bool DeleteRepairRequestResolver(string resolverId);
        List<RepairRequestAttachmentApiModel> GetRepairRequestAttachments(string repairRequestId);
        bool DeleteAttachment(string repairRequestId, string attachmentId, out string fileToDelete);
        RepairRequestApiModel GetRepairRequestById(string repairRequestId);
        RepairRequestApiModel GetRepairRequestDetails(string repairRequestId);
        ResolverForWorkOrderApiModel GetResolverDetailsForWorkOrder(string resolverId);
        ResolverForWorkOrderApiModel UpdateWorkOrderByDirectLink(WorkOrderApiModel model);
        Dictionary<string, string> GetDefaultEmailTokens(string repairRequestId, string employeeId = null);
        bool MarkCompletedByBuyer(BuyerAgreementApiModel buyerAgreement);
        bool UpdateRework(ReWorkApiModel updateRepairRequest);
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestTypeList();
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestNatureList();
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestCauseList();
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestCauserList();
        IEnumerable<CommonKeyValueApiModel> GetRepairRequestCarryOutAsTypeList();
        bool UpdateRepairRequestKeyValue(string repairRequestId, List<CommonKeyValueApiModel> lstKeyValues);
        string AddRepairRequestResolver(string repairRequestId, string organisationId, string relationId = null);
        bool UpdateWorkOrder(string resolverId, List<CommonKeyValueApiModel> lstKeyValues);
        RepairRequestResolverApiModel GetWorkOrderDetails(string resolverId);
        IEnumerable<RepairRequestResolverApiModel> GetWorkOrdersByProjectId(string projectId);
        bool CreateWorkOrder(string resolverId);
        string UpdateWorkOrderStatus(UpdateWorkOrderApiModel workOrder);
        bool UpdateRepairRequestStatus(string repairRequestId, bool isComplete, string completeOrRejectionText);
        Dictionary<string, string> GetDefaultEmailTokensForWorkOrder(string resolverId, string employeeId);
        RepairRequestEmailModel GetEmailsForWorkOrderResolver(string resolverId);
        bool MarkResolverStatusAsInformed(string resolverId);
        string GetResolverEmail(string resolverId);
        List<string> GetSiteManagerEmails(string resolverId);
        string GetSalutationForEmail(string resolverId, string notifyTo, string siteManagerEmail = null);
        RepairRequestEmailModel GetEmailsForRepairRequest(string repairRequestId);
        Dictionary<string, List<string>> GetRepairRequestToEmailsAndSalutation(string repairRequestId, RepairRequestEmailModel repairRequestEmails);
        string CreateCommunicationForWorkOrder(string resolverId, string emailTemplateWithoutToken, string employeeId, string emailAccountId);
        void CreateContactsForWorkorder(string resolverId, string communicationId, string emailTemplatewithToken, string toEmails, string ccEmails, ContactDetailsforReceiver receiverContactDetails);
        ContactDetailsforReceiver GetReceiverContactDetails(string resolverId, string toEmailBuildingRole);
        string GetOrgRelationName(string relationId, string organisationId = null);
        string CopyWorkOrder(string organisationId, string resolverId);
    }
}
