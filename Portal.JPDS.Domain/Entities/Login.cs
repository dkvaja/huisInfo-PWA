using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Login : BaseEntity
    {
        public Login()
        {
            BijlageDossiers = new HashSet<BijlageDossier>();
            Chat = new HashSet<Chat>();
            ChatDeelnemer = new HashSet<ChatDeelnemer>();
            Oplossers = new HashSet<Oplosser>();
            LoginRolWerks = new HashSet<LoginRolWerk>();
            DossierLoginLaatstGelezens = new HashSet<DossierLoginLaatstGelezen>();
            Dossiers = new HashSet<Dossier>();
            LoginBijlageDossiers = new HashSet<LoginBijlageDossier>();
            LoginDossierRechts = new HashSet<LoginDossierRecht>();
        }

        public byte? LoginRol { get; set; }
        public byte? LoginAccountVoor { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string KoperHuurderGuid { get; set; }
        public string MedewerkerGuid { get; set; }
        public string PersoonGuid { get; set; }
        public string Naam { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Wachtwoord { get; set; }
        public string Email { get; set; }
        public bool? OptIn { get; set; }
        public bool? Actief { get; set; }
        public DateTime? LaatsteLogin { get; set; }
        public string VorigWachtwoord { get; set; }
        public DateTime? VorigWachtwoordGeresetOp { get; set; }
        public DateTime? WijzigWachtwoordLinkAangemaakt { get; set; }
        public virtual KoperHuurder KoperHuurderGu { get; set; }
        public virtual Medewerker MedewerkerGu { get; set; }
        public virtual Persoon PersoonGu { get; set; }
        public virtual Relatie RelatieGu { get; set; }
        public virtual ICollection<Chat> Chat { get; set; }
        public virtual ICollection<ChatDeelnemer> ChatDeelnemer { get; set; }
        public bool? OfflineMode { get; set; }
        public int? OfflineInspectieOpslaanAantalDagen { get; set; }
        public int? OfflineOpleveringOpslaanAantalDagen { get; set; }
        public int? OfflineTweedeHandtekeningOpslaanAantalDagen { get; set; }
        public int? OfflineVoorschouwOpslaanAantalDagen { get; set; }
        public string CentralLoginGuid { get; set; }
        public bool Verwijderd { get; set; }
        public virtual ICollection<Oplosser> Oplossers { get; set; }
        public virtual ICollection<BijlageDossier> BijlageDossiers { get; set; }
        public virtual ICollection<DossierLoginLaatstGelezen> DossierLoginLaatstGelezens { get; set; }
        public virtual ICollection<Dossier> Dossiers { get; set; }
        public virtual ICollection<LoginBijlageDossier> LoginBijlageDossiers { get; set; }
        public virtual ICollection<LoginDossierRecht> LoginDossierRechts { get; set; }
        public virtual ICollection<LoginRolWerk> LoginRolWerks { get; set; }
    }
}
