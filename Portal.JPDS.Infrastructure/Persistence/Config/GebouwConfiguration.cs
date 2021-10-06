using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class GebouwConfiguration : IEntityTypeConfiguration<Gebouw>
    {
        public void Configure(EntityTypeBuilder<Gebouw> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("gebouw_pk");

            builder.ToTable("gebouw");

            builder.HasIndex(e => e.AdresGuid)
                .HasName("fk_ref_adres_gebouw");

            builder.HasIndex(e => e.BouwnummerIntern)
                .HasName("unique_bouwnummer_intern")
                .IsUnique()
                .HasFilter("([bouwnummer_intern] IS NOT NULL)");

            builder.HasIndex(e => e.BouwstroomGuid)
                .HasName("fk_ref_bouwstroom_gebouw_integriteit");

            builder.HasIndex(e => e.GarantieregelingGuid)
                .HasName("fk_ref_garantieregeling_gebouw");

            builder.HasIndex(e => e.GebouwGebruikerOrganisatieGuid)
                .HasName("fk_ref_organisatie_gebouw_gebruiker");

            builder.HasIndex(e => e.GebouwSoortGuid)
                .HasName("fk_ref_gebouw_soort_gebouw");

            builder.HasIndex(e => e.GebouwStatusGuid)
                .HasName("fk_ref_gebouw_status_gebouw");

            builder.HasIndex(e => e.KoperHuurderGuid)
                .HasName("fk_ref_koper_huurder_gebouw");

            builder.HasIndex(e => e.WoningSoortGuid)
                .HasName("fk_ref_woning_soort_gebouw");

            builder.HasIndex(e => e.WoningTypeGuid)
                .HasName("fk_ref_woning_type_gebouw_integriteit");

            builder.HasIndex(e => new { e.WerkGuid, e.BouwnummerIntern })
                .HasName("so_gebouw");

            builder.HasIndex(e => new { e.WerkGuid, e.BouwstroomGuid })
                .HasName("fk_ref_bouwstroom_gebouw_lookup");

            builder.HasIndex(e => new { e.WerkGuid, e.WoningTypeGuid })
                .HasName("fk_ref_woning_type_gebouw_lookup");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AanneemsomBtwTarief)
                .HasColumnName("aanneemsom_btw_tarief")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.AanneemsomExclBtw)
                .HasColumnName("aanneemsom_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanneemsomInclBtw)
                .HasColumnName("aanneemsom_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenAanneemsomExclBtw)
                .HasColumnName("aanvullingen_aanneemsom_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenAanneemsomInclBtw)
                .HasColumnName("aanvullingen_aanneemsom_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenGrondkostenExclBtw)
                .HasColumnName("aanvullingen_grondkosten_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenGrondkostenInclBtw)
                .HasColumnName("aanvullingen_grondkosten_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenVerkoopkostenExclBtw)
                .HasColumnName("aanvullingen_verkoopkosten_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AanvullingenVerkoopkostenInclBtw)
                .HasColumnName("aanvullingen_verkoopkosten_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.AdresBatchGuid)
                .HasColumnName("adres_batch_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AdresGuid)
                .HasColumnName("adres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AdresaanhefGeenNaam)
                .HasColumnName("adresaanhef_geen_naam")
                .HasMaxLength(60);

            builder.Property(e => e.Adresblok)
                .HasColumnName("adresblok")
                .HasMaxLength(800);

            builder.Property(e => e.BouwnummerExtern)
                .HasColumnName("bouwnummer_extern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwnummerIntern)
                .HasColumnName("bouwnummer_intern")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.BouwstroomGuid)
                .HasColumnName("bouwstroom_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.CertificaatGeldigTm)
                .HasColumnName("certificaat_geldig_tm")
                .HasColumnType("date");

            builder.Property(e => e.CertificaatGeldigVa)
                .HasColumnName("certificaat_geldig_va")
                .HasColumnType("date");

            builder.Property(e => e.CertificaatNummer)
                .HasColumnName("certificaat_nummer")
                .HasMaxLength(40);

            builder.Property(e => e.DatumEindeOnderhoudstermijn)
                .HasColumnName("datum_einde_onderhoudstermijn")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteFactuur)
                .HasColumnName("datum_laatste_factuur")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteFinancieelOverzicht)
                .HasColumnName("datum_laatste_financieel_overzicht")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteMeterkastlijst)
                .HasColumnName("datum_laatste_meterkastlijst")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteOfferte)
                .HasColumnName("datum_laatste_offerte")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteOfferteSluitingsdatum)
                .HasColumnName("datum_laatste_offerte_sluitingsdatum")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteOpdrachtbevestiging)
                .HasColumnName("datum_laatste_opdrachtbevestiging")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteOptieDefintief)
                .HasColumnName("datum_laatste_optie_defintief")
                .HasColumnType("date");

            builder.Property(e => e.DatumLaatsteTekening)
                .HasColumnName("datum_laatste_tekening")
                .HasColumnType("date");

            builder.Property(e => e.DatumOplevering)
                .HasColumnName("datum_oplevering")
                .HasColumnType("date");

            builder.Property(e => e.DatumStartBouw)
                .HasColumnName("datum_start_bouw")
                .HasColumnType("date");

            builder.Property(e => e.DatumVoorschouw)
                .HasColumnName("datum_voorschouw")
                .HasColumnType("date");

            builder.Property(e => e.EanElektriciteit)
                .HasColumnName("ean_elektriciteit")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.EanGas)
                .HasColumnName("ean_gas")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GarantieregelingGuid)
                .HasColumnName("garantieregeling_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGebruikerNaam)
                .HasColumnName("gebouw_gebruiker_naam")
                .HasMaxLength(100)
                .HasComputedColumnSql("([dbo].[cff_proxy_gebouw_gebouw_gebruiker_naam]([guid]))");

            builder.Property(e => e.GebouwGebruikerOrganisatieGuid)
                .HasColumnName("gebouw_gebruiker_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwSoortGuid)
                .HasColumnName("gebouw_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwStaatLeeg)
                .IsRequired()
                .HasColumnName("gebouw_staat_leeg")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GebouwStatusGuid)
                .HasColumnName("gebouw_status_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.GrondprijsBtwTarief)
                .HasColumnName("grondprijs_btw_tarief")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.GrondprijsExclBtw)
                .HasColumnName("grondprijs_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.GrondprijsInclBtw)
                .HasColumnName("grondprijs_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.GrondprijsPerM2BtwTarief)
                .HasColumnName("grondprijs_per_m2_btw_tarief")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.GrondprijsPerM2ExclBtw)
                .HasColumnName("grondprijs_per_m2_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.GrondprijsPerM2InclBtw)
                .HasColumnName("grondprijs_per_m2_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.KavelBreedte)
                .HasColumnName("kavel_breedte")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.KavelDiepte)
                .HasColumnName("kavel_diepte")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.KoperHuurderGuid)
                .HasColumnName("koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.KoptekstKeuzelijst)
                .HasColumnName("koptekst_keuzelijst")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstOfferte)
                .HasColumnName("koptekst_offerte")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstOpdrachtbevestiging)
                .HasColumnName("koptekst_opdrachtbevestiging")
                .HasMaxLength(4000);

            builder.Property(e => e.MinderprijsCasco)
                .HasColumnName("minderprijs_casco")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100);

            builder.Property(e => e.NotitiesExtern).HasColumnName("notities_extern");

            builder.Property(e => e.NotitiesIntern).HasColumnName("notities_intern");

            builder.Property(e => e.TijdOpleveringTm).HasColumnName("tijd_oplevering_tm");

            builder.Property(e => e.TijdOpleveringVa).HasColumnName("tijd_oplevering_va");

            builder.Property(e => e.TijdVoorschouwTm).HasColumnName("tijd_voorschouw_tm");

            builder.Property(e => e.TijdVoorschouwVa).HasColumnName("tijd_voorschouw_va");

            builder.Property(e => e.TotaalInkoopKoperswijzigingenExclBtw)
                .HasColumnName("totaal_inkoop_koperswijzigingen_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasComputedColumnSql("([dbo].[cff_proxy_gebouw_totaal_inkoop_koperswijzigingen_excl_btw]([guid]))");

            builder.Property(e => e.TotaalVerkoopKoperswijzigingenExclBtw)
                .HasColumnName("totaal_verkoop_koperswijzigingen_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasComputedColumnSql("([dbo].[cff_proxy_gebouw_totaal_verkoop_koperswijzigingen_excl_btw]([guid]))");

            builder.Property(e => e.TotaalVerkoopKoperswijzigingenInclBtw)
                .HasColumnName("totaal_verkoop_koperswijzigingen_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasComputedColumnSql("([dbo].[cff_proxy_gebouw_totaal_verkoop_koperswijzigingen_incl_btw]([guid]))");

            builder.Property(e => e.TotaalVerrekeningAannemerExclBtw)
                .HasColumnName("totaal_verrekening_aannemer_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.TotaalVerrekeningAannemerInclBtw)
                .HasColumnName("totaal_verrekening_aannemer_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.VariabeleTekst)
                .HasColumnName("variabele_tekst")
                .HasMaxLength(250);

            builder.Property(e => e.VoettekstKeuzelijst)
                .HasColumnName("voettekst_keuzelijst")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstOfferte)
                .HasColumnName("voettekst_offerte")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstOpdrachtbevestiging)
                .HasColumnName("voettekst_opdrachtbevestiging")
                .HasMaxLength(4000);

            builder.Property(e => e.VrijOpNaamPrijsBtwTarief)
                .HasColumnName("vrij_op_naam_prijs_btw_tarief")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.VrijOpNaamPrijsExclBtw)
                .HasColumnName("vrij_op_naam_prijs_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.VrijOpNaamPrijsInclBtw)
                .HasColumnName("vrij_op_naam_prijs_incl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasDefaultValueSql("('0.00')");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WoningSituatie)
                .HasColumnName("woning_situatie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.WoningSoortGuid)
                .HasColumnName("woning_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WoningTypeGuid)
                .HasColumnName("woning_type_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.AdresGu)
                .WithMany(p => p.Gebouw)
                .HasForeignKey(d => d.AdresGuid)
                .HasConstraintName("ref_adres_gebouw");

            builder.HasOne(d => d.BouwstroomGu)
                .WithMany(p => p.Gebouw)
                .HasForeignKey(d => d.BouwstroomGuid)
                .HasConstraintName("ref_bouwstroom_gebouw_integriteit");

            //builder.HasOne(d => d.GarantieregelingGu)
            //    .WithMany(p => p.Gebouw)
            //    .HasForeignKey(d => d.GarantieregelingGuid)
            //    .HasConstraintName("ref_garantieregeling_gebouw");

            builder.HasOne(d => d.GebouwGebruikerOrganisatieGu)
                .WithMany(p => p.Gebouw)
                .HasForeignKey(d => d.GebouwGebruikerOrganisatieGuid)
                .HasConstraintName("ref_organisatie_gebouw_gebruiker");

            builder.HasOne(d => d.GebouwSoortGu)
                .WithMany(p => p.Gebouws)
                .HasForeignKey(d => d.GebouwSoortGuid)
                .HasConstraintName("ref_gebouw_soort_gebouw");

            builder.HasOne(d => d.GebouwStatusGu)
                .WithMany(p => p.Gebouws)
                .HasForeignKey(d => d.GebouwStatusGuid)
                .HasConstraintName("ref_gebouw_status_gebouw");

            builder.HasOne(d => d.KoperHuurderGu)
                .WithMany(p => p.Gebouw)
                .HasForeignKey(d => d.KoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_gebouw");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Gebouw)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_gebouw");

            //builder.HasOne(d => d.WoningSoortGu)
            //    .WithMany(p => p.Gebouw)
            //    .HasForeignKey(d => d.WoningSoortGuid)
            //    .HasConstraintName("ref_woning_soort_gebouw");

            builder.HasOne(d => d.WoningTypeGu)
                .WithMany(p => p.Gebouws)
                .HasForeignKey(d => d.WoningTypeGuid)
                .HasConstraintName("ref_woning_type_gebouw_integriteit");
        }
    }
}
