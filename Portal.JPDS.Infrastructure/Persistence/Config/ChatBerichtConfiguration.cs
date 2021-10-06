using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ChatBerichtConfiguration : IEntityTypeConfiguration<ChatBericht>
    {
        public void Configure(EntityTypeBuilder<ChatBericht> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("chat_bericht_pk");

            builder.ToTable("chat_bericht");

            builder.HasIndex(e => e.ActieGuid)
                .HasName("fk_ref_actie_chat_bericht");

            builder.HasIndex(e => e.ReactieOpChatBerichtGuid)
                .HasName("fk_ref_chat_bericht_chat_bericht");

            builder.HasIndex(e => e.VerwijderdDoorDeelnemerGuid)
                .HasName("fk_ref_chat_deelnemer_chat_bericht_verwijderd_door_integriteit");

            builder.HasIndex(e => e.VerzenderChatDeelnemerGuid)
                .HasName("fk_ref_chat_deelnemer_chat_bericht");

            builder.HasIndex(e => new { e.ChatGuid, e.DatumEnTijd })
                .HasName("so_chat_bericht");

            builder.HasIndex(e => new { e.ChatGuid, e.VerwijderdDoorDeelnemerGuid })
                .HasName("fk_ref_chat_deelnemer_chat_bericht_verwijderd_door_lookup");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieGuid)
                .HasColumnName("actie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Bericht)
                .IsRequired()
                .HasColumnName("bericht");

            builder.Property(e => e.ChatGuid)
                .IsRequired()
                .HasColumnName("chat_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DatumEnTijd)
                .HasColumnName("datum_en_tijd")
                .HasColumnType("datetime");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.ReactieOpChatBerichtGuid)
                .HasColumnName("reactie_op_chat_bericht_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Verwijderd).HasColumnName("verwijderd");

            builder.Property(e => e.VerwijderdDoorDeelnemerGuid)
                .HasColumnName("verwijderd_door_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerzenderChatDeelnemerGuid)
                .HasColumnName("verzender_chat_deelnemer_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Belangrijk).HasColumnName("belangrijk");

            //builder.HasOne(d => d.ActieGu)
            //    .WithMany(p => p.ChatBericht)
            //    .HasForeignKey(d => d.ActieGuid)
            //    .HasConstraintName("ref_actie_chat_bericht");

            builder.HasOne(d => d.ChatGu)
                .WithMany(p => p.ChatBericht)
                .HasForeignKey(d => d.ChatGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_chat_chat_bericht");

            builder.HasOne(d => d.ReactieOpChatBerichtGu)
                .WithMany(p => p.InverseReactieOpChatBerichtGu)
                .HasForeignKey(d => d.ReactieOpChatBerichtGuid)
                .HasConstraintName("ref_chat_bericht_chat_bericht");

            builder.HasOne(d => d.VerwijderdDoorDeelnemerGu)
                .WithMany(p => p.ChatBerichtVerwijderdDoorDeelnemerGu)
                .HasForeignKey(d => d.VerwijderdDoorDeelnemerGuid)
                .HasConstraintName("ref_chat_deelnemer_chat_bericht_verwijderd_door_integriteit");

            builder.HasOne(d => d.VerzenderChatDeelnemerGu)
                .WithMany(p => p.ChatBerichtVerzenderChatDeelnemerGu)
                .HasForeignKey(d => d.VerzenderChatDeelnemerGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_chat_deelnemer_chat_bericht");
        }
    }
}
