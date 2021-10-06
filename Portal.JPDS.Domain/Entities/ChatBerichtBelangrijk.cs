using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ChatBerichtBelangrijk : BaseEntity
    {
        public string ChatBerichtGuid { get; set; }
        public string ChatDeelnemerGuid { get; set; }
        

        public virtual ChatBericht ChatBerichtGu { get; set; }
        public virtual ChatDeelnemer ChatDeelnemerGu { get; set; }
    }
}
