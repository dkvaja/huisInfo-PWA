using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ChatBericht : BaseEntity
    {
        public ChatBericht()
        {
            Bijlage = new HashSet<Bijlage>();
            ChatBerichtBelangrijk = new HashSet<ChatBerichtBelangrijk>();
            InverseReactieOpChatBerichtGu = new HashSet<ChatBericht>();
        }

        public string ChatGuid { get; set; }
        public string VerzenderChatDeelnemerGuid { get; set; }
        public string ReactieOpChatBerichtGuid { get; set; }
        public string Bericht { get; set; }
        public DateTime DatumEnTijd { get; set; }
        public string ActieGuid { get; set; }
        public bool Verwijderd { get; set; }
        public string VerwijderdDoorDeelnemerGuid { get; set; }
        public bool Belangrijk { get; set; }


        //public virtual Actie ActieGu { get; set; }
        public virtual Chat ChatGu { get; set; }
        public virtual ChatBericht ReactieOpChatBerichtGu { get; set; }
        public virtual ChatDeelnemer VerwijderdDoorDeelnemerGu { get; set; }
        public virtual ChatDeelnemer VerzenderChatDeelnemerGu { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<ChatBerichtBelangrijk> ChatBerichtBelangrijk { get; set; }
        public virtual ICollection<ChatBericht> InverseReactieOpChatBerichtGu { get; set; }
    }
}
