using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Functie : BaseEntity
    {
        public Functie()
        {
            BetrokkeneRelaties = new HashSet<BetrokkeneRelatie>();
            //FunctieWerkFases = new HashSet<FunctieWerkFase>();
            Relaties = new HashSet<Relatie>();
        }

        public string Functie1 { get; set; }

        public virtual ICollection<BetrokkeneRelatie> BetrokkeneRelaties { get; set; }
        //public virtual ICollection<FunctieWerkFase> FunctieWerkFases { get; set; }
        public virtual ICollection<Relatie> Relaties { get; set; }
    }
}
