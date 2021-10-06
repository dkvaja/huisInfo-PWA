using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class ProjectInfoApiModel
    {
        public ProjectInfoApiModel() { }
        public ProjectInfoApiModel(ViewPortalWerk entity)
        {
            ProjectNo = entity.Werknummer;
            ProjectName = entity.Werknaam;
            Place = entity.Plaats;
            ProjectType = entity.WerkType;
            ProjectConstructionType = entity.WerkSoort;
            ProjectPhase = entity.WerkFase;
            ObjectsCount = entity.AantalObjecten;
            SaleStartDate = entity.DatumStartVerkoop;
            DateStartConstruction = entity.DatumStartBouw;
            DateEndConstruction = entity.DatumEindBouw;
            GeneralInfo = entity.AlgemeneInfo;
        }

        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public string Place { get; set; }
        public string ProjectType { get; set; }
        public string ProjectConstructionType { get; set; }
        public string ProjectPhase { get; set; }
        public int? ObjectsCount { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? DateStartConstruction { get; set; }
        public DateTime? DateEndConstruction { get; set; }
        public string GeneralInfo { get; set; }
    }
}
