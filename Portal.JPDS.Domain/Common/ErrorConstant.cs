using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    public class ErrorConstant

    {
        public const string PasswordConfirmPasswordMismatch = "PasswordConfirmPasswordMismatch-1001";
        public const string OldPasswordMissing = "OldPasswordMissing-1002";
        public const string NewPasswordMissing = "NewPasswordMissing-1003";
        public const string ConfirmPasswordMissing = "ConfirmPasswordMissing-1004";
        public const string InvalidPassword = "InvalidPassword-1005";
        public const string OldPasswordDoesNotExist = "OldPasswordDoesNotExist-1006";
        public const string OldPasswordNewPasswordSame = "OldPasswordNewPasswordSame-1007";
    }
}
