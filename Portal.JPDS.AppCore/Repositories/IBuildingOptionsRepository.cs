using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
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
    public interface IBuildingOptionsRepository
    {
        IEnumerable<QuotationApiModel> GetQuotationsForBuilding(string buildingId);
        int GetQuotationsCountForBuilding(string buildingId);
        IEnumerable<QuotationApiModel> GetQuotationsByBuildingIdAndStatuses(string buildingId, SelectedOptionStatus status, SelectedOptionSubStatus? subStatus);
        QuotationApiModel GetQuotationById(string quoteId);
        IEnumerable<OptionCategoryApiModel> GetStandardOptionCategoriesByBuildingId(string buildingId);
        IEnumerable<OptionHeaderApiModel> GetStandardOptionHeadersByProjectId(string projectId);
        IEnumerable<StandardOptionApiModel> GetStandardOptionsByBuildingId(string buildingId);
        IEnumerable<StandardOptionProjectApiModel> GetStandardOptionsByProjectId(string projectId);
        IEnumerable<SelectedOptionApiModel> GetSelectedOptionsByBuildingIdAndStatuses(string buildingId, SelectedOptionStatus status, SelectedOptionSubStatus subStatus);
        bool UpdateQuotationStatus(string quoteId, SelectedOptionStatus optionStatus, SelectedOptionSubStatus optionSubStatus, string documentStatus = null, long? documentId = null);
        IEnumerable<SelectedOptionApiModel> GetAvailableIndividualOptions(string buildingId);
        void AddOrUpdateToCartIndividualOption(AddToCartApiModel model, string userId);
        void AddOrUpdateToCartStandardOption(AddToCartApiModel model, string userId);
        void DeleteSelectedOption(string selectedOptionId);
        /// <summary>
        /// Request all selected options in shopping cart and Return true if any of them has Additional Description
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        bool RequestSelectedOptions(string buildingId);
        IEnumerable<SelectedOptionApiModel> GetRequestedOptionsByBuildingId(string buildingId);
        IEnumerable<SelectedOptionApiModel> GetSeletedDefiniteOptionsByBuildingId(string buildingId);
        int GetCountOfItemsInShoppingCart(string buildingId);
        DigitalSignInfoModel GetSigningInfoForQuotation(string quoteId, string userId);
        string GetProjectIdForStandardOption(string optionStandardId);
    }
}
