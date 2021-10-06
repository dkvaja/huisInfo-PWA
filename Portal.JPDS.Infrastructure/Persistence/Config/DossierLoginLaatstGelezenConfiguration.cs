using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class DossierLoginLaatstGelezenConfiguration : IEntityTypeConfiguration<DossierLoginLaatstGelezen>
    {
        public void Configure(EntityTypeBuilder<DossierLoginLaatstGelezen> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("dossier_login_laatst_gelezen_pk");

            builder.ToTable("dossier_login_laatst_gelezen");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.BijlageDossierGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("bijlage_dossier_guid");

            builder.Property(e => e.DossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dossier_guid");

            builder.Property(e => e.GebouwGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gebouw_guid");

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

            builder.Property(e => e.LaatstGelezen)
                .HasColumnType("datetime")
                .HasColumnName("laatst_gelezen");

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("login_guid");

            builder.HasOne(d => d.BijlageDossierGu)
                .WithMany(p => p.DossierLoginLaatstGelezens)
                .HasForeignKey(d => d.BijlageDossierGuid)
                .HasConstraintName("ref_bijlage_dossier_dossier_login_laatst_gelezen");

            builder.HasOne(d => d.DossierGu)
                .WithMany(p => p.DossierLoginLaatstGelezens)
                .HasForeignKey(d => d.DossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_dossier_dossier_login_laatst_gelezen");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.DossierLoginLaatstGelezens)
                .HasForeignKey(d => d.GebouwGuid)
                .HasConstraintName("ref_gebouw_dossier_login_laatst_gelezen");

            builder.HasOne(d => d.LoginGu)
                .WithMany(p => p.DossierLoginLaatstGelezens)
                .HasForeignKey(d => d.LoginGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_login_dossier_login_laatst_gelezen");
        }
    }
}
