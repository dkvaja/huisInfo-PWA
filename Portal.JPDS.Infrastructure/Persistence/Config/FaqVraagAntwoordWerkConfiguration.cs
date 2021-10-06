using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class FaqVraagAntwoordWerkConfiguration : IEntityTypeConfiguration<FaqVraagAntwoordWerk>
    {
        public void Configure(EntityTypeBuilder<FaqVraagAntwoordWerk> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("faq_vraag_antwoord_werk_pk");

            builder.ToTable("faq_vraag_antwoord_werk");

            builder.HasIndex(e => e.FaqRubriekGuid)
                .HasName("fk_ref_faq_rubriek_faq_vraag_antwoord_werk");

            builder.HasIndex(e => new { e.WerkGuid, e.FaqRubriekGuid, e.Vraag })
                .HasName("unique_werk_guid_faq_rubriek_guid_vraag")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Antwoord)
                .IsRequired()
                .HasColumnName("antwoord");

            builder.Property(e => e.FaqRubriekGuid)
                .IsRequired()
                .HasColumnName("faq_rubriek_guid")
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

            builder.Property(e => e.Vraag)
                .IsRequired()
                .HasColumnName("vraag")
                .HasMaxLength(250);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.FaqRubriekGu)
                .WithMany(p => p.FaqVraagAntwoordWerk)
                .HasForeignKey(d => d.FaqRubriekGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_faq_rubriek_faq_vraag_antwoord_werk");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.FaqVraagAntwoordWerk)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_faq_vraag_antwoord_werk");
        }
    }
}
