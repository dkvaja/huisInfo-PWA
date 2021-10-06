using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Organisatie : BaseEntity
    {
        public Organisatie()
        {
            //Bewakingscode = new HashSet<Bewakingscode>();
            Bijlage = new HashSet<Bijlage>();
            Chat = new HashSet<Chat>();
            //ChatContact = new HashSet<ChatContact>();
            ConfiguratieAlgemeen = new HashSet<ConfiguratieAlgemeen>();
            Contacts = new HashSet<Contact>();
            //FactuurOrganisatieGu = new HashSet<Factuur>();
            //FactuurWerkmaatschappijOrganisatieGu = new HashSet<Factuur>();
            //Garantieregeling = new HashSet<Garantieregeling>();
            Gebouw = new HashSet<Gebouw>();
            //Groepslid = new HashSet<Groepslid>();
            KoperHuurder = new HashSet<KoperHuurder>();
            Medewerker = new HashSet<Medewerker>();
            MeldingGebouwGebruikerOrganisatieGu = new HashSet<Melding>();
            MeldingKostenCrediteurOrganisatieGu = new HashSet<MeldingKosten>();
            MeldingKostenDebiteurOrganisatieGu = new HashSet<MeldingKosten>();
            MeldingMelderOrganisatieGu = new HashSet<Melding>();
            MeldingOpdrachtgeverOrganisatieGu = new HashSet<Melding>();
            MeldingVastgoedBeheerderOrganisatieGu = new HashSet<Melding>();
            MeldingVeroorzakerOrganisatieGu = new HashSet<Melding>();
            MeldingVveBeheerderOrganisatieGu = new HashSet<Melding>();
            MeldingVveOrganisatieGu = new HashSet<Melding>();
            Oplosser = new HashSet<Oplosser>();
            OrganisatieProducts = new HashSet<OrganisatieProduct>();
            //OrganisatieVestigingHoofdvestigingOrganisatieGu = new HashSet<OrganisatieVestiging>();
            //OrganisatieVestigingNevenvestigingOrganisatieGu = new HashSet<OrganisatieVestiging>();
            Relatie = new HashSet<Relatie>();
            //SocialMediaKoppeling = new HashSet<SocialMediaKoppeling>();
            //SoftwareLicentie = new HashSet<SoftwareLicentie>();
            //SoftwareVerbeterpuntMelder = new HashSet<SoftwareVerbeterpuntMelder>();
            Werk = new HashSet<Werk>();
        }

        public string Naam { get; set; }
        public string Onderdeel { get; set; }
        public string NaamOnderdeel { get; set; }
        public string NaamFactuur { get; set; }
        public string Zoeknaam { get; set; }
        public byte Status { get; set; }
        public string PostadresGuid { get; set; }
        public string BezoekadresGuid { get; set; }
        public string FactuuradresGuid { get; set; }
        public string Telefoon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string BrancheGuid { get; set; }
        public string OrganisatieSoortGuid { get; set; }
        public string OrganisatieTypeGuid { get; set; }
        public string KvkNummer { get; set; }
        public string BtwNummer { get; set; }
        public string BankGuid { get; set; }
        public string Bic { get; set; }
        public string Iban { get; set; }
        public bool? Debiteur { get; set; }
        public string Debiteurnummer { get; set; }
        public byte? Betaalwijze { get; set; }
        public int? Betalingstermijn { get; set; }
        public bool? FactuurDigitaal { get; set; }
        public string EmailFactuur { get; set; }
        public string Logo { get; set; }
        public string Notities { get; set; }
        public bool? OnderdeelInAdresblok { get; set; }
        public bool? IsoCertificaat { get; set; }
        public DateTime? IsoGeldigTm { get; set; }
        public bool? VcaCertificaat { get; set; }
        public DateTime? VcaGeldigTm { get; set; }
        public bool? Wka { get; set; }
        public DateTime? WkaGeldigTm { get; set; }
        public string StartFactuurnummer { get; set; }
        public bool? EigenOrganisatie { get; set; }
        public string HoofdvestigingOrganisatieGuid { get; set; }
        public string Adresblok { get; set; }
        public DateTime? GecontroleerdOp { get; set; }

        //public virtual Bank BankGu { get; set; }
        public virtual Adres BezoekadresGu { get; set; }
        //public virtual Branche BrancheGu { get; set; }
        public virtual Adres FactuuradresGu { get; set; }
        //public virtual OrganisatieSoort OrganisatieSoortGu { get; set; }
        //public virtual OrganisatieType OrganisatieTypeGu { get; set; }
        public virtual Adres PostadresGu { get; set; }
        //public virtual ICollection<Bewakingscode> Bewakingscode { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<Chat> Chat { get; set; }
        //public virtual ICollection<ChatContact> ChatContact { get; set; }
        public virtual ICollection<ConfiguratieAlgemeen> ConfiguratieAlgemeen { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Factuur> FactuurOrganisatieGu { get; set; }
        //public virtual ICollection<Factuur> FactuurWerkmaatschappijOrganisatieGu { get; set; }
        //public virtual ICollection<Garantieregeling> Garantieregeling { get; set; }
        public virtual ICollection<Gebouw> Gebouw { get; set; }
        //public virtual ICollection<Groepslid> Groepslid { get; set; }
        public virtual ICollection<KoperHuurder> KoperHuurder { get; set; }
        public virtual ICollection<Medewerker> Medewerker { get; set; }
        public virtual ICollection<Melding> MeldingGebouwGebruikerOrganisatieGu { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKostenCrediteurOrganisatieGu { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKostenDebiteurOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingMelderOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingOpdrachtgeverOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingVastgoedBeheerderOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingVeroorzakerOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingVveBeheerderOrganisatieGu { get; set; }
        public virtual ICollection<Melding> MeldingVveOrganisatieGu { get; set; }
        public virtual ICollection<Oplosser> Oplosser { get; set; }
        public virtual ICollection<OrganisatieProduct> OrganisatieProducts { get; set; }
        //public virtual ICollection<OrganisatieVestiging> OrganisatieVestigingHoofdvestigingOrganisatieGu { get; set; }
        //public virtual ICollection<OrganisatieVestiging> OrganisatieVestigingNevenvestigingOrganisatieGu { get; set; }
        public virtual ICollection<Relatie> Relatie { get; set; }
        //public virtual ICollection<SocialMediaKoppeling> SocialMediaKoppeling { get; set; }
        //public virtual ICollection<SoftwareLicentie> SoftwareLicentie { get; set; }
        //public virtual ICollection<SoftwareVerbeterpuntMelder> SoftwareVerbeterpuntMelder { get; set; }
        public virtual ICollection<Werk> Werk { get; set; }
    }
}
