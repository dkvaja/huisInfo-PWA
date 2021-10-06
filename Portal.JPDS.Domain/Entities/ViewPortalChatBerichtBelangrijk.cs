using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChatBerichtBelangrijk
    {
        public string ChatBerichtBelangrijkGuid { get; set; }
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
        public DateTime DatumEnTijd { get; set; }
        public string ChatOnderwerp { get; set; }
        public DateTime? ChatBerichtBelangrijkIngevoerdOp { get; set; }
        public string BijlageGuid { get; set; }
        public string Bijlage { get; set; }
    }
}
