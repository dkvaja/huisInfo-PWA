using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class WerkConfiguration : IEntityTypeConfiguration<Werk>
    {
        public void Configure(EntityTypeBuilder<Werk> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("werk_pk");

            builder.ToTable("werk");

            builder.HasIndex(e => e.GarantieregelingGuid)
                .HasName("fk_ref_garantieregeling_werk");

            builder.HasIndex(e => e.HoofdaannemerOrganisatieGuid)
                .HasName("fk_ref_organisatie_werk");

            builder.HasIndex(e => e.MeldingVerantwoordelijkeManagementRelatieGuid)
                .HasName("fk_ref_relatie_werk_verantwoordelijke_management");

            builder.HasIndex(e => e.MeldingVerantwoordelijkeUitvoeringRelatieGuid)
                .HasName("fk_ref_relatie_werk_verantwoordelijke_uitvoering");

            builder.HasIndex(e => e.OnderhoudstermijnGuid)
                .HasName("fk_ref_onderhoudstermijn_werk");

            builder.HasIndex(e => e.WerkFaseGuid)
                .HasName("fk_ref_werk_fase_werk");

            builder.HasIndex(e => new { e.Werknummer, e.Werknaam })
                .HasName("so_werk");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AantalDagenOptellenOfferteDatum).HasColumnName("aantal_dagen_optellen_offerte_datum");

            builder.Property(e => e.AantalObjecten).HasColumnName("aantal_objecten");

            builder.Property(e => e.AbsoluteWaardeKosten1)
                .IsRequired()
                .HasColumnName("absolute_waarde_kosten_1")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeKosten2)
                .IsRequired()
                .HasColumnName("absolute_waarde_kosten_2")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeKosten3)
                .IsRequired()
                .HasColumnName("absolute_waarde_kosten_3")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeKosten4)
                .IsRequired()
                .HasColumnName("absolute_waarde_kosten_4")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeKosten5)
                .IsRequired()
                .HasColumnName("absolute_waarde_kosten_5")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeToeslag1)
                .IsRequired()
                .HasColumnName("absolute_waarde_toeslag_1")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AbsoluteWaardeToeslag2)
                .IsRequired()
                .HasColumnName("absolute_waarde_toeslag_2")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AchtergrondWebPortal)
                .HasColumnName("achtergrond_web_portal")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.AlgemeneInfo).HasColumnName("algemene_info");

            builder.Property(e => e.BedragenAfrondenPerOptie)
                .IsRequired()
                .HasColumnName("bedragen_afronden_per_optie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.BepalenSluitingsdatum)
                .HasColumnName("bepalen_sluitingsdatum")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.BerekeningswijzeVerkoopprijs)
                .HasColumnName("berekeningswijze_verkoopprijs")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.DagenOptellenOfferteDatum)
                .IsRequired()
                .HasColumnName("dagen_optellen_offerte_datum")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.DatumEindBouw)
                .HasColumnName("datum_eind_bouw")
                .HasColumnType("date");

            builder.Property(e => e.DatumMeterkastlijstIsDatumOpdrachtbevestiging)
                .IsRequired()
                .HasColumnName("datum_meterkastlijst_is_datum_opdrachtbevestiging")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.DatumMeterkastlijstVragen)
                .IsRequired()
                .HasColumnName("datum_meterkastlijst_vragen")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.DatumStartBouw)
                .HasColumnName("datum_start_bouw")
                .HasColumnType("date");

            builder.Property(e => e.DatumStartVerkoop)
                .HasColumnName("datum_start_verkoop")
                .HasColumnType("date");

            builder.Property(e => e.Fax)
                .HasColumnName("fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.GarantieregelingGuid)
                .HasColumnName("garantieregeling_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Geblokkeerd)
                .IsRequired()
                .HasColumnName("geblokkeerd")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GebruikJpbmWebPortal).HasColumnName("gebruik_jpbm_web_portal");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.GrondslagBedragKosten2IsKaleKostprijsPlusBedragKosten1)
                .IsRequired()
                .HasColumnName("grondslag_bedrag_kosten_2_is_kale_kostprijs_plus_bedrag_kosten_1")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.HoofdaannemerOrganisatieGuid)
                .HasColumnName("hoofdaannemer_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.InkoopprijzenUitInkoopprijslijst)
                .IsRequired()
                .HasColumnName("inkoopprijzen_uit_inkoopprijslijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.InvullenVerkoopprijsInclBtw)
                .IsRequired()
                .HasColumnName("invullen_verkoopprijs_incl_btw")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.KaleKostprijsAutomatischBerekenen)
                .IsRequired()
                .HasColumnName("kale_kostprijs_automatisch_berekenen")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.KoptekstFactuurOpdracht)
                .HasColumnName("koptekst_factuur_opdracht")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstFactuurOplevering)
                .HasColumnName("koptekst_factuur_oplevering")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstKeuzelijst)
                .HasColumnName("koptekst_keuzelijst")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstOfferte)
                .HasColumnName("koptekst_offerte")
                .HasMaxLength(4000);

            builder.Property(e => e.KoptekstOpdrachtbevestiging)
                .HasColumnName("koptekst_opdrachtbevestiging")
                .HasMaxLength(4000);

            builder.Property(e => e.Logo)
                .HasColumnName("logo")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.LogoWebPortal)
                .HasColumnName("logo_web_portal")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.MapBijlagen)
                .HasColumnName("map_bijlagen")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.MeldingVerantwoordelijkeManagementRelatieGuid)
                .HasColumnName("melding_verantwoordelijke_management_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MeldingVerantwoordelijkeUitvoeringRelatieGuid)
                .HasColumnName("melding_verantwoordelijke_uitvoering_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            //builder.Property(e => e.Mutatieoverzicht)
            //    .IsRequired()
            //    .HasColumnName("mutatieoverzicht")
            //    .HasDefaultValueSql("('0')");Delete this not in new database

            builder.Property(e => e.Notities).HasColumnName("notities");

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

            builder.Property(e => e.OmschrijvingenWijzigbaarPerOptie)
                .IsRequired()
                .HasColumnName("omschrijvingen_wijzigbaar_per_optie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OnderhoudstermijnGuid)
                .HasColumnName("onderhoudstermijn_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieIndividueelCommercieleOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("optie_individueel_commerciele_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OptieIndividueelTechnischeOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("optie_individueel_technische_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OptieStandaardCommercieleOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("optie_standaard_commerciele_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OptieStandaardTechnischeOmschrijvingOpMeterkastlijst)
                .IsRequired()
                .HasColumnName("optie_standaard_technische_omschrijving_op_meterkastlijst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OptieStatusGekozenStandaardOptie)
                .HasColumnName("optie_status_gekozen_standaard_optie")
                .HasDefaultValueSql("('3')");

            builder.Property(e => e.PercentageKosten1)
                .HasColumnName("percentage_kosten_1")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageKosten2)
                .HasColumnName("percentage_kosten_2")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageKosten3)
                .HasColumnName("percentage_kosten_3")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageKosten4)
                .HasColumnName("percentage_kosten_4")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageKosten5)
                .HasColumnName("percentage_kosten_5")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageToeslag1)
                .HasColumnName("percentage_toeslag_1")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentageToeslag2)
                .HasColumnName("percentage_toeslag_2")
                .HasColumnType("numeric(6, 2)");

            builder.Property(e => e.PercentagesWijzigbaarPerOptie)
                .IsRequired()
                .HasColumnName("percentages_wijzigbaar_per_optie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Plaats)
                .HasColumnName("plaats")
                .HasMaxLength(40);

            builder.Property(e => e.Planregistratienummer)
                .HasColumnName("planregistratienummer")
                .HasMaxLength(20);

            builder.Property(e => e.SluitingsdatumVragen)
                .IsRequired()
                .HasColumnName("sluitingsdatum_vragen")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.SubmapPerGebouw)
                .IsRequired()
                .HasColumnName("submap_per_gebouw")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Telefoon)
                .HasColumnName("telefoon")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.VerkoopprijsExclBtwIsGrondslagToeslag2)
                .IsRequired()
                .HasColumnName("verkoopprijs_excl_btw_is_grondslag_toeslag_2")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.VoettekstFactuurOpdracht)
                .HasColumnName("voettekst_factuur_opdracht")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstFactuurOplevering)
                .HasColumnName("voettekst_factuur_oplevering")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstKeuzelijst)
                .HasColumnName("voettekst_keuzelijst")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstOfferte)
                .HasColumnName("voettekst_offerte")
                .HasMaxLength(4000);

            builder.Property(e => e.VoettekstOpdrachtbevestiging)
                .HasColumnName("voettekst_opdrachtbevestiging")
                .HasMaxLength(4000);

            builder.Property(e => e.WebPortal)
                .HasColumnName("web_portal")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Website)
                .HasColumnName("website")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.WerkFaseGuid)
                .IsRequired()
                .HasColumnName("werk_fase_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkSoort).HasColumnName("werk_soort");

            builder.Property(e => e.WerkType)
                .HasColumnName("werk_type")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Werknaam)
                .IsRequired()
                .HasColumnName("werknaam")
                .HasMaxLength(100);

            builder.Property(e => e.Werknummer)
                .IsRequired()
                .HasColumnName("werknummer")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.WerknummerWerknaam)
                .IsRequired()
                .HasColumnName("werknummer_werknaam")
                .HasMaxLength(113)
                .HasComputedColumnSql("([werknummer]+isnull(' - '+[werknaam],''))");

            //builder.Property(e => e.WitruimteVoorblad)
            //    .HasColumnName("witruimte_voorblad")
            //    .HasMaxLength(4000);Delete this not in new database

            //builder.HasOne(d => d.GarantieregelingGu)
            //    .WithMany(p => p.Werk)
            //    .HasForeignKey(d => d.GarantieregelingGuid)
            //    .HasConstraintName("ref_garantieregeling_werk");

            builder.HasOne(d => d.HoofdaannemerOrganisatieGu)
                .WithMany(p => p.Werk)
                .HasForeignKey(d => d.HoofdaannemerOrganisatieGuid)
                .HasConstraintName("ref_organisatie_werk");

            builder.HasOne(d => d.MeldingVerantwoordelijkeManagementRelatieGu)
                .WithMany(p => p.WerkMeldingVerantwoordelijkeManagementRelatieGu)
                .HasForeignKey(d => d.MeldingVerantwoordelijkeManagementRelatieGuid)
                .HasConstraintName("ref_relatie_werk_verantwoordelijke_management");

            builder.HasOne(d => d.MeldingVerantwoordelijkeUitvoeringRelatieGu)
                .WithMany(p => p.WerkMeldingVerantwoordelijkeUitvoeringRelatieGu)
                .HasForeignKey(d => d.MeldingVerantwoordelijkeUitvoeringRelatieGuid)
                .HasConstraintName("ref_relatie_werk_verantwoordelijke_uitvoering");

            //builder.HasOne(d => d.OnderhoudstermijnGu)
            //    .WithMany(p => p.Werk)
            //    .HasForeignKey(d => d.OnderhoudstermijnGuid)
            //    .HasConstraintName("ref_onderhoudstermijn_werk");

            builder.HasOne(d => d.WerkFaseGu)
                .WithMany(p => p.Werk)
                .HasForeignKey(d => d.WerkFaseGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_fase_werk");
        }
    }
}
