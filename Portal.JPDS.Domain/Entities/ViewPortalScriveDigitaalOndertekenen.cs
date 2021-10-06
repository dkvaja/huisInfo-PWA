using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalScriveDigitaalOndertekenen
    {
        public string OptieGekozenOfferteGuid { get; set; }
        public string GebouwGuid { get; set; }
        public int Offertenummer { get; set; }
        public DateTime Sluitingsdatum { get; set; }
        public byte? Ondertekening { get; set; }
        public int? OndertekeningAantalPersonen { get; set; }
        public string KoperHuurderGuid { get; set; }
        public string Persoon1Guid { get; set; }
        public string Persoon2Guid { get; set; }
        public string Login1Guid { get; set; }
        public string Login1Naam { get; set; }
        public string Login1Email { get; set; }
        public string Login2Guid { get; set; }
        public string Login2Naam { get; set; }
        public string Login2Email { get; set; }
    }
}
