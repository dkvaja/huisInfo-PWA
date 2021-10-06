using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ChatDeelnemer : BaseEntity
    {
        public ChatDeelnemer()
        {
            ChatBerichtBelangrijk = new HashSet<ChatBerichtBelangrijk>();
            ChatBerichtVerwijderdDoorDeelnemerGu = new HashSet<ChatBericht>();
            ChatBerichtVerzenderChatDeelnemerGu = new HashSet<ChatBericht>();
        }

        public string ChatGuid { get; set; }
        public string LoginGuid { get; set; }
        public DateTime? LaatstGelezenChatBericht { get; set; }

        public virtual Chat ChatGu { get; set; }
        public virtual Login LoginGu { get; set; }
        public virtual ICollection<ChatBerichtBelangrijk> ChatBerichtBelangrijk { get; set; }
        public virtual ICollection<ChatBericht> ChatBerichtVerwijderdDoorDeelnemerGu { get; set; }
        public virtual ICollection<ChatBericht> ChatBerichtVerzenderChatDeelnemerGu { get; set; }
    }
}
