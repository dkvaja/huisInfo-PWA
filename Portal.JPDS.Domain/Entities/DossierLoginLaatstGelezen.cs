using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class DossierLoginLaatstGelezen : BaseEntity
    {
        public string LoginGuid { get; set; }
        public string DossierGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string BijlageDossierGuid { get; set; }
        public DateTime LaatstGelezen { get; set; }
        public virtual BijlageDossier BijlageDossierGu { get; set; }
        public virtual Dossier DossierGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual Login LoginGu { get; set; }
    }
}
