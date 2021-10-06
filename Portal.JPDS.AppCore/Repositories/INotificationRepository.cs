using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Repositories
{
    /// <summary>
    /// Author : Abhishek Saini
    /// This is interface which should be implemented in outer layer.
    /// </summary>
    public interface INotificationRepository
    {
        IEnumerable<EmailNotificationModel> GetEmailNotifications();
        IEnumerable<EmailNotificationModel> GetResolverNewOrInformedNotifications();
        IEnumerable<EmailNotificationModel> GetSiteManagerNotifications();
    }
}
