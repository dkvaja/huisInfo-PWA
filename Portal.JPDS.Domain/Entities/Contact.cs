using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string CommunicatieGuid { get; set; }
        public byte Contact1 { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string KoperHuurderGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string EmailBericht { get; set; }
        public string Bestandsnaam { get; set; }
        public string Email { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string Naam { get; set; }
        public bool? Formeel { get; set; }
        public DateTime? EmailVerzonden { get; set; }

        public virtual Communicatie CommunicatieGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual KoperHuurder KoperHuurderGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual Persoon PersoonGu { get; set; }
        public virtual Relatie RelatieGu { get; set; }
    }
}
