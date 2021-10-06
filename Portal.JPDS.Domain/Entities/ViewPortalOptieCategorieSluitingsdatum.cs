using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalOptieCategorieSluitingsdatum
    {
        public string OptieCategorieWerkGuid { get; set; }
        public string Categorie { get; set; }
        public short? Volgorde { get; set; }
        public DateTime? Sluitingsdatum { get; set; }
        public string GebouwGuid { get; set; }
    }
}
