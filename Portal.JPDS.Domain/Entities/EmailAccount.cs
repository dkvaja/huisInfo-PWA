using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class EmailAccount : BaseEntity
    {
        public EmailAccount()
        {
            ConfiguratieWebPortals = new HashSet<ConfiguratieWebPortal>();
        }
        public string MedewerkerGuid { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpPasswordHash { get; set; }
        public string SmtpFromEmail { get; set; }
        public bool? PrimairEmailAccount { get; set; }
        public bool? SmtpUseSsl { get; set; }
        public virtual Medewerker MedewerkerGu { get; set; }
        public virtual ICollection<ConfiguratieWebPortal> ConfiguratieWebPortals { get; set; }
    }
}
