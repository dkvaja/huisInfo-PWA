using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalEmailNotification
    {
        public string LoginGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string WerknummerWerknaam { get; set; }
        public string BouwnummerExtern { get; set; }
        public string Email { get; set; }
        public string Naam { get; set; }
        public string BriefaanhefFormeel { get; set; }
        public string BriefaanhefInFormeel { get; set; }
        public string Bericht { get; set; }
        public string HoofdaannemerNaam { get; set; }
    }
}
