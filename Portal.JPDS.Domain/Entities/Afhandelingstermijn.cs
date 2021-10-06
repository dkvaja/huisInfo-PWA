using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Afhandelingstermijn
    {
        public Afhandelingstermijn()
        {
            ConfiguratieKlachtenbeheer = new HashSet<ConfiguratieKlachtenbeheer>();
            Melding = new HashSet<Melding>();
        }

        public string Guid { get; set; }
        public string Omschrijving { get; set; }
        public int AantalDagen { get; set; }
        public bool? Werkdagen { get; set; }
        public DateTime? IngevoerdOp { get; set; }
        public string IngevoerdDoor { get; set; }
        public DateTime? GewijzigdOp { get; set; }
        public string GewijzigdDoor { get; set; }

        public virtual ICollection<ConfiguratieKlachtenbeheer> ConfiguratieKlachtenbeheer { get; set; }
        public virtual ICollection<Melding> Melding { get; set; }
    }
}
