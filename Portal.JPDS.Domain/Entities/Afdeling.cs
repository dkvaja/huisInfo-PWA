using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Afdeling : BaseEntity
    {
        public Afdeling()
        {
            Communicaties = new HashSet<Communicatie>();
            Medewerkers = new HashSet<Medewerker>();
            Relaties = new HashSet<Relatie>();
        }

        public string Afdeling1 { get; set; }
        public string CodeTbvCommunicatieKenmerk { get; set; }
        public bool? CorrespondentieInSubmap { get; set; }
        public string Submap { get; set; }
        public bool? InterneAfdeling { get; set; }
        public virtual ICollection<Communicatie> Communicaties { get; set; }
        public virtual ICollection<Medewerker> Medewerkers { get; set; }
        public virtual ICollection<Relatie> Relaties { get; set; }
    }
}
