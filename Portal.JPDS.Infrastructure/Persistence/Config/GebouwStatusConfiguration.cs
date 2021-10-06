using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class GebouwStatusConfiguration : IEntityTypeConfiguration<GebouwStatus>
    {
        public void Configure(EntityTypeBuilder<GebouwStatus> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("gebouw_status_pk");

            builder.ToTable("gebouw_status");

            builder.HasIndex(e => e.GebouwStatus1, "gebouw_status")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.GebouwStatus1)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gebouw_status");

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
