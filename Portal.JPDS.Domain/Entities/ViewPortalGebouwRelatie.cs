using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalGebouwRelatie
    {
        public string GebouwGuid { get; set; }
        public string WerkGuid { get; set; }
        public string GebouwBouwstroomGuid { get; set; }
        public string GebouwKoperHuurderGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string LoginGuid { get; set; }
        public string WerkWerknummer { get; set; }
        public string WerkWerknaam { get; set; }
        public string GebouwBouwnummerIntern { get; set; }
        public string GebouwBouwnummerExtern { get; set; }
        public string GebouwSoort { get; set; }
        public string GebouwWoningType { get; set; }
        public string GebouwWoningSoort { get; set; }
        public string GebouwStatus { get; set; }
        public string AdresStraat { get; set; }
        public string AdresNummer { get; set; }
        public string AdresNummerToevoeging { get; set; }
        public string AdresPostcode { get; set; }
        public string AdresPlaats { get; set; }
        public string AdresStraatNummerToevoeging { get; set; }
        public string AdresVolledigAdres { get; set; }
    }
}
