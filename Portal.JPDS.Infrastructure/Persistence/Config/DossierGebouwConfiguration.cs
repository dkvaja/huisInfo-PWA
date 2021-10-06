using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class DossierGebouwConfiguration : IEntityTypeConfiguration<DossierGebouw>
    {
        public void Configure(EntityTypeBuilder<DossierGebouw> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("dossier_gebouw_pk");

            builder.ToTable("dossier_gebouw");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Actief).HasColumnName("actief");

            builder.Property(e => e.AfgeslotenOp)
                    .HasColumnType("datetime")
                    .HasColumnName("afgesloten_op");

            builder.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");

            builder.Property(e => e.DossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dossier_guid");

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
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

            builder.Property(e => e.Status).HasColumnName("status");

            builder.Property(e => e.Verwijderd).HasColumnName("verwijderd");

            builder.HasOne(d => d.DossierGu)
                .WithMany(p => p.DossierGebouws)
                .HasForeignKey(d => d.DossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_dossier_dossier_gebouw");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.DossierGebouws)
                .HasForeignKey(d => d.GebouwGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_gebouw_dossier_gebouw");
        }
    }
}
