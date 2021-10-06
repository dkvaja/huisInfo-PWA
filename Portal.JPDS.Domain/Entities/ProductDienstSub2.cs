using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ProductDienstSub2 : BaseEntity
    {
        public string ProductDienstSub1Guid { get; set; }
        public string Omschrijving { get; set; }
        public bool? Actief { get; set; }
        public virtual ProductDienstSub1 ProductDienstSub1Gu { get; set; }
    }
}
