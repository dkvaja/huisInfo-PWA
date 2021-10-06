using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingStatusConfiguration : IEntityTypeConfiguration<MeldingStatus>
    {
        public void Configure(EntityTypeBuilder<MeldingStatus> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("melding_status_pk");

            builder.ToTable("melding_status");

            builder.HasIndex(e => e.MeldingStatus1)
                .HasName("melding_status")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Afgehandeld)
                .IsRequired()
                .HasColumnName("afgehandeld")
                .HasDefaultValueSql("('0')");

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

            builder.Property(e => e.MeldingStatus1)
                .IsRequired()
                .HasColumnName("melding_status")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");
        }
    }
}
