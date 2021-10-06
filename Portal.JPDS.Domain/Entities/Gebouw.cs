using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Gebouw : BaseEntity
    {
        public Gebouw()
        {
            Actie = new HashSet<Actie>();
            Bijlage = new HashSet<Bijlage>();
            BijlageDossiers = new HashSet<BijlageDossier>();
            Chat = new HashSet<Chat>();
            //ChatContact = new HashSet<ChatContact>();
            Contacts = new HashSet<Contact>();
            DossierGebouws = new HashSet<DossierGebouw>();
            DossierLoginLaatstGelezens = new HashSet<DossierLoginLaatstGelezen>();
            //GebouwBegrotingsregel = new HashSet<GebouwBegrotingsregel>();
            //Inkoop = new HashSet<Inkoop>();
            OptieGekozen = new HashSet<OptieGekozen>();
            OptieGekozenOfferte = new HashSet<OptieGekozenOfferte>();
            //VolgjewoningOptieGedownload = new HashSet<VolgjewoningOptieGedownload>();
            LoginRolWerks = new HashSet<LoginRolWerk>();
        }

        public string WerkGuid { get; set; }
        public string GebouwSoortGuid { get; set; }
        public string WoningTypeGuid { get; set; }
        public string WoningSoortGuid { get; set; }
        public string BouwstroomGuid { get; set; }
        public string BouwnummerIntern { get; set; }
        public string BouwnummerExtern { get; set; }
        public string KoperHuurderGuid { get; set; }
        public string GebouwGebruikerOrganisatieGuid { get; set; }
        public string GebouwGebruikerNaam { get; set; }
        public string AdresGuid { get; set; }
        public string AdresBatchGuid { get; set; }
        public string GebouwStatusGuid { get; set; }
        public byte? WoningSituatie { get; set; }
        public string Naam { get; set; }
        public bool? GebouwStaatLeeg { get; set; }
        public string GarantieregelingGuid { get; set; }
        public string CertificaatNummer { get; set; }
        public DateTime? CertificaatGeldigVa { get; set; }
        public DateTime? CertificaatGeldigTm { get; set; }
        public decimal? KavelBreedte { get; set; }
        public decimal? KavelDiepte { get; set; }
        public string EanElektriciteit { get; set; }
        public string EanGas { get; set; }
        public byte AanneemsomBtwTarief { get; set; }
        public decimal? AanneemsomExclBtw { get; set; }
        public decimal? AanneemsomInclBtw { get; set; }
        public decimal? AanvullingenAanneemsomExclBtw { get; set; }
        public decimal? AanvullingenAanneemsomInclBtw { get; set; }
        public byte GrondprijsPerM2BtwTarief { get; set; }
        public decimal? GrondprijsPerM2ExclBtw { get; set; }
        public decimal? GrondprijsPerM2InclBtw { get; set; }
        public byte? GrondprijsBtwTarief { get; set; }
        public decimal? GrondprijsExclBtw { get; set; }
        public decimal? GrondprijsInclBtw { get; set; }
        public decimal? AanvullingenGrondkostenExclBtw { get; set; }
        public decimal? AanvullingenGrondkostenInclBtw { get; set; }
        public decimal? AanvullingenVerkoopkostenExclBtw { get; set; }
        public decimal? AanvullingenVerkoopkostenInclBtw { get; set; }
        public byte VrijOpNaamPrijsBtwTarief { get; set; }
        public decimal? VrijOpNaamPrijsExclBtw { get; set; }
        public decimal? VrijOpNaamPrijsInclBtw { get; set; }
        public decimal? TotaalVerkoopKoperswijzigingenExclBtw { get; set; }
        public decimal? TotaalVerkoopKoperswijzigingenInclBtw { get; set; }
        public decimal? TotaalInkoopKoperswijzigingenExclBtw { get; set; }
        public decimal? TotaalVerrekeningAannemerExclBtw { get; set; }
        public decimal? TotaalVerrekeningAannemerInclBtw { get; set; }
        public decimal? MinderprijsCasco { get; set; }
        public DateTime? DatumStartBouw { get; set; }
        public DateTime? DatumVoorschouw { get; set; }
        public TimeSpan? TijdVoorschouwVa { get; set; }
        public TimeSpan? TijdVoorschouwTm { get; set; }
        public DateTime? DatumOplevering { get; set; }
        public TimeSpan? TijdOpleveringVa { get; set; }
        public TimeSpan? TijdOpleveringTm { get; set; }
        public DateTime? DatumEindeOnderhoudstermijn { get; set; }
        public DateTime? DatumLaatsteTekening { get; set; }
        public DateTime? DatumLaatsteOfferte { get; set; }
        public DateTime? DatumLaatsteOpdrachtbevestiging { get; set; }
        public DateTime? DatumLaatsteOptieDefintief { get; set; }
        public DateTime? DatumLaatsteMeterkastlijst { get; set; }
        public DateTime? DatumLaatsteFinancieelOverzicht { get; set; }
        public DateTime? DatumLaatsteFactuur { get; set; }
        public DateTime? DatumLaatsteOfferteSluitingsdatum { get; set; }
        public string NotitiesIntern { get; set; }
        public string NotitiesExtern { get; set; }
        public string KoptekstKeuzelijst { get; set; }
        public string VoettekstKeuzelijst { get; set; }
        public string KoptekstOfferte { get; set; }
        public string VoettekstOfferte { get; set; }
        public string KoptekstOpdrachtbevestiging { get; set; }
        public string VoettekstOpdrachtbevestiging { get; set; }
        public string VariabeleTekst { get; set; }
        public string AdresaanhefGeenNaam { get; set; }
        public string Adresblok { get; set; }


        public virtual Adres AdresGu { get; set; }
        public virtual Bouwstroom BouwstroomGu { get; set; }
        //public virtual Garantieregeling GarantieregelingGu { get; set; }
        public virtual Organisatie GebouwGebruikerOrganisatieGu { get; set; }
        public virtual GebouwSoort GebouwSoortGu { get; set; }
        public virtual GebouwStatus GebouwStatusGu { get; set; }
        public virtual KoperHuurder KoperHuurderGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        //public virtual WoningSoort WoningSoortGu { get; set; }
        public virtual WoningType WoningTypeGu { get; set; }
        public virtual ICollection<Actie> Actie { get; set; }
        public virtual ICollection<BijlageDossier> BijlageDossiers { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<Chat> Chat { get; set; }
        //public virtual ICollection<ChatContact> ChatContact { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<DossierGebouw> DossierGebouws { get; set; }
        public virtual ICollection<DossierLoginLaatstGelezen> DossierLoginLaatstGelezens { get; set; }
        //public virtual ICollection<GebouwBegrotingsregel> GebouwBegrotingsregel { get; set; }
        //public virtual ICollection<Inkoop> Inkoop { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozen { get; set; }
        public virtual ICollection<OptieGekozenOfferte> OptieGekozenOfferte { get; set; }
        public virtual ICollection<LoginRolWerk> LoginRolWerks { get; set; }
        //public virtual ICollection<VolgjewoningOptieGedownload> VolgjewoningOptieGedownload { get; set; }
    }
}
