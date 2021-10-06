using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class OrganisationApiModel
    {    
        public OrganisationApiModel(Organisatie entity)
        {
            Id = entity.Guid;
            Name = entity.NaamOnderdeel;
            Relations = entity.Relatie.Select(x => new OrganisationEmployeeApiModel(x));
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<OrganisationEmployeeApiModel> Relations { get; set; }
    }

    public class OrganisationEmployeeApiModel
    {
        public OrganisationEmployeeApiModel(Relatie relatie)
        {
            Id = relatie.Guid;
            Name = relatie.Zoeknaam;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
