using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalGebouwAlgemeenConfiguration : IEntityTypeConfiguration<ViewPortalGebouwAlgemeen>
    {
        public void Configure(EntityTypeBuilder<ViewPortalGebouwAlgemeen> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_gebouw_algemeen");

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.DatumEindeOnderhoudstermijn)
                .HasColumnName("datum_einde_onderhoudstermijn")
                .HasColumnType("date");

            builder.Property(e => e.EanElektriciteit)
                .HasColumnName("ean_elektriciteit")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.EanGas)
                .HasColumnName("ean_gas")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GarantieCertificaatGeldigTm)
                .HasColumnName("garantie_certificaat_geldig_tm")
                .HasColumnType("date");

            builder.Property(e => e.GarantieCertificaatGeldigVa)
                .HasColumnName("garantie_certificaat_geldig_va")
                .HasColumnType("date");

            builder.Property(e => e.GarantieCertificaatNummer)
                .HasColumnName("garantie_certificaat_nummer")
                .HasMaxLength(40);

            builder.Property(e => e.Garantieregeling)
                .HasColumnName("garantieregeling")
                .HasMaxLength(40);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwSoort)
                .HasColumnName("gebouw_soort")
                .HasMaxLength(40);

            builder.Property(e => e.Huisnummer)
                .HasColumnName("huisnummer")
                .HasMaxLength(12);

            builder.Property(e => e.HuisnummerToevoeging)
                .HasColumnName("huisnummer_toevoeging")
                .HasMaxLength(10);

            builder.Property(e => e.Plaats)
                .HasColumnName("plaats")
                .HasMaxLength(40);

            builder.Property(e => e.Postcode)
                .HasColumnName("postcode")
                .HasMaxLength(8);

            builder.Property(e => e.Straat)
                .HasColumnName("straat")
                .HasMaxLength(40);

            builder.Property(e => e.VrijOpNaamPrijsInclBtw)
                .HasColumnName("vrij_op_naam_prijs_incl_btw")
                .HasColumnType("numeric(12, 2)");

            builder.Property(e => e.WoningType)
                .HasColumnName("woning_type")
                .HasMaxLength(80);
        }
    }
}
