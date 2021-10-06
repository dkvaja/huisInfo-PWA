using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class GebouwSoortConfiguration : IEntityTypeConfiguration<GebouwSoort>
    {
        public void Configure(EntityTypeBuilder<GebouwSoort> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("gebouw_soort_pk");

            builder.ToTable("gebouw_soort");

            builder.HasIndex(e => e.GebouwSoort1, "index_gebouw_soort")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.GebouwGebruiker)
                .HasColumnName("gebouw_gebruiker")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GebouwNaamRegistreren)
                .IsRequired()
                .HasColumnName("gebouw_naam_registreren")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GebouwSoort1)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("gebouw_soort");

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

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");
        }
    }
}
