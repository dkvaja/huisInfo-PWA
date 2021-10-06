using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
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
    public interface IConfigRepository
    {
        string GetBrowserTabNameInternal();
        string GetBrowserTabNameExternal();
        string GetWebPortalLogo();
        string GetWebPortalLoginBackground();
        SmtpModel GetSmtpConfig();
        EmailTemplateModel GetStandardNotificationEmailTemplate();
        DigitalSigningApiConfigModel GetDigitalSigningApiConfig();
        SettingsModel GetConfigSettings();
        EmailTemplateModel GetOfficialReportOfCompletionEmailTemplate();
        EmailTemplateModel RepairRequestConfirmationEmailTemplate();
        EmailTemplateModel GetForgotPasswordEmailTemplate();
        IEnumerable<SettlementTermApiModel> GetSettlementTerms();
        OfflineConfigApiModel GetOfflineConfig(string userId);
        string GetRepairRequestAddWarningText();
        EmailTemplateModel WorkOrderEmailTemplate();
        EmailTemplateModel RepairRequestEmailTemplate();
        SmtpModel GetConfigSettingForSender(string employeeId);
        EmailTemplateModel DossierNotificationEmailTemplate();
    }
}
