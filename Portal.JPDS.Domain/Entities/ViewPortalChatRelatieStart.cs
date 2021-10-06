using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChatRelatieStart
    {
        public string ChatContactGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string PersoonNaam { get; set; }
        public string OrganisatieGuid { get; set; }
        public string OrganisatieNaam { get; set; }
        public string GebouwGuid { get; set; }
        public string WerkGuid { get; set; }
        public string BouwnummerIntern { get; set; }
        public string BouwnummerExtern { get; set; }
    }
}
