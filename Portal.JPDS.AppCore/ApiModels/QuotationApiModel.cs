using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class QuotationApiModel
    {
        public string QuoteId { get; set; }
        public int QuoteNo { get; set; }
        public string BuildingId { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool IsDigitalSigning { get; set; }
        public long? DigitalDocumentId { get; set; }
        public string DigitalDocumentStatus { get; set; }
        public IEnumerable<SelectedOptionApiModel> Options { get; set; }
    }
}
