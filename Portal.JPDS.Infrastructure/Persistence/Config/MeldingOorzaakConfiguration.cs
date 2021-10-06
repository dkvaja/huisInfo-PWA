using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingOorzaakConfiguration : IEntityTypeConfiguration<MeldingOorzaak>
    {
        public void Configure(EntityTypeBuilder<MeldingOorzaak> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("melding_oorzaak_pk");

            builder.ToTable("melding_oorzaak");

            builder.HasIndex(e => e.Oorzaak)
                .HasName("oorzaak")
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

            builder.Property(e => e.Oorzaak)
                .IsRequired()
                .HasColumnName("oorzaak")
                .HasMaxLength(40);
        }
    }
}
