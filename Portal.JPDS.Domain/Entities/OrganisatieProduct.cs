using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class OrganisatieProduct : BaseEntity
    {
        public string OrganisatieGuid { get; set; }
        public string ProductDienstGuid { get; set; }
        public byte? Waardering { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual ProductDienst ProductDienstGu { get; set; }
    }
}
