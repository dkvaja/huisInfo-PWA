using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OptieGekozenOfferteConfiguration : IEntityTypeConfiguration<OptieGekozenOfferte>
    {
        public void Configure(EntityTypeBuilder<OptieGekozenOfferte> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("optie_gekozen_offerte_pk");

            builder.ToTable("optie_gekozen_offerte");

            builder.HasIndex(e => e.GebouwGuid)
                .HasName("fk_ref_gebouw_optie_gekozen_offerte");

            builder.HasIndex(e => new { e.Offertenummer, e.GebouwGuid })
                .HasName("so_optie_gekozen_offerte");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
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

            builder.Property(e => e.Offertenummer).HasColumnName("offertenummer");

            builder.Property(e => e.Ondertekening).HasColumnName("ondertekening");

            builder.Property(e => e.OndertekeningAantalPersonen).HasColumnName("ondertekening_aantal_personen");

            builder.Property(e => e.ScriveDocumentId).HasColumnName("scrive_document_id");

            builder.Property(e => e.ScriveDocumentStatus)
                .HasColumnName("scrive_document_status")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Sluitingsdatum)
                .HasColumnName("sluitingsdatum")
                .HasColumnType("date");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.OptieGekozenOfferte)
                .HasForeignKey(d => d.GebouwGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_gebouw_optie_gekozen_offerte");
        }
    }
}
