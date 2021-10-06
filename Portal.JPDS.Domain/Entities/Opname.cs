using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Opname : BaseEntity
    {
        public Opname()
        {
            Melding = new HashSet<Melding>();
        }

        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public byte OpnameSoort { get; set; }
        public DateTime? Datum { get; set; }
        public string UitgevoerdDoorMedewerkerGuid { get; set; }
        public string OpmerkingenUitvoerder { get; set; }
        public string OpmerkingenKoperHuurder { get; set; }
        public int? MeterstandElektra1 { get; set; }
        public int? MeterstandElektra2 { get; set; }
        public int? MeterstandElektraRetour1 { get; set; }
        public int? MeterstandElektraRetour2 { get; set; }
        public int? MeterstandGasWarmte { get; set; }
        public int? MeterstandWater { get; set; }
        public string KoptekstPvo { get; set; }
        public string VoettekstPvo { get; set; }
        public int? AantalGebreken { get; set; }
        public string VolledigeNaamUitvoerder { get; set; }
        public byte[] HandtekeningUitvoerderFileOpslag { get; set; }
        public string HandtekeningUitvoerder { get; set; }
        public string VolledigeNaamKh1 { get; set; }
        public byte[] HandtekeningKh1FileOpslag { get; set; }
        public string HandtekeningKh1 { get; set; }
        public string VolledigeNaamKh2 { get; set; }
        public byte[] HandtekeningKh2FileOpslag { get; set; }
        public string HandtekeningKh2 { get; set; }
        public byte OpnameStatus { get; set; }
        public bool TweedeHandtekeningGestart { get; set; }
        public byte[] Handtekening2Kh1FileOpslag { get; set; }
        public string Handtekening2Kh1 { get; set; }
        public byte[] Handtekening2Kh2FileOpslag { get; set; }
        public string Handtekening2Kh2 { get; set; }
        public DateTime? DatumTweedeHandtekeningOndertekening { get; set; }
        public string Pvo1BijlageGuid { get; set; }
        public string Pvo2BijlageGuid { get; set; }

        public virtual Bijlage Pvo1BijlageGu { get; set; }
        public virtual Bijlage Pvo2BijlageGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Melding> Melding { get; set; }
    }
}
