using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Relatie
    {
        public Relatie()
        {
            Contacts = new HashSet<Contact>();
            //Factuur = new HashSet<Factuur>();
            //Groepslid = new HashSet<Groepslid>();
            KoperHuurder = new HashSet<KoperHuurder>();
            Login = new HashSet<Login>();
            Medewerker = new HashSet<Medewerker>();
            Oplossers = new HashSet<Oplosser>();
            //SoftwareVerbeterpuntMelder = new HashSet<SoftwareVerbeterpuntMelder>();
            WerkMeldingVerantwoordelijkeManagementRelatieGu = new HashSet<Werk>();
            WerkMeldingVerantwoordelijkeUitvoeringRelatieGu = new HashSet<Werk>();
        }

        public string Guid { get; set; }
        public string PersoonGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string AfdelingGuid { get; set; }
        public string FunctieGuid { get; set; }
        public bool? AfdelingInAdressering { get; set; }
        public bool? AfdelingVoorAfdelingsnaam { get; set; }
        public bool? GebruikAdresPersoon { get; set; }
        public bool? GebruikAdresOrganisatie { get; set; }
        public string AdresGuid { get; set; }
        public string Doorkiesnummer { get; set; }
        public string Mobiel { get; set; }
        public string EmailZakelijk { get; set; }
        public DateTime? Ingangsdatum { get; set; }
        public DateTime? Einddatum { get; set; }
        public bool? UitDienst { get; set; }
        public bool? AanwezigMaandagOchtend { get; set; }
        public bool? AanwezigMaandagMiddag { get; set; }
        public bool? AanwezigDinsdagOchtend { get; set; }
        public bool? AanwezigDinsdagMiddag { get; set; }
        public bool? AanwezigWoensdagOchtend { get; set; }
        public bool? AanwezigWoensdagMiddag { get; set; }
        public bool? AanwezigDonderdagOchtend { get; set; }
        public bool? AanwezigDonderdagMiddag { get; set; }
        public bool? AanwezigVrijdagOchtend { get; set; }
        public bool? AanwezigVrijdagMiddag { get; set; }
        public string Notities { get; set; }
        public string Adresblok { get; set; }
        public string Zoeknaam { get; set; }
        public DateTime? GecontroleerdOp { get; set; }
        public DateTime? IngevoerdOp { get; set; }
        public string IngevoerdDoor { get; set; }
        public DateTime? GewijzigdOp { get; set; }
        public string GewijzigdDoor { get; set; }

        public virtual Afdeling AfdelingGu { get; set; }
        public virtual Functie FunctieGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual Persoon PersoonGu { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Factuur> Factuur { get; set; }
        //public virtual ICollection<Groepslid> Groepslid { get; set; }
        public virtual ICollection<KoperHuurder> KoperHuurder { get; set; }
        public virtual ICollection<Login> Login { get; set; }
        public virtual ICollection<Medewerker> Medewerker { get; set; }
        public virtual ICollection<Oplosser> Oplossers { get; set; }
        //public virtual ICollection<SoftwareVerbeterpuntMelder> SoftwareVerbeterpuntMelder { get; set; }
        public virtual ICollection<Werk> WerkMeldingVerantwoordelijkeManagementRelatieGu { get; set; }
        public virtual ICollection<Werk> WerkMeldingVerantwoordelijkeUitvoeringRelatieGu { get; set; }
    }
}
