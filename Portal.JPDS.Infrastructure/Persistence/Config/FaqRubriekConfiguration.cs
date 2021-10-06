using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class FaqRubriekConfiguration : IEntityTypeConfiguration<FaqRubriek>
    {
        public void Configure(EntityTypeBuilder<FaqRubriek> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("faq_rubriek_pk");

            builder.ToTable("faq_rubriek");

            builder.HasIndex(e => new { e.Volgorde, e.FaqRubriek1 })
                .HasName("so_faq_rubriek");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.FaqRubriek1)
                .IsRequired()
                .HasColumnName("faq_rubriek")
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

            builder.Property(e => e.Systeemwaarde).HasColumnName("systeemwaarde");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");
        }
    }
}
