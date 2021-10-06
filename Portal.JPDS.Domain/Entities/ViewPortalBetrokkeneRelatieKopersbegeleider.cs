using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalBetrokkeneRelatieKopersbegeleider
    {
        public string BetrokkeneRelatieGuid { get; set; }
        public string WerkGuid { get; set; }
        public string BetrokkeneGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string LoginGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string VolledigeNaam { get; set; }
        public string Naam { get; set; }
        public string Functie { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
        public string Mobiel { get; set; }
        public string Foto { get; set; }
        public short? Volgorde { get; set; }
    }
}
