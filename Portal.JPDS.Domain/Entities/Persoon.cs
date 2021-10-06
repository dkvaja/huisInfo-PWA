using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Persoon : BaseEntity
    {
        public Persoon()
        {
            Contacts = new HashSet<Contact>();
            //Factuur = new HashSet<Factuur>();
            //Groepslid = new HashSet<Groepslid>();
            InversePartnerPersoonGu = new HashSet<Persoon>();
            KoperHuurderPersoon1Gu = new HashSet<KoperHuurder>();
            KoperHuurderPersoon2Gu = new HashSet<KoperHuurder>();
            Login = new HashSet<Login>();
            Medewerker = new HashSet<Medewerker>();
            Relatie = new HashSet<Relatie>();
            //SocialMediaKoppeling = new HashSet<SocialMediaKoppeling>();
        }

        public byte? PersoonType { get; set; }
        public string Achternaam { get; set; }
        public string TussenvoegselGuid { get; set; }
        public string Voorletters { get; set; }
        public string Voornaam { get; set; }
        public string VolledigeNaam { get; set; }
        public byte Geslacht { get; set; }
        public string TitulatuurVoorGuid { get; set; }
        public string TitulatuurAchterGuid { get; set; }
        public string PriveAdresGuid { get; set; }
        public string Telefoon { get; set; }
        public string Mobiel { get; set; }
        public string EmailPrive { get; set; }
        public string PartnerPersoonGuid { get; set; }
        public DateTime? Geboortedatum { get; set; }
        public string Geboorteplaats { get; set; }
        public DateTime? Overlijdensdatum { get; set; }
        public string BankGuid { get; set; }
        public string Bic { get; set; }
        public string Iban { get; set; }
        public bool? Debiteur { get; set; }
        public string Debiteurnummer { get; set; }
        public byte? Betaalwijze { get; set; }
        public int? Betalingstermijn { get; set; }
        public bool? FactuurDigitaal { get; set; }
        public string EmailFactuur { get; set; }
        public string PersoonSoortGuid { get; set; }
        public string TaalGuid { get; set; }
        public string Foto { get; set; }
        public string Notitie { get; set; }
        public string Zoeknaam { get; set; }
        public string Naam { get; set; }
        public string Adresblok { get; set; }
        public DateTime? GecontroleerdOp { get; set; }
       

        //public virtual Bank BankGu { get; set; }
        public virtual Persoon PartnerPersoonGu { get; set; }
        //public virtual PersoonSoort PersoonSoortGu { get; set; }
        public virtual Adres PriveAdresGu { get; set; }
        //public virtual Taal TaalGu { get; set; }
        //public virtual TitulatuurAchter TitulatuurAchterGu { get; set; }
        //public virtual TitulatuurVoor TitulatuurVoorGu { get; set; }
        //public virtual Tussenvoegsel TussenvoegselGu { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Factuur> Factuur { get; set; }
        //public virtual ICollection<Groepslid> Groepslid { get; set; }
        public virtual ICollection<Persoon> InversePartnerPersoonGu { get; set; }
        public virtual ICollection<KoperHuurder> KoperHuurderPersoon1Gu { get; set; }
        public virtual ICollection<KoperHuurder> KoperHuurderPersoon2Gu { get; set; }
        public virtual ICollection<Login> Login { get; set; }
        public virtual ICollection<Medewerker> Medewerker { get; set; }
        public virtual ICollection<Relatie> Relatie { get; set; }
        //public virtual ICollection<SocialMediaKoppeling> SocialMediaKoppeling { get; set; }
    }
}
