using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingLocatie : BaseEntity
    {
        public MeldingLocatie()
        {
            Melding = new HashSet<Melding>();
        }

        public string Locatie { get; set; }
        public bool? Actief { get; set; }
        
        public virtual ICollection<Melding> Melding { get; set; }
    }
}
