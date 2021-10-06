using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ConfiguratieWebPortalConfiguration : IEntityTypeConfiguration<ConfiguratieWebPortal>
    {
        public void Configure(EntityTypeBuilder<ConfiguratieWebPortal> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("configuratie_web_portal_pk");

            builder.ToTable("configuratie_web_portal");

            builder.HasIndex(e => e.AfzenderEmailAccountGuid)
                .HasName("fk_ref_email_account_configuratie_web_portal");

            builder.HasIndex(e => e.StandaardNotificatieEmailSjabloonGuid)
                .HasName("fk_ref_sjabloon_configuratie_web_portal");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AchtergrondLoginWebPortal)
                .HasColumnName("achtergrond_login_web_portal")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.AfzenderEmailAccountGuid)
                .HasColumnName("afzender_email_account_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DigitaalOndertekenenAccessSecret)
                .HasColumnName("digitaal_ondertekenen_access_secret")
                .HasMaxLength(40);

            builder.Property(e => e.DigitaalOndertekenenAccessToken)
                .HasColumnName("digitaal_ondertekenen_access_token")
                .HasMaxLength(40);

            builder.Property(e => e.DigitaalOndertekenenApiSecret)
                .HasColumnName("digitaal_ondertekenen_api_secret")
                .HasMaxLength(40);

            builder.Property(e => e.DigitaalOndertekenenApiToken)
                .HasColumnName("digitaal_ondertekenen_api_token")
                .HasMaxLength(40);

            builder.Property(e => e.DigitaalOndertekenenApiUrl)
                .HasColumnName("digitaal_ondertekenen_api_url")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.DigitaalOndertekenenProvider).HasColumnName("digitaal_ondertekenen_provider");

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

            builder.Property(e => e.NaamTabBrowserExtern)
                .HasColumnName("naam_tab_browser_extern")
                .HasMaxLength(100);

            builder.Property(e => e.NaamTabBrowserIntern)
                .HasColumnName("naam_tab_browser_intern")
                .HasMaxLength(100);

            builder.Property(e => e.StandaardLogoWebPortal)
                .HasColumnName("standaard_logo_web_portal")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.StandaardNotificatieEmailSjabloonGuid)
                .HasColumnName("standaard_notificatie_email_sjabloon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardInloggegevensSjabloonGuid)
                    .HasColumnName("standaard_inloggegevens_sjabloon_guid")
                    .HasMaxLength(40)
                    .IsUnicode(false);

            builder.Property(e => e.StandaardPvoSjabloonGuid)
                    .HasColumnName("standaard_pvo_sjabloon_guid")
                    .HasMaxLength(40)
                    .IsUnicode(false);

            builder.Property(e => e.StandaardResetWachtwoordSjabloonGuid)
                    .HasColumnName("standaard_reset_wachtwoord_sjabloon_guid")
                    .HasMaxLength(40)
                    .IsUnicode(false);

            builder.Property(e => e.StandaardTekstAanvragenOpties).HasColumnName("standaard_tekst_aanvragen_opties");

            builder.Property(e => e.StandaardTekstBedanktVoorAanvraag).HasColumnName("standaard_tekst_bedankt_voor_aanvraag");

            builder.Property(e => e.StandaardTekstBedanktVoorBestelling).HasColumnName("standaard_tekst_bedankt_voor_bestelling");

            builder.Property(e => e.StandaardTekstBedanktVoorBestellingDigitaalOndertekenen).HasColumnName("standaard_tekst_bedankt_voor_bestelling_digitaal_ondertekenen");

            builder.Property(e => e.StandaardTekstBestellenOpties).HasColumnName("standaard_tekst_bestellen_opties");

            builder.Property(e => e.StandaardTekstBestellenOptiesDigitaalOndertekenen).HasColumnName("standaard_tekst_bestellen_opties_digitaal_ondertekenen");

            builder.Property(e => e.StandaardTekstBestellenOptiesDigitaalOndertekenenIdin).HasColumnName("standaard_tekst_bestellen_opties_digitaal_ondertekenen_idin");

            builder.Property(e => e.StandaardTekstNieuweOfferte)
                .HasColumnName("standaard_tekst_nieuwe_offerte")
                .HasMaxLength(4000);

            builder.Property(e => e.AfgehandeldKoperHuurderMeldingStatusGuid)
                .HasColumnName("afgehandeld_koper_huurder_melding_status_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AfgehandeldOplosserMeldingStatusGuid)
                .HasColumnName("afgehandeld_oplosser_melding_status_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardOfflineMode)
                .IsRequired()
                .HasColumnName("standaard_offline_mode")
                .HasDefaultValueSql("('1')");
            builder.Property(e => e.StandaardOfflineInspectieOpslaanAantalDagen).HasColumnName("standaard_offline_inspectie_opslaan_aantal_dagen");
            builder.Property(e => e.StandaardOfflineOpleveringOpslaanAantalDagen).HasColumnName("standaard_offline_oplevering_opslaan_aantal_dagen");
            builder.Property(e => e.StandaardOfflineTweedeHandtekeningOpslaanAantalDagen).HasColumnName("standaard_offline_tweede_handtekening_opslaan_aantal_dagen");
            builder.Property(e => e.StandaardOfflineVoorschouwOpslaanAantalDagen).HasColumnName("standaard_offline_voorschouw_opslaan_aantal_dagen");

            //builder.HasOne(d => d.AfzenderEmailAccountGu)
            //    .WithMany(p => p.ConfiguratieWebPortal)
            //    .HasForeignKey(d => d.AfzenderEmailAccountGuid)
            //    .OnDelete(DeleteBehavior.SetNull)
            //    .HasConstraintName("ref_email_account_configuratie_web_portal");

            builder.HasOne(d => d.StandaardNotificatieEmailSjabloonGu)
                .WithMany(p => p.ConfiguratieWebPortal)
                .HasForeignKey(d => d.StandaardNotificatieEmailSjabloonGuid)
                .HasConstraintName("ref_sjabloon_configuratie_web_portal");
        }
    }
}
