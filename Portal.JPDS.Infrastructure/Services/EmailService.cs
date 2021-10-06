using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepoSupervisor _repoSupervisor;
        public EmailService(IRepoSupervisor repoSupervisor)
        {
            _repoSupervisor = repoSupervisor;
        }

        public async Task<SenderEmployeeModel> SendEmailAsync(MailMessage mailMessage,string senderEmployeeId = null)
        {
            var smtpSettings = _repoSupervisor.Config.GetSmtpConfig();

            if (!string.IsNullOrWhiteSpace(senderEmployeeId))
            {
                var senderSmtpSettings = _repoSupervisor.Config.GetConfigSettingForSender(senderEmployeeId);
                if (senderSmtpSettings != null)
                {
                    smtpSettings.EmailAccountId = senderSmtpSettings.EmailAccountId;
                    smtpSettings.EmployeeId = senderSmtpSettings.EmployeeId;
                    smtpSettings.Email = senderSmtpSettings.Email;
                    smtpSettings.User = senderSmtpSettings.User;
                    smtpSettings.UseSSL = senderSmtpSettings.UseSSL;
                    smtpSettings.Password = senderSmtpSettings.Password;
                }
            }

            if (mailMessage.From == null)
            {
                mailMessage.From = new MailAddress(smtpSettings.Email, smtpSettings.User);
            }

            SmtpClient client = new SmtpClient(smtpSettings.Host);
            client.Port = smtpSettings.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (!string.IsNullOrEmpty(smtpSettings.Password))
            {
                client.Credentials = new System.Net.NetworkCredential(smtpSettings.Email, smtpSettings.Password);
            }
            else
            {
                client.UseDefaultCredentials = true;
            }

            client.EnableSsl = smtpSettings.UseSSL;
            await client.SendMailAsync(mailMessage);
            return new SenderEmployeeModel
            {
                EmployeeId = smtpSettings.EmployeeId,
                EmailAccountId = smtpSettings.EmailAccountId
            };
        }
    }
}
