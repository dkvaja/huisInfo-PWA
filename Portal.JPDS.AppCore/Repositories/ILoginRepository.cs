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
    public interface ILoginRepository
    {
        /// <summary>
        /// This will return a User api model object based on email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserApiModel GetLoginByEmailAndPasswordAndUpdateLastLoginTime(string email, string password);

        UserApiModel GetLoginById(string guid);

        UserApiModel GetActiveAccountByEmailAndUpdateResetPasswordLinkCreatedDateTime(string email);

        string GetUserNameById(string userId);

        bool IsOldPassword(string userId, string newPassword);

        UserApiModel UpdatePassword(string userId, string newPassword);

        bool IsValidResetPasswordLinkCreatedDateTime(string userId, DateTime ResetPasswordLinkCreatedDateTimeFromClaim);

        Dictionary<string, string> GetDefaultEmailTokensForUser(string email);
        bool UpdateOfflineConfig(string userId, OfflineConfigApiModel offlineConfig);
        bool VerifyCurrentPassword(string userId, string oldPassword);
        List<UserModuleModel> GetAvailableModulesWithRolesForUser(string userId, string projectId = null, string buildingId = null);
    }
}
