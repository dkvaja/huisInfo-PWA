using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Sjabloon : BaseEntity
    {
        public Sjabloon()
        {
            Communicaties = new HashSet<Communicatie>();
            ConfiguratieAlgemeenStandaardCorrespondentieSjabloonGu = new HashSet<ConfiguratieAlgemeen>();
            ConfiguratieAlgemeenStandaardEmailSjabloonGu = new HashSet<ConfiguratieAlgemeen>();
            //ConfiguratieFactureringStandaardFactuurAlgemeenBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurAlgemeenEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKlachtenbeheerMeldingskostenBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKlachtenbeheerMeldingskostenEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingBouwtermijnBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingBouwtermijnEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingCreditIvmCascoBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingCreditIvmCascoEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwCorrectieBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwCorrectieEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpdrachtBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpdrachtEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpleveringBriefSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpleveringEmailSjabloonGu = new HashSet<ConfiguratieFacturering>();
            //ConfiguratieKopersbegeleidingStandaardOfferteBriefSjabloonGu = new HashSet<ConfiguratieKopersbegeleiding>();
            //ConfiguratieKopersbegeleidingStandaardOfferteEmailSjabloonGu = new HashSet<ConfiguratieKopersbegeleiding>();
            //ConfiguratieKopersbegeleidingStandaardOpdrachtbevestigingBriefSjabloonGu = new HashSet<ConfiguratieKopersbegeleiding>();
            //ConfiguratieKopersbegeleidingStandaardOpdrachtbevestigingEmailSjabloonGu = new HashSet<ConfiguratieKopersbegeleiding>();
            ConfiguratieWebPortal = new HashSet<ConfiguratieWebPortal>();
            //SjabloonKopersbegeleiding = new HashSet<SjabloonKopersbegeleiding>();
            //SjabloonMelding = new HashSet<SjabloonMelding>();
        }

        public string Omschrijving { get; set; }
        public string Betreft { get; set; }
        public byte SjabloonSoort { get; set; }
        public string Documentcode { get; set; }
        public string Submap { get; set; }
        public bool? Standaard { get; set; }
        public bool? Actief { get; set; }
        public string SjabloonBestand { get; set; }
        public string Sjabloon1 { get; set; }
        public string SjabloonHtml { get; set; }
        public string WerkGuid { get; set; }
        public string ActieStandaard1Guid { get; set; }
        public string ActieStandaard2Guid { get; set; }
      

        public virtual Werk WerkGu { get; set; }
        //public virtual SjabloonFacturering SjabloonFacturering { get; set; }
        public virtual ICollection<Communicatie> Communicaties { get; set; }
        public virtual ICollection<ConfiguratieAlgemeen> ConfiguratieAlgemeenStandaardCorrespondentieSjabloonGu { get; set; }
        public virtual ICollection<ConfiguratieAlgemeen> ConfiguratieAlgemeenStandaardEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurAlgemeenBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurAlgemeenEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKlachtenbeheerMeldingskostenBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKlachtenbeheerMeldingskostenEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingBouwtermijnBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingBouwtermijnEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingCreditIvmCascoBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingCreditIvmCascoEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwCorrectieBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwCorrectieEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpdrachtBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpdrachtEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpleveringBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieFacturering> ConfiguratieFactureringStandaardFactuurKopersbegeleidingMmwOpleveringEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieKopersbegeleiding> ConfiguratieKopersbegeleidingStandaardOfferteBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieKopersbegeleiding> ConfiguratieKopersbegeleidingStandaardOfferteEmailSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieKopersbegeleiding> ConfiguratieKopersbegeleidingStandaardOpdrachtbevestigingBriefSjabloonGu { get; set; }
        //public virtual ICollection<ConfiguratieKopersbegeleiding> ConfiguratieKopersbegeleidingStandaardOpdrachtbevestigingEmailSjabloonGu { get; set; }
        public virtual ICollection<ConfiguratieWebPortal> ConfiguratieWebPortal { get; set; }
        //public virtual ICollection<SjabloonKopersbegeleiding> SjabloonKopersbegeleiding { get; set; }
        //public virtual ICollection<SjabloonMelding> SjabloonMelding { get; set; }
    }
}
