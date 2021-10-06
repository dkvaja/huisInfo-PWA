using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OptieGekozenConfiguration : IEntityTypeConfiguration<OptieGekozen>
    {
        public void Configure(EntityTypeBuilder<OptieGekozen> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("optie_gekozen_pk");

            builder.ToTable("optie_gekozen");

            builder.HasIndex(e => e.EenheidGuid)
                .HasName("fk_ref_eenheid_optie_gekozen");

            builder.HasIndex(e => e.GebouwGuid)
                .HasName("fk_ref_gebouw_optie_gekozen_integriteit");

            builder.HasIndex(e => e.OptieCategorieWerkGuid)
                .HasName("fk_ref_optie_categorie_werk_optie_gekozen_integriteit");

            builder.HasIndex(e => e.OptieGekozenOfferteGuid)
                .HasName("fk_ref_optie_gekozen_offerte_optie_gekozen");

            builder.HasIndex(e => e.OptieRubriekWerkGuid)
                .HasName("fk_ref_optie_rubriek_werk_optie_gekozen_integriteit");

            builder.HasIndex(e => e.OptieStandaardGuid)
                .HasName("fk_ref_optie_standaard_optie_gekozen_integriteit");

            builder.HasIndex(e => e.TekstGelimiteerdeGarantieGuid)
                .HasName("fk_ref_tekst_gelimiteerde_garantie_optie_gekozen");

            builder.HasIndex(e => e.TekstPrijskolomGuid)
                .HasName("fk_ref_tekst_prijskolom_optie_gekozen");

            builder.HasIndex(e => new { e.WerkGuid, e.GebouwGuid })
                .HasName("fk_ref_gebouw_optie_gekozen_lookup");

            builder.HasIndex(e => new { e.WerkGuid, e.OptieCategorieWerkGuid })
                .HasName("fk_ref_optie_categorie_werk_optie_gekozen_lookup");

            builder.HasIndex(e => new { e.WerkGuid, e.OptieRubriekWerkGuid })
                .HasName("fk_ref_optie_rubriek_werk_optie_gekozen_lookup");

            builder.HasIndex(e => new { e.WerkGuid, e.OptieStandaardGuid })
                .HasName("fk_ref_optie_standaard_optie_gekozen_lookup");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Aantal)
                .HasColumnName("aantal")
                .HasColumnType("numeric(10, 2)")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanvullendeOmschrijving).HasColumnName("aanvullende_omschrijving");

            builder.Property(e => e.Afbeelding)
                .HasColumnName("afbeelding")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.BedragBtw)
                .HasColumnName("bedrag_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKaleKostprijs)
                .HasColumnName("bedrag_kale_kostprijs")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten1)
                .HasColumnName("bedrag_kosten_1")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten2)
                .HasColumnName("bedrag_kosten_2")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten3)
                .HasColumnName("bedrag_kosten_3")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten4)
                .HasColumnName("bedrag_kosten_4")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten5)
                .HasColumnName("bedrag_kosten_5")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragToeslag1)
                .HasColumnName("bedrag_toeslag_1")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragToeslag2)
                .HasColumnName("bedrag_toeslag_2")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BerekenKaleKostprijs)
                .IsRequired()
                .HasColumnName("bereken_kale_kostprijs")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.CommercieleOmschrijving).HasColumnName("commerciele_omschrijving");

            builder.Property(e => e.CommercieleOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("commerciele_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.DatumDefinitief)
                .HasColumnName("datum_definitief")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteMeterkastlijst)
                .HasColumnName("datum_laatste_meterkastlijst")
                .HasColumnType("date");

            builder.Property(e => e.DatumVervallen)
                .HasColumnName("datum_vervallen")
                .HasColumnType("date");

            builder.Property(e => e.EenheidGuid)
                .HasColumnName("eenheid_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.EigenVerkoopprijs)
                .HasColumnName("eigen_verkoopprijs")
                .HasColumnType("numeric(14, 2)")
                .HasComputedColumnSql("(coalesce([verkoopprijs_excl_btw],(0.00))-(coalesce([bedrag_toeslag_1],(0.00))+coalesce([bedrag_toeslag_2],(0.00))))");

            builder.Property(e => e.Factureren)
                .IsRequired()
                .HasColumnName("factureren")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.GebouwGuid)
                .IsRequired()
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Gecontroleerd)
                .IsRequired()
                .HasColumnName("gecontroleerd")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GelimiteerdeGarantie)
                .IsRequired()
                .HasColumnName("gelimiteerde_garantie")
                .HasDefaultValueSql("('0')");

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

            builder.Property(e => e.Omschrijving)
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.OmschrijvingKosten1)
                .HasColumnName("omschrijving_kosten_1")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingKosten2)
                .HasColumnName("omschrijving_kosten_2")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingKosten3)
                .HasColumnName("omschrijving_kosten_3")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingKosten4)
                .HasColumnName("omschrijving_kosten_4")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingKosten5)
                .HasColumnName("omschrijving_kosten_5")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingToeslag1)
                .HasColumnName("omschrijving_toeslag_1")
                .HasMaxLength(40);

            builder.Property(e => e.OmschrijvingToeslag2)
                .HasColumnName("omschrijving_toeslag_2")
                .HasMaxLength(40);

            builder.Property(e => e.OpTekeningVerwerkt)
                .IsRequired()
                .HasColumnName("op_tekening_verwerkt")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OpmerkingenWebsite).HasColumnName("opmerkingen_website");

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

            builder.Property(e => e.OptienummerOmschrijving)
                .HasColumnName("optienummer_omschrijving")
                .HasMaxLength(98)
                .HasComputedColumnSql("(([optienummer]+' - ')+[omschrijving])");

            builder.Property(e => e.PercentageBtw)
                .HasColumnName("percentage_btw")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageKosten1)
                .HasColumnName("percentage_kosten_1")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten2)
                .HasColumnName("percentage_kosten_2")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten3)
                .HasColumnName("percentage_kosten_3")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten4)
                .HasColumnName("percentage_kosten_4")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten5)
                .HasColumnName("percentage_kosten_5")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageToeslag1)
                .HasColumnName("percentage_toeslag_1")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageToeslag2)
                .HasColumnName("percentage_toeslag_2")
                .HasColumnType("numeric(6, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.Resultaat)
                .HasColumnName("resultaat")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.ResultaatTotaal)
                .HasColumnName("resultaat_totaal")
                .HasColumnType("numeric(23, 4)")
                .HasComputedColumnSql("(coalesce([aantal],(1))*coalesce([resultaat],(0.00)))");

            builder.Property(e => e.Sluitingsdatum)
                .HasColumnName("sluitingsdatum")
                .HasColumnType("date");

            builder.Property(e => e.Soort)
                .HasColumnName("soort")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.SorteringRubriek)
                .HasColumnName("sortering_rubriek")
                .HasMaxLength(60)
                .HasComputedColumnSql("([dbo].[cff_proxy_optie_gekozen_sortering_rubriek]([optie_rubriek_werk_guid]))");

            builder.Property(e => e.TechnischeOmschrijving).HasColumnName("technische_omschrijving");

            builder.Property(e => e.TechnischeOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("technische_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.TekstGelimiteerdeGarantieGuid)
                .HasColumnName("tekst_gelimiteerde_garantie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.TekstPrijskolomGuid)
                .HasColumnName("tekst_prijskolom_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ToelichtingGelimiteerdeGarantie)
                .HasColumnName("toelichting_gelimiteerde_garantie")
                .HasMaxLength(4000);

            builder.Property(e => e.TotaalBedragKosten)
                .HasColumnName("totaal_bedrag_kosten")
                .HasColumnType("numeric(16, 2)")
                .HasComputedColumnSql("((((coalesce([bedrag_kosten_1],(0.00))+coalesce([bedrag_kosten_2],(0.00)))+coalesce([bedrag_kosten_3],(0.00)))+coalesce([bedrag_kosten_4],(0.00)))+coalesce([bedrag_kosten_5],(0.00)))");

            builder.Property(e => e.TotaalBedragToeslagen)
                .HasColumnName("totaal_bedrag_toeslagen")
                .HasColumnType("numeric(13, 2)")
                .HasComputedColumnSql("(coalesce([bedrag_toeslag_1],(0.00))+coalesce([bedrag_toeslag_2],(0.00)))");

            builder.Property(e => e.VerkoopbedragExclBtw)
                .HasColumnName("verkoopbedrag_excl_btw")
                .HasColumnType("numeric(23, 4)")
                .HasComputedColumnSql("(coalesce([aantal],(1))*coalesce([verkoopprijs_excl_btw],(0.00)))");

            builder.Property(e => e.VerkoopbedragExclBtwOfTekstPrijskolom)
                .HasColumnName("verkoopbedrag_excl_btw_of_tekst_prijskolom")
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[cff_proxy_optie_gekozen_verkoopbedrag_excl_btw_of_tekst_prijskolom]([aantal],[verkoopprijs_ntb],[tekst_prijskolom_guid],[verkoopprijs_excl_btw]))");

            builder.Property(e => e.VerkoopbedragInclBtw)
                .HasColumnName("verkoopbedrag_incl_btw")
                .HasColumnType("numeric(23, 4)")
                .HasComputedColumnSql("(coalesce([aantal],(1))*coalesce([verkoopprijs_incl_btw],(0.00)))");

            builder.Property(e => e.VerkoopbedragInclBtwOfTekstPrijskolom)
                .HasColumnName("verkoopbedrag_incl_btw_of_tekst_prijskolom")
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[cff_proxy_optie_gekozen_verkoopbedrag_incl_btw_of_tekst_prijskolom]([aantal],[verkoopprijs_ntb],[tekst_prijskolom_guid],[verkoopprijs_incl_btw]))");

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

            builder.Property(e => e.VerkoopprijsIsStelpost)
                .IsRequired()
                .HasColumnName("verkoopprijs_is_stelpost")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.VerkoopprijsNtb)
                .IsRequired()
                .HasColumnName("verkoopprijs_ntb")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.ViaWebsite)
                .IsRequired()
                .HasColumnName("via_website")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.EenheidGu)
                .WithMany(p => p.OptieGekozen)
                .HasForeignKey(d => d.EenheidGuid)
                .HasConstraintName("ref_eenheid_optie_gekozen");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.OptieGekozen)
                .HasForeignKey(d => d.GebouwGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_gebouw_optie_gekozen_integriteit");

            //builder.HasOne(d => d.OptieCategorieWerkGu)
            //    .WithMany(p => p.OptieGekozen)
            //    .HasForeignKey(d => d.OptieCategorieWerkGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_optie_categorie_werk_optie_gekozen_integriteit");

            builder.HasOne(d => d.OptieGekozenOfferteGu)
                .WithMany(p => p.OptieGekozen)
                .HasForeignKey(d => d.OptieGekozenOfferteGuid)
                .HasConstraintName("ref_optie_gekozen_offerte_optie_gekozen");

            //builder.HasOne(d => d.OptieRubriekWerkGu)
            //    .WithMany(p => p.OptieGekozen)
            //    .HasForeignKey(d => d.OptieRubriekWerkGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_optie_rubriek_werk_optie_gekozen_integriteit");

            builder.HasOne(d => d.OptieStandaardGu)
                .WithMany(p => p.OptieGekozens)
                .HasForeignKey(d => d.OptieStandaardGuid)
                .HasConstraintName("ref_optie_standaard_optie_gekozen_integriteit");

            //builder.HasOne(d => d.TekstGelimiteerdeGarantieGu)
            //    .WithMany(p => p.OptieGekozen)
            //    .HasForeignKey(d => d.TekstGelimiteerdeGarantieGuid)
            //    .HasConstraintName("ref_tekst_gelimiteerde_garantie_optie_gekozen");

            //builder.HasOne(d => d.TekstPrijskolomGu)
            //    .WithMany(p => p.OptieGekozen)
            //    .HasForeignKey(d => d.TekstPrijskolomGuid)
            //    .HasConstraintName("ref_tekst_prijskolom_optie_gekozen");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.OptieGekozen)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_optie_gekozen");
        }
    }
}
