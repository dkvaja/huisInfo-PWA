using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalEmailNotificationConfiguration : IEntityTypeConfiguration<ViewPortalEmailNotification>
    {
        public void Configure(EntityTypeBuilder<ViewPortalEmailNotification> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_email_notification");

            builder.Property(e => e.Bericht)
                .HasColumnName("bericht")
                .HasMaxLength(108);

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BriefaanhefFormeel)
                .HasColumnName("briefaanhef_formeel")
                .HasMaxLength(200);

            builder.Property(e => e.BriefaanhefInFormeel)
                .HasColumnName("briefaanhef_informeel")
                .HasMaxLength(200);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.HoofdaannemerNaam)
                .HasColumnName("hoofdaannemer_naam")
                .HasMaxLength(100);

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100);

            builder.Property(e => e.PersoonGuid)
                .HasColumnName("persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerknummerWerknaam)
                .IsRequired()
                .HasColumnName("werknummer_werknaam")
                .HasMaxLength(113);
        }
    }
}
