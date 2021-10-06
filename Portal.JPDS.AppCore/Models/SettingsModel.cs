using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class SettingsModel
    {
        public string CompanyName { get; set; }
        public string BusinessUnit { get; set; }
        public AddressModel VisitingAddress { get; set; }
        public string Telephone { get; set; }
        public string EmailGeneral { get; set; }
        public string EmailRepairRequest { get; set; }
        public string NoteSendOnlineRepairRequest { get; set; }
    }
}
