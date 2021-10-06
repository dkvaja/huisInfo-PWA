using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalDashboardPlanningKopersbegeleiderConfiguration : IEntityTypeConfiguration<ViewPortalDashboardPlanningKopersbegeleider>
    {
        public void Configure(EntityTypeBuilder<ViewPortalDashboardPlanningKopersbegeleider> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_dashboard_planning_kopersbegeleider");

            builder.Property(e => e.ActieDatum)
                .HasColumnName("actie_datum")
                .HasColumnType("date");

            builder.Property(e => e.ActieStarttijd).HasColumnName("actie_starttijd");

            builder.Property(e => e.GebouwGuid).HasColumnName("gebouw_guid");

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Omschrijving)
                .HasColumnName("omschrijving")
                .HasMaxLength(98);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
