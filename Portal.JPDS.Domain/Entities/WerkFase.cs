using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class WerkFase:BaseEntity
    {
        public WerkFase()
        {
            //FunctieWerkFase = new HashSet<FunctieWerkFase>();
            Werk = new HashSet<Werk>();
        }

        public string Fase { get; set; }

        //public virtual ICollection<FunctieWerkFase> FunctieWerkFase { get; set; }
        public virtual ICollection<Werk> Werk { get; set; }
    }
}
