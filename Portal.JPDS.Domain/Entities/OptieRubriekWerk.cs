using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class OptieRubriekWerk : BaseEntity
    {
        public OptieRubriekWerk()
        {
            OptieGekozen = new HashSet<OptieGekozen>();
            OptieStandaards = new HashSet<OptieStandaard>();
        }

        public string WerkGuid { get; set; }
        public string OptieRubriekStandaardGuid { get; set; }
        public string Rubriek { get; set; }
        public short? Volgorde { get; set; }
        public string Toelichting { get; set; }
        

        //public virtual OptieRubriekStandaard OptieRubriekStandaardGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozen { get; set; }
        public virtual ICollection<OptieStandaard> OptieStandaards { get; set; }
    }
}
