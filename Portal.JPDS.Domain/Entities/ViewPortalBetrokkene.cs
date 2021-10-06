using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalBetrokkene
    {
        public string BetrokkeneGuid { get; set; }
        public string WerkGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string Werknummer { get; set; }
        public string Werknaam { get; set; }
        public string WerkLogo { get; set; }
        public string ProductCode { get; set; }
        public string ProductOmschrijving { get; set; }
        public string OrganisatieNaamOnderdeel { get; set; }
        public string OrganisatiePostadresStraatNummerToevoeging { get; set; }
        public string OrganisatiePostadresPostcode { get; set; }
        public string OrganisatiePostadresPlaats { get; set; }
        public string OrganisatieBezoekadresStraatNummerToevoeging { get; set; }
        public string OrganisatieBezoekadresPostcode { get; set; }
        public string OrganisatieBezoekadresPlaats { get; set; }
        public string OrganisatieTelefoon { get; set; }
        public string OrganisatieEmail { get; set; }
        public string OrganisatieWebsite { get; set; }
        public string OrganisatieLogo { get; set; }
    }
}
