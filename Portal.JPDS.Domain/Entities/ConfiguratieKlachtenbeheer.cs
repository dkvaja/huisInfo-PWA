using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ConfiguratieKlachtenbeheer : BaseEntity
    {
        public string OpmaakMeldingNummer { get; set; }
        public bool? MeldingNummersPerJaar { get; set; }
        public string StandaardMeldingStatusGuid { get; set; }
        public string StandaardAfhandelingstermijnGuid { get; set; }
        public byte OverdrachtsmomentUitvoeringNazorg { get; set; }
        public int? AfmeldingsverzoekSturenNa { get; set; }
        public string EmailNazorg { get; set; }
        public string OpmerkingVersturenOnlineMelding { get; set; }
        public bool? ProductDienstVerplichtBijAfhandelingMelding { get; set; }
        public bool? ProductDienstSub1VerplichtBijAfhandelingMelding { get; set; }
        public bool? ProductDienstSub2VerplichtBijAfhandelingMelding { get; set; }
        public bool? TypeVerplichtBijAfhandelingMelding { get; set; }
        public bool? AardVerplichtBijAfhandelingMelding { get; set; }
        public bool? LocatieVerplichtBijAfhandelingMelding { get; set; }
        public bool? OorzaakVerplichtBijAfhandelingMelding { get; set; }
        public bool? VeroorzakerVerplichtBijAfhandelingMelding { get; set; }
        public string StandaardRegistratiebevestigingSjabloonGuid { get; set; }

        public virtual Afhandelingstermijn StandaardAfhandelingstermijnGu { get; set; }
        public virtual MeldingStatus StandaardMeldingStatusGu { get; set; }
    }
}
