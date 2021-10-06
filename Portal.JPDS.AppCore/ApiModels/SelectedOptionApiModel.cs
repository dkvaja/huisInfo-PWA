using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class SelectedOptionApiModel
    {
        public SelectedOptionApiModel() { }
        public SelectedOptionApiModel(OptieGekozen entity)
        {
            BuildingId = entity.GebouwGuid;
            OptionCategoryProjectId = entity.OptieCategorieWerkGuid;
            OptionHeaderProjectId = entity.OptieRubriekWerkGuid;
            OptionId = entity.Guid;
            OptionNo = entity.Optienummer;
            OptionStandardId = entity.OptieStandaardGuid;
            Type = entity.Soort;
            Description = entity.Omschrijving;
            CommercialDescription = entity.CommercieleOmschrijving;
            AdditionalDescription = entity.AanvullendeOmschrijving;
            Quantity = entity.Aantal;
            UnitDecimalPlaces = entity.EenheidGu?.AantalDecimalen;
            Unit = entity.EenheidGu?.Eenheid1;
            Status = entity.OptieStatus;
            SubStatus = entity.OptieSubstatus;
            SalesPriceEstimated = entity.VerkoopprijsIsStelpost == true;
            SalesPriceToBeDetermined = entity.VerkoopprijsNtb == true;
            SalesPriceInclVAT = entity.VerkoopprijsInclBtw;
            SalesPriceInclVAT_Text = entity.VerkoopprijsInclBtwOfTekstPrijskolom;
            Closed = entity.Sluitingsdatum < DateTime.Now.Date;
            ModifiedOn = entity.GewijzigdOp;
        }
        public SelectedOptionApiModel(ViewPortalOptieGekozen entity)
        {
            BuildingId = entity.GebouwGuid;
            OptionCategoryProjectId = entity.OptieCategorieWerkGuid;
            Category = entity.Categorie;
            OptionHeaderProjectId = entity.OptieRubriekWerkGuid;
            Header = entity.Rubriek;
            OptionId = entity.Guid;
            OptionNo = entity.Optienummer;
            OptionStandardId = entity.OptieStandaardGuid;
            Type = entity.Soort;
            Description = entity.Omschrijving;
            CommercialDescription = entity.CommercieleOmschrijving;
            AdditionalDescription = entity.AanvullendeOmschrijving;
            Quantity = entity.Aantal;
            UnitDecimalPlaces = entity.AantalDecimalen;
            Unit = entity.Eenheid;
            Status = entity.OptieStatus;
            SubStatus = entity.OptieSubstatus;
            SalesPriceEstimated = entity.VerkoopprijsIsStelpost;
            SalesPriceToBeDetermined = entity.VerkoopprijsNtb;
            SalesPriceInclVAT = entity.VerkoopprijsInclBtw;
            SalesPriceInclVAT_Text = entity.VerkoopprijsInclBtwOfTekstPrijskolom;
            Closed = entity.Sluitingsdatum < DateTime.Now.Date;
            ModifiedOn = null;
        }

        public string BuildingId { get; set; }
        public string OptionCategoryProjectId { get; set; }
        public string Category { get; set; }
        public string OptionHeaderProjectId { get; set; }
        public string Header { get; set; }
        public string OptionId { get; set; }
        public string OptionNo { get; set; }
        public string OptionStandardId { get; set; }
        public short Type { get; set; }
        public string Description { get; set; }
        public string CommercialDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitDecimalPlaces { get; set; }
        public string Unit { get; set; }
        public short Status { get; set; }
        public int? SubStatus { get; set; }

        /// <summary>
        /// Dutch: VerkoopprijsIsStelpost
        /// </summary>
        public bool SalesPriceEstimated { get; set; }
        /// <summary>
        /// Dutch: VerkoopprijsNtb
        /// </summary>
        public bool SalesPriceToBeDetermined { get; set; }
        public decimal? SalesPriceInclVAT { get; set; }
        public string SalesPriceInclVAT_Text { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Closed { get; set; }
    }

    public class SelectedOptionsOverviewApiModel
    {
        public IEnumerable<SelectedOptionApiModel> AvailableIndividualOptions { get; set; }
        public IEnumerable<SelectedOptionApiModel> OptionsInShoppingCart { get; set; }
        public IEnumerable<SelectedOptionApiModel> RequestedToBeJudged { get; set; }
        public IEnumerable<QuotationApiModel> AvailableQuotations { get; set; }
        public IEnumerable<QuotationApiModel> OrderedOnlineButNotSentToBeSigned { get; set; }
        public IEnumerable<QuotationApiModel> OrderedOnlineAndSentToBeSigned { get; set; }
        public IEnumerable<QuotationApiModel> SignedToBeReviewed { get; set; }
        public IEnumerable<SelectedOptionApiModel> Definite { get; set; }
        public IEnumerable<QuotationApiModel> Cancelled { get; set; }
    }
}
