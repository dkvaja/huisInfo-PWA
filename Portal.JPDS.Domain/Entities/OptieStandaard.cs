using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class OptieStandaard : BaseEntity 
    {
        public OptieStandaard()
        {
            Bijlages = new HashSet<Bijlage>();
            //Calculatieregels = new HashSet<Calculatieregel>();
            //Inkoopprijs = new HashSet<Inkoopprij>();
            OptieGekozens = new HashSet<OptieGekozen>();
            //OptieStandaardWoningTypes = new HashSet<OptieStandaardWoningType>();
            //VolgjewoningOptieGedownloads = new HashSet<VolgjewoningOptieGedownload>();
        }

        public string WerkGuid { get; set; }
        public string OptieRubriekWerkGuid { get; set; }
        public string OptieCategorieWerkGuid { get; set; }
        public string Omschrijving { get; set; }
        public string Optienummer { get; set; }
        public string OptienummerOmschrijving { get; set; }
        public decimal? Aantal { get; set; }
        public string EenheidGuid { get; set; }
        public bool? CommercieleOmschrijvingOpMeterkastlijst { get; set; }
        public string CommercieleOmschrijving { get; set; }
        public bool? TechnischeOmschrijvingOpMeterkastlijst { get; set; }
        public string TechnischeOmschrijving { get; set; }
        public bool? VerkoopprijsNtb { get; set; }
        public string VerkoopprijsExclBtwOfTekstPrijskolom { get; set; }
        public string VerkoopprijsInclBtwOfTekstPrijskolom { get; set; }
        public bool? VerkoopprijsIsStelpost { get; set; }
        public string TekstPrijskolomGuid { get; set; }
        public bool? Publiceren { get; set; }
        public bool? Gecontroleerd { get; set; }
        public bool? Factureren { get; set; }
        public bool? RubriekVragen { get; set; }
        public bool? InvulvakjeOpKeuzelijst { get; set; }
        public bool? GeimporteerdUitCufXml { get; set; }
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
        public string SorteringRubriek { get; set; }

        public virtual Eenheid EenheidGu { get; set; }
        //public virtual OptieCategorieWerk OptieCategorieWerkGu { get; set; }
        public virtual OptieRubriekWerk OptieRubriekWerkGu { get; set; }
        //public virtual TekstGelimiteerdeGarantie TekstGelimiteerdeGarantieGu { get; set; }
        //public virtual TekstPrijskolom TekstPrijskolomGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Bijlage> Bijlages { get; set; }
        //public virtual ICollection<Calculatieregel> Calculatieregels { get; set; }
        //public virtual ICollection<Inkoopprij> Inkoopprijs { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozens { get; set; }
        //public virtual ICollection<OptieStandaardWoningType> OptieStandaardWoningTypes { get; set; }
        //public virtual ICollection<VolgjewoningOptieGedownload> VolgjewoningOptieGedownloads { get; set; }
    }
}
