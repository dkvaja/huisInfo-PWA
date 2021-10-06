using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class BijlageRubriek : BaseEntity
    {
        public BijlageRubriek()
        {
            Bijlages = new HashSet<Bijlage>();
        }
        public string Rubriek { get; set; }
        public short? Volgorde { get; set; }
        public bool? Systeemwaarde { get; set; }
        public virtual ICollection<Bijlage> Bijlages { get; set; }
    }
}
