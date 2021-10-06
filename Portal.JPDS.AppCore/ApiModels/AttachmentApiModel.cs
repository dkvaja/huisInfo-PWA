using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class AttachmentApiModel
    {
        public AttachmentApiModel() { }
        public AttachmentApiModel(AttachmentWithHeaderApiModel attachment)
        {
            Id = attachment.Id;
            Description = attachment.Description;
            DateTime = attachment.DateTime;
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public DateTime? DateTime { get; set; }
    }

    public class AttachmentGroupByHeaderApiModel
    {
        public string HeaderId { get; set; }
        public string Header { get; set; }
        public IEnumerable<AttachmentApiModel> Attachments { get; set; }
    }

    public class AttachmentWithHeaderApiModel
    {
        public AttachmentWithHeaderApiModel() { }
        public AttachmentWithHeaderApiModel(Bijlage entity)
        {
            Id = entity.Guid;
            Description = entity.Omschrijving;
            DateTime = entity.Datum;
            HeaderId = entity.BijlageRubriekGuid;
            Header = entity.BijlageRubriekGu.Rubriek;
            ProjectId = entity.WerkGuid;
            BuildingId = entity.GebouwGuid;
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime? DateTime { get; set; }
        public string HeaderId { get; set; }
        public string Header { get; set; }
        public string ProjectId { get; set; }
        public string BuildingId { get; set; }
        public static IEnumerable<AttachmentGroupByHeaderApiModel> GroupByHeader(IEnumerable<AttachmentWithHeaderApiModel> attachments)
        {
            var resultList = new List<AttachmentGroupByHeaderApiModel>();
            foreach (var groupItem in attachments.GroupBy(x => x.HeaderId.ToUpperInvariant()))
            {
                var item = new AttachmentGroupByHeaderApiModel() { HeaderId = groupItem.Key, Header = groupItem.Select(x => x.Header).FirstOrDefault() };
                item.Attachments = groupItem.Select(x => new AttachmentApiModel(x));

                resultList.Add(item);
            }

            return resultList;
        }
    }
}
