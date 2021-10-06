using System;
using System.Collections.Generic;

namespace Portal.JPDS.Domain.Entities
{
    public class Melding : BaseEntity
    {
        public Melding()
        {
            Bijlage = new HashSet<Bijlage>();
            Communicaties = new HashSet<Communicatie>();
            MeldingKosten = new HashSet<MeldingKosten>();
            Oplosser = new HashSet<Oplosser>();
        }

        public string MeldingNummer { get; set; }
        public int? MeldingNummerVolgnummer { get; set; }
        public DateTime MeldingDatum { get; set; }
        public string MeldingSoortGuid { get; set; }
        public string MeldingStatusGuid { get; set; }
        public bool? Afgehandeld { get; set; }
        public DateTime? DatumAfhandeling { get; set; }
        public string Omschrijving { get; set; }
        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public string AdresGuid { get; set; }
        public byte Melder { get; set; }
        public string MelderKoperHuurderGuid { get; set; }
        public string MelderOrganisatieGuid { get; set; }
        public string MelderRelatieGuid { get; set; }
        public string MeldingTypeGuid { get; set; }
        public string MeldingAardGuid { get; set; }
        public string MeldingLocatieGuid { get; set; }
        public string MeldingLocatieSpecificatie { get; set; }
        public string MeldingOorzaakGuid { get; set; }
        public string MeldingVeroorzakerGuid { get; set; }
        public string ProductDienstGuid { get; set; }
        public string ProductDienstSub1Guid { get; set; }
        public string ProductDienstSub2Guid { get; set; }
        public string VeroorzakerOrganisatieGuid { get; set; }
        public string ReferentieOpdrachtgever { get; set; }
        public string OpnameGuid { get; set; }
        public string MelderMedewerkerGuid { get; set; }
        public string AangenomenDoorMedewerkerGuid { get; set; }
        public byte? MeldingOntvangenVia { get; set; }
        public byte? Prioriteit { get; set; }
        public string AfhandelingstermijnGuid { get; set; }
        public int? Inboektermijn { get; set; }
        public int? Doorlooptijd { get; set; }
        public DateTime? StreefdatumAfhandeling { get; set; }
        public DateTime? DatumOplevering { get; set; }
        public DateTime? DatumEindeOnderhoudstermijn { get; set; }
        public DateTime? DatumEindeGarantietermijn { get; set; }
        public int? DagenNaStreefdatumAfhandeling { get; set; }
        public string Melding1 { get; set; }
        public string TekstWerkbon { get; set; }
        public string Notities { get; set; }
        public string Oplossing { get; set; }
        public bool KoperHuurderIsAkkoord { get; set; }
        public string AfsprakenTweedeHandtekening { get; set; }
        public bool Herstelwerkzaamheden { get; set; }
        public string GebouwGebruikerKoperHuurderGuid { get; set; }
        public string GebouwGebruikerOrganisatieGuid { get; set; }
        public string GebouwGebruikerRelatieGuid { get; set; }
        public string GebouwGebruikerNaam { get; set; }
        public string OpdrachtgeverOrganisatieGuid { get; set; }
        public string OpdrachtgeverRelatieGuid { get; set; }
        public string VveOrganisatieGuid { get; set; }
        public string VveRelatieGuid { get; set; }
        public string VveBeheerderOrganisatieGuid { get; set; }
        public string VveBeheerderRelatieGuid { get; set; }
        public string VastgoedBeheerderOrganisatieGuid { get; set; }
        public string VastgoedBeheerderRelatieGuid { get; set; }
        public byte AanspreekpuntVoorOplosser { get; set; }
        public string RedenAfwijzing { get; set; }
        public string VoorkeurstijdstipAfspraak { get; set; }
        public virtual Medewerker AangenomenDoorMedewerkerGu { get; set; }
        public virtual Afhandelingstermijn AfhandelingstermijnGu { get; set; }
        public virtual KoperHuurder GebouwGebruikerKoperHuurderGu { get; set; }
        public virtual Organisatie GebouwGebruikerOrganisatieGu { get; set; }
        public virtual KoperHuurder MelderKoperHuurderGu { get; set; }
        public virtual Medewerker MelderMedewerkerGu { get; set; }
        public virtual Organisatie MelderOrganisatieGu { get; set; }
        public virtual MeldingAard MeldingAardGu { get; set; }
        public virtual MeldingLocatie MeldingLocatieGu { get; set; }
        public virtual MeldingOorzaak MeldingOorzaakGu { get; set; }
        public virtual MeldingSoort MeldingSoortGu { get; set; }
        public virtual MeldingStatus MeldingStatusGu { get; set; }
        public virtual MeldingType MeldingTypeGu { get; set; }
        public virtual MeldingVeroorzaker MeldingVeroorzakerGu { get; set; }
        public virtual Organisatie OpdrachtgeverOrganisatieGu { get; set; }
        public virtual Opname OpnameGu { get; set; }
        public virtual ProductDienst ProductDienstGu { get; set; }
        public virtual Organisatie VastgoedBeheerderOrganisatieGu { get; set; }
        public virtual Organisatie VeroorzakerOrganisatieGu { get; set; }
        public virtual Organisatie VveBeheerderOrganisatieGu { get; set; }
        public virtual Organisatie VveOrganisatieGu { get; set; }
        public virtual Werk WerkGu { get; set; }
        public virtual ICollection<Bijlage> Bijlage { get; set; }
        public virtual ICollection<Communicatie> Communicaties { get; set; }
        public virtual ICollection<MeldingKosten> MeldingKosten { get; set; }
        public virtual ICollection<Oplosser> Oplosser { get; set; }
    }
}
