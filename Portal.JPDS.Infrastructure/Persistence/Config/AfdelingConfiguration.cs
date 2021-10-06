using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class AfdelingConfiguration : IEntityTypeConfiguration<Afdeling>
    {
        public void Configure(EntityTypeBuilder<Afdeling> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("afdeling_pk");

            builder.ToTable("afdeling");

            builder.HasIndex(e => e.Afdeling1, "index_afdeling")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Afdeling1)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("afdeling");

            builder.Property(e => e.CodeTbvCommunicatieKenmerk)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("code_tbv_communicatie_kenmerk");

            builder.Property(e => e.CorrespondentieInSubmap)
                .IsRequired()
                .HasColumnName("correspondentie_in_submap")
                .HasDefaultValueSql("('0')");

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

            builder.Property(e => e.InterneAfdeling)
                .IsRequired()
                .HasColumnName("interne_afdeling")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Submap)
                .HasMaxLength(40)
                .HasColumnName("submap");
        }
    }
}
