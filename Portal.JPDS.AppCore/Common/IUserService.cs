using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Common
{
    public interface IUserService
    {
        UserApiModel Authenticate(string email, string password);
        UserApiModel GetById(string guid);
        UserApiModel GetUserForViewOnly(string userId);
        Apps[] GetUserApps(string userId);
        string GetTokenForResetPasswordAndUpdateResetPasswordLinkCreatedDateTime(string email);
        string GetUserNameById(string userId);
        bool IsOldPassword(string userId, string newPassword);
        bool UpdatePassword(string userId, string newPassword);
        bool IsValidResetPasswordLinkCreatedDateTime(string userId, DateTime resetPasswordLinkCreatedDateTimeFromClaim);
        Dictionary<string, string> GetDefaultEmailTokensForUser(string email);
        EmailTemplateModel GetForgotPasswordEmailTemplate();
        OfflineConfigApiModel GetOfflineConfig(string userId);
        bool UpdateOfflineConfig(string userId, OfflineConfigApiModel offlineConfig);
        bool VerifyCurrentPassword(string userId, string oldPassword);
    }
}
