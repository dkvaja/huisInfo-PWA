using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    public class APIConstant

    {
        public const string PasswordPolicy = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
    }
}
