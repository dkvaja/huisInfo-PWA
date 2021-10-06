using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class NewsApiModel
    {
        public NewsApiModel() { }
        public NewsApiModel(Nieuws entity)
        {
            NewsId = entity.Guid;
            Description = entity.Omschrijving;
            HasImage = !string.IsNullOrEmpty(entity.Afbeelding);
            Date = entity.Datum;
            NewsItem = entity.Nieuwsbericht;
        }
        public string NewsId { get; set; }
        public string Description { get; set; }
        public bool HasImage { get; set; }
        public DateTime Date { get; set; }
        public string NewsItem { get; set; }
    }
}
