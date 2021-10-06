using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingConfiguration : IEntityTypeConfiguration<Melding>
    {
        public void Configure(EntityTypeBuilder<Melding> builder)
        {
            builder.HasKey(e => e.Guid)
                      .HasName("melding_pk");

            builder.ToTable("melding");

            builder.HasIndex(e => e.AangenomenDoorMedewerkerGuid)
                .HasName("fk_ref_medewerker_melding_aangenomen_door");

            builder.HasIndex(e => e.AfhandelingstermijnGuid)
                .HasName("fk_ref_afhandelingstermijn_melding");

            builder.HasIndex(e => e.GebouwGebruikerKoperHuurderGuid)
                .HasName("fk_ref_koper_huurder_melding_gebouw_gebruiker");

            builder.HasIndex(e => e.MelderKoperHuurderGuid)
                .HasName("fk_ref_koper_huurder_melding_melder");

            builder.HasIndex(e => e.MelderMedewerkerGuid)
                .HasName("fk_ref_medewerker_melding_melder");

            builder.HasIndex(e => e.MeldingAardGuid)
                .HasName("fk_ref_melding_aard_melding");

            builder.HasIndex(e => e.MeldingLocatieGuid)
                .HasName("fk_ref_melding_locatie_melding");

            builder.HasIndex(e => e.MeldingNummer)
                .HasName("so_melding");

            builder.HasIndex(e => e.MeldingOorzaakGuid)
                .HasName("fk_ref_melding_oorzaak_melding");

            builder.HasIndex(e => e.MeldingSoortGuid)
                .HasName("fk_ref_melding_soort_melding");

            builder.HasIndex(e => e.MeldingStatusGuid)
                .HasName("fk_ref_melding_status_melding");

            builder.HasIndex(e => e.MeldingTypeGuid)
                .HasName("fk_ref_melding_type_melding");

            builder.HasIndex(e => e.MeldingVeroorzakerGuid)
                .HasName("fk_ref_melding_veroorzaker_melding");

            builder.HasIndex(e => e.OpnameGuid)
                .HasName("fk_ref_opname_melding");

            builder.HasIndex(e => e.VeroorzakerOrganisatieGuid)
                .HasName("fk_ref_organisatie_melding_veroorzaker");

            builder.HasIndex(e => new { e.GebouwGebruikerOrganisatieGuid, e.GebouwGebruikerRelatieGuid })
                .HasName("fk_ref_relatie_melding_gebouwgebruiker");

            builder.HasIndex(e => new { e.MelderOrganisatieGuid, e.MelderRelatieGuid })
                .HasName("fk_ref_relatie_melding_melder");

            builder.HasIndex(e => new { e.OpdrachtgeverOrganisatieGuid, e.OpdrachtgeverRelatieGuid })
                .HasName("fk_ref_relatie_melding_opdrachtgever");

            builder.HasIndex(e => new { e.ProductDienstGuid, e.ProductDienstSub1Guid })
                .HasName("fk_ref_product_dienst_sub1_melding");

            builder.HasIndex(e => new { e.ProductDienstSub1Guid, e.ProductDienstSub2Guid })
                .HasName("fk_ref_product_dienst_sub2_melding");

            builder.HasIndex(e => new { e.VastgoedBeheerderOrganisatieGuid, e.VastgoedBeheerderRelatieGuid })
                .HasName("fk_ref_relatie_melding_vastgoed_beheerder");

            builder.HasIndex(e => new { e.VveBeheerderOrganisatieGuid, e.VveBeheerderRelatieGuid })
                .HasName("fk_ref_relatie_melding_vve_beheerder");

            builder.HasIndex(e => new { e.VveOrganisatieGuid, e.VveRelatieGuid })
                .HasName("fk_ref_relatie_melding_vve");

            builder.HasIndex(e => new { e.WerkGuid, e.GebouwGuid })
                .HasName("fk_ref_gebouw_melding");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AangenomenDoorMedewerkerGuid)
                .HasColumnName("aangenomen_door_medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AanspreekpuntVoorOplosser)
                .HasColumnName("aanspreekpunt_voor_oplosser")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AdresGuid)
                .HasColumnName("adres_guid")
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_adres_guid]([gebouw_guid]))");

            builder.Property(e => e.Afgehandeld)
                .HasColumnName("afgehandeld")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_afgehandeld]([melding_status_guid]))");

            builder.Property(e => e.AfhandelingstermijnGuid)
                .IsRequired()
                .HasColumnName("afhandelingstermijn_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DagenNaStreefdatumAfhandeling)
                .HasColumnName("dagen_na_streefdatum_afhandeling")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_dagen_na_streefdatum_afhandeling]([melding_datum],[afhandelingstermijn_guid]))");

            builder.Property(e => e.DatumAfhandeling)
                .HasColumnName("datum_afhandeling")
                .HasColumnType("date");

            builder.Property(e => e.DatumEindeGarantietermijn)
                .HasColumnName("datum_einde_garantietermijn")
                .HasColumnType("date")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_datum_einde_garantietermijn]([gebouw_guid],[product_dienst_guid],[product_dienst_sub1_guid],[product_dienst_sub2_guid]))");

            builder.Property(e => e.DatumEindeOnderhoudstermijn)
                .HasColumnName("datum_einde_onderhoudstermijn")
                .HasColumnType("date")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_datum_einde_onderhoudstermijn]([gebouw_guid]))");

            builder.Property(e => e.DatumOplevering)
                .HasColumnName("datum_oplevering")
                .HasColumnType("date")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_datum_oplevering]([gebouw_guid]))");

            builder.Property(e => e.Doorlooptijd)
                .HasColumnName("doorlooptijd")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_doorlooptijd]([melding_datum],[datum_afhandeling]))");

            builder.Property(e => e.GebouwGebruikerKoperHuurderGuid)
                .HasColumnName("gebouw_gebruiker_koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGebruikerNaam)
                .HasColumnName("gebouw_gebruiker_naam")
                .HasMaxLength(100)
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_gebouw_gebruiker_naam]([gebouw_guid]))");

            builder.Property(e => e.GebouwGebruikerOrganisatieGuid)
                .HasColumnName("gebouw_gebruiker_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGebruikerRelatieGuid)
                .HasColumnName("gebouw_gebruiker_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Inboektermijn)
                .HasColumnName("inboektermijn")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_inboektermijn]([melding_datum],[ingevoerd_op]))");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Melder).HasColumnName("melder");

            builder.Property(e => e.MelderKoperHuurderGuid)
                .HasColumnName("melder_koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MelderMedewerkerGuid)
                .HasColumnName("melder_medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MelderOrganisatieGuid)
                .HasColumnName("melder_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MelderRelatieGuid)
                .HasColumnName("melder_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Melding1).HasColumnName("melding");

            builder.Property(e => e.MeldingAardGuid)
                .HasColumnName("melding_aard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingDatum)
                .HasColumnName("melding_datum")
                .HasColumnType("date");

            builder.Property(e => e.MeldingLocatieGuid)
                .HasColumnName("melding_locatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingLocatieSpecificatie)
                .HasColumnName("melding_locatie_specificatie")
                .HasMaxLength(40);

            builder.Property(e => e.MeldingNummer)
                .IsRequired()
                .HasColumnName("melding_nummer")
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.MeldingNummerVolgnummer).HasColumnName("melding_nummer_volgnummer");

            builder.Property(e => e.MeldingOntvangenVia).HasColumnName("melding_ontvangen_via");

            builder.Property(e => e.MeldingOorzaakGuid)
                .HasColumnName("melding_oorzaak_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingSoortGuid)
                .HasColumnName("melding_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingStatusGuid)
                .HasColumnName("melding_status_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingTypeGuid)
                .HasColumnName("melding_type_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingVeroorzakerGuid)
                .HasColumnName("melding_veroorzaker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(60);

            builder.Property(e => e.OpdrachtgeverOrganisatieGuid)
                .HasColumnName("opdrachtgever_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OpdrachtgeverRelatieGuid)
                .HasColumnName("opdrachtgever_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Oplossing)
                .HasColumnName("oplossing")
                .HasMaxLength(4000);

            builder.Property(e => e.KoperHuurderIsAkkoord)
                .IsRequired()
                .HasColumnName("koper_huurder_is_akkoord")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AfsprakenTweedeHandtekening).HasColumnName("afspraken_tweede_handtekening");
            
            builder.Property(e => e.Herstelwerkzaamheden)
                .IsRequired()
                .HasColumnName("herstelwerkzaamheden")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OpnameGuid)
                .HasColumnName("opname_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Prioriteit)
                .HasColumnName("prioriteit")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.ProductDienstGuid)
                .HasColumnName("product_dienst_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ProductDienstSub1Guid)
                .HasColumnName("product_dienst_sub1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ProductDienstSub2Guid)
                .HasColumnName("product_dienst_sub2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RedenAfwijzing)
                    .HasMaxLength(4000)
                    .HasColumnName("reden_afwijzing");

            builder.Property(e => e.ReferentieOpdrachtgever)
                .HasColumnName("referentie_opdrachtgever")
                .HasMaxLength(40);

            builder.Property(e => e.StreefdatumAfhandeling)
                .HasColumnName("streefdatum_afhandeling")
                .HasColumnType("date")
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_streefdatum_afhandeling]([melding_datum],[afhandelingstermijn_guid]))");

            builder.Property(e => e.TekstWerkbon).HasColumnName("tekst_werkbon");

            builder.Property(e => e.VastgoedBeheerderOrganisatieGuid)
                .HasColumnName("vastgoed_beheerder_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VastgoedBeheerderRelatieGuid)
                .HasColumnName("vastgoed_beheerder_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VeroorzakerOrganisatieGuid)
                .HasColumnName("veroorzaker_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VoorkeurstijdstipAfspraak)
                    .HasMaxLength(100)
                    .HasColumnName("voorkeurstijdstip_afspraak");

            builder.Property(e => e.VveBeheerderOrganisatieGuid)
                .HasColumnName("vve_beheerder_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VveBeheerderRelatieGuid)
                .HasColumnName("vve_beheerder_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VveOrganisatieGuid)
                .HasColumnName("vve_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VveRelatieGuid)
                .HasColumnName("vve_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.AangenomenDoorMedewerkerGu)
                .WithMany(p => p.MeldingAangenomenDoorMedewerkerGu)
                .HasForeignKey(d => d.AangenomenDoorMedewerkerGuid)
                .HasConstraintName("ref_medewerker_melding_aangenomen_door");

            builder.HasOne(d => d.AfhandelingstermijnGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.AfhandelingstermijnGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_afhandelingstermijn_melding");

            builder.HasOne(d => d.GebouwGebruikerKoperHuurderGu)
                .WithMany(p => p.MeldingGebouwGebruikerKoperHuurderGu)
                .HasForeignKey(d => d.GebouwGebruikerKoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_melding_gebouw_gebruiker");

            builder.HasOne(d => d.GebouwGebruikerOrganisatieGu)
                .WithMany(p => p.MeldingGebouwGebruikerOrganisatieGu)
                .HasForeignKey(d => d.GebouwGebruikerOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_gebouwgebruiker");

            builder.HasOne(d => d.MelderKoperHuurderGu)
                .WithMany(p => p.MeldingMelderKoperHuurderGu)
                .HasForeignKey(d => d.MelderKoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_melding_melder");

            builder.HasOne(d => d.MelderMedewerkerGu)
                .WithMany(p => p.MeldingMelderMedewerkerGu)
                .HasForeignKey(d => d.MelderMedewerkerGuid)
                .HasConstraintName("ref_medewerker_melding_melder");

            builder.HasOne(d => d.MelderOrganisatieGu)
                .WithMany(p => p.MeldingMelderOrganisatieGu)
                .HasForeignKey(d => d.MelderOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_melder");

            builder.HasOne(d => d.MeldingAardGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingAardGuid)
                .HasConstraintName("ref_melding_aard_melding");

            builder.HasOne(d => d.MeldingLocatieGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingLocatieGuid)
                .HasConstraintName("ref_melding_locatie_melding");

            builder.HasOne(d => d.MeldingOorzaakGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingOorzaakGuid)
                .HasConstraintName("ref_melding_oorzaak_melding");

            builder.HasOne(d => d.MeldingSoortGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingSoortGuid)
                .HasConstraintName("ref_melding_soort_melding");

            builder.HasOne(d => d.MeldingStatusGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingStatusGuid)
                .HasConstraintName("ref_melding_status_melding");

            builder.HasOne(d => d.MeldingTypeGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingTypeGuid)
                .HasConstraintName("ref_melding_type_melding");

            builder.HasOne(d => d.MeldingVeroorzakerGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.MeldingVeroorzakerGuid)
                .HasConstraintName("ref_melding_veroorzaker_melding");

            builder.HasOne(d => d.OpdrachtgeverOrganisatieGu)
                .WithMany(p => p.MeldingOpdrachtgeverOrganisatieGu)
                .HasForeignKey(d => d.OpdrachtgeverOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_opdrachtgever");

            builder.HasOne(d => d.OpnameGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.OpnameGuid)
                .HasConstraintName("ref_opname_melding");

            builder.HasOne(d => d.ProductDienstGu)
                .WithMany(p => p.Meldings)
                .HasForeignKey(d => d.ProductDienstGuid)
                .HasConstraintName("ref_product_dienst_melding");

            builder.HasOne(d => d.VastgoedBeheerderOrganisatieGu)
                .WithMany(p => p.MeldingVastgoedBeheerderOrganisatieGu)
                .HasForeignKey(d => d.VastgoedBeheerderOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_vastgoed_beheerder");

            builder.HasOne(d => d.VeroorzakerOrganisatieGu)
                .WithMany(p => p.MeldingVeroorzakerOrganisatieGu)
                .HasForeignKey(d => d.VeroorzakerOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_veroorzaker");

            builder.HasOne(d => d.VveBeheerderOrganisatieGu)
                .WithMany(p => p.MeldingVveBeheerderOrganisatieGu)
                .HasForeignKey(d => d.VveBeheerderOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_vve_beheerder");

            builder.HasOne(d => d.VveOrganisatieGu)
                .WithMany(p => p.MeldingVveOrganisatieGu)
                .HasForeignKey(d => d.VveOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_vve");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Melding)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_melding");
        }
    }
}
