using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class EenheidConfiguration : IEntityTypeConfiguration<Eenheid>
    {
        public void Configure(EntityTypeBuilder<Eenheid> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("eenheid_pk");

            builder.ToTable("eenheid");

            builder.HasIndex(e => e.Eenheid1)
                .HasName("so_eenheid")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AantalDecimalen).HasColumnName("aantal_decimalen");

            builder.Property(e => e.Eenheid1)
                .IsRequired()
                .HasColumnName("eenheid")
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.EenheidCalculatieprogramma)
                .HasColumnName("eenheid_calculatieprogramma")
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.EenheidMeervoud)
                .HasColumnName("eenheid_meervoud")
                .HasMaxLength(10)
                .IsUnicode(false);

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

            builder.Property(e => e.Maatvoering)
                .IsRequired()
                .HasColumnName("maatvoering")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(40);

            builder.Property(e => e.Overige)
                .IsRequired()
                .HasColumnName("overige")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Tijd)
                .IsRequired()
                .HasColumnName("tijd")
                .HasDefaultValueSql("('0')");
        }
    }
}
