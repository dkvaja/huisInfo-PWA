using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Bouwstroom : BaseEntity
    {
        public Bouwstroom()
        {
            Gebouw = new HashSet<Gebouw>();
            //Sluitingsdatum = new HashSet<Sluitingsdatum>();
        }

        public string WerkGuid { get; set; }
        public string Bouwstroom1 { get; set; }

        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Gebouw> Gebouw { get; set; }
        //public virtual ICollection<Sluitingsdatum> Sluitingsdatum { get; set; }
    }
}
