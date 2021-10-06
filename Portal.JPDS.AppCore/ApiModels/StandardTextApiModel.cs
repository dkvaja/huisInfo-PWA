using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class StandardTextApiModel
    {
        public StandardTextApiModel() { }
        public StandardTextApiModel(Tekstblok entity)
        {
            TextId = entity.Guid;
            ProjectId = entity.WerkGuid;
            Hashtag = entity.Zoekterm;
            TextBlock = entity.Tekstblok1;
            IsSignature = entity.TekstblokIsHandtekening;
        }

        public string TextId { get; set; }
        public string ProjectId { get; set; }
        public string Hashtag { get; set; }
        public string TextBlock { get; set; }
        public bool IsSignature { get; set; }
    }
}
