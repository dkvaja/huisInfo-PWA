using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Adres:BaseEntity
    {
        public Adres()
        {
            Gebouw = new HashSet<Gebouw>();
            KoperHuurder = new HashSet<KoperHuurder>();
            OrganisatieBezoekadresGu = new HashSet<Organisatie>();
            OrganisatieFactuuradresGu = new HashSet<Organisatie>();
            OrganisatiePostadresGu = new HashSet<Organisatie>();
            Persoon = new HashSet<Persoon>();
        }

        public string Straat { get; set; }
        public string Nummer { get; set; }
        public string NummerToevoeging { get; set; }
        public string Postcode { get; set; }
        public string Plaats { get; set; }
        public string LandGuid { get; set; }
        public string Netnummer { get; set; }
        public string StraatNummerToevoeging { get; set; }
        public string VolledigAdres { get; set; }

        //public virtual Land LandGu { get; set; }
        public virtual ICollection<Gebouw> Gebouw { get; set; }
        public virtual ICollection<KoperHuurder> KoperHuurder { get; set; }
        public virtual ICollection<Organisatie> OrganisatieBezoekadresGu { get; set; }
        public virtual ICollection<Organisatie> OrganisatieFactuuradresGu { get; set; }
        public virtual ICollection<Organisatie> OrganisatiePostadresGu { get; set; }
        public virtual ICollection<Persoon> Persoon { get; set; }
    }
}
