using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingType:BaseEntity
    {
        public MeldingType()
        {
            Melding = new HashSet<Melding>();
        }

        public string MeldingType1 { get; set; }
        public bool? Actief { get; set; }

        public virtual ICollection<Melding> Melding { get; set; }
    }
}
