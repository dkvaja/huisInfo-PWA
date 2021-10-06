using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ProductDienst : BaseEntity
    {
        public ProductDienst()
        {
            Betrokkenes = new HashSet<Betrokkene>();
            //Bewakingscodes = new HashSet<Bewakingscode>();
            //Calculatieregels = new HashSet<Calculatieregel>();
            //ConfiguratieKopersbegeleidings = new HashSet<ConfiguratieKopersbegeleiding>();
            //Garantietermijns = new HashSet<Garantietermijn>();
            //Inkoopprijslijstregels = new HashSet<Inkoopprijslijstregel>();
            Meldings = new HashSet<Melding>();
            OrganisatieProducts = new HashSet<OrganisatieProduct>();
            ProductDienstSub1s = new HashSet<ProductDienstSub1>();
        }

        public string Omschrijving { get; set; }
        public string ProductCode { get; set; }
        public string BouwdeelCode { get; set; }
        public bool? ToevoegenAlsBetrokkene { get; set; }
        public bool? BetrokkenePubliceren { get; set; }
        public bool? Actief { get; set; }
        public bool? Systeemwaarde { get; set; }
        
        public virtual ICollection<Betrokkene> Betrokkenes { get; set; }
        //public virtual ICollection<Bewakingscode> Bewakingscodes { get; set; }
        //public virtual ICollection<Calculatieregel> Calculatieregels { get; set; }
        //public virtual ICollection<ConfiguratieKopersbegeleiding> ConfiguratieKopersbegeleidings { get; set; }
        //public virtual ICollection<Garantietermijn> Garantietermijns { get; set; }
        //public virtual ICollection<Inkoopprijslijstregel> Inkoopprijslijstregels { get; set; }
        public virtual ICollection<Melding> Meldings { get; set; }
        public virtual ICollection<OrganisatieProduct> OrganisatieProducts { get; set; }
        public virtual ICollection<ProductDienstSub1> ProductDienstSub1s { get; set; }
    }
}
