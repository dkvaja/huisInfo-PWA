using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalGebouwAlgemeen
    {
        public string GebouwGuid { get; set; }
        public string BouwnummerExtern { get; set; }
        public string WoningType { get; set; }
        public string GebouwSoort { get; set; }
        public decimal? VrijOpNaamPrijsInclBtw { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string HuisnummerToevoeging { get; set; }
        public string Postcode { get; set; }
        public string Plaats { get; set; }
        public string EanElektriciteit { get; set; }
        public string EanGas { get; set; }
        public string Garantieregeling { get; set; }
        public DateTime? DatumEindeOnderhoudstermijn { get; set; }
        public string GarantieCertificaatNummer { get; set; }
        public DateTime? GarantieCertificaatGeldigVa { get; set; }
        public DateTime? GarantieCertificaatGeldigTm { get; set; }
    }
}
