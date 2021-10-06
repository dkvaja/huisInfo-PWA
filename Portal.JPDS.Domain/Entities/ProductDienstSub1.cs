using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ProductDienstSub1 : BaseEntity
    {
        public ProductDienstSub1()
        {
            ProductDienstSub2s = new HashSet<ProductDienstSub2>();
        }
        public string ProductDienstGuid { get; set; }
        public string Omschrijving { get; set; }
        public bool? Actief { get; set; }       

        public virtual ProductDienst ProductDienstGu { get; set; }
        public virtual ICollection<ProductDienstSub2> ProductDienstSub2s { get; set; }
    }
}
