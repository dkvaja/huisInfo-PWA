using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Actie : BaseEntity
    {
        public Actie()
        {
            ChatBericht = new HashSet<ChatBericht>();
        }

        public string ActieStandaardGuid { get; set; }
        public string ActieMedewerkerGuid { get; set; }
        public string AfgehandeldMedewerkerGuid { get; set; }
        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string Omschrijving { get; set; }
        public string OmschrijvingUitgebreid { get; set; }
        public short? Volgorde { get; set; }
        public DateTime? ActieDatum { get; set; }
        public TimeSpan? ActieStarttijd { get; set; }
        public TimeSpan? ActieEindtijd { get; set; }
        public DateTime? AfgehandeldOp { get; set; }

        public virtual Medewerker ActieMedewerkerGu { get; set; }
        public virtual Medewerker AfgehandeldMedewerkerGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<ChatBericht> ChatBericht { get; set; }
    }
}
