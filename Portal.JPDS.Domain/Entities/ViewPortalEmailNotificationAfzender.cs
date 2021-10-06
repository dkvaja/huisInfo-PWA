using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewPortalEmailNotificationAfzender
    {
        public string EmailAccountGuid { get; set; }
        public string MedewerkerGuid { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPasswordDecrypted { get; set; }
        public string SmtpFromEmail { get; set; }
        public bool SmtpUseSsl { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
