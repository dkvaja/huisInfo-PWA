using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewLogin
    {
        public Guid CentralGuid { get; set; }
        public string Guid { get; set; }
        public string Email { get; set; }
        public string Gebruikersnaam { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool Actief { get; set; }
        public bool OptIn { get; set; }
        public string Naam { get; set; }
        public byte? LoginAccountVoor { get; set; }
        public string WachtwoordHash { get; set; }
        public string Wachtwoord { get; set; }
        public DateTime? LaatsteLogin { get; set; }
        public string VorigWachtwoord { get; set; }
        public string VorigWachtwoordHash { get; set; }
        public DateTime? VorigWachtwoordGeresetOp { get; set; }
        public DateTime? WijzigWachtwoordLinkAangemaakt { get; set; }
        public string KoperHuurderGuid { get; set; }
        public string MedewerkerGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string RelatieGuid { get; set; }
    }
}
