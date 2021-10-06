using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class StandardOptionApiModel
    {
        public StandardOptionApiModel() { }
        public StandardOptionApiModel(ViewPortalOptieStandaardPerGebouw entity)
        {
            OptionStandardId = entity.OptieStandaardGuid;
            BuildingId = entity.GebouwGuid;
            OptionCategoryProjectId = entity.OptieCategorieWerkGuid;
            OptionHeaderProjectId = entity.OptieRubriekWerkGuid;
            OptionNo = entity.Optienummer;
            Description = entity.Omschrijving;
            CommercialDescription = entity.CommercieleOmschrijving;
            Quantity = entity.Aantal;
            Unit = entity.Eenheid;
            UnitDecimalPlaces = entity.EenheidAantalDecimalen;
            SalesPriceToBeDetermined = entity.VerkoopprijsNtb;
            SalesPriceEstimated = entity.VerkoopprijsIsStelpost;
            SalesPriceExclVAT = entity.VerkoopprijsExclBtw;
            SalesPriceExclVAT_Text = entity.VerkoopprijsExclBtwOfTekstPrijskolom;
            SalesPriceInclVAT = entity.VerkoopprijsInclBtw;
            SalesPriceInclVAT_Text = entity.VerkoopprijsInclBtwOfTekstPrijskolom;
        }
        public string OptionStandardId { get; set; }
        public string BuildingId { get; set; }
        public string OptionCategoryProjectId { get; set; }
        public string OptionHeaderProjectId { get; set; }
        public string OptionNo { get; set; }
        public string Description { get; set; }
        public string CommercialDescription { get; set; }
        public decimal? Quantity { get; set; }
        public string Unit { get; set; }
        public int? UnitDecimalPlaces { get; set; }
        /// <summary>
        /// Dutch: VerkoopprijsIsStelpost
        /// </summary>
        public bool SalesPriceEstimated { get; set; }
        /// <summary>
        /// Dutch: VerkoopprijsNtb
        /// </summary>
        public bool SalesPriceToBeDetermined { get; set; }
        public decimal? SalesPriceExclVAT { get; set; }
        public string SalesPriceExclVAT_Text { get; set; }
        public decimal? SalesPriceInclVAT { get; set; }
        public string SalesPriceInclVAT_Text { get; set; }
    }

    public class OptionCategoryApiModel
    {
        public OptionCategoryApiModel() { }
        public OptionCategoryApiModel(ViewPortalOptieCategorieSluitingsdatum entity)
        {
            OptionCategoryProjectId = entity.OptieCategorieWerkGuid;
            Category = entity.Categorie;
            ClosingDate = entity.Sluitingsdatum;
            Closed = entity.Sluitingsdatum < DateTime.Now.Date;
            Order = entity.Volgorde;
        }

        public string OptionCategoryProjectId { get; set; }
        public string Category { get; set; }
        public bool Closed { get; set; }
        public DateTime? ClosingDate { get; set; }
        public short? Order { get; set; }
        public IEnumerable<OptionHeaderApiModel> Headers { get; set; }
    }

    public class OptionHeaderApiModel
    {
        public OptionHeaderApiModel() { }
        public OptionHeaderApiModel(OptieRubriekWerk entity)
        {
            OptionHeaderProjectId = entity.Guid;
            Header = entity.Rubriek;
            Order = entity.Volgorde;
        }
        public string OptionHeaderProjectId { get; set; }
        public string Header { get; set; }
        public short? Order { get; set; }
    }

    public class OptionGroupedCategoryHeaderApiModel<T>
    {
        public IEnumerable<OptionCategoryApiModel> Categories { get; set; }
        public IEnumerable<T> Options { get; set; }
    }

    public class StandardOptionProjectApiModel
    {
        public StandardOptionProjectApiModel() { }
        public StandardOptionProjectApiModel(ViewPortalOptieStandaardPerWerk entity)
        {
            OptionStandardId = entity.OptieStandaardGuid;
            Category = entity.Categorie;
            Header = entity.Rubriek;
            OptionNo = entity.Optienummer;
            Description = entity.Omschrijving;
            CommercialDescription = entity.CommercieleOmschrijving;
            Quantity = entity.Aantal;
            Unit = entity.Eenheid;
            UnitDecimalPlaces = entity.EenheidAantalDecimalen;
            SalesPriceToBeDetermined = entity.VerkoopprijsNtb;
            SalesPriceEstimated = entity.VerkoopprijsIsStelpost;
            SalesPriceExclVAT = entity.VerkoopprijsExclBtw;
            SalesPriceExclVAT_Text = entity.VerkoopprijsExclBtwOfTekstPrijskolom;
            SalesPriceInclVAT = entity.VerkoopprijsInclBtw;
            SalesPriceInclVAT_Text = entity.VerkoopprijsInclBtwOfTekstPrijskolom;
            NoOfAttachments = entity.AantalBijlagen;
        }

        public string OptionStandardId { get; set; }
        public string BuildingId { get; set; }
        public string Category { get; set; }
        public string Header { get; set; }
        public string OptionNo { get; set; }
        public string Description { get; set; }
        public string CommercialDescription { get; set; }
        public decimal? Quantity { get; set; }
        public string Unit { get; set; }
        public int? UnitDecimalPlaces { get; set; }
        /// <summary>
        /// Dutch: VerkoopprijsIsStelpost
        /// </summary>
        public bool SalesPriceEstimated { get; set; }
        /// <summary>
        /// Dutch: VerkoopprijsNtb
        /// </summary>
        public bool SalesPriceToBeDetermined { get; set; }
        public decimal? SalesPriceExclVAT { get; set; }
        public string SalesPriceExclVAT_Text { get; set; }
        public decimal? SalesPriceInclVAT { get; set; }
        public string SalesPriceInclVAT_Text { get; set; }
        public int? NoOfAttachments { get; set; }
    }
}
