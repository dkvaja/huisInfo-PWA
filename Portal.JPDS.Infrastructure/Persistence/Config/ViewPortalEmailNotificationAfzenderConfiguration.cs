using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalEmailNotificationAfzenderConfiguration : IEntityTypeConfiguration<ViewPortalEmailNotificationAfzender>
    {
        public void Configure(EntityTypeBuilder<ViewPortalEmailNotificationAfzender> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_email_notification_afzender");

            builder.Property(e => e.EmailAccountGuid)
                .IsRequired()
                .HasColumnName("email_account_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MedewerkerGuid)
                .IsRequired()
                .HasColumnName("medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.SmtpFromEmail)
                .HasColumnName("smtp_from_email")
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.Property(e => e.SmtpHost)
                .IsRequired()
                .HasColumnName("smtp_host")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.SmtpPasswordDecrypted)
                .HasColumnName("smtp_password_decrypted")
                .HasMaxLength(250);

            builder.Property(e => e.SmtpPort).HasColumnName("smtp_port");

            builder.Property(e => e.SmtpUseSsl).HasColumnName("smtp_use_ssl");

            builder.Property(e => e.SmtpUser)
                .HasColumnName("smtp_user")
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
