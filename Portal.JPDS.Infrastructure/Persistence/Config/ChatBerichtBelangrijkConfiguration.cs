using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ChatBerichtBelangrijkConfiguration : IEntityTypeConfiguration<ChatBerichtBelangrijk>
    {
        public void Configure(EntityTypeBuilder<ChatBerichtBelangrijk> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("chat_bericht_belangrijk_pk");

            builder.ToTable("chat_bericht_belangrijk");

            builder.HasIndex(e => e.ChatDeelnemerGuid)
                .HasName("fk_ref_chat_deelnemer_chat_bericht_belangrijk_integriteit");

            builder.HasIndex(e => new { e.ChatBerichtGuid, e.ChatDeelnemerGuid })
                .HasName("so_chat_bericht_belangrijk");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
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

            builder.HasOne(d => d.ChatBerichtGu)
                .WithMany(p => p.ChatBerichtBelangrijk)
                .HasForeignKey(d => d.ChatBerichtGuid)
                .HasConstraintName("ref_chat_bericht_chat_bericht_belangrijk");

            builder.HasOne(d => d.ChatDeelnemerGu)
                .WithMany(p => p.ChatBerichtBelangrijk)
                .HasForeignKey(d => d.ChatDeelnemerGuid)
                .HasConstraintName("ref_chat_deelnemer_chat_bericht_belangrijk_integriteit");
        }
    }
}
