using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class UserObjectApiModel
    {
        public UserObjectApiModel()
        {
        }

        public UserObjectApiModel(ViewPortalGebouwKoperHuurder entity)
        {
            BuildingId = entity.GebouwGuid;
            ProjectId = entity.WerkGuid;
            BuildingBuyerRenterId = entity.GebouwKoperHuurderGuid;
            BuyerGuideId = entity.RelatieGuid;
            ProjectNo = entity.WerkWerknummer;
            ProjectName = entity.WerkWerknaam;
            BuildingNoIntern = entity.GebouwBouwnummerIntern;
            BuildingNoExtern = entity.GebouwBouwnummerExtern;
        }
        public UserObjectApiModel(ViewPortalGebouwRelatie entity, Gebouw building)
        {
            BuildingId = entity.GebouwGuid;
            ProjectId = entity.WerkGuid;
            BuildingBuyerRenterId = entity.GebouwKoperHuurderGuid;
            BuyerGuideId = entity.RelatieGuid;
            ProjectNo = entity.WerkWerknummer;
            ProjectName = entity.WerkWerknaam;
            BuildingNoIntern = entity.GebouwBouwnummerIntern;
            BuildingNoExtern = entity.GebouwBouwnummerExtern;
            PropertyType = entity.GebouwWoningType;
            BuildingType = entity.GebouwSoort;
            BuyerRenterName = building?.KoperHuurderGu?.VolledigeNaam;
            ConstructionFlow = building?.BouwstroomGu?.Bouwstroom1;
            Status = entity.GebouwStatus;
            Address = entity.AdresVolledigAdres;
        }

        public UserObjectApiModel(Gebouw entity)
        {
            BuildingId = entity.Guid;
            ProjectId = entity.WerkGuid;
            BuildingBuyerRenterId = entity.KoperHuurderGuid;
            ProjectNo = entity.WerkGu.Werknummer;
            ProjectName = entity.WerkGu.WerknummerWerknaam;
            BuildingNoIntern = entity.BouwnummerIntern;
            BuildingNoExtern = entity.BouwnummerExtern;
            Address = entity.AdresGu?.VolledigAdres;
        }

        public string BuildingId { get; set; }
        public string ProjectId { get; set; }
        public string BuildingBuyerRenterId { get; set; }
        public string BuyerGuideId { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public string BuildingNoIntern { get; set; }
        public string BuildingNoExtern { get; set; }
        public string PropertyType { get; set; }
        public string BuildingType { get; set; }
        public string BuyerRenterName { get; set; }
        public string ConstructionFlow { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserObjectForSurveyApiModel
    {
        public UserObjectForSurveyApiModel()
        { }

        public UserObjectForSurveyApiModel(Opname entity, Gebouw gebouw)
        {
            SurveyId = entity.Guid;
            SurveyType = (SurveyType)entity.OpnameSoort;
            BuildingId = entity.GebouwGuid;
            ProjectId = entity.WerkGuid;
            BuildingBuyerRenterId = gebouw.KoperHuurderGuid ?? string.Empty;
            ProjectNo = entity.WerkGu.Werknummer ?? string.Empty;
            ProjectName = entity.WerkGu.WerknummerWerknaam ?? string.Empty;
            BuildingNoIntern = gebouw.BouwnummerIntern ?? string.Empty;
            BuildingNoExtern = gebouw.BouwnummerExtern ?? string.Empty;
            Address = gebouw.AdresGu?.VolledigAdres ?? string.Empty;
            ExecutedBy = entity.VolledigeNaamUitvoerder ?? string.Empty;
            Date = entity.Datum;
            Status = (SurveyStatus)entity.OpnameStatus;
            BuyerRenter1 = entity.VolledigeNaamKh1 ?? string.Empty;
            BuyerRenter2 = entity.VolledigeNaamKh2 ?? string.Empty;
            RepairRequestsCount = entity.Melding.Count;
        }
        public string SurveyId { get; set; }
        public SurveyType SurveyType { get; set; }
        public string BuildingId { get; set; }
        public string ProjectId { get; set; }
        public string BuildingBuyerRenterId { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public string BuildingNoIntern { get; set; }
        public string BuildingNoExtern { get; set; }
        public string Address { get; set; }
        public string ExecutedBy { get; set; }
        public DateTime? Date { get; set; }
        public SurveyStatus Status { get; set; }
        public string BuyerRenter1 { get; set; }
        public string BuyerRenter2 { get; set; }
        public int RepairRequestsCount { get; set; }
    }
}
