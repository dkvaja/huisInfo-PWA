using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class KoperHuurder : BaseEntity
    {
        public KoperHuurder()
        {
            Contacts = new HashSet<Contact>();
            //Factuur = new HashSet<Factuur>();
            Gebouw = new HashSet<Gebouw>();
            Login = new HashSet<Login>();
            MeldingGebouwGebruikerKoperHuurderGu = new HashSet<Melding>();
            MeldingKosten = new HashSet<MeldingKosten>();
            MeldingMelderKoperHuurderGu = new HashSet<Melding>();
        }

        public byte Soort { get; set; }
        public string Persoon1Guid { get; set; }
        public string Persoon2Guid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string CorrespondentieadresGuid { get; set; }
        public byte? Adresaanhef { get; set; }
        public bool? NaamInTweeRegels { get; set; }
        public string Persoon1TelefoonWerk { get; set; }
        public string Persoon1Fax { get; set; }
        public string Persoon1EmailWerk { get; set; }
        public string Persoon2TelefoonWerk { get; set; }
        public string Persoon2Fax { get; set; }
        public string Persoon2EmailWerk { get; set; }
        public string TaalGuid { get; set; }
        public bool? EmailNaarAlleEmailadressen { get; set; }
        public string VoorkeurEmailadresVeld { get; set; }
        public bool? ToestemmingVrijgevenGegevens { get; set; }
        public string BankGuid { get; set; }
        public string Bic { get; set; }
        public string Iban { get; set; }
        public byte? Betaalwijze { get; set; }
        public bool? Debiteur { get; set; }
        public string Debiteurnummer { get; set; }
        public int? Betalingstermijn { get; set; }
        public bool? FactuurDigitaal { get; set; }
        public string EmailFactuur { get; set; }
        public byte KoperHuurderStatus { get; set; }
        public int? KoperHuurderAantal { get; set; }
        public string Notities { get; set; }
        public string Zoeknaam { get; set; }
        public string Adresblok { get; set; }
        public string VolledigeNaam { get; set; }
        public string VolledigeNaamEenRegel { get; set; }


        //public virtual Bank BankGu { get; set; }
        public virtual Adres CorrespondentieadresGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual Persoon Persoon1Gu { get; set; }
        public virtual Persoon Persoon2Gu { get; set; }
        public virtual Relatie RelatieGu { get; set; }
        //public virtual Taal TaalGu { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Factuur> Factuur { get; set; }
        public virtual ICollection<Gebouw> Gebouw { get; set; }
        public virtual ICollection<Login> Login { get; set; }
        public virtual ICollection<Melding> MeldingGebouwGebruikerKoperHuurderGu { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKosten { get; set; }
        public virtual ICollection<Melding> MeldingMelderKoperHuurderGu { get; set; }
    }
}
