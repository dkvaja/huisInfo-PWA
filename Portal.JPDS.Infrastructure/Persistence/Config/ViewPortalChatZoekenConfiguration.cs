using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatZoekenConfiguration : IEntityTypeConfiguration<ViewPortalChatZoeken>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChatZoeken> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat_zoeken");

            builder.Property(e => e.Bericht).HasColumnName("bericht");

            builder.Property(e => e.BijlageAanwezig).HasColumnName("bijlage_aanwezig");

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerIntern)
                .HasColumnName("bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.ChatBerichtGuid)
                .IsRequired()
                .HasColumnName("chat_bericht_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

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

            builder.Property(e => e.ChatOnderwerp)
                .HasColumnName("chat_onderwerp")
                .HasMaxLength(40);

            builder.Property(e => e.DatumEnTijd)
                .HasColumnName("datum_en_tijd")
                .HasColumnType("datetime");

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieNaam)
                .HasColumnName("organisatie_naam")
                .HasMaxLength(100);

            builder.Property(e => e.VerzenderChatDeelnemerGuid)
                .HasColumnName("verzender_chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerzenderNaam)
                .HasColumnName("verzender_naam")
                .HasMaxLength(100);

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
