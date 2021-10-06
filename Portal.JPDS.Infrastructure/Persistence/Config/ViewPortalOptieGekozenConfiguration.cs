using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalOptieGekozenConfiguration : IEntityTypeConfiguration<ViewPortalOptieGekozen>
    {
        public void Configure(EntityTypeBuilder<ViewPortalOptieGekozen> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_optie_gekozen");

            builder.Property(e => e.Aantal)
                .HasColumnName("aantal")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.AantalDecimalen).HasColumnName("aantal_decimalen");

            builder.Property(e => e.AanvullendeOmschrijving).HasColumnName("aanvullende_omschrijving");

            builder.Property(e => e.Categorie)
                .IsRequired()
                .HasColumnName("categorie")
                .HasMaxLength(40);

            builder.Property(e => e.CategorieVolgorde).HasColumnName("categorie_volgorde");

            builder.Property(e => e.CommercieleOmschrijving).HasColumnName("commerciele_omschrijving");

            builder.Property(e => e.Eenheid)
                .HasColumnName("eenheid")
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.EenheidGuid)
                .HasColumnName("eenheid_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Offertenummer).HasColumnName("offertenummer");

            builder.Property(e => e.Omschrijving)
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.OptieCategorieWerkGuid)
                .IsRequired()
                .HasColumnName("optie_categorie_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieGekozenOfferteGuid)
                .HasColumnName("optie_gekozen_offerte_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieRubriekWerkGuid)
                .IsRequired()
                .HasColumnName("optie_rubriek_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieStandaardGuid)
                .HasColumnName("optie_standaard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieStatus).HasColumnName("optie_status");

            builder.Property(e => e.OptieSubstatus).HasColumnName("optie_substatus");

            builder.Property(e => e.Optienummer)
                .HasColumnName("optienummer")
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.Rubriek)
                .IsRequired()
                .HasColumnName("rubriek")
                .HasMaxLength(40);

            builder.Property(e => e.RubriekVolgorde).HasColumnName("rubriek_volgorde");

            builder.Property(e => e.Sluitingsdatum)
                .HasColumnName("sluitingsdatum")
                .HasColumnType("date");

            builder.Property(e => e.Soort).HasColumnName("soort");

            builder.Property(e => e.VerkoopprijsInclBtw)
                .HasColumnName("verkoopprijs_incl_btw")
                .HasColumnType("numeric(12, 2)");

            builder.Property(e => e.VerkoopprijsInclBtwOfTekstPrijskolom)
                .HasColumnName("verkoopprijs_incl_btw_of_tekst_prijskolom")
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.VerkoopprijsIsStelpost).HasColumnName("verkoopprijs_is_stelpost");

            builder.Property(e => e.VerkoopprijsNtb).HasColumnName("verkoopprijs_ntb");
        }
    }
}
