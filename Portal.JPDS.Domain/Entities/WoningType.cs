using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class WoningType : BaseEntity
    {
        public WoningType()
        {
            Gebouws = new HashSet<Gebouw>();
            //OptieStandaardWoningTypes = new HashSet<OptieStandaardWoningType>();
        }
        public string WerkGuid { get; set; }
        public string WoningType1 { get; set; }

        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Gebouw> Gebouws { get; set; }
        //public virtual ICollection<OptieStandaardWoningType> OptieStandaardWoningTypes { get; set; }
    }
}
