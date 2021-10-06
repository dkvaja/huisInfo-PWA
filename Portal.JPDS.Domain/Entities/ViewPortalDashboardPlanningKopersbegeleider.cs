using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalDashboardPlanningKopersbegeleider
    {
        public string Guid { get; set; }
        public string WerkGuid { get; set; }
        public int? GebouwGuid { get; set; }
        public string Omschrijving { get; set; }
        public DateTime? ActieDatum { get; set; }
        public TimeSpan? ActieStarttijd { get; set; }
    }
}
