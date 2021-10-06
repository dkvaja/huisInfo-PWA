using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalMedewerkerConfiguration : IEntityTypeConfiguration<ViewPortalMedewerker>
    {
        public void Configure(EntityTypeBuilder<ViewPortalMedewerker> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_medewerker");

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonVolledigeNaam)
                .HasColumnName("persoon_volledige_naam")
                .HasMaxLength(150);
        }
    }
}
