using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OptieStandaardConfiguration : IEntityTypeConfiguration<OptieStandaard>
    {
        public void Configure(EntityTypeBuilder<OptieStandaard> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("optie_standaard_pk");

            builder.ToTable("optie_standaard");

            builder.HasIndex(e => e.EenheidGuid, "fk_ref_eenheid_optie_standaard");

            builder.HasIndex(e => e.OptieCategorieWerkGuid, "fk_ref_optie_categorie_werk_optie_standaard_integriteit");

            builder.HasIndex(e => new { e.WerkGuid, e.OptieCategorieWerkGuid }, "fk_ref_optie_categorie_werk_optie_standaard_lookup");

            builder.HasIndex(e => e.OptieRubriekWerkGuid, "fk_ref_optie_rubriek_werk_optie_standaard_integriteit");

            builder.HasIndex(e => new { e.WerkGuid, e.OptieRubriekWerkGuid }, "fk_ref_optie_rubriek_werk_optie_standaard_lookup");

            builder.HasIndex(e => e.TekstGelimiteerdeGarantieGuid, "fk_ref_tekst_gelimiteerde_garantie_optie_standaard");

            builder.HasIndex(e => e.TekstPrijskolomGuid, "fk_ref_tekst_prijskolom_optie_standaard");

            builder.HasIndex(e => new { e.WerkGuid, e.Optienummer }, "so_optie_standaard");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Aantal)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("aantal")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Afbeelding)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("afbeelding");

            builder.Property(e => e.BedragBtw)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_btw")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKaleKostprijs)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kale_kostprijs")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten1)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kosten_1")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten2)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kosten_2")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten3)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kosten_3")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten4)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kosten_4")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragKosten5)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_kosten_5")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragToeslag1)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_toeslag_1")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.BedragToeslag2)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("bedrag_toeslag_2")
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

            builder.Property(e => e.EenheidGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("eenheid_guid");

            builder.Property(e => e.EigenVerkoopprijs)
                .HasColumnType("numeric(14, 2)")
                .HasColumnName("eigen_verkoopprijs")
                .HasComputedColumnSql("(coalesce([verkoopprijs_excl_btw],(0.00))-(coalesce([bedrag_toeslag_1],(0.00))+coalesce([bedrag_toeslag_2],(0.00))))", false);

            builder.Property(e => e.Factureren)
                .IsRequired()
                .HasColumnName("factureren")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Gecontroleerd)
                .IsRequired()
                .HasColumnName("gecontroleerd")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GeimporteerdUitCufXml)
                .IsRequired()
                .HasColumnName("geimporteerd_uit_cuf_xml")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GelimiteerdeGarantie)
                .IsRequired()
                .HasColumnName("gelimiteerde_garantie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GewijzigdDoor)
                .HasMaxLength(40)
                .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                .HasColumnType("datetime")
                .HasColumnName("gewijzigd_op");

            builder.Property(e => e.IngevoerdDoor)
                .HasMaxLength(40)
                .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                .HasColumnType("datetime")
                .HasColumnName("ingevoerd_op");

            builder.Property(e => e.InvulvakjeOpKeuzelijst)
                .IsRequired()
                .HasColumnName("invulvakje_op_keuzelijst")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Omschrijving)
                .HasMaxLength(80)
                .HasColumnName("omschrijving");

            builder.Property(e => e.OmschrijvingKosten1)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_kosten_1");

            builder.Property(e => e.OmschrijvingKosten2)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_kosten_2");

            builder.Property(e => e.OmschrijvingKosten3)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_kosten_3");

            builder.Property(e => e.OmschrijvingKosten4)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_kosten_4");

            builder.Property(e => e.OmschrijvingKosten5)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_kosten_5");

            builder.Property(e => e.OmschrijvingToeslag1)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_toeslag_1");

            builder.Property(e => e.OmschrijvingToeslag2)
                .HasMaxLength(40)
                .HasColumnName("omschrijving_toeslag_2");

            builder.Property(e => e.OptieCategorieWerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("optie_categorie_werk_guid");

            builder.Property(e => e.OptieRubriekWerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("optie_rubriek_werk_guid");

            builder.Property(e => e.Optienummer)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("optienummer");

            builder.Property(e => e.OptienummerOmschrijving)
                .HasMaxLength(98)
                .HasColumnName("optienummer_omschrijving")
                .HasComputedColumnSql("(([optienummer]+' - ')+[omschrijving])", false);

            builder.Property(e => e.PercentageBtw)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_btw");

            builder.Property(e => e.PercentageKosten1)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_kosten_1")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten2)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_kosten_2")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten3)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_kosten_3")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten4)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_kosten_4")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageKosten5)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_kosten_5")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageToeslag1)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_toeslag_1")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.PercentageToeslag2)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("percentage_toeslag_2")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.Publiceren)
                .IsRequired()
                .HasColumnName("publiceren")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Resultaat)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("resultaat")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.RubriekVragen)
                .IsRequired()
                .HasColumnName("rubriek_vragen")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.SorteringRubriek)
                .HasMaxLength(60)
                .HasColumnName("sortering_rubriek")
                .HasComputedColumnSql("([dbo].[cff_proxy_optie_standaard_sortering_rubriek]([optie_rubriek_werk_guid]))", false);

            builder.Property(e => e.TechnischeOmschrijving).HasColumnName("technische_omschrijving");

            builder.Property(e => e.TechnischeOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("technische_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.TekstGelimiteerdeGarantieGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("tekst_gelimiteerde_garantie_guid");

            builder.Property(e => e.TekstPrijskolomGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("tekst_prijskolom_guid");

            builder.Property(e => e.ToelichtingGelimiteerdeGarantie)
                .HasMaxLength(4000)
                .HasColumnName("toelichting_gelimiteerde_garantie");

            builder.Property(e => e.TotaalBedragKosten)
                .HasColumnType("numeric(16, 2)")
                .HasColumnName("totaal_bedrag_kosten")
                .HasComputedColumnSql("((((coalesce([bedrag_kosten_1],(0.00))+coalesce([bedrag_kosten_2],(0.00)))+coalesce([bedrag_kosten_3],(0.00)))+coalesce([bedrag_kosten_4],(0.00)))+coalesce([bedrag_kosten_5],(0.00)))", false);

            builder.Property(e => e.TotaalBedragToeslagen)
                .HasColumnType("numeric(13, 2)")
                .HasColumnName("totaal_bedrag_toeslagen")
                .HasComputedColumnSql("(coalesce([bedrag_toeslag_1],(0.00))+coalesce([bedrag_toeslag_2],(0.00)))", false);

            builder.Property(e => e.VerkoopprijsExclBtw)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("verkoopprijs_excl_btw");

            builder.Property(e => e.VerkoopprijsExclBtwOfTekstPrijskolom)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("verkoopprijs_excl_btw_of_tekst_prijskolom");

            builder.Property(e => e.VerkoopprijsInclBtw)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("verkoopprijs_incl_btw");

            builder.Property(e => e.VerkoopprijsInclBtwOfTekstPrijskolom)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("verkoopprijs_incl_btw_of_tekst_prijskolom");

            builder.Property(e => e.VerkoopprijsIsStelpost)
                .IsRequired()
                .HasColumnName("verkoopprijs_is_stelpost")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.VerkoopprijsNtb)
                .IsRequired()
                .HasColumnName("verkoopprijs_ntb")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.HasOne(d => d.EenheidGu)
                .WithMany(p => p.OptieStandaards)
                .HasForeignKey(d => d.EenheidGuid)
                .HasConstraintName("ref_eenheid_optie_standaard");

            //builder.HasOne(d => d.OptieCategorieWerkGu)
            //    .WithMany(p => p.OptieStandaards)
            //    .HasForeignKey(d => d.OptieCategorieWerkGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_optie_categorie_werk_optie_standaard_integriteit");

            builder.HasOne(d => d.OptieRubriekWerkGu)
                .WithMany(p => p.OptieStandaards)
                .HasForeignKey(d => d.OptieRubriekWerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_optie_rubriek_werk_optie_standaard_integriteit");

            //builder.HasOne(d => d.TekstGelimiteerdeGarantieGu)
            //    .WithMany(p => p.OptieStandaards)
            //    .HasForeignKey(d => d.TekstGelimiteerdeGarantieGuid)
            //    .HasConstraintName("ref_tekst_gelimiteerde_garantie_optie_standaard");

            //builder.HasOne(d => d.TekstPrijskolomGu)
            //    .WithMany(p => p.OptieStandaards)
            //    .HasForeignKey(d => d.TekstPrijskolomGuid)
            //    .HasConstraintName("ref_tekst_prijskolom_optie_standaard");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.OptieStandaards)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_optie_standaard");
        }
    }
}
