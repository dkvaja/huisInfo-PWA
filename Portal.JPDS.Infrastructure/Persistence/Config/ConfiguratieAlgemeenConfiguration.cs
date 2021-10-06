using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ConfiguratieAlgemeenConfiguration : IEntityTypeConfiguration<ConfiguratieAlgemeen>
    {
        public void Configure(EntityTypeBuilder<ConfiguratieAlgemeen> builder)
        {
            builder.HasKey(e => e.Guid)
                      .HasName("configuratie_algemeen_pk");

            builder.ToTable("configuratie_algemeen");

            builder.HasIndex(e => e.HoofdvestigingEigenOrganisatieGuid)
                .HasName("fk_ref_organisatie_configuratie_algemeen");

            builder.HasIndex(e => e.StandaardCorrespondentieSjabloonGuid)
                .HasName("fk_ref_sjabloon_configuratie_algemeen_standaard_brief");

            builder.HasIndex(e => e.StandaardEmailSjabloonGuid)
                .HasName("fk_ref_sjabloon_configuratie_algemeen_standaard_email");

            builder.HasIndex(e => e.StandaardLandGuid)
                .HasName("fk_ref_land_configuratie_algemeen");

            builder.HasIndex(e => e.StandaardTaalGuid)
                .HasName("fk_ref_taal_configuratie_algemeen");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BijlageOpslagMethode)
                .HasColumnName("bijlage_opslag_methode")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.CommunicatieKenmerkAutomatisch)
                .IsRequired()
                .HasColumnName("communicatie_kenmerk_automatisch")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.CommunicatieKenmerkOpmaak)
                .IsRequired()
                .HasColumnName("communicatie_kenmerk_opmaak")
                .HasMaxLength(40)
                .HasDefaultValueSql("('[I]/[J2]-[N4]')");

            builder.Property(e => e.CommunicatieKenmerkWijzigbaar)
                .IsRequired()
                .HasColumnName("communicatie_kenmerk_wijzigbaar")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.DatabaseMailProfiel)
                .HasColumnName("database_mail_profiel")
                .HasMaxLength(100);

            builder.Property(e => e.Foutopsporing)
                .IsRequired()
                .HasColumnName("foutopsporing")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.HoofdvestigingEigenOrganisatieGuid)
                .HasColumnName("hoofdvestiging_eigen_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.MaatwerkPrefix)
                .HasColumnName("maatwerk_prefix")
                .HasMaxLength(20);

            builder.Property(e => e.MapBijlagen)
                .HasColumnName("map_bijlagen")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.MapSjablonen)
                .HasColumnName("map_sjablonen")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.StandaardCorrespondentieSjabloonGuid)
                .HasColumnName("standaard_correspondentie_sjabloon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardEmailSjabloonGuid)
                .HasColumnName("standaard_email_sjabloon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardLandGuid)
                .IsRequired()
                .HasColumnName("standaard_land_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardTaalGuid)
                .IsRequired()
                .HasColumnName("standaard_taal_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.HoofdvestigingEigenOrganisatieGu)
                .WithMany(p => p.ConfiguratieAlgemeen)
                .HasForeignKey(d => d.HoofdvestigingEigenOrganisatieGuid)
                .HasConstraintName("ref_organisatie_configuratie_algemeen");

            builder.HasOne(d => d.StandaardCorrespondentieSjabloonGu)
                .WithMany(p => p.ConfiguratieAlgemeenStandaardCorrespondentieSjabloonGu)
                .HasForeignKey(d => d.StandaardCorrespondentieSjabloonGuid)
                .HasConstraintName("ref_sjabloon_configuratie_algemeen_standaard_brief");

            builder.HasOne(d => d.StandaardEmailSjabloonGu)
                .WithMany(p => p.ConfiguratieAlgemeenStandaardEmailSjabloonGu)
                .HasForeignKey(d => d.StandaardEmailSjabloonGuid)
                .HasConstraintName("ref_sjabloon_configuratie_algemeen_standaard_email");

            //builder.HasOne(d => d.StandaardLandGu)
            //    .WithMany(p => p.ConfiguratieAlgemeen)
            //    .HasForeignKey(d => d.StandaardLandGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_land_configuratie_algemeen");

            //builder.HasOne(d => d.StandaardTaalGu)
            //    .WithMany(p => p.ConfiguratieAlgemeen)
            //    .HasForeignKey(d => d.StandaardTaalGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_taal_configuratie_algemeen");
        }
    }
}
