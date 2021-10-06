using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class GebouwSoort : BaseEntity
    {
        public GebouwSoort()
        {
            Gebouws = new HashSet<Gebouw>();
        }
        public string GebouwSoort1 { get; set; }
        public byte? GebouwGebruiker { get; set; }
        public bool? GebouwNaamRegistreren { get; set; }
        public bool? Systeemwaarde { get; set; }

        public virtual ICollection<Gebouw> Gebouws { get; set; }
    }
}
