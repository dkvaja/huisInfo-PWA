using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class FaqVraagAntwoordWerk : BaseEntity
    {
        public FaqVraagAntwoordWerk()
        {
            Bijlage = new HashSet<Bijlage>();
        }

        public string WerkGuid { get; set; }
        public string FaqRubriekGuid { get; set; }
        public string Vraag { get; set; }
        public string Antwoord { get; set; }
       

        public virtual FaqRubriek FaqRubriekGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
    }
}
