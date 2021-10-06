using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewInputKoperHuurder
    {
        public string Guid { get; set; }
        public byte Soort { get; set; }
        public byte KoperHuurderStatus { get; set; }
        public int P1BekendePersoon { get; set; }
        public string P1Guid { get; set; }
        public string P1Achternaam { get; set; }
        public string P1TussenvoegselGuid { get; set; }
        public string P1Voorletters { get; set; }
        public string P1Voornaam { get; set; }
        public byte? P1Geslacht { get; set; }
        public string P1TitulatuurVoorGuid { get; set; }
        public string P1TitulatuurAchterGuid { get; set; }
        public string P1TelefoonPrive { get; set; }
        public string P1TelefoonWerk { get; set; }
        public string P1Mobiel { get; set; }
        public string P1Fax { get; set; }
        public string P1EmailPrive { get; set; }
        public string P1EmailWerk { get; set; }
        public int P2BekendePersoon { get; set; }
        public string P2Guid { get; set; }
        public string P2Achternaam { get; set; }
        public string P2TussenvoegselGuid { get; set; }
        public string P2Voorletters { get; set; }
        public string P2Voornaam { get; set; }
        public byte? P2Geslacht { get; set; }
        public string P2TitulatuurVoorGuid { get; set; }
        public string P2TitulatuurAchterGuid { get; set; }
        public string P2TelefoonPrive { get; set; }
        public string P2TelefoonWerk { get; set; }
        public string P2Mobiel { get; set; }
        public string P2Fax { get; set; }
        public string P2EmailPrive { get; set; }
        public string P2EmailWerk { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string CorrespondentieadresGuid { get; set; }
        public byte? Adresaanhef { get; set; }
        public bool NaamInTweeRegels { get; set; }
        public string BankGuid { get; set; }
        public string Bic { get; set; }
        public string Iban { get; set; }
        public bool Debiteur { get; set; }
        public string Debiteurnummer { get; set; }
        public int? Betalingstermijn { get; set; }
        public byte? Betaalwijze { get; set; }
        public bool FactuurDigitaal { get; set; }
        public string EmailFactuur { get; set; }
        public string TaalGuid { get; set; }
        public bool EmailNaarAlleEmailadressen { get; set; }
        public string VoorkeurEmailadresVeldNieuw { get; set; }
        public string VoorkeurEmailadresVeld { get; set; }
        public bool ToestemmingVrijgevenGegevens { get; set; }
        public string Notities { get; set; }
        public string Zoeknaam { get; set; }
        public string Adresblok { get; set; }
        public string VolledigeNaam { get; set; }
        public string VolledigeNaamEenRegel { get; set; }
        public string BriefaanhefFormeel { get; set; }
        public int InputWaarschuwingNegeren { get; set; }
        public DateTime? IngevoerdOp { get; set; }
        public string IngevoerdDoor { get; set; }
        public DateTime? GewijzigdOp { get; set; }
        public string GewijzigdDoor { get; set; }
    }
}
