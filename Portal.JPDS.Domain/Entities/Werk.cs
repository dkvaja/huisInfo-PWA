using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Werk : BaseEntity
    {
        public Werk()
        {
            Actie = new HashSet<Actie>();
            //ActieWerk = new HashSet<ActieWerk>();
            Betrokkene = new HashSet<Betrokkene>();
            Bijlage = new HashSet<Bijlage>();
            Bouwstroom = new HashSet<Bouwstroom>();
            Chat = new HashSet<Chat>();
            //ChatContact = new HashSet<ChatContact>();
            Communicaties = new HashSet<Communicatie>();
            DossierVolgordes = new HashSet<DossierVolgorde>();
            //Factuur = new HashSet<Factuur>();
            FaqVraagAntwoordWerk = new HashSet<FaqVraagAntwoordWerk>();
            Gebouw = new HashSet<Gebouw>();
            //Inkoop = new HashSet<Inkoop>();
            //Inkoopprijs = new HashSet<Inkoopprijs>();
            Melding = new HashSet<Melding>();
            Nieuws = new HashSet<Nieuws>();
            Opname = new HashSet<Opname>();
            //OptieCategorieWerk = new HashSet<OptieCategorieWerk>();
            OptieGekozen = new HashSet<OptieGekozen>();
            //OptieRubriekWerk = new HashSet<OptieRubriekWerk>();
            OptieStandaards = new HashSet<OptieStandaard>();
            Sjabloon = new HashSet<Sjabloon>();
            //Sluitingsdatum = new HashSet<Sluitingsdatum>();
            //TermijnschemaWerk = new HashSet<TermijnschemaWerk>();
            //VolgjewoningKoppeling = new HashSet<VolgjewoningKoppeling>();
            WoningType = new HashSet<WoningType>();
            LoginRolWerks = new HashSet<LoginRolWerk>();
        }

        public string Werknummer { get; set; }
        public string Werknaam { get; set; }
        public string WerknummerWerknaam { get; set; }
        public byte WerkType { get; set; }
        public byte WerkSoort { get; set; }
        public string WerkFaseGuid { get; set; }
        public int? AantalObjecten { get; set; }
        public string Plaats { get; set; }
        public string Telefoon { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string WebPortal { get; set; }
        public bool GebruikJpbmWebPortal { get; set; }
        public DateTime? DatumStartVerkoop { get; set; }
        public string HoofdaannemerOrganisatieGuid { get; set; }
        public DateTime? DatumStartBouw { get; set; }
        public DateTime? DatumEindBouw { get; set; }
        public string GarantieregelingGuid { get; set; }
        public string OnderhoudstermijnGuid { get; set; }
        public string Planregistratienummer { get; set; }
        public string MeldingVerantwoordelijkeManagementRelatieGuid { get; set; }
        public string MeldingVerantwoordelijkeUitvoeringRelatieGuid { get; set; }
        public string Notities { get; set; }
        public string KoptekstKeuzelijst { get; set; }
        public string VoettekstKeuzelijst { get; set; }
        public string KoptekstOfferte { get; set; }
        public string VoettekstOfferte { get; set; }
        public string KoptekstOpdrachtbevestiging { get; set; }
        public string VoettekstOpdrachtbevestiging { get; set; }
        //public string WitruimteVoorblad { get; set; }Delete this not in new database
        public string KoptekstFactuurOpdracht { get; set; }
        public string VoettekstFactuurOpdracht { get; set; }
        public string KoptekstFactuurOplevering { get; set; }
        public string VoettekstFactuurOplevering { get; set; }
        public string MapBijlagen { get; set; }
        public bool? SubmapPerGebouw { get; set; }
        public byte? OptieStatusGekozenStandaardOptie { get; set; }
        public bool? OptieStandaardCommercieleOmschrijvingOpMeterkastlijst { get; set; }
        public bool? OptieStandaardTechnischeOmschrijvingOpMeterkastlijst { get; set; }
        public bool? OptieIndividueelCommercieleOmschrijvingOpMeterkastlijst { get; set; }
        public bool? OptieIndividueelTechnischeOmschrijvingOpMeterkastlijst { get; set; }
        public string Logo { get; set; }
        public string LogoWebPortal { get; set; }
        public string AchtergrondWebPortal { get; set; }
        public string AlgemeneInfo { get; set; }
        //public bool? Mutatieoverzicht { get; set; }Delete this not in new database
        public bool? DatumMeterkastlijstIsDatumOpdrachtbevestiging { get; set; }
        public bool? DatumMeterkastlijstVragen { get; set; }
        public byte? BepalenSluitingsdatum { get; set; }
        public int? AantalDagenOptellenOfferteDatum { get; set; }
        public bool? SluitingsdatumVragen { get; set; }
        public bool? DagenOptellenOfferteDatum { get; set; }
        public string OmschrijvingKosten1 { get; set; }
        public string OmschrijvingKosten2 { get; set; }
        public string OmschrijvingKosten3 { get; set; }
        public string OmschrijvingKosten4 { get; set; }
        public string OmschrijvingKosten5 { get; set; }
        public decimal? PercentageKosten1 { get; set; }
        public decimal? PercentageKosten2 { get; set; }
        public decimal? PercentageKosten3 { get; set; }
        public decimal? PercentageKosten4 { get; set; }
        public decimal? PercentageKosten5 { get; set; }
        public bool? AbsoluteWaardeKosten1 { get; set; }
        public bool? AbsoluteWaardeKosten2 { get; set; }
        public bool? AbsoluteWaardeKosten3 { get; set; }
        public bool? AbsoluteWaardeKosten4 { get; set; }
        public bool? AbsoluteWaardeKosten5 { get; set; }
        public string OmschrijvingToeslag1 { get; set; }
        public string OmschrijvingToeslag2 { get; set; }
        public decimal? PercentageToeslag1 { get; set; }
        public decimal? PercentageToeslag2 { get; set; }
        public bool? AbsoluteWaardeToeslag1 { get; set; }
        public bool? AbsoluteWaardeToeslag2 { get; set; }
        public bool? OmschrijvingenWijzigbaarPerOptie { get; set; }
        public bool? PercentagesWijzigbaarPerOptie { get; set; }
        public bool? InvullenVerkoopprijsInclBtw { get; set; }
        public bool? BedragenAfrondenPerOptie { get; set; }
        public bool? KaleKostprijsAutomatischBerekenen { get; set; }
        public bool? InkoopprijzenUitInkoopprijslijst { get; set; }
        public bool? GrondslagBedragKosten2IsKaleKostprijsPlusBedragKosten1 { get; set; }
        public bool? VerkoopprijsExclBtwIsGrondslagToeslag2 { get; set; }
        public byte? BerekeningswijzeVerkoopprijs { get; set; }
        public bool? Geblokkeerd { get; set; }


        //public virtual Garantieregeling GarantieregelingGu { get; set; }
        public virtual Organisatie HoofdaannemerOrganisatieGu { get; set; }
        public virtual Relatie MeldingVerantwoordelijkeManagementRelatieGu { get; set; }
        public virtual Relatie MeldingVerantwoordelijkeUitvoeringRelatieGu { get; set; }
        //public virtual Onderhoudstermijn OnderhoudstermijnGu { get; set; }
        public virtual WerkFase WerkFaseGu { get; set; }
        public virtual ICollection<Actie> Actie { get; set; }
        //public virtual ICollection<ActieWerk> ActieWerk { get; set; }
        public virtual ICollection<Betrokkene> Betrokkene { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<Bouwstroom> Bouwstroom { get; set; }
        public virtual ICollection<Chat> Chat { get; set; }
        //public virtual ICollection<ChatContact> ChatContact { get; set; }
        public virtual ICollection<Communicatie> Communicaties { get; set; }
        public virtual ICollection<DossierVolgorde> DossierVolgordes { get; set; }
        //public virtual ICollection<Factuur> Factuur { get; set; }
        public virtual ICollection<FaqVraagAntwoordWerk> FaqVraagAntwoordWerk { get; set; }
        public virtual ICollection<Gebouw> Gebouw { get; set; }
        //public virtual ICollection<Inkoop> Inkoop { get; set; }
        //public virtual ICollection<Inkoopprijs> Inkoopprijs { get; set; }
        public virtual ICollection<Melding> Melding { get; set; }
        public virtual ICollection<Nieuws> Nieuws { get; set; }
        public virtual ICollection<Opname> Opname { get; set; }
        //public virtual ICollection<OptieCategorieWerk> OptieCategorieWerk { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozen { get; set; }
        //public virtual ICollection<OptieRubriekWerk> OptieRubriekWerk { get; set; }
        public virtual ICollection<OptieStandaard> OptieStandaards { get; set; }
        public virtual ICollection<LoginRolWerk> LoginRolWerks { get; set; }
        public virtual ICollection<Sjabloon> Sjabloon { get; set; }
        //public virtual ICollection<Sluitingsdatum> Sluitingsdatum { get; set; }
        //public virtual ICollection<TermijnschemaWerk> TermijnschemaWerk { get; set; }
        //public virtual ICollection<VolgjewoningKoppeling> VolgjewoningKoppeling { get; set; }
        public virtual ICollection<WoningType> WoningType { get; set; }
    }
}
