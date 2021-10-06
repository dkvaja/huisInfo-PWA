using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalDashboardPlanningKoperHuurderConfiguration : IEntityTypeConfiguration<ViewPortalDashboardPlanningKoperHuurder>
    {
        public void Configure(EntityTypeBuilder<ViewPortalDashboardPlanningKoperHuurder> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_dashboard_planning_koper_huurder");

            builder.Property(e => e.ActieDatum)
                .HasColumnName("actie_datum")
                .HasColumnType("date");

            builder.Property(e => e.ActieStarttijd).HasColumnName("actie_starttijd");

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Omschrijving)
                .HasColumnName("omschrijving")
                .HasMaxLength(55);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
