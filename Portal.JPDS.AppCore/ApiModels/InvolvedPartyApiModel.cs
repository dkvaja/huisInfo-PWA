using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class InvolvedPartyApiModel
    {
        public InvolvedPartyApiModel() { }
        public InvolvedPartyApiModel(ViewPortalBetrokkene entity)
        {
            ProductCode = entity.ProductCode;
            ProductDescription = entity.ProductOmschrijving;
            OrganisationNamePart = entity.OrganisatieNaamOnderdeel;
            OrganisationPostAdress_StreeNumberAddition = entity.OrganisatiePostadresStraatNummerToevoeging;
            OrganisationPostAdress_Postcode = entity.OrganisatiePostadresPostcode;
            OrganisationPostAdress_Place = entity.OrganisatiePostadresPlaats;
            OrganisationTelephone = entity.OrganisatieTelefoon;
            OrganisationEmail = entity.OrganisatieEmail;
            OrganisationWebsite = entity.OrganisatieWebsite;
        }

        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string OrganisationNamePart { get; set; }
        public string OrganisationPostAdress_StreeNumberAddition { get; set; }
        public string OrganisationPostAdress_Postcode { get; set; }
        public string OrganisationPostAdress_Place { get; set; }
        public string OrganisationTelephone { get; set; }
        public string OrganisationEmail { get; set; }
        public string OrganisationWebsite { get; set; }
    }
}
