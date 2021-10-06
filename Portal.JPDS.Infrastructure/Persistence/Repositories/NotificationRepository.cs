using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using Microsoft.Data.SqlClient;
using Portal.JPDS.AppCore.Models;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<EmailNotificationModel> GetEmailNotifications()
        {
            List<EmailNotificationModel> lstEmailNotificationModel = new List<EmailNotificationModel>();

            var usersWithBuildingNotifications = _dbContext.ViewPortalEmailNotification.Select(x => new { x.LoginGuid, x.GebouwGuid }).Distinct();

            foreach (var notification in usersWithBuildingNotifications)
            {
                var messages = _dbContext.ViewPortalEmailNotification.Where(x => x.LoginGuid == notification.LoginGuid && x.GebouwGuid == notification.GebouwGuid).ToList();
                if (messages.Count > 0)
                {
                    var firstItem = messages.First();
                    lstEmailNotificationModel.Add(new EmailNotificationModel
                    {
                        BuildingNoExtern = firstItem.BouwnummerExtern,
                        Email = firstItem.Email,
                        LetterSalutationFormal = firstItem.BriefaanhefFormeel,
                        LetterSalutationInformal = firstItem.BriefaanhefInFormeel,
                        MainContractorName = firstItem.HoofdaannemerNaam,
                        Name = firstItem.Naam,
                        ProjectNoAndName = firstItem.WerknummerWerknaam,
                        Messages = messages.Select(y => y.Bericht).ToList()
                    });
                }
            }
            return lstEmailNotificationModel;
        }

        public IEnumerable<EmailNotificationModel> GetResolverNewOrInformedNotifications()
        {
            List<EmailNotificationModel> lstEmailNotificationModel = new List<EmailNotificationModel>();
            var resolverNewNotification = GetResolverNotification(ResolverStatus.New);
            var resolverInformedNotification = GetResolverNotification(ResolverStatus.Informed);
            foreach (var notification in resolverNewNotification)
            {
                lstEmailNotificationModel.Add(notification);
            }
            foreach (var notification in resolverInformedNotification)
            {
                lstEmailNotificationModel.Add(notification);
            }
            return lstEmailNotificationModel;
        }

        public IEnumerable<EmailNotificationModel> GetSiteManagerNotifications()
        {
            List<EmailNotificationModel> lstEmailNotificationModel = new List<EmailNotificationModel>();

            var siteManagerData = _dbContext.ViewLogins.Join(_dbContext.LoginRolWerks,
                     x => x.Guid,
                     y => y.LoginGuid,
                     (login, loginrolwerk) => new
                     {
                         login,
                         loginrolwerk
                     }).Join(_dbContext.ViewModuleRoles,
                     x => new { x.loginrolwerk.ModuleGuid, RoleGuid = x.loginrolwerk.RolGuid},
                     y => new { y.ModuleGuid, y.RoleGuid },
                     (x, y) => new
                     {
                         x.login,
                         x.loginrolwerk,
                         y.ModuleName,
                         y.RoleName,
                         y.Active
                     }).Join(_dbContext.Oplosser.Include(y => y.MeldingGu).ThenInclude(x => x.WerkGu).ThenInclude(x => x.HoofdaannemerOrganisatieGu).Include(x => x.OrganisatieGu),
                     x =>  x.loginrolwerk.WerkGuid ,
                     y =>  y.MeldingGu.WerkGuid ,
                     (login, oplosser) => new
                     {
                         login.login,
                         login.loginrolwerk,
                         login.ModuleName,
                         login.RoleName,
                         login.Active,
                         oplosser
                     }).Where(y => (y.oplosser.OplosserStatus == (int)ResolverStatus.Completed || y.oplosser.OplosserStatus == (int)ResolverStatus.TurnedDown)
                       && !string.IsNullOrWhiteSpace(y.oplosser.Werkbonnummer) && y.oplosser.GecontroleerdOp == null
                       && y.ModuleName == LoginModules.ConstructionQuality && y.RoleName == LoginRoles.SiteManager
                       && y.login.OptIn == true && (y.loginrolwerk.GebouwGuid == null || y.loginrolwerk.GebouwGuid == y.oplosser.MeldingGu.GebouwGuid) && y.Active == true && y.loginrolwerk.Actief == true)
                       .Select(x => new
                       {
                           LoginId = x.login.Guid,
                           LoginEmail = x.login.Email,
                           LoginOrganisationId = x.login.OrganisatieGuid,
                           LoginPersonId = x.login.PersoonGuid,
                           LoginRelationId = x.login.RelatieGuid,
                           LoginName = x.login.Naam,
                           ResolverId = x.oplosser.Guid,
                           ProjectId = x.oplosser.MeldingGu.WerkGuid,
                           MainContractorName = x.oplosser.MeldingGu.WerkGu.HoofdaannemerOrganisatieGu.Naam,
                           ProjectNoAndName = x.oplosser.MeldingGu.WerkGu.WerknummerWerknaam
                       }).Distinct().ToList();

            foreach (var groupItem in siteManagerData.GroupBy(x => new { x.LoginId, x.ProjectId }))
            {
                var firstItem = groupItem.FirstOrDefault();
                var item = new EmailNotificationModel()
                {
                    Email = firstItem.LoginEmail,
                    LetterSalutationFormal = _dbContext.GetSalutationForEmail(firstItem.LoginOrganisationId, firstItem.LoginPersonId, firstItem.LoginRelationId, null, true),
                    LetterSalutationInformal = _dbContext.GetSalutationForEmail(firstItem.LoginOrganisationId, firstItem.LoginPersonId, firstItem.LoginRelationId, null, false),
                    MainContractorName = firstItem.MainContractorName,
                    Name = firstItem.LoginName,
                    ProjectNoAndName = firstItem.ProjectNoAndName,
                    Messages = new List<string> { "Actie vereist op de werkbon: " + groupItem.Count() }
                };
                lstEmailNotificationModel.Add(item);
            }
            return lstEmailNotificationModel;
        }

        private IEnumerable<EmailNotificationModel> GetResolverNotification(ResolverStatus resolverStatus)
        {
            List<EmailNotificationModel> lstEmailNotificationModel = new List<EmailNotificationModel>();
            string messageText = string.Empty;

            var resolverData = _dbContext.ViewLogins.Join(_dbContext.LoginRolWerks,
                    x => x.Guid,
                    y => y.LoginGuid,
                    (login, loginrolwerk) => new
                    {
                        login,
                        loginrolwerk
                    }).Join(_dbContext.ViewModuleRoles,
                    x => new { x.loginrolwerk.ModuleGuid, RoleGuid = x.loginrolwerk.RolGuid },
                    y => new { y.ModuleGuid, y.RoleGuid },
                    (x, y) => new
                    {
                        x.login,
                        x.loginrolwerk,
                        y.ModuleName,
                        y.RoleName,
                        y.Active
                    }).Join(_dbContext.Oplosser.Include(y => y.MeldingGu).ThenInclude(x => x.WerkGu).ThenInclude(x => x.HoofdaannemerOrganisatieGu).Include(x => x.OrganisatieGu),
                    x => new { x.login.OrganisatieGuid, x.loginrolwerk.WerkGuid },
                    y => new { y.OrganisatieGuid, y.MeldingGu.WerkGuid },
                    (login, oplosser) => new
                    {
                        login.loginrolwerk,
                        login.login,
                        login.ModuleName,
                        login.RoleName,
                        login.Active,
                        oplosser
                    }).Where(y => y.oplosser.OplosserStatus == (int)resolverStatus
                     && !string.IsNullOrWhiteSpace(y.oplosser.Werkbonnummer)
                     && y.ModuleName == LoginModules.ConstructionQuality
                     && y.RoleName == LoginRoles.SubContractor
                     && (y.loginrolwerk.GebouwGuid == null || y.loginrolwerk.GebouwGuid == y.oplosser.MeldingGu.GebouwGuid)
                     && y.login.OptIn == true && y.Active == true && y.loginrolwerk.Actief == true)
                      .Select(x => new
                      {
                          LoginId = x.login.Guid,
                          LoginEmail = x.login.Email,
                          LoginOrganisationId = x.login.OrganisatieGuid,
                          LoginPersonId = x.login.PersoonGuid,
                          LoginRelationId = x.login.RelatieGuid,
                          LoginName = x.login.Naam,
                          ResolverId = x.oplosser.Guid,
                          ProjectId = x.oplosser.MeldingGu.WerkGuid,
                          MainContractorName = x.oplosser.MeldingGu.WerkGu.HoofdaannemerOrganisatieGu.Naam,
                          ProjectNoAndName = x.oplosser.MeldingGu.WerkGu.WerknummerWerknaam
                      }).Distinct().ToList();

            if (resolverStatus == ResolverStatus.New)
            {
                messageText = "Nieuwe werkbonnen: ";
            }
            else if (resolverStatus == ResolverStatus.Informed)
            {
                messageText = "Eerder verstuurde werkbonnen: ";
            }
            foreach (var groupItem in resolverData.GroupBy(x => new { x.LoginId, x.ProjectId }))
            {
                var firstItem = groupItem.FirstOrDefault();
                var item = new EmailNotificationModel()
                {
                    Email = firstItem.LoginEmail,
                    LetterSalutationFormal = _dbContext.GetSalutationForEmail(firstItem.LoginOrganisationId, firstItem.LoginPersonId, firstItem.LoginRelationId, null, true),
                    LetterSalutationInformal = _dbContext.GetSalutationForEmail(firstItem.LoginOrganisationId, firstItem.LoginPersonId, firstItem.LoginRelationId, null, false),
                    MainContractorName = firstItem.MainContractorName,
                    Name = firstItem.LoginName,
                    ProjectNoAndName = firstItem.ProjectNoAndName,
                    Messages = new List<string> { messageText + groupItem.Count() },
                    ResolverIds = resolverStatus == ResolverStatus.New ? groupItem.Select(x => x.ResolverId) : null
                };
                lstEmailNotificationModel.Add(item);
            }
            return lstEmailNotificationModel;
        }
    }
}
