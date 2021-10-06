using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class EmailNotificationModel
    {
        public string ProjectNoAndName { get; set; }
        public string BuildingNoExtern { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LetterSalutationFormal { get; set; }
        public string LetterSalutationInformal { get; set; }
        public string MainContractorName { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<string> ResolverIds { get; set; }
    }
}
