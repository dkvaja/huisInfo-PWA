using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class EmailRecipientsModel
    {
        public EmailRecipientsModel()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }

        public bool IsModelValid()
        {
            var allEmails = (To ?? new List<string>())
                .Union(Cc ?? new List<string>())
                .Union(Bcc ?? new List<string>());

            if (!allEmails.Any() || allEmails.Any(x => !x.IsValidEmail()))
            {
                return false;
            }

            return true;
        }
    }
}
