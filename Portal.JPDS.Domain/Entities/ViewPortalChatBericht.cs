using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChatBericht
    {
        public string ChatBerichtGuid { get; set; }
        public string ChatGuid { get; set; }
        public string VerzenderChatDeelnemerGuid { get; set; }
        public string VerzenderLoginGuid { get; set; }
        public string VerzenderNaam { get; set; }
        public string Bericht { get; set; }
        public DateTime DatumEnTijd { get; set; }
        public string ActieGuid { get; set; }
        public bool Belangrijk { get; set; }
        public bool Verwijderd { get; set; }
        public string VerwijderdDoorDeelnemer { get; set; }
        public string BijlageGuid { get; set; }
        public string Bijlage { get; set; }
        public string ReactieOpChatBerichtGuid { get; set; }
        public string ReactieOpChatBerichtBericht { get; set; }
        public string ReactieOpChatBerichtVerzenderChatDeelnemerGuid { get; set; }
        public bool? ReactieOpChatBerichtVerwijderd { get; set; }
        public string ReactieOpChatBerichtBijlage { get; set; }
    }
}
