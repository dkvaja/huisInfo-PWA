using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class NieuwsConfiguration : IEntityTypeConfiguration<Nieuws>
    {
        public void Configure(EntityTypeBuilder<Nieuws> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("nieuws_pk");

            builder.ToTable("nieuws");

            builder.HasIndex(e => new { e.WerkGuid, e.Datum, e.Omschrijving })
                .HasName("so_nieuws");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Afbeelding)
                .HasColumnName("afbeelding")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Datum)
                .HasColumnName("datum")
                .HasColumnType("date");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Nieuwsbericht)
                .IsRequired()
                .HasColumnName("nieuwsbericht");

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.Publiceren)
                .IsRequired()
                .HasColumnName("publiceren")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Nieuws)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_nieuws");
        }
    }
}
