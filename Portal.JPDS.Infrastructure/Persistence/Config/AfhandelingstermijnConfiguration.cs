using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class AfhandelingstermijnConfiguration : IEntityTypeConfiguration<Afhandelingstermijn>
    {
        public void Configure(EntityTypeBuilder<Afhandelingstermijn> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("afhandelingstermijn_pk");

            builder.ToTable("afhandelingstermijn");

            builder.HasIndex(e => e.Omschrijving)
                .HasName("so_afhandelingstermijn");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AantalDagen).HasColumnName("aantal_dagen");

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

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(40);

            builder.Property(e => e.Werkdagen)
                .IsRequired()
                .HasColumnName("werkdagen")
                .HasDefaultValueSql("('0')");
        }
    }
}
