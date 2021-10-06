using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalBijlageConfiguration : IEntityTypeConfiguration<ViewPortalBijlage>
    {
        public void Configure(EntityTypeBuilder<ViewPortalBijlage> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_bijlage");

            builder.Property(e => e.Bijlage)
                .HasColumnName("bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageGuid)
                .IsRequired()
                .HasColumnName("bijlage_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BijlageRubriek)
                .HasColumnName("bijlage_rubriek")
                .HasMaxLength(40);

            builder.Property(e => e.BijlageRubriekGuid)
                .HasColumnName("bijlage_rubriek_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Datum)
                .HasColumnName("datum")
                .HasColumnType("date");

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(100);

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
