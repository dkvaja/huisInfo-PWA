using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class RelaionInfoApiModel
    {
        public RelaionInfoApiModel() { }
        public RelaionInfoApiModel(ViewPortalBetrokkeneRelatieKopersbegeleider entity)
        {
            InvolvedRelationId = entity.BetrokkeneRelatieGuid;
            PersonId = entity.PersoonGuid;
            FullName = entity.VolledigeNaam;
            Function = entity.Functie;
            Email = entity.Email;
            Telephone = entity.Telefoon;
            Mobile = entity.Mobiel;
            HasPhoto = !string.IsNullOrEmpty(entity.Foto);
        }
        public string InvolvedRelationId { get; set; }
        public string PersonId { get; set; }
        public string FullName { get; set; }
        public string Function { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public bool HasPhoto { get; set; }
    }
}
