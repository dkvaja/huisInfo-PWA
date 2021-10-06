using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class SmtpModel
    {
        public SmtpModel(ViewPortalEmailNotificationAfzender entity)
        {
            User = entity.SmtpUser;
            Password = entity.SmtpPasswordDecrypted;
            Email = entity.SmtpFromEmail;
            UseSSL = entity.SmtpUseSsl;
            Host = entity.SmtpHost;
            Port = entity.SmtpPort;
            EmailAccountId = entity.EmailAccountGuid;
            EmployeeId = entity.MedewerkerGuid;
        }

        public string User { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool UseSSL { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string EmailAccountId { get; set; }
        public string EmployeeId { get; set; }
    }

    public class SenderEmployeeModel
    {
        public string EmployeeId { get; set; }
        public string EmailAccountId { get; set; }
    }
}
