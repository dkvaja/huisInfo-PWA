using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class SettlementTermApiModel
    {
        public SettlementTermApiModel(Afhandelingstermijn x, bool isDefault)
        {
            TermId = x.Guid;
            Description = x.Omschrijving;
            NoOfDays = x.AantalDagen;
            IsDefault = isDefault;
            WorkingDays = x.Werkdagen;
        }

        public string TermId { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public bool IsDefault { get; set; }
        public bool? WorkingDays { get; set; }
    }
}