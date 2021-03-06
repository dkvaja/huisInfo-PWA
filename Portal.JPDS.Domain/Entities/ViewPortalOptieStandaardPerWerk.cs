using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalOptieStandaardPerWerk
    {
        public string OptieStandaardGuid { get; set; }
        public string WerkGuid { get; set; }
        public string OptieCategorieWerkGuid { get; set; }
        public string Categorie { get; set; }
        public string OptieRubriekWerkGuid { get; set; }
        public string Rubriek { get; set; }
        public string Optienummer { get; set; }
        public string Omschrijving { get; set; }
        public string CommercieleOmschrijving { get; set; }
        public decimal? Aantal { get; set; }
        public string EenheidGuid { get; set; }
        public string Eenheid { get; set; }
        public int? EenheidAantalDecimalen { get; set; }
        public bool VerkoopprijsNtb { get; set; }
        public bool VerkoopprijsIsStelpost { get; set; }
        public decimal? VerkoopprijsExclBtw { get; set; }
        public string VerkoopprijsExclBtwOfTekstPrijskolom { get; set; }
        public decimal? VerkoopprijsInclBtw { get; set; }
        public string VerkoopprijsInclBtwOfTekstPrijskolom { get; set; }
        public string TekstPrijskolomGuid { get; set; }
        public string TekstPrijskolom { get; set; }
        public int? AantalBijlagen { get; set; }
    }
}
