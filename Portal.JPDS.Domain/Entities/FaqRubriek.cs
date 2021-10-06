using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class FaqRubriek : BaseEntity
    {
        public FaqRubriek()
        {
            //FaqVraagAntwoord = new HashSet<FaqVraagAntwoord>();
            FaqVraagAntwoordWerk = new HashSet<FaqVraagAntwoordWerk>();
        }

        public string FaqRubriek1 { get; set; }
        public short? Volgorde { get; set; }
        public bool? Actief { get; set; }
        public bool Systeemwaarde { get; set; }
       

        //public virtual ICollection<FaqVraagAntwoord> FaqVraagAntwoord { get; set; }
        public virtual ICollection<FaqVraagAntwoordWerk> FaqVraagAntwoordWerk { get; set; }
    }
}
