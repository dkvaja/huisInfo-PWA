using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class OptieGekozenOfferte : BaseEntity
    {
        public OptieGekozenOfferte()
        {
            Bijlage = new HashSet<Bijlage>();
            OptieGekozen = new HashSet<OptieGekozen>();
        }

        public string GebouwGuid { get; set; }
        public int Offertenummer { get; set; }
        public DateTime Sluitingsdatum { get; set; }
        public byte? Ondertekening { get; set; }
        public int? OndertekeningAantalPersonen { get; set; }
        public long? ScriveDocumentId { get; set; }
        public string ScriveDocumentStatus { get; set; }
        

        public virtual Gebouw GebouwGu { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozen { get; set; }
    }
}
