using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class BetrokkeneRelatie : BaseEntity
    {
        public string BetrokkeneGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string FunctieGuid { get; set; }
        public short? Volgorde { get; set; }
        public bool? Publiceren { get; set; }
        public string Doorkiesnummer { get; set; }
        public string Mobiel { get; set; }
        public string EmailZakelijk { get; set; }
        public virtual Betrokkene BetrokkeneGu { get; set; }
        public virtual Functie FunctieGu { get; set; }
    }
}
