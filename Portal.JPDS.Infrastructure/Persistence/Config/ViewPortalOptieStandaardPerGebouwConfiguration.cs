using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalOptieStandaardPerGebouwConfiguration : IEntityTypeConfiguration<ViewPortalOptieStandaardPerGebouw>
    {
        public void Configure(EntityTypeBuilder<ViewPortalOptieStandaardPerGebouw> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_optie_standaard_per_gebouw");

            builder.Property(e => e.Aantal)
                .HasColumnName("aantal")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.CommercieleOmschrijving).HasColumnName("commerciele_omschrijving");

            builder.Property(e => e.Eenheid)
                .HasColumnName("eenheid")
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.EenheidAantalDecimalen).HasColumnName("eenheid_aantal_decimalen");

            builder.Property(e => e.EenheidGuid)
                .HasColumnName("eenheid_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Omschrijving)
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.OptieCategorieWerkGuid)
                .IsRequired()
                .HasColumnName("optie_categorie_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieRubriekWerkGuid)
                .IsRequired()
                .HasColumnName("optie_rubriek_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieStandaardGuid)
                .IsRequired()
                .HasColumnName("optie_standaard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Optienummer)
                .HasColumnName("optienummer")
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.TekstPrijskolom)
                .HasColumnName("tekst_prijskolom")
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.TekstPrijskolomGuid)
                .HasColumnName("tekst_prijskolom_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerkoopprijsExclBtw)
                .HasColumnName("verkoopprijs_excl_btw")
                .HasColumnType("numeric(12, 2)");

            builder.Property(e => e.VerkoopprijsExclBtwOfTekstPrijskolom)
                .HasColumnName("verkoopprijs_excl_btw_of_tekst_prijskolom")
                .HasMaxLength(15)
                .IsUnicode(false);

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
