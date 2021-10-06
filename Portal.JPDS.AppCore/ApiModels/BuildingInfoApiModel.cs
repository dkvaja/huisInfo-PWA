using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class BuildingInfoApiModel
    {
        public BuildingInfoApiModel() { }
        public BuildingInfoApiModel(ViewPortalGebouwAlgemeen entity)
        {
            BuildingId = entity.GebouwGuid;
            BuildingNoExtern = entity.BouwnummerExtern;
            BuildingType = entity.GebouwSoort;
            GuaranteeScheme = entity.Garantieregeling;
            GuaranteeCertNo = entity.GarantieCertificaatNummer;
            GuaranteeCertValidFrom = entity.GarantieCertificaatGeldigVa;
            GuaranteeCertValidUntil = entity.GarantieCertificaatGeldigTm;
            PropertyType = entity.WoningType;
            FreeInNamePriceInclVAT = entity.VrijOpNaamPrijsInclBtw;
            Street = entity.Straat;
            HouseNo = entity.Huisnummer;
            HouseNoAddition = entity.HuisnummerToevoeging;
            Postcode = entity.Postcode;
            Place = entity.Plaats;
            DateEndMaintenancePeriod = entity.DatumEindeOnderhoudstermijn;
            EanElectricity = entity.EanElektriciteit;
            EanGas = entity.EanGas;
        }

        public string BuildingId { get; set; }
        public string BuildingNoExtern { get; set; }
        public string BuildingType { get; set; }
        public string GuaranteeScheme { get; set; }
        public string GuaranteeCertNo { get; set; }
        public DateTime? GuaranteeCertValidFrom { get; set; }
        public DateTime? GuaranteeCertValidUntil { get; set; }
        public string PropertyType { get; set; }
        public decimal? FreeInNamePriceInclVAT { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string HouseNoAddition { get; set; }
        public string Postcode { get; set; }
        public string Place { get; set; }
        public DateTime? DateEndMaintenancePeriod { get; set; }
        public string EanElectricity { get; set; }
        public string EanGas { get; set; }
    }
}
