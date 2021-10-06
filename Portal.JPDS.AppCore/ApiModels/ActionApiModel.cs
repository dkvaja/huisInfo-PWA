using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class ActionApiModel
    {
        public string BuildingId { get; set; }
        public string EmployeeId { get; set; }
        public string Description { get; set; }
        public string DescriptionExtended { get; set; }
        public DateTime DateTime { get; set; }
    }
}
