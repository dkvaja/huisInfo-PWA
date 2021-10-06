using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Tekstblok
    {
        public string Guid { get; set; }
        public string LoginGuid { get; set; }
        public string StandaardTekstblokGuid { get; set; }
        public string WerkGuid { get; set; }
        public string Zoekterm { get; set; }
        public string Tekstblok1 { get; set; }
        public bool TekstblokIsHandtekening { get; set; }
        public short? Volgorde { get; set; }
        public DateTime? GewijzigdOp { get; set; }
        public string GewijzigdDoor { get; set; }
        public DateTime? IngevoerdOp { get; set; }
        public string IngevoerdDoor { get; set; }
    }
}
