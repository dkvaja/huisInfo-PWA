using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Common;
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
    public interface IUserObjectRepository
    {
        IEnumerable<object> GetUserObjectsForApp(string userId, Apps app);
        string GetProjectIdForBuilding(string buildingId);
        string GetOrganisationIdForProject(string projectId);
        string GetUploadLocationForBuilding(string buildingId, string repairRequestNo = null);
        BuildingInfoApiModel GetBuildingInfo(string buildingId);
        ProjectInfoApiModel GetProjectInfo(string projectId);
        IEnumerable<RelaionInfoApiModel> GetProjectRelations(string projectId);
        IEnumerable<InvolvedPartyApiModel> GetInvolvedPartiesByProject(string projectId);
        BuyersInfoApiModel GetBuyerInfo(string buyerRenterId);
        IEnumerable<CommonKeyValueApiModel> GetProjectsForSelectionByPhases(string[] phases);
        IEnumerable<CommonKeyValueApiModel> GetBuildingsForSelectionByProject(string projectId);
        Dictionary<string, string> GetDefaultEmailTokensForBuyers(string buildingId);
        List<string> GetBuyerEmails(string buildingId);
        void AddFeedback(string userId, string comment);
        string GetNameForObjectBasedOnConstructionType(string buildingId);
        public IEnumerable<ProductServiceApiModel> GetProductServices();
    }
}
