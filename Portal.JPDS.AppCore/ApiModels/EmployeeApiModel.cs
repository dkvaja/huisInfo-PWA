using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class EmployeeApiModel
    {
        public EmployeeApiModel() { }
        public EmployeeApiModel(ViewPortalMedewerker viewPortalMedewerker)
        {
            Id = viewPortalMedewerker.Guid;
            Name = viewPortalMedewerker.PersoonVolledigeNaam;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
