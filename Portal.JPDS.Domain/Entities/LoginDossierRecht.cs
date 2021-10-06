using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class LoginDossierRecht : BaseEntity
    {
        public string LoginGuid { get; set; }
        public string DossierGuid { get; set; }
        public string ModuleGuid { get; set; }
        public string RolGuid { get; set; }
        public bool Extern { get; set; }
        public bool ExternRelatieZichtbaar { get; set; }
        public bool ExternBestandWijzigen { get; set; }
        public bool Intern { get; set; }
        public bool InternRelatieZichtbaar { get; set; }
        public bool InternBestandWijzigen { get; set; }
        public bool DossierToevoegen { get; set; }
        public bool DossierWijzigen { get; set; }
        public bool DossierArchiveren { get; set; }
        public bool DossierAfsluiten { get; set; }
        public bool BestandUpload { get; set; }
        public bool BestandDownload { get; set; }
        public bool BestandArchiveren { get; set; }
        public virtual Dossier DossierGu { get; set; }
        public virtual Login LoginGu { get; set; }
    }
}
