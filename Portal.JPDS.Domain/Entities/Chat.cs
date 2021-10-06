using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public Chat()
        {
            ChatBericht = new HashSet<ChatBericht>();
            ChatDeelnemer = new HashSet<ChatDeelnemer>();
        }

        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string Onderwerp { get; set; }
        public string ChatBegonnenDoorLoginGuid { get; set; }

        public virtual Login ChatBegonnenDoorLoginGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<ChatBericht> ChatBericht { get; set; }
        public virtual ICollection<ChatDeelnemer> ChatDeelnemer { get; set; }
    }
}
