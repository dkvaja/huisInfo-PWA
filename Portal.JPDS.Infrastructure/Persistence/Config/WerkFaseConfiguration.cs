using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class WerkFaseConfiguration : IEntityTypeConfiguration<WerkFase>
    {
        public void Configure(EntityTypeBuilder<WerkFase> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("werk_fase_pk");

            builder.ToTable("werk_fase");

            builder.HasIndex(e => e.Fase)
                .HasName("index_fase")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Fase)
                .IsRequired()
                .HasColumnName("fase")
                .HasMaxLength(40);

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
        }
    }
}
