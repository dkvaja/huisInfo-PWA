using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatConfiguration : IEntityTypeConfiguration<ViewPortalChat>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChat> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat");

            builder.Property(e => e.AantalOngelezenBerichten).HasColumnName("aantal_ongelezen_berichten");

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerIntern)
                .HasColumnName("bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.ChatBegonnenDoorLoginGuid)
                .HasColumnName("chat_begonnen_door_login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatBegonnenOp)
                .HasColumnName("chat_begonnen_op")
                .HasColumnType("datetime");

            builder.Property(e => e.ChatDeelnemerGuid)
                .IsRequired()
                .HasColumnName("chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatGuid)
                .HasColumnName("chat_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatOnderwerp)
                .HasColumnName("chat_onderwerp")
                .HasMaxLength(40);

            builder.Property(e => e.DatumEnTijdLaatstVerzondenChatBericht)
                .HasColumnName("datum_en_tijd_laatst_verzonden_chat_bericht")
                .HasColumnType("datetime");

            builder.Property(e => e.EersteRegelLaatsteChatBericht).HasColumnName("eerste_regel_laatste_chat_bericht");

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.LaatstGelezenChatBericht)
                .HasColumnName("laatst_gelezen_chat_bericht")
                .HasColumnType("datetime");

            builder.Property(e => e.LaatsteChatBerichtBijlage)
                .HasColumnName("laatste_chat_bericht_bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.LaatsteChatBerichtVerzondenDoorChatDeelnemerGuid)
                .HasColumnName("laatste_chat_bericht_verzonden_door_chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.LaatsteChatBerichtVerzondenDoorChatDeelnemerNaam)
                .HasColumnName("laatste_chat_bericht_verzonden_door_chat_deelnemer_naam")
                .HasMaxLength(100);

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
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
