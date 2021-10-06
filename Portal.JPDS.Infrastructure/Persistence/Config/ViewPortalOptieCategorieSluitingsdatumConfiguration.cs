using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalOptieCategorieSluitingsdatumConfiguration : IEntityTypeConfiguration<ViewPortalOptieCategorieSluitingsdatum>
    {
        public void Configure(EntityTypeBuilder<ViewPortalOptieCategorieSluitingsdatum> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_optie_categorie_sluitingsdatum");

            builder.Property(e => e.Categorie)
                .IsRequired()
                .HasColumnName("categorie")
                .HasMaxLength(40);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieCategorieWerkGuid)
                .IsRequired()
                .HasColumnName("optie_categorie_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Sluitingsdatum)
                .HasColumnName("sluitingsdatum")
                .HasColumnType("date");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");
        }
    }
}
