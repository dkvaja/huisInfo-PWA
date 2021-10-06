using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class DashboardActionApiModel
    {
        public DashboardActionApiModel() { }
        public string ActionId { get; set; }
        public string ProjectId { get; set; }
        public string BuildingId { get; set; }
        public string EmployeeId { get; set; }
        public string Description { get; set; }
        public string DescriptionExtended { get; set; }
        public DateTime? ActionDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string BuildingNoIntern { get; set; }
        public string BuildingNoExtern { get; set; }
        public byte? BuyerRenterType { get; set; }
        public string BuyerRenterP1Name { get; set; }
        public string BuyerRenterP2Name { get; set; }
        public string BuyerRenterOrganisationName { get; set; }
        public string BuyerRenterRelationName { get; set; }
    }
}
