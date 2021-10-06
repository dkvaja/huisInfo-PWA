using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Dossier : BaseEntity 
    {
        public Dossier()
        {
            BijlageDossiers = new HashSet<BijlageDossier>();
            DossierGebouws = new HashSet<DossierGebouw>();
            DossierLoginLaatstGelezens = new HashSet<DossierLoginLaatstGelezen>();
            DossierVolgordes = new HashSet<DossierVolgorde>();
            LoginDossierRechts = new HashSet<LoginDossierRecht>();
        }
        public string WerkGuid { get; set; }
        public string Naam { get; set; }
        public string AlgemeneInformatie { get; set; }
        public string Afbeelding { get; set; }
        public DateTime? Deadline { get; set; }
        public byte Status { get; set; }
        public bool Gearchiveerd { get; set; }
        public string AangemaaktDoorLoginGuid { get; set; }
        public DateTime? AfgeslotenOp { get; set; }
        public bool AlgemeneBestanden { get; set; }
        public bool ObjectgebondenBestanden { get; set; }
        public bool Extern { get; set; }
        public virtual Login AangemaaktDoorLoginGu { get; set; }
        public virtual ICollection<BijlageDossier> BijlageDossiers { get; set; }
        public virtual ICollection<DossierGebouw> DossierGebouws { get; set; }
        public virtual ICollection<DossierLoginLaatstGelezen> DossierLoginLaatstGelezens { get; set; }
        public virtual ICollection<DossierVolgorde> DossierVolgordes { get; set; }
        public virtual ICollection<LoginDossierRecht> LoginDossierRechts { get; set; }
    }
}
