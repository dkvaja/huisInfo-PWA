using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class OptieGekozen : BaseEntity
    {
        public OptieGekozen()
        {
            Bijlage = new HashSet<Bijlage>();
            //Factuurregel = new HashSet<Factuurregel>();
            //Inkoop = new HashSet<Inkoop>();
            //VolgjewoningOptieGedownload = new HashSet<VolgjewoningOptieGedownload>();
        }

        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string OptieStandaardGuid { get; set; }
        public string OptieRubriekWerkGuid { get; set; }
        public string OptieCategorieWerkGuid { get; set; }
        public byte Soort { get; set; }
        public string Omschrijving { get; set; }
        public string Optienummer { get; set; }
        public string OptienummerOmschrijving { get; set; }
        public decimal Aantal { get; set; }
        public string EenheidGuid { get; set; }
        public byte OptieStatus { get; set; }
        public int? OptieSubstatus { get; set; }
        public DateTime? Sluitingsdatum { get; set; }
        public DateTime? DatumDefinitief { get; set; }
        public DateTime? DatumVervallen { get; set; }
        public DateTime? DatumLaatsteMeterkastlijst { get; set; }
        public bool? CommercieleOmschrijvingOpMeterkastlijst { get; set; }
        public string CommercieleOmschrijving { get; set; }
        public string AanvullendeOmschrijving { get; set; }
        public bool? TechnischeOmschrijvingOpMeterkastlijst { get; set; }
        public string TechnischeOmschrijving { get; set; }
        public string OpmerkingenWebsite { get; set; }
        public bool? VerkoopprijsNtb { get; set; }
        public string VerkoopprijsExclBtwOfTekstPrijskolom { get; set; }
        public string VerkoopprijsInclBtwOfTekstPrijskolom { get; set; }
        public bool? VerkoopprijsIsStelpost { get; set; }
        public string TekstPrijskolomGuid { get; set; }
        public bool? Gecontroleerd { get; set; }
        public string OptieGekozenOfferteGuid { get; set; }
        public bool? Factureren { get; set; }
        public bool? ViaWebsite { get; set; }
        public bool? OpTekeningVerwerkt { get; set; }
        public bool? GelimiteerdeGarantie { get; set; }
        public string ToelichtingGelimiteerdeGarantie { get; set; }
        public string TekstGelimiteerdeGarantieGuid { get; set; }
        public string Afbeelding { get; set; }
        public bool? BerekenKaleKostprijs { get; set; }
        public decimal? BedragKaleKostprijs { get; set; }
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
        public decimal? BedragKosten1 { get; set; }
        public decimal? BedragKosten2 { get; set; }
        public decimal? BedragKosten3 { get; set; }
        public decimal? BedragKosten4 { get; set; }
        public decimal? BedragKosten5 { get; set; }
        public decimal? TotaalBedragKosten { get; set; }
        public decimal? Resultaat { get; set; }
        public decimal? EigenVerkoopprijs { get; set; }
        public string OmschrijvingToeslag1 { get; set; }
        public string OmschrijvingToeslag2 { get; set; }
        public decimal? PercentageToeslag1 { get; set; }
        public decimal? PercentageToeslag2 { get; set; }
        public decimal? BedragToeslag1 { get; set; }
        public decimal? BedragToeslag2 { get; set; }
        public decimal? TotaalBedragToeslagen { get; set; }
        public decimal? VerkoopprijsExclBtw { get; set; }
        public decimal? PercentageBtw { get; set; }
        public decimal? BedragBtw { get; set; }
        public decimal? VerkoopprijsInclBtw { get; set; }
        public decimal? VerkoopbedragExclBtw { get; set; }
        public decimal? ResultaatTotaal { get; set; }
        public string VerkoopbedragExclBtwOfTekstPrijskolom { get; set; }
        public decimal? VerkoopbedragInclBtw { get; set; }
        public string VerkoopbedragInclBtwOfTekstPrijskolom { get; set; }
        public string SorteringRubriek { get; set; }
        

        public virtual Eenheid EenheidGu { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        //public virtual OptieCategorieWerk OptieCategorieWerkGu { get; set; }
        public virtual OptieGekozenOfferte OptieGekozenOfferteGu { get; set; }
        //public virtual OptieRubriekWerk OptieRubriekWerkGu { get; set; }
        public virtual OptieStandaard OptieStandaardGu { get; set; }
        //public virtual TekstGelimiteerdeGarantie TekstGelimiteerdeGarantieGu { get; set; }
        //public virtual TekstPrijskolom TekstPrijskolomGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        //public virtual ICollection<Factuurregel> Factuurregel { get; set; }
        //public virtual ICollection<Inkoop> Inkoop { get; set; }
        //public virtual ICollection<VolgjewoningOptieGedownload> VolgjewoningOptieGedownload { get; set; }
    }
}
