using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Medewerker:BaseEntity
    {
        public Medewerker()
        {
            ActieActieMedewerkerGu = new HashSet<Actie>();
            ActieAfgehandeldMedewerkerGu = new HashSet<Actie>();
            CommunicatieHerinneringMedewerkerGus = new HashSet<Communicatie>();
            CommunicatieMedewerkerGus = new HashSet<Communicatie>();
            EmailAccounts = new HashSet<EmailAccount>();
            Login = new HashSet<Login>();
            MeldingAangenomenDoorMedewerkerGu = new HashSet<Melding>();
            MeldingMelderMedewerkerGu = new HashSet<Melding>();
            //VolgjewoningKoppeling = new HashSet<VolgjewoningKoppeling>();
        }

        public string EigenOrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string AfdelingGuid { get; set; }
        public string NetwerkDomein { get; set; }
        public string Inlognaam { get; set; }
        public string Initialen { get; set; }
        public bool? UitDienst { get; set; }
        public string Slotgroet { get; set; }
        public string Handtekening { get; set; }
        public string Ondertekenaar { get; set; }
        public bool? HerinneringGeboortedatum { get; set; }
        public int? HerinneringGeboortedatumAantalDagen { get; set; }
        public bool? HerinneringActieDatum { get; set; }
        public int? HerinneringActieDatumAantalDagen { get; set; }
        public bool? HerinneringSluitingsdatumWerk { get; set; }
        public int? HerinneringSluitingsdatumWerkAantalDagen { get; set; }
        public bool? HerinneringSluitingsdatumOfferte { get; set; }
        public int? HerinneringSluitingsdatumOfferteAantalDagen { get; set; }
        public bool? HerinneringVervaldatumFactuur { get; set; }
        public int? HerinneringVervaldatumFactuurAantalDagen { get; set; }
        public bool? HerinneringVoorschouw { get; set; }
        public int? HerinneringVoorschouwAantalDagen { get; set; }
        public bool? HerinneringOplevering { get; set; }
        public int? HerinneringOpleveringAantalDagen { get; set; }
        public bool? HerinneringAfhandelingMelding { get; set; }
        public int? HerinneringAfhandelingMeldingAantalDagen { get; set; }
        public bool? HerinneringCommunicatie { get; set; }
        public int? HerinneringCommunicatieAantalDagen { get; set; }

        public virtual Afdeling AfdelingGu { get; set; }
        public virtual Organisatie EigenOrganisatieGu { get; set; }
        public virtual Persoon PersoonGu { get; set; }
        public virtual Relatie RelatieGu { get; set; }
        public virtual ICollection<Actie> ActieActieMedewerkerGu { get; set; }
        public virtual ICollection<Actie> ActieAfgehandeldMedewerkerGu { get; set; }
        public virtual ICollection<Communicatie> CommunicatieHerinneringMedewerkerGus { get; set; }
        public virtual ICollection<Communicatie> CommunicatieMedewerkerGus { get; set; }
        public virtual ICollection<EmailAccount> EmailAccounts { get; set; }
        public virtual ICollection<Login> Login { get; set; }
        public virtual ICollection<Melding> MeldingAangenomenDoorMedewerkerGu { get; set; }
        public virtual ICollection<Melding> MeldingMelderMedewerkerGu { get; set; }
        //public virtual ICollection<VolgjewoningKoppeling> VolgjewoningKoppeling { get; set; }
    }
}
