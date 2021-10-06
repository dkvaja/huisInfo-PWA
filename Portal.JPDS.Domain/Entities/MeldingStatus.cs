using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingStatus : BaseEntity
    {
        public MeldingStatus()
        {
            ConfiguratieKlachtenbeheer = new HashSet<ConfiguratieKlachtenbeheer>();
            Melding = new HashSet<Melding>();
            //SjabloonMelding = new HashSet<SjabloonMelding>();
        }

        public string MeldingStatus1 { get; set; }
        public bool? Afgehandeld { get; set; }
        public bool? Systeemwaarde { get; set; }

        public virtual ICollection<ConfiguratieKlachtenbeheer> ConfiguratieKlachtenbeheer { get; set; }
        public virtual ICollection<Melding> Melding { get; set; }
        //public virtual ICollection<SjabloonMelding> SjabloonMelding { get; set; }
    }
}
