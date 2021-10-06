using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Common
{
    public  interface IEmailService
    {
        Task<SenderEmployeeModel> SendEmailAsync(MailMessage mailMessage, string senderEmployeeId = null);
    }
}
