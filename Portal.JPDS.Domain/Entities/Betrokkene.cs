using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public partial class Betrokkene : BaseEntity
    {
        public Betrokkene()
        {
            BetrokkeneRelatie = new HashSet<BetrokkeneRelatie>();
            //Inkoop = new HashSet<Inkoop>();
            //Inkoopprijs = new HashSet<Inkoopprijs>();
        }

        public string WerkGuid { get; set; }
        public string ProductDienstGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string BetrokkeneSoortGuid { get; set; }
        public bool? Hoofdaannemer { get; set; }
        public bool? Publiceren { get; set; }
        public string Notities { get; set; }
        public string VoettekstOrderbevestiging { get; set; }
        public DateTime? LaatsteOrderbevestiging { get; set; }

        //public virtual BetrokkeneSoort BetrokkeneSoortGu { get; set; }
        public virtual ProductDienst ProductDienstGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<BetrokkeneRelatie> BetrokkeneRelatie { get; set; }
        //public virtual ICollection<Inkoop> Inkoop { get; set; }
        //public virtual ICollection<Inkoopprijs> Inkoopprijs { get; set; }
    }
}
