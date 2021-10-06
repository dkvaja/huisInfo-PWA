using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BijlageDossierConfiguration : IEntityTypeConfiguration<BijlageDossier>
    {
        public void Configure(EntityTypeBuilder<BijlageDossier> builder)
        {
            builder.HasKey(e => e.Guid)
                  .HasName("bijlage_dossier_pk");

            builder.ToTable("bijlage_dossier");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.AangemaaktDoorLoginGuid)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("aangemaakt_door_login_guid");

            builder.Property(e => e.BijlageGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("bijlage_guid");

            builder.Property(e => e.DossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dossier_guid");

            builder.Property(e => e.Gearchiveerd).HasColumnName("gearchiveerd");

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

            builder.Property(e => e.Intern).HasColumnName("intern");

            builder.Property(e => e.Verwijderd).HasColumnName("verwijderd");

            builder.HasOne(d => d.AangemaaktDoorLoginGu)
                   .WithMany(p => p.BijlageDossiers)
                   .HasForeignKey(d => d.AangemaaktDoorLoginGuid)
                   .HasConstraintName("ref_login_bijlage_dossier");

            builder.HasOne(d => d.BijlageGu)
                .WithMany(p => p.BijlageDossiers)
                .HasForeignKey(d => d.BijlageGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_bijlage_bijlage_dossier");

            builder.HasOne(d => d.DossierGu)
                .WithMany(p => p.BijlageDossiers)
                .HasForeignKey(d => d.DossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_dossier_bijlage_dossier");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.BijlageDossiers)
                .HasForeignKey(d => d.GebouwGuid)
                .HasConstraintName("ref_gebouw_bijlage_dossier");
        }
    }
}
