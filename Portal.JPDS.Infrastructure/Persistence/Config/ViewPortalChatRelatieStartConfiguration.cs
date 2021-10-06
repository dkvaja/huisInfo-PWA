using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatRelatieStartConfiguration : IEntityTypeConfiguration<ViewPortalChatRelatieStart>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChatRelatieStart> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat_relatie_start");

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerIntern)
                .HasColumnName("bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.ChatContactGuid)
                .IsRequired()
                .HasColumnName("chat_contact_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieNaam)
                .IsRequired()
                .HasColumnName("organisatie_naam")
                .HasMaxLength(100);

            builder.Property(e => e.PersoonNaam)
                .HasColumnName("persoon_naam")
                .HasMaxLength(100);

            builder.Property(e => e.RelatieGuid)
                .IsRequired()
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
