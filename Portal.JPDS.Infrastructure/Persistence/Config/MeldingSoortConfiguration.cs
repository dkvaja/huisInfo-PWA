using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingSoortConfiguration : IEntityTypeConfiguration<MeldingSoort>
    {
        public void Configure(EntityTypeBuilder<MeldingSoort> builder)
        {
            builder.HasKey(e => e.Guid)
                       .HasName("melding_soort_pk");

            builder.ToTable("melding_soort");

            builder.HasIndex(e => e.MeldingSoort1)
                .HasName("melding_soort")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('1')");

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

            builder.Property(e => e.MeldingSoort1)
                .IsRequired()
                .HasColumnName("melding_soort")
                .HasMaxLength(40);

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");
        }
    }
}
