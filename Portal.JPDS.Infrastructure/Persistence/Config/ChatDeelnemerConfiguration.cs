using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ChatDeelnemerConfiguration : IEntityTypeConfiguration<ChatDeelnemer>
    {
        public void Configure(EntityTypeBuilder<ChatDeelnemer> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("chat_deelnemer_pk");

            builder.ToTable("chat_deelnemer");

            builder.HasIndex(e => e.LoginGuid)
                .HasName("fk_ref_login_chat_deelnemer");

            builder.HasIndex(e => new { e.ChatGuid, e.LoginGuid })
                .HasName("so_chat_deelnemer");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatGuid)
                .IsRequired()
                .HasColumnName("chat_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

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

            builder.Property(e => e.LaatstGelezenChatBericht)
                .HasColumnName("laatst_gelezen_chat_bericht")
                .HasColumnType("datetime");

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.ChatGu)
                .WithMany(p => p.ChatDeelnemer)
                .HasForeignKey(d => d.ChatGuid)
                .HasConstraintName("ref_chat_chat_deelnemer_integriteit");

            builder.HasOne(d => d.LoginGu)
                .WithMany(p => p.ChatDeelnemer)
                .HasForeignKey(d => d.LoginGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_login_chat_deelnemer");
        }
    }
}
