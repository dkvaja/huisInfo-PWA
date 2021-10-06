using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatDeelnemerConfiguration : IEntityTypeConfiguration<ViewPortalChatDeelnemer>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChatDeelnemer> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat_deelnemer");

            builder.Property(e => e.ChatDeelnemerGuid)
                .IsRequired()
                .HasColumnName("chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatGuid)
                .IsRequired()
                .HasColumnName("chat_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.LoginAccountVoor).HasColumnName("login_account_voor");

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100);
        }
    }
}
