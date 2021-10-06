using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.JPDS.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static UserApiModel RemovePassword(this UserApiModel user)
        {
            user.Password = null;
            return user;
        }

        public static Claim[] GetUserClaims(this UserApiModel user)
        {
            var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FullName", user.Name),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                    new Claim(PolicyClaimType.Access,PolicyClaimValue.FullAccess)
                    };
            return claims;
        }

        public static Claim[] GetUserViewOnlyClaims(this UserApiModel user)
        {
            var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FullName", user.Name),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                    new Claim(PolicyClaimType.Access,PolicyClaimValue.ViewOnly)
                    };
            return claims;
        }

        public static Claim[] GetForgotPasswordUserClaims(this UserApiModel user)
        {
            var resetPasswordLinkCreatedDateTime = user.ResetPasswordLinkCreatedDateTime.HasValue ? user.ResetPasswordLinkCreatedDateTime.Value.ToString() : "";
            var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim("ResetPasswordLinkCreatedDateTime", resetPasswordLinkCreatedDateTime),
                    new Claim(PolicyClaimType.Access,PolicyClaimValue.ResetPasswordOnly)
                    };
            return claims;
        }

        public static string GetClaim(this ClaimsPrincipal principal, string ClaimType)
        {
            return principal.FindFirstValue(ClaimType);
        }
    }
}
