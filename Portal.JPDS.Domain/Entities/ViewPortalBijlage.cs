using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalBijlage
    {
        public string BijlageGuid { get; set; }
        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string BijlageRubriekGuid { get; set; }
        public string BijlageRubriek { get; set; }
        public DateTime? Datum { get; set; }
        public string Omschrijving { get; set; }
        public string Bijlage { get; set; }
    }
}
