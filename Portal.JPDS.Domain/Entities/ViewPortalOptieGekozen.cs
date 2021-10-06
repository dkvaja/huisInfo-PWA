using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalOptieGekozen
    {
        public string Guid { get; set; }
        public string OptieStandaardGuid { get; set; }
        public string OptieCategorieWerkGuid { get; set; }
        public string Categorie { get; set; }
        public short? CategorieVolgorde { get; set; }
        public string GebouwGuid { get; set; }
        public string OptieRubriekWerkGuid { get; set; }
        public string Rubriek { get; set; }
        public short? RubriekVolgorde { get; set; }
        public byte Soort { get; set; }
        public string Optienummer { get; set; }
        public string Omschrijving { get; set; }
        public decimal Aantal { get; set; }
        public string EenheidGuid { get; set; }
        public string Eenheid { get; set; }
        public int? AantalDecimalen { get; set; }
        public string CommercieleOmschrijving { get; set; }
        public string AanvullendeOmschrijving { get; set; }
        public bool VerkoopprijsNtb { get; set; }
        public bool VerkoopprijsIsStelpost { get; set; }
        public byte OptieStatus { get; set; }
        public int? OptieSubstatus { get; set; }
        public DateTime? Sluitingsdatum { get; set; }
        public string VerkoopprijsInclBtwOfTekstPrijskolom { get; set; }
        public decimal? VerkoopprijsInclBtw { get; set; }
        public string OptieGekozenOfferteGuid { get; set; }
        public int? Offertenummer { get; set; }
    }
}
