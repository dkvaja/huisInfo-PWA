using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class BijlageDossier : BaseEntity
    {
        public BijlageDossier()
        {
            DossierLoginLaatstGelezens = new HashSet<DossierLoginLaatstGelezen>();
            LoginBijlageDossiers = new HashSet<LoginBijlageDossier>();
        }

        public string DossierGuid { get; set; }
        public string BijlageGuid { get; set; }
        public string GebouwGuid { get; set; }
        public bool Intern { get; set; }
        public bool Gearchiveerd { get; set; }
        public bool Verwijderd { get; set; }
        public string AangemaaktDoorLoginGuid { get; set; }
        public virtual Login AangemaaktDoorLoginGu { get; set; }
        public virtual Bijlage BijlageGu { get; set; }
        public virtual Dossier DossierGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual ICollection<DossierLoginLaatstGelezen> DossierLoginLaatstGelezens { get; set; }
        public virtual ICollection<LoginBijlageDossier> LoginBijlageDossiers { get; set; }
    }
}
