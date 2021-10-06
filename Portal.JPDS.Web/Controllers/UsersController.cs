using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Web.Dtos;
using Portal.JPDS.Web.Helpers;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.AppCore.ApiModels;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Portal.JPDS.AppCore.Helpers;
using System.Text.RegularExpressions;

namespace Portal.JPDS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly AppSettings _appSettings;
        private readonly IMimeMappingService _mimeMappingService;
        public UsersController(IUserService userService, IMimeMappingService mimeMappingService, IOptions<AppSettings> appSettings, IEmailService emailService, IRepoSupervisor repoSupervisor)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _emailService = emailService;
            _repoSupervisor = repoSupervisor;
            _mimeMappingService = mimeMappingService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginDto loginDto)
        {
            if (loginDto != null && !string.IsNullOrWhiteSpace(loginDto.Email) && !string.IsNullOrWhiteSpace(loginDto.Password))
            {
                var user = _userService.Authenticate(loginDto.Email, loginDto.Password);
                if (user != null)
                {
                    if (user.Active)
                    {
                        if (!string.IsNullOrEmpty(user.Token))
                        {
                            //Save token in session object
                            HttpContext.Session.SetString("JWToken", user.Token);
                        }
                        if (!loginDto.Remember)
                        {
                            user.Token = null;
                        }

                        return Ok(user);
                    }
                    else
                    {
                        return BadRequest("login.accountdisabled.error");
                    }
                }
            }
            return BadRequest("login.badcredentials.error");
        }

        [Authorize(PolicyConstants.ViewOnlyAccess)]
        [HttpGet("GetLoggedInUser")]
        public IActionResult GetLoggedInUser()
        {
            var user = _userService.GetById(User.Identity.Name);
            if (user == null)
                return BadRequest("login.nouserfound.error");
            
            //check if user is still active...
            if(User.GetClaim(PolicyClaimType.Access) == PolicyClaimValue.FullAccess && !user.Active)
                return BadRequest("login.nouserfound.error");


            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                var getTokenTask = HttpContext.GetTokenAsync("access_token");
                getTokenTask.Wait();
            }

            return Ok(user);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("GetViewAsUserInfo/{loginId}")]
        public IActionResult GetViewAsUserInfo(Guid loginId)
        {

            var buildingsForLoggedInUser = (IEnumerable<UserObjectApiModel>)_repoSupervisor.UserObjects.GetUserObjectsForApp(User.Identity.Name.ToUpperInvariant(), Apps.BuyerGuide);
            var buildingsForViewAsUser = (IEnumerable<UserObjectApiModel>)_repoSupervisor.UserObjects.GetUserObjectsForApp(loginId.ToUpperString(), Apps.BuyerGuide);
            if (!buildingsForLoggedInUser.Any(x => buildingsForViewAsUser.Any(y => y.BuildingId == x.BuildingId)))
            {
                return Forbid();
            }

            var loggedInUser = _userService.GetById(User.Identity.Name);
            var viewAsUser = _userService.GetUserForViewOnly(loginId.ToUpperString());

            if (loggedInUser == null || viewAsUser == null)
                return BadRequest("login.nouserfound.error");

            if (loggedInUser.Type == (int)AccountType.Buyer || viewAsUser.Type != (int)AccountType.Buyer)
            {
                return Forbid();
            }

            return Ok(viewAsUser);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpGet("SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("RequestPasswordReset")]
        public IActionResult RequestPasswordReset([FromBody] LoginDto loginDto)
        {
            if (loginDto != null && !string.IsNullOrWhiteSpace(loginDto.Email))
            {
                var resetToken = _userService.GetTokenForResetPasswordAndUpdateResetPasswordLinkCreatedDateTime(loginDto.Email);

                if (!string.IsNullOrEmpty(resetToken))
                {
                    var emailTemplate = _userService.GetForgotPasswordEmailTemplate();
                    var tokens = _userService.GetDefaultEmailTokensForUser(loginDto.Email);
                    tokens["[nieuw_wachtwoord_url]"] = _appSettings.SiteUrl + "reset?AccessToken=" + (resetToken);
                    
                    emailTemplate.UpdateTokenValues(tokens);

                    using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                    {

                        mail.To.Add(loginDto.Email);

                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailTemplate.TemplateHtml;
                        mail.IsBodyHtml = true;
                        try
                        {
                            var task = _emailService.SendEmailAsync(mail);
                            task.Wait();

                            if (task.Exception != null)
                                return BadRequest("general.email.error");
                        }
                        catch (Exception ex)
                        {
                            return BadRequest("general.email.error");
                        }
                    }
                }
                return Ok(new { status = true });
            }
            return BadRequest("reset.badcredentials.error");
        }

        [Authorize(PolicyConstants.ResetPasswordOnly)]
        [HttpGet("GetUserNameForResetPassword")]
        public IActionResult GetUserNameForResetPassword()
        {
            var resetPasswordLinkCreatedDateTime = User.GetClaim("ResetPasswordLinkCreatedDateTime");
            var userId = User.Identity.Name;

            if (!string.IsNullOrEmpty(resetPasswordLinkCreatedDateTime) && !string.IsNullOrEmpty(userId))
            {
                bool isValidRequest = _userService.IsValidResetPasswordLinkCreatedDateTime(userId, Convert.ToDateTime(resetPasswordLinkCreatedDateTime));
                if (isValidRequest)
                {
                    return Ok(new { userName = _userService.GetUserNameById(userId) });
                }
            }
            return BadRequest("reset.invalid.error");
        }

        [Authorize(PolicyConstants.ResetPasswordOnly)]
        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword([FromBody] string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                var resetPasswordLinkCreatedDateTime = User.GetClaim("ResetPasswordLinkCreatedDateTime");
                var userId = User.Identity.Name;

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(resetPasswordLinkCreatedDateTime))
                {
                    bool isValidRequest = _userService.IsValidResetPasswordLinkCreatedDateTime(userId, Convert.ToDateTime(resetPasswordLinkCreatedDateTime));
                    if (isValidRequest)
                    {
                        var isOldPassword = _userService.IsOldPassword(userId, password);
                        if (!isOldPassword)
                        {
                            if (_userService.UpdatePassword(userId, password))
                            {
                                return Ok();
                            }
                            else
                            {
                                return BadRequest("reset.badcredentials.error");
                            }
                        }
                        return BadRequest("reset.update.error");
                    }
                }
                return BadRequest("reset.invalid.error");
            }
            return BadRequest("reset.password.error");
        }

        [HttpGet("GetOfflineConfig")]
        public IActionResult GetOfflineConfig()
        {
            var userId = User.Identity.Name;
            var config = _userService.GetOfflineConfig(userId);
            return Ok(config);
        }

        [Authorize(PolicyConstants.FullAccess)]
        [HttpPost("UpdateOfflineConfig")]
        public IActionResult UpdateOfflineConfig(OfflineConfigApiModel offlineConfig)
        {
            if (offlineConfig == null)
                return BadRequest("Model is empty");
            if (
                offlineConfig.Mode == true
                && (
                    offlineConfig.DaysForDelivery <= 0
                    || offlineConfig.DaysForInspection <= 0
                    || offlineConfig.DaysForPreDelivery <= 0
                    || offlineConfig.DaysForSecondSignature <= 0
                    )
                )
            {
                return BadRequest("Model is invalid. Value can not be less than 1 day");
            }

            var userId = User.Identity.Name;
            var config = _userService.UpdateOfflineConfig(userId, offlineConfig);

            return Ok(config);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordApiModel changePasswordRequest)
        {
            if (changePasswordRequest == null) return BadRequest("Model is empty.");

            if (string.IsNullOrEmpty(changePasswordRequest.OldPassword))
                throw new HttpResponseException(ErrorConstant.OldPasswordMissing, "Old Password is missing", (int)HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(changePasswordRequest.NewPassword))
                throw new HttpResponseException(ErrorConstant.NewPasswordMissing, "New Password is missing", (int)HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(changePasswordRequest.ConfirmPassword))
                throw new HttpResponseException(ErrorConstant.ConfirmPasswordMissing, "Confirm Password is missing", (int)HttpStatusCode.BadRequest);

            if (!changePasswordRequest.NewPassword.Equals(changePasswordRequest.ConfirmPassword, StringComparison.InvariantCulture))
                throw new HttpResponseException(ErrorConstant.PasswordConfirmPasswordMismatch, "Password & Confirm Password doesn't match", (int)HttpStatusCode.BadRequest);

            if (!ValidatePasswordPolicy(changePasswordRequest.NewPassword))
                throw new HttpResponseException(ErrorConstant.InvalidPassword, "Invalid Password", (int)HttpStatusCode.BadRequest);


            var userId = User.Identity.Name;


            if (!_userService.VerifyCurrentPassword(userId, changePasswordRequest.OldPassword))
                throw new HttpResponseException(ErrorConstant.OldPasswordDoesNotExist, "Old password is incorrect", (int)HttpStatusCode.NotFound);
           
            if (_userService.IsOldPassword(userId, changePasswordRequest.NewPassword))
                throw new HttpResponseException(ErrorConstant.OldPasswordNewPasswordSame, "Old password and new password is same", (int)HttpStatusCode.BadRequest);

            if (!_userService.UpdatePassword(userId, changePasswordRequest.NewPassword)) return BadRequest();


            return Ok("Password changed successfully.");

        }


        private bool ValidatePasswordPolicy(string password)
        {
            var passwordPolicy = APIConstant.PasswordPolicy;
            return Regex.IsMatch(password, passwordPolicy);
        }
    }
}