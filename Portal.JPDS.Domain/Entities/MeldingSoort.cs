using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingSoort : BaseEntity
    {
        public MeldingSoort()
        {
            Melding = new HashSet<Melding>();
            Oplossers = new HashSet<Oplosser>();
        }

        public string MeldingSoort1 { get; set; }
        public bool? Actief { get; set; }
        public bool? Systeemwaarde { get; set; }

        public virtual ICollection<Melding> Melding { get; set; }
        public virtual ICollection<Oplosser> Oplossers { get; set; }
    }
}
