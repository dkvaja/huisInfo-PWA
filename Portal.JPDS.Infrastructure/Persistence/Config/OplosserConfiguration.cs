using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OplosserConfiguration : IEntityTypeConfiguration<Oplosser>
    {
        public void Configure(EntityTypeBuilder<Oplosser> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("oplosser_pk");

            builder.ToTable("oplosser");

            builder.HasIndex(e => e.RelatieGuid, "fk_ref_relatie_oplosser_integriteit");

            builder.HasIndex(e => new { e.OrganisatieGuid, e.RelatieGuid }, "fk_ref_relatie_oplosser_lookup");

            builder.HasIndex(e => e.MeldingGuid, "so_oplosser");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.AfhandelingstermijnGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("afhandelingstermijn_guid");

            builder.Property(e => e.DatumAfhandeling)
                .HasColumnType("date")
                .HasColumnName("datum_afhandeling");

            builder.Property(e => e.DatumAfmeldingsverzoek)
                .HasColumnType("date")
                .HasColumnName("datum_afmeldingsverzoek");

            builder.Property(e => e.DatumInAfwachtingTot)
                .HasColumnType("date")
                .HasColumnName("datum_in_afwachting_tot");

            builder.Property(e => e.DatumIngelicht)
                .HasColumnType("date")
                .HasColumnName("datum_ingelicht");

            builder.Property(e => e.GecontroleerdDoorLoginGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gecontroleerd_door_login_guid");

            builder.Property(e => e.GecontroleerdOp)
                .HasColumnType("datetime")
                .HasColumnName("gecontroleerd_op");

            builder.Property(e => e.GewijzigdDoor)
                .HasMaxLength(40)
                .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                .HasColumnType("datetime")
                .HasColumnName("gewijzigd_op");

            builder.Property(e => e.IngevoerdDoor)
                .HasMaxLength(40)
                .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                .HasColumnType("datetime")
                .HasColumnName("ingevoerd_op");

            builder.Property(e => e.LaatsteAfmeldingsverzoekGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("laatste_afmeldingsverzoek_guid");

            builder.Property(e => e.MeldingGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("melding_guid");

            builder.Property(e => e.MeldingSoortGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("melding_soort_guid");

            builder.Property(e => e.Omschrijving)
                .HasMaxLength(60)
                .HasColumnName("omschrijving");

            builder.Property(e => e.OplosserStatus)
                .HasColumnName("oplosser_status")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Oplossing)
                .HasMaxLength(4000)
                .HasColumnName("oplossing");

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("organisatie_guid");

            builder.Property(e => e.RedenAfwijzing)
                    .HasMaxLength(4000)
                    .HasColumnName("reden_afwijzing");

            builder.Property(e => e.RelatieGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("relatie_guid");

            builder.Property(e => e.StreefdatumAfhandeling)
                .HasColumnType("date")
                .HasColumnName("streefdatum_afhandeling");

            builder.Property(e => e.TekstWerkbon).HasColumnName("tekst_werkbon");

            builder.Property(e => e.Toelichting).HasColumnName("toelichting");

            builder.Property(e => e.VervolgVanWerkbonOplosserGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("vervolg_van_werkbon_oplosser_guid");

            builder.Property(e => e.VervolgWerkbon)
                .IsRequired()
                .HasColumnName("vervolg_werkbon")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Werkbonnummer)
                    .HasColumnName("werkbonnummer")
                    .HasComputedColumnSql("([dbo].[cff_proxy_oplosser_werkbonnummer]([melding_guid],[werkbonnummer_volgnummer]))");

            builder.Property(e => e.WerkbonnummerVolgnummer).HasColumnName("werkbonnummer_volgnummer");

            builder.HasOne(d => d.GecontroleerdDoorLoginGu)
                .WithMany(p => p.Oplossers)
                .HasForeignKey(d => d.GecontroleerdDoorLoginGuid)
                .HasConstraintName("ref_login_oplosser");

            builder.HasOne(d => d.MeldingGu)
                .WithMany(p => p.Oplosser)
                .HasForeignKey(d => d.MeldingGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_melding_oplosser");

            builder.HasOne(d => d.MeldingSoortGu)
                .WithMany(p => p.Oplossers)
                .HasForeignKey(d => d.MeldingSoortGuid)
                .HasConstraintName("ref_melding_soort_oplosser");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.Oplosser)
                .HasForeignKey(d => d.OrganisatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_organisatie_oplosser");

            builder.HasOne(d => d.RelatieGu)
                .WithMany(p => p.Oplossers)
                .HasForeignKey(d => d.RelatieGuid)
                .HasConstraintName("ref_relatie_oplosser_integriteit");

            builder.HasOne(d => d.VervolgVanWerkbonOplosserGu)
                    .WithMany(p => p.InverseVervolgVanWerkbonOplosserGu)
                    .HasForeignKey(d => d.VervolgVanWerkbonOplosserGuid)
                    .HasConstraintName("ref_oplosser_oplosser");
        }
    }
}
