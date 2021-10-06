using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalBetrokkeneConfiguration : IEntityTypeConfiguration<ViewPortalBetrokkene>
    {
        public void Configure(EntityTypeBuilder<ViewPortalBetrokkene> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_betrokkene");

            builder.Property(e => e.BetrokkeneGuid)
                .IsRequired()
                .HasColumnName("betrokkene_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieBezoekadresPlaats)
                .HasColumnName("organisatie_bezoekadres_plaats")
                .HasMaxLength(40);

            builder.Property(e => e.OrganisatieBezoekadresPostcode)
                .HasColumnName("organisatie_bezoekadres_postcode")
                .HasMaxLength(8);

            builder.Property(e => e.OrganisatieBezoekadresStraatNummerToevoeging)
                .HasColumnName("organisatie_bezoekadres_straat_nummer_toevoeging")
                .HasMaxLength(64);

            builder.Property(e => e.OrganisatieEmail)
                .HasColumnName("organisatie_email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieLogo)
                .HasColumnName("organisatie_logo")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieNaamOnderdeel)
                .HasColumnName("organisatie_naam_onderdeel")
                .HasMaxLength(203);

            builder.Property(e => e.OrganisatiePostadresPlaats)
                .HasColumnName("organisatie_postadres_plaats")
                .HasMaxLength(40);

            builder.Property(e => e.OrganisatiePostadresPostcode)
                .HasColumnName("organisatie_postadres_postcode")
                .HasMaxLength(8);

            builder.Property(e => e.OrganisatiePostadresStraatNummerToevoeging)
                .HasColumnName("organisatie_postadres_straat_nummer_toevoeging")
                .HasMaxLength(64);

            builder.Property(e => e.OrganisatieTelefoon)
                .HasColumnName("organisatie_telefoon")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieWebsite)
                .HasColumnName("organisatie_website")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.ProductCode)
                .HasColumnName("product_code")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.ProductOmschrijving)
                .HasColumnName("product_omschrijving")
                .HasMaxLength(40);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkLogo)
                .HasColumnName("werk_logo")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Werknaam)
                .HasColumnName("werknaam")
                .HasMaxLength(100);

            builder.Property(e => e.Werknummer)
                .HasColumnName("werknummer")
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
