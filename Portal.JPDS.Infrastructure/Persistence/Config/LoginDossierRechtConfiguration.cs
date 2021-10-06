using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class LoginDossierRechtConfiguration : IEntityTypeConfiguration<LoginDossierRecht>
    {
        public void Configure(EntityTypeBuilder<LoginDossierRecht> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("login_dossier_recht_pk");

            builder.ToTable("login_dossier_recht");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.BestandArchiveren).HasColumnName("bestand_archiveren");

            builder.Property(e => e.BestandDownload).HasColumnName("bestand_download");

            builder.Property(e => e.BestandUpload).HasColumnName("bestand_upload");

            builder.Property(e => e.DossierAfsluiten).HasColumnName("dossier_afsluiten");

            builder.Property(e => e.DossierArchiveren).HasColumnName("dossier_archiveren");

            builder.Property(e => e.DossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dossier_guid");

            builder.Property(e => e.DossierToevoegen).HasColumnName("dossier_toevoegen");

            builder.Property(e => e.DossierWijzigen).HasColumnName("dossier_wijzigen");

            builder.Property(e => e.GewijzigdDoor)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                .HasColumnType("datetime")
                .HasColumnName("gewijzigd_op");

            builder.Property(e => e.IngevoerdDoor)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                .HasColumnType("datetime")
                .HasColumnName("ingevoerd_op");

            builder.Property(e => e.ExternBestandWijzigen).HasColumnName("extern_bestand_wijzigen");

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("login_guid");

            builder.Property(e => e.ModuleGuid)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("module_guid");

            builder.Property(e => e.ExternRelatieZichtbaar).HasColumnName("extern_relatie_zichtbaar");

            builder.Property(e => e.RolGuid)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("rol_guid");

            builder.HasOne(d => d.DossierGu)
                .WithMany(p => p.LoginDossierRechts)
                .HasForeignKey(d => d.DossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_dossier_login_dossier_recht");

            builder.HasOne(d => d.LoginGu)
                .WithMany(p => p.LoginDossierRechts)
                .HasForeignKey(d => d.LoginGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_login_login_dossier_recht");

            builder.Property(e => e.Intern).HasColumnName("intern");

            builder.Property(e => e.InternBestandWijzigen).HasColumnName("intern_bestand_wijzigen");

            builder.Property(e => e.InternRelatieZichtbaar).HasColumnName("intern_relatie_zichtbaar");

            builder.Property(e => e.Extern).HasColumnName("extern");
        }
    }
}
