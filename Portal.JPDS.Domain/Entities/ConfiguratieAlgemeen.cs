using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ConfiguratieAlgemeen : BaseEntity
    {
        public string HoofdvestigingEigenOrganisatieGuid { get; set; }
        public string StandaardLandGuid { get; set; }
        public string StandaardTaalGuid { get; set; }
        public string StandaardCorrespondentieSjabloonGuid { get; set; }
        public string StandaardEmailSjabloonGuid { get; set; }
        public string MapSjablonen { get; set; }
        public string MapBijlagen { get; set; }
        public byte BijlageOpslagMethode { get; set; }
        public bool? CommunicatieKenmerkAutomatisch { get; set; }
        public bool? CommunicatieKenmerkWijzigbaar { get; set; }
        public string CommunicatieKenmerkOpmaak { get; set; }
        public bool? Foutopsporing { get; set; }
        public string MaatwerkPrefix { get; set; }
        public string DatabaseMailProfiel { get; set; }

        public virtual Organisatie HoofdvestigingEigenOrganisatieGu { get; set; }
        public virtual Sjabloon StandaardCorrespondentieSjabloonGu { get; set; }
        public virtual Sjabloon StandaardEmailSjabloonGu { get; set; }
        //public virtual Land StandaardLandGu { get; set; }
        //public virtual Taal StandaardTaalGu { get; set; }
    }
}
