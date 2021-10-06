using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class DossierVolgorde : BaseEntity
    {
        public string WerkGuid { get; set; }
        public string DossierGuid { get; set; }
        public short Volgorde { get; set; }
        public virtual Dossier DossierGu { get; set; }
        public virtual Werk WerkGu { get; set; }
    }
}
