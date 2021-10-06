using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalChatDeelnemer
    {
        public string ChatDeelnemerGuid { get; set; }
        public string ChatGuid { get; set; }
        public string LoginGuid { get; set; }
        public string Naam { get; set; }
        public byte? LoginAccountVoor { get; set; }
    }
}
