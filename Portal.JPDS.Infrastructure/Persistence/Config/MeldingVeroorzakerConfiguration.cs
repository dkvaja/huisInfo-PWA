using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingVeroorzakerConfiguration : IEntityTypeConfiguration<MeldingVeroorzaker>
    {
        public void Configure(EntityTypeBuilder<MeldingVeroorzaker> builder)
        {
            builder.HasKey(e => e.Guid)
                       .HasName("melding_veroorzaker_pk");

            builder.ToTable("melding_veroorzaker");

            builder.HasIndex(e => e.Veroorzaker)
                .HasName("veroorzaker")
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

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Veroorzaker)
                .IsRequired()
                .HasColumnName("veroorzaker")
                .HasMaxLength(40);
        }
    }
}
