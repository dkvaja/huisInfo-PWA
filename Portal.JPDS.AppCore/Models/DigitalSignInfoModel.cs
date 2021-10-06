using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class DigitalSignInfoModel
    {
        public string QuoteId { get; set; }
        public int QuoteNo { get; set; }
        public DateTime ClosingDate { get; set; }
        public int? SignatureType { get; set; }
        public SigningParty SigningParty1 { get; set; }
        public SigningParty SigningParty2 { get; set; }
    }

    public class SigningParty
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
