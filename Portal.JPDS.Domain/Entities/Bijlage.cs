using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Bijlage : BaseEntity
    {
        public Bijlage()
        {
            BijlageDossiers = new HashSet<BijlageDossier>();
            OpnamePvo1BijlageGu = new HashSet<Opname>();
            OpnamePvo2BijlageGu = new HashSet<Opname>();
        }

        public string CommunicatieGuid { get; set; }
        public string BijlageRubriekGuid { get; set; }
        public string Bijlage1 { get; set; }
        public string BijlageOrigineel { get; set; }
        public string BijlageUpload { get; set; }
        public byte[] BijlageBinaireData { get; set; }
        public string BestandsopslagGuid { get; set; }
        public string Omschrijving { get; set; }
        public byte KoppelenAan { get; set; }
        public string OrganisatieGuid { get; set; }
        public string WerkGuid { get; set; }
        public string OptieStandaardGuid { get; set; }
        public string OptieGekozenGuid { get; set; }
        public string OptieGekozenOfferteGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string MeldingGuid { get; set; }
        public string OplosserGuid { get; set; }
        public string SoftwareVerbeterpuntGuid { get; set; }
        public string ChatBerichtGuid { get; set; }
        public string FaqVraagAntwoordGuid { get; set; }
        public string FaqVraagAntwoordWerkGuid { get; set; }
        public short? Volgorde { get; set; }
        public string Code { get; set; }
        public string Versie { get; set; }
        public DateTime? Datum { get; set; }
        public bool Publiceren { get; set; }
        public bool? BijlageVerwijderenBijVerwijderenRecord { get; set; }
        public virtual BijlageRubriek BijlageRubriekGu { get; set; }
        public virtual ChatBericht ChatBerichtGu { get; set; }
        //public virtual Communicatie CommunicatieGu { get; set; }
        //public virtual FaqVraagAntwoord FaqVraagAntwoordGu { get; set; }
        //public virtual FaqVraagAntwoordWerk FaqVraagAntwoordWerkGu { get; set; }
        //public virtual Gebouw GebouwGu { get; set; }
        public virtual Melding MeldingGu { get; set; }
        //public virtual OptieGekozen OptieGekozenGu { get; set; }
        //public virtual OptieGekozenOfferte OptieGekozenOfferteGu { get; set; }
        public virtual OptieStandaard OptieStandaardGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        //public virtual SoftwareVerbeterpunt SoftwareVerbeterpuntGu { get; set; }
        //public virtual Werk WerkGu { get; set; }
        public virtual ICollection<BijlageDossier> BijlageDossiers { get; set; }
        public virtual ICollection<Opname> OpnamePvo1BijlageGu { get; set; }
        public virtual ICollection<Opname> OpnamePvo2BijlageGu { get; set; }
    }
}
