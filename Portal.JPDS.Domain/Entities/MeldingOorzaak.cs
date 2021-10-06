using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingOorzaak : BaseEntity
    {
        public MeldingOorzaak()
        {
            Melding = new HashSet<Melding>();
        }

        public string Oorzaak { get; set; }
        public bool? Actief { get; set; }
        
        public virtual ICollection<Melding> Melding { get; set; }
    }
}
