using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class Oplosser : BaseEntity
    {
        public Oplosser()
        {
            InverseVervolgVanWerkbonOplosserGu = new HashSet<Oplosser>();
        }

        public string MeldingGuid { get; set; }
        public string OrganisatieGuid { get; set; }
        public string RelatieGuid { get; set; }
        public string Omschrijving { get; set; }
        public byte? OplosserStatus { get; set; }
        public DateTime? DatumIngelicht { get; set; }
        public DateTime? DatumInAfwachtingTot { get; set; }
        public DateTime? DatumAfhandeling { get; set; }
        public string AfhandelingstermijnGuid { get; set; }
        public DateTime? DatumAfmeldingsverzoek { get; set; }
        public string LaatsteAfmeldingsverzoekGuid { get; set; }
        public string Toelichting { get; set; }
        public bool? VervolgWerkbon { get; set; }
        public string VervolgVanWerkbonOplosserGuid { get; set; }
        public string GecontroleerdDoorLoginGuid { get; set; }
        public DateTime? GecontroleerdOp { get; set; }
        public string TekstWerkbon { get; set; }
        public string Oplossing { get; set; }
        public string MeldingSoortGuid { get; set; }
        public string Werkbonnummer { get; set; }
        public int? WerkbonnummerVolgnummer { get; set; }
        public DateTime? StreefdatumAfhandeling { get; set; }
        public string RedenAfwijzing { get; set; }

        public virtual Login GecontroleerdDoorLoginGu { get; set; }
        public virtual Melding MeldingGu { get; set; }
        public virtual MeldingSoort MeldingSoortGu { get; set; }
        public virtual Organisatie OrganisatieGu { get; set; }
        public virtual Relatie RelatieGu { get; set; }
        public virtual Oplosser VervolgVanWerkbonOplosserGu { get; set; }
        public virtual ICollection<Oplosser> InverseVervolgVanWerkbonOplosserGu { get; set; }
    }
}
