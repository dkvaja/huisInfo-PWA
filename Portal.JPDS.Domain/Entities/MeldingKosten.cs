using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class MeldingKosten : BaseEntity
    {
        public MeldingKosten()
        {
            //Factuurregel = new HashSet<Factuurregel>();
        }

        public string MeldingGuid { get; set; }
        public string Omschrijving { get; set; }
        public string CrediteurOrganisatieGuid { get; set; }
        public string InkoopFactuurnummer { get; set; }
        public decimal? InkoopAantal { get; set; }
        public string InkoopEenheidGuid { get; set; }
        public decimal? InkoopPrijsPerEenheidExclBtw { get; set; }
        public decimal? InkoopBedragExclBtw { get; set; }
        public byte? InkoopBtwTarief { get; set; }
        public bool? Doorberekenen { get; set; }
        public byte? DoorberekenenAan { get; set; }
        public string DebiteurKoperHuurderGuid { get; set; }
        public string DebiteurOrganisatieGuid { get; set; }
        public bool? Gefactureerd { get; set; }
        public string FactuurGuid { get; set; }
        public string VerkoopFactuurnummer { get; set; }
        public decimal? VerkoopAantal { get; set; }
        public string VerkoopEenheidGuid { get; set; }
        public decimal? VerkoopPrijsPerEenheidExclBtw { get; set; }
        public decimal? VerkoopBedragExclBtw { get; set; }
        public byte? VerkoopBtwTarief { get; set; }
        public string NaamDebiteur { get; set; }

        public virtual Organisatie CrediteurOrganisatieGu { get; set; }
        public virtual KoperHuurder DebiteurKoperHuurderGu { get; set; }
        public virtual Organisatie DebiteurOrganisatieGu { get; set; }
        //public virtual Factuur FactuurGu { get; set; }
        public virtual Eenheid InkoopEenheidGu { get; set; }
        public virtual Melding MeldingGu { get; set; }
        public virtual Eenheid VerkoopEenheidGu { get; set; }
        //public virtual ICollection<Factuurregel> Factuurregel { get; set; }
    }
}
