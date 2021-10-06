using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class OfflineConfigApiModel
    {
        public bool Mode { get; set; }
        public int? DaysForDelivery { get; set; }
        public int? DaysForPreDelivery { get; set; }
        public int? DaysForInspection { get; set; }
        public int? DaysForSecondSignature { get; set; }
    }
}
