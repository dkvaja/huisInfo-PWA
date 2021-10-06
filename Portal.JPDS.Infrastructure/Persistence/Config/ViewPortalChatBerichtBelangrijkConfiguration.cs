using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatBerichtBelangrijkConfiguration : IEntityTypeConfiguration<ViewPortalChatBerichtBelangrijk>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChatBerichtBelangrijk> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat_bericht_belangrijk");

            builder.Property(e => e.Bericht)
                .IsRequired()
                .HasColumnName("bericht");

            builder.Property(e => e.Bijlage)
                .HasColumnName("bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageGuid)
                .HasColumnName("bijlage_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerIntern)
                .HasColumnName("bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.ChatBerichtBelangrijkGuid)
                .IsRequired()
                .HasColumnName("chat_bericht_belangrijk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatBerichtBelangrijkIngevoerdOp)
                .HasColumnName("chat_bericht_belangrijk_ingevoerd_op")
                .HasColumnType("datetime");

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

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
