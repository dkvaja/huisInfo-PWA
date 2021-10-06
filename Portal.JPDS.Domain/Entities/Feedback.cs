using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public Feedback()
        {
        }
        public string LoginGuid { get; set; }
        public string Opmerking { get; set; }
    }
}
