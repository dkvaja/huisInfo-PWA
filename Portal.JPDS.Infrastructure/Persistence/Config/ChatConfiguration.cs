using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("chat_pk");

            builder.ToTable("chat");

            builder.HasIndex(e => e.ChatBegonnenDoorLoginGuid)
                .HasName("fk_ref_login_chat");

            builder.HasIndex(e => e.OrganisatieGuid)
                .HasName("fk_ref_organisatie_chat");

            builder.HasIndex(e => e.WerkGuid)
                .HasName("fk_ref_werk_chat");

            builder.HasIndex(e => new { e.GebouwGuid, e.OrganisatieGuid })
                .HasName("so_chat");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ChatBegonnenDoorLoginGuid)
                .IsRequired()
                .HasColumnName("chat_begonnen_door_login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
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

            builder.Property(e => e.Onderwerp)
                .HasColumnName("onderwerp")
                .HasMaxLength(40);

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.ChatBegonnenDoorLoginGu)
                .WithMany(p => p.Chat)
                .HasForeignKey(d => d.ChatBegonnenDoorLoginGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_login_chat");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.Chat)
                .HasForeignKey(d => d.GebouwGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_gebouw_chat");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.Chat)
                .HasForeignKey(d => d.OrganisatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_organisatie_chat");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Chat)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_chat");
        }
    }
}
