using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class FaqApiModel
    {
        public string Ques { get; set; }
        public string Ans { get; set; }
    }

    public class FaqGroupedByHeaderApiModel
    {
        public string Header { get; set; }
        public IEnumerable<FaqApiModel> Faqs { get; set; }
    }
}
