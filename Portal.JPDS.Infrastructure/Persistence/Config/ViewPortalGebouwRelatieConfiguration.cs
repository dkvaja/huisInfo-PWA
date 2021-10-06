using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalGebouwRelatieConfiguration : IEntityTypeConfiguration<ViewPortalGebouwRelatie>
    {
        public void Configure(EntityTypeBuilder<ViewPortalGebouwRelatie> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_gebouw_relatie");

            builder.Property(e => e.AdresNummer)
                .HasColumnName("adres_nummer")
                .HasMaxLength(12);

            builder.Property(e => e.AdresNummerToevoeging)
                .HasColumnName("adres_nummer_toevoeging")
                .HasMaxLength(10);

            builder.Property(e => e.AdresPlaats)
                .HasColumnName("adres_plaats")
                .HasMaxLength(40);

            builder.Property(e => e.AdresPostcode)
                .HasColumnName("adres_postcode")
                .HasMaxLength(8);

            builder.Property(e => e.AdresStraat)
                .HasColumnName("adres_straat")
                .HasMaxLength(40);

            builder.Property(e => e.AdresStraatNummerToevoeging)
                .HasColumnName("adres_straat_nummer_toevoeging")
                .HasMaxLength(64);

            builder.Property(e => e.AdresVolledigAdres)
                .HasColumnName("adres_volledig_adres")
                .HasMaxLength(116);

            builder.Property(e => e.GebouwBouwnummerExtern)
                .HasColumnName("gebouw_bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.GebouwBouwnummerIntern)
                .HasColumnName("gebouw_bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.GebouwBouwstroomGuid)
                .HasColumnName("gebouw_bouwstroom_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwKoperHuurderGuid)
                .HasColumnName("gebouw_koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwSoort)
                .HasColumnName("gebouw_soort")
                .HasMaxLength(40);

            builder.Property(e => e.GebouwStatus)
                .HasColumnName("gebouw_status")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwWoningSoort)
                .HasColumnName("gebouw_woning_soort")
                .HasMaxLength(40);

            builder.Property(e => e.GebouwWoningType)
                .HasColumnName("gebouw_woning_type")
                .HasMaxLength(80);

            builder.Property(e => e.LoginGuid)
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .IsRequired()
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkWerknaam)
                .IsRequired()
                .HasColumnName("werk_werknaam")
                .HasMaxLength(100);

            builder.Property(e => e.WerkWerknummer)
                .IsRequired()
                .HasColumnName("werk_werknummer")
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
