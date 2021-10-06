using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Eenheid : BaseEntity
    {
        public Eenheid()
        {
            //Artikel = new HashSet<Artikel>();
            //Calculatieregel = new HashSet<Calculatieregel>();
            //Factuurregel = new HashSet<Factuurregel>();
            //Garantietermijn = new HashSet<Garantietermijn>();
            //Inkoop = new HashSet<Inkoop>();
            //Inkoopprijs = new HashSet<Inkoopprijs>();
            //Inkoopprijslijstregel = new HashSet<Inkoopprijslijstregel>();
            MeldingKostenInkoopEenheidGu = new HashSet<MeldingKosten>();
            MeldingKostenVerkoopEenheidGu = new HashSet<MeldingKosten>();
            //Onderhoudstermijn = new HashSet<Onderhoudstermijn>();
            OptieGekozen = new HashSet<OptieGekozen>();
            OptieStandaards = new HashSet<OptieStandaard>();
        }

        public string Omschrijving { get; set; }
        public string Eenheid1 { get; set; }
        public string EenheidMeervoud { get; set; }
        public string EenheidCalculatieprogramma { get; set; }
        public int AantalDecimalen { get; set; }
        public bool? Tijd { get; set; }
        public bool? Maatvoering { get; set; }
        public bool? Overige { get; set; }
        public bool? Systeemwaarde { get; set; }


        //public virtual ICollection<Artikel> Artikel { get; set; }
        //public virtual ICollection<Calculatieregel> Calculatieregel { get; set; }
        //public virtual ICollection<Factuurregel> Factuurregel { get; set; }
        //public virtual ICollection<Garantietermijn> Garantietermijn { get; set; }
        //public virtual ICollection<Inkoop> Inkoop { get; set; }
        //public virtual ICollection<Inkoopprijs> Inkoopprijs { get; set; }
        //public virtual ICollection<Inkoopprijslijstregel> Inkoopprijslijstregel { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKostenInkoopEenheidGu { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKostenVerkoopEenheidGu { get; set; }
        //public virtual ICollection<Onderhoudstermijn> Onderhoudstermijn { get; set; }
        public virtual ICollection<OptieGekozen> OptieGekozen { get; set; }
        public virtual ICollection<OptieStandaard> OptieStandaards { get; set; }
    }
}
