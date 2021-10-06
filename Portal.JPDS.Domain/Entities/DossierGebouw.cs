using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class DossierGebouw : BaseEntity
    {
        public string DossierGuid { get; set; }
        public string GebouwGuid { get; set; }
        public DateTime? Deadline { get; set; }
        public byte Status { get; set; }
        public bool Actief { get; set; }
        public bool Verwijderd { get; set; }
        public DateTime? AfgeslotenOp { get; set; }
        public virtual Dossier DossierGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
    }
}
