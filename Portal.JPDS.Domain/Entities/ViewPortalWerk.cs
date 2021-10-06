using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalWerk
    {
        public string WerkGuid { get; set; }
        public string Werknummer { get; set; }
        public string Werknaam { get; set; }
        public string Plaats { get; set; }
        public string WerkType { get; set; }
        public string WerkSoort { get; set; }
        public string WerkFase { get; set; }
        public int? AantalObjecten { get; set; }
        public DateTime? DatumStartVerkoop { get; set; }
        public DateTime? DatumStartBouw { get; set; }
        public DateTime? DatumEindBouw { get; set; }
        public string Logo { get; set; }
        public string AchtergrondWebPortal { get; set; }
        public string AlgemeneInfo { get; set; }
    }
}
