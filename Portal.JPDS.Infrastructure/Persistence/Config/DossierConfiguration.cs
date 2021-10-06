using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class DossierConfiguration : IEntityTypeConfiguration<Dossier>
    {
        public void Configure(EntityTypeBuilder<Dossier> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("dossier_pk");

            builder.ToTable("dossier");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.AangemaaktDoorLoginGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("aangemaakt_door_login_guid");

            builder.Property(e => e.Afbeelding)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("afbeelding");

            builder.Property(e => e.AfgeslotenOp)
                .HasColumnType("datetime")
                .HasColumnName("afgesloten_op");

            builder.Property(e => e.AlgemeneInformatie)
                    .HasMaxLength(4000)
                    .HasColumnName("algemene_informatie");

            builder.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");

            builder.Property(e => e.Gearchiveerd).HasColumnName("gearchiveerd");

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

            builder.Property(e => e.Naam)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("naam");

            builder.Property(e => e.Status).HasColumnName("status");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.Property(e => e.AlgemeneBestanden).HasColumnName("algemene_bestanden");

            builder.Property(e => e.ObjectgebondenBestanden).HasColumnName("objectgebonden_bestanden");

            builder.Property(e => e.Extern).HasColumnName("extern");

            //builder.HasOne(d => d.AangemaaktDoorLoginGu)
            //    .WithMany(p => p.Dossiers)
            //    .HasForeignKey(d => d.AangemaaktDoorLoginGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_login_dossier");
        }
    }
}
