using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class PlanningApiModel
    {
        public PlanningApiModel() { }
        public PlanningApiModel(ViewPortalDashboardPlanningKoperHuurder entity)
        {
            Description = entity.Omschrijving;
            Date = entity.ActieDatum;
            StartTime = entity.ActieStarttijd?.ToString(@"hh\:mm");
        }
        public PlanningApiModel(ViewPortalDashboardPlanningKopersbegeleider entity)
        {
            Description = entity.Omschrijving;
            Date = entity.ActieDatum;
            StartTime = entity.ActieStarttijd?.ToString(@"hh\:mm");
        }

        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string StartTime { get; set; }
        public string DossierId { get; set; }
        public bool IsDossierExternal { get; set; }
    }
}
