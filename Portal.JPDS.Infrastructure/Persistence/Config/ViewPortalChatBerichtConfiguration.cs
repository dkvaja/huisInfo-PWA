using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalChatBerichtConfiguration : IEntityTypeConfiguration<ViewPortalChatBericht>
    {
        public void Configure(EntityTypeBuilder<ViewPortalChatBericht> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_chat_bericht");

            builder.Property(e => e.ActieGuid)
                .HasColumnName("actie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Belangrijk).HasColumnName("belangrijk");

            builder.Property(e => e.Bericht).HasColumnName("bericht");

            builder.Property(e => e.Bijlage)
                .HasColumnName("bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageGuid)
                .HasColumnName("bijlage_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatBerichtGuid)
                .IsRequired()
                .HasColumnName("chat_bericht_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatGuid)
                .IsRequired()
                .HasColumnName("chat_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DatumEnTijd)
                .HasColumnName("datum_en_tijd")
                .HasColumnType("datetime");

            builder.Property(e => e.ReactieOpChatBerichtBericht).HasColumnName("reactie_op_chat_bericht_bericht");

            builder.Property(e => e.ReactieOpChatBerichtBijlage)
                .HasColumnName("reactie_op_chat_bericht_bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.ReactieOpChatBerichtGuid)
                .HasColumnName("reactie_op_chat_bericht_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ReactieOpChatBerichtVerwijderd).HasColumnName("reactie_op_chat_bericht_verwijderd");

            builder.Property(e => e.ReactieOpChatBerichtVerzenderChatDeelnemerGuid)
                .HasColumnName("reactie_op_chat_bericht_verzender_chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Verwijderd).HasColumnName("verwijderd");

            builder.Property(e => e.VerwijderdDoorDeelnemer)
                .HasColumnName("verwijderd_door_deelnemer")
                .HasMaxLength(100);

            builder.Property(e => e.VerzenderChatDeelnemerGuid)
                .HasColumnName("verzender_chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerzenderLoginGuid)
                .HasColumnName("verzender_login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerzenderNaam)
                .HasColumnName("verzender_naam")
                .HasMaxLength(100);
        }
    }
}
