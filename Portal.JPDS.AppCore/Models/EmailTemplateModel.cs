using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class EmailTemplateModel
    {
        public EmailTemplateModel() { }
        public EmailTemplateModel(Sjabloon entity)
        {
            Subject = entity.Betreft;
            Template = entity.Sjabloon1;
            TemplateHtml = entity.SjabloonHtml;
        }

        public string Subject { get; set; }
        public string Template { get; set; }
        public string TemplateHtml { get; set; }

        public void UpdateTokenValues(Dictionary<string, string> tokens)
        {
            foreach (var token in tokens)
            {
                Subject = Subject.Replace(token.Key, token.Value, StringComparison.InvariantCulture);
                TemplateHtml = TemplateHtml.Replace(token.Key, token.Value, StringComparison.InvariantCulture);
            }
        }
    }
}
