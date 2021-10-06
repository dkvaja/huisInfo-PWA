using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Communicatie : BaseEntity
    {
        public Communicatie()
        {
            Bijlages = new HashSet<Bijlage>();
            Contacts = new HashSet<Contact>();
        }

        public byte CommunicatieSoort { get; set; }
        public byte CommunicatieType { get; set; }
        public string EmailId { get; set; }
        public string WerkGuid { get; set; }
        public string ActieStandaard1Guid { get; set; }
        public string ActieStandaard2Guid { get; set; }
        public string FactuurGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string MeldingGuid { get; set; }
        public string SjabloonGuid { get; set; }
        public byte? SjabloonSoort { get; set; }
        public string Verslag { get; set; }
        public string OorspronkelijkVerslag { get; set; }
        public string Betreft { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan? TijdAanvang { get; set; }
        public TimeSpan? TijdEinde { get; set; }
        public string OnsKenmerk { get; set; }
        public string UwKenmerk { get; set; }
        public int? Groep { get; set; }
        public string MedewerkerGuid { get; set; }
        public string AfdelingGuid { get; set; }
        public string AfzenderEmailAccountGuid { get; set; }
        public int? CommunicatieKenmerkVolgnummer { get; set; }
        public byte? CommunicatieStatus { get; set; }
        public DateTime? HerinneringDatum { get; set; }
        public string HerinneringMedewerkerGuid { get; set; }
        public virtual Afdeling AfdelingGu { get; set; }
        //public virtual Factuur FactuurGu { get; set; }
        public virtual Medewerker HerinneringMedewerkerGu { get; set; }
        public virtual Medewerker MedewerkerGu { get; set; }
        public virtual Melding MeldingGu { get; set; }
        public virtual Sjabloon SjabloonGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Bijlage> Bijlages { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
