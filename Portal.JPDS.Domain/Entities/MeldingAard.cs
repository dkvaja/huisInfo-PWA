using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingAard:BaseEntity
    {
        public MeldingAard()
        {
            Melding = new HashSet<Melding>();
        }

        public string Aard { get; set; }
        public bool? Actief { get; set; }
        
        public virtual ICollection<Melding> Melding { get; set; }
    }
}
