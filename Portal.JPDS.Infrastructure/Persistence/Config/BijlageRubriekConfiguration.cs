using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BijlageRubriekConfiguration : IEntityTypeConfiguration<BijlageRubriek>
    {
        public void Configure(EntityTypeBuilder<BijlageRubriek> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("bijlage_rubriek_pk");

            builder.ToTable("bijlage_rubriek");

            builder.HasIndex(e => new { e.Volgorde, e.Rubriek }, "so_bijlage_rubriek");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

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

            builder.Property(e => e.Rubriek)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("rubriek");

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");
        }
    }
}
