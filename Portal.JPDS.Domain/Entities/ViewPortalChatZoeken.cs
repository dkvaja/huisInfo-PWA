using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChatZoeken
    {
        public string ChatGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string WerkGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string ChatDeelnemerGuid { get; set; }
        public string LoginGuid { get; set; }
        public string OrganisatieNaam { get; set; }
        public string BouwnummerIntern { get; set; }
        public string BouwnummerExtern { get; set; }
        public string ChatBerichtGuid { get; set; }
        public string Bericht { get; set; }
        public string VerzenderChatDeelnemerGuid { get; set; }
        public string VerzenderNaam { get; set; }
        public DateTime DatumEnTijd { get; set; }
        public int BijlageAanwezig { get; set; }
        public string ChatOnderwerp { get; set; }
    }
}
