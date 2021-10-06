using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingVeroorzaker : BaseEntity
    {
        public MeldingVeroorzaker()
        {
            Melding = new HashSet<Melding>();
        }

        public string Veroorzaker { get; set; }
        public bool? Actief { get; set; }
        public bool? Systeemwaarde { get; set; }

        public virtual ICollection<Melding> Melding { get; set; }
    }
}
