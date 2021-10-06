using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChat
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
        public string ChatBegonnenDoorLoginGuid { get; set; }
        public DateTime? ChatBegonnenOp { get; set; }
        public DateTime? LaatstGelezenChatBericht { get; set; }
        public DateTime? DatumEnTijdLaatstVerzondenChatBericht { get; set; }
        public int? AantalOngelezenBerichten { get; set; }
        public string EersteRegelLaatsteChatBericht { get; set; }
        public string LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid { get; set; }
        public string LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam { get; set; }
        public string LaatsteChatBerichtBijlage { get; set; }
        public string ChatOnderwerp { get; set; }
    }
}
