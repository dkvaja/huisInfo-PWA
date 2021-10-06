using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class GebouwStatus : BaseEntity
    {
        public GebouwStatus()
        {
            //ActieStandaards = new HashSet<ActieStandaard>();
            Gebouws = new HashSet<Gebouw>();
        }

        public string GebouwStatus1 { get; set; }
        public bool? Systeemwaarde { get; set; }

        //public virtual ICollection<ActieStandaard> ActieStandaards { get; set; }
        public virtual ICollection<Gebouw> Gebouws { get; set; }
    }
}
