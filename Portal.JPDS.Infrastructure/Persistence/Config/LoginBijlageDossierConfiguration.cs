using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class LoginBijlageDossierConfiguration : IEntityTypeConfiguration<LoginBijlageDossier>
    {
        public void Configure(EntityTypeBuilder<LoginBijlageDossier> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("login_bijlage_dossier_pk");

            builder.ToTable("login_bijlage_dossier");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.BijlageDossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("bijlage_dossier_guid");

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

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("login_guid");

            builder.HasOne(d => d.BijlageDossierGu)
                .WithMany(p => p.LoginBijlageDossiers)
                .HasForeignKey(d => d.BijlageDossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_bijlage_dossier_login_bijlage_dossier");

            //builder.HasOne(d => d.LoginGu)
            //    .WithMany(p => p.LoginBijlageDossiers)
            //    .HasForeignKey(d => d.LoginGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_login_login_bijlage_dossier_");
        }
    }
}
