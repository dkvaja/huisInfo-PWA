using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MedewerkerConfiguration : IEntityTypeConfiguration<Medewerker>
    {
        public void Configure(EntityTypeBuilder<Medewerker> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("medewerker_pk");

            builder.ToTable("medewerker");

            builder.HasIndex(e => e.AfdelingGuid)
                .HasName("fk_ref_afdeling_medewerker");

            builder.HasIndex(e => e.EigenOrganisatieGuid)
                .HasName("fk_ref_organisatie_medewerker");

            builder.HasIndex(e => e.Inlognaam)
                .HasName("index_inlognaam")
                .IsUnique();

            builder.HasIndex(e => e.PersoonGuid)
                .HasName("fk_ref_persoon_medewerker");

            builder.HasIndex(e => e.RelatieGuid)
                .HasName("fk_ref_relatie_medewerker");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AfdelingGuid)
                .IsRequired()
                .HasColumnName("afdeling_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.EigenOrganisatieGuid)
                .HasColumnName("eigen_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Handtekening)
                .HasColumnName("handtekening")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.HerinneringActieDatum)
                .IsRequired()
                .HasColumnName("herinnering_actie_datum")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringActieDatumAantalDagen)
                .HasColumnName("herinnering_actie_datum_aantal_dagen")
                .HasDefaultValueSql("('7')");

            builder.Property(e => e.HerinneringAfhandelingMelding)
                .IsRequired()
                .HasColumnName("herinnering_afhandeling_melding")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringAfhandelingMeldingAantalDagen)
                .HasColumnName("herinnering_afhandeling_melding_aantal_dagen")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.HerinneringCommunicatie)
                .IsRequired()
                .HasColumnName("herinnering_communicatie")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringCommunicatieAantalDagen)
                .HasColumnName("herinnering_communicatie_aantal_dagen")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.HerinneringGeboortedatum)
                .IsRequired()
                .HasColumnName("herinnering_geboortedatum")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringGeboortedatumAantalDagen)
                .HasColumnName("herinnering_geboortedatum_aantal_dagen")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringOplevering)
                .IsRequired()
                .HasColumnName("herinnering_oplevering")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringOpleveringAantalDagen)
                .HasColumnName("herinnering_oplevering_aantal_dagen")
                .HasDefaultValueSql("('7')");

            builder.Property(e => e.HerinneringSluitingsdatumOfferte)
                .IsRequired()
                .HasColumnName("herinnering_sluitingsdatum_offerte")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringSluitingsdatumOfferteAantalDagen)
                .HasColumnName("herinnering_sluitingsdatum_offerte_aantal_dagen")
                .HasDefaultValueSql("('7')");

            builder.Property(e => e.HerinneringSluitingsdatumWerk)
                .IsRequired()
                .HasColumnName("herinnering_sluitingsdatum_werk")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringSluitingsdatumWerkAantalDagen)
                .HasColumnName("herinnering_sluitingsdatum_werk_aantal_dagen")
                .HasDefaultValueSql("('7')");

            builder.Property(e => e.HerinneringVervaldatumFactuur)
                .IsRequired()
                .HasColumnName("herinnering_vervaldatum_factuur")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringVervaldatumFactuurAantalDagen)
                .HasColumnName("herinnering_vervaldatum_factuur_aantal_dagen")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.HerinneringVoorschouw)
                .IsRequired()
                .HasColumnName("herinnering_voorschouw")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HerinneringVoorschouwAantalDagen)
                .HasColumnName("herinnering_voorschouw_aantal_dagen")
                .HasDefaultValueSql("('7')");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Initialen)
                .HasColumnName("initialen")
                .HasMaxLength(20);

            builder.Property(e => e.Inlognaam)
                .IsRequired()
                .HasColumnName("inlognaam")
                .HasMaxLength(40);

            builder.Property(e => e.NetwerkDomein)
                .HasColumnName("netwerk_domein")
                .HasMaxLength(40);

            builder.Property(e => e.Ondertekenaar)
                .HasColumnName("ondertekenaar")
                .HasMaxLength(100);

            builder.Property(e => e.PersoonGuid)
                .HasColumnName("persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .IsRequired()
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Slotgroet)
                .HasColumnName("slotgroet")
                .HasMaxLength(40);

            builder.Property(e => e.UitDienst)
                .HasColumnName("uit_dienst")
                .HasComputedColumnSql("([dbo].[cff_proxy_medewerker_uit_dienst]([relatie_guid]))");

            builder.HasOne(d => d.AfdelingGu)
                .WithMany(p => p.Medewerkers)
                .HasForeignKey(d => d.AfdelingGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_afdeling_medewerker");

            builder.HasOne(d => d.EigenOrganisatieGu)
                .WithMany(p => p.Medewerker)
                .HasForeignKey(d => d.EigenOrganisatieGuid)
                .HasConstraintName("ref_organisatie_medewerker");

            builder.HasOne(d => d.PersoonGu)
                .WithMany(p => p.Medewerker)
                .HasForeignKey(d => d.PersoonGuid)
                .HasConstraintName("ref_persoon_medewerker");

            builder.HasOne(d => d.RelatieGu)
                .WithMany(p => p.Medewerker)
                .HasForeignKey(d => d.RelatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_relatie_medewerker");
        }
    }
}
