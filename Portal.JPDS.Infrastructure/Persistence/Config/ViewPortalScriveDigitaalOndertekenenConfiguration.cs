using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalScriveDigitaalOndertekenenConfiguration : IEntityTypeConfiguration<ViewPortalScriveDigitaalOndertekenen>
    {
        public void Configure(EntityTypeBuilder<ViewPortalScriveDigitaalOndertekenen> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_scrive_digitaal_ondertekenen");

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.KoperHuurderGuid)
                .HasColumnName("koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Login1Email)
                .HasColumnName("login_1_email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Login1Guid)
                .HasColumnName("login_1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Login1Naam)
                .HasColumnName("login_1_naam")
                .HasMaxLength(100);

            builder.Property(e => e.Login2Email)
                .HasColumnName("login_2_email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Login2Guid)
                .HasColumnName("login_2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Login2Naam)
                .HasColumnName("login_2_naam")
                .HasMaxLength(100);

            builder.Property(e => e.Offertenummer).HasColumnName("offertenummer");

            builder.Property(e => e.Ondertekening).HasColumnName("ondertekening");

            builder.Property(e => e.OndertekeningAantalPersonen).HasColumnName("ondertekening_aantal_personen");

            builder.Property(e => e.OptieGekozenOfferteGuid)
                .IsRequired()
                .HasColumnName("optie_gekozen_offerte_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Persoon1Guid)
                .HasColumnName("persoon_1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Persoon2Guid)
                .HasColumnName("persoon_2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Sluitingsdatum)
                .HasColumnName("sluitingsdatum")
                .HasColumnType("date");
        }
    }
}
