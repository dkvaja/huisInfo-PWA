using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Nieuws : BaseEntity
    {
        public string WerkGuid { get; set; }
        public string Omschrijving { get; set; }
        public string Afbeelding { get; set; }
        public DateTime Datum { get; set; }
        public string Nieuwsbericht { get; set; }
        public bool? Publiceren { get; set; }
        

        public virtual Werk WerkGu { get; set; }
    }
}
