using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewInputKoperHuurderConfiguration : IEntityTypeConfiguration<ViewInputKoperHuurder>
    {
        public void Configure(EntityTypeBuilder<ViewInputKoperHuurder> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_input_koper_huurder");

            builder.Property(e => e.Adresaanhef).HasColumnName("adresaanhef");

            builder.Property(e => e.Adresblok)
                .HasColumnName("adresblok")
                .HasMaxLength(800);

            builder.Property(e => e.BankGuid)
                .HasColumnName("bank_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Betaalwijze).HasColumnName("betaalwijze");

            builder.Property(e => e.Betalingstermijn).HasColumnName("betalingstermijn");

            builder.Property(e => e.Bic)
                .HasColumnName("bic")
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.BriefaanhefFormeel)
                .HasColumnName("briefaanhef_formeel")
                .HasMaxLength(200);

            builder.Property(e => e.CorrespondentieadresGuid)
                .IsRequired()
                .HasColumnName("correspondentieadres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Debiteur).HasColumnName("debiteur");

            builder.Property(e => e.Debiteurnummer)
                .HasColumnName("debiteurnummer")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.EmailFactuur)
                .HasColumnName("email_factuur")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.EmailNaarAlleEmailadressen).HasColumnName("email_naar_alle_emailadressen");

            builder.Property(e => e.FactuurDigitaal).HasColumnName("factuur_digitaal");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Iban)
                .HasColumnName("iban")
                .HasMaxLength(34)
                .IsUnicode(false);

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.InputWaarschuwingNegeren).HasColumnName("input_waarschuwing_negeren");

            builder.Property(e => e.KoperHuurderStatus).HasColumnName("koper_huurder_status");

            builder.Property(e => e.NaamInTweeRegels).HasColumnName("naam_in_twee_regels");

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P1Achternaam)
                .HasColumnName("p1_achternaam")
                .HasMaxLength(40);

            builder.Property(e => e.P1BekendePersoon).HasColumnName("p1_bekende_persoon");

            builder.Property(e => e.P1EmailPrive)
                .HasColumnName("p1_email_prive")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.P1EmailWerk)
                .HasColumnName("p1_email_werk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.P1Fax)
                .HasColumnName("p1_fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P1Geslacht).HasColumnName("p1_geslacht");

            builder.Property(e => e.P1Guid)
                .HasColumnName("p1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P1Mobiel)
                .HasColumnName("p1_mobiel")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P1TelefoonPrive)
                .HasColumnName("p1_telefoon_prive")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P1TelefoonWerk)
                .HasColumnName("p1_telefoon_werk")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P1TitulatuurAchterGuid)
                .HasColumnName("p1_titulatuur_achter_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P1TitulatuurVoorGuid)
                .HasColumnName("p1_titulatuur_voor_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P1TussenvoegselGuid)
                .HasColumnName("p1_tussenvoegsel_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P1Voorletters)
                .HasColumnName("p1_voorletters")
                .HasMaxLength(15);

            builder.Property(e => e.P1Voornaam)
                .HasColumnName("p1_voornaam")
                .HasMaxLength(40);

            builder.Property(e => e.P2Achternaam)
                .HasColumnName("p2_achternaam")
                .HasMaxLength(40);

            builder.Property(e => e.P2BekendePersoon).HasColumnName("p2_bekende_persoon");

            builder.Property(e => e.P2EmailPrive)
                .HasColumnName("p2_email_prive")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.P2EmailWerk)
                .HasColumnName("p2_email_werk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.P2Fax)
                .HasColumnName("p2_fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P2Geslacht).HasColumnName("p2_geslacht");

            builder.Property(e => e.P2Guid)
                .HasColumnName("p2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P2Mobiel)
                .HasColumnName("p2_mobiel")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P2TelefoonPrive)
                .HasColumnName("p2_telefoon_prive")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P2TelefoonWerk)
                .HasColumnName("p2_telefoon_werk")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.P2TitulatuurAchterGuid)
                .HasColumnName("p2_titulatuur_achter_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P2TitulatuurVoorGuid)
                .HasColumnName("p2_titulatuur_voor_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P2TussenvoegselGuid)
                .HasColumnName("p2_tussenvoegsel_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.P2Voorletters)
                .HasColumnName("p2_voorletters")
                .HasMaxLength(15);

            builder.Property(e => e.P2Voornaam)
                .HasColumnName("p2_voornaam")
                .HasMaxLength(40);

            builder.Property(e => e.RelatieGuid)
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Soort).HasColumnName("soort");

            builder.Property(e => e.TaalGuid)
                .HasColumnName("taal_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ToestemmingVrijgevenGegevens).HasColumnName("toestemming_vrijgeven_gegevens");

            builder.Property(e => e.VolledigeNaam)
                .HasColumnName("volledige_naam")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.VolledigeNaamEenRegel)
                .HasColumnName("volledige_naam_een_regel")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.VoorkeurEmailadresVeld)
                .HasColumnName("voorkeur_emailadres_veld")
                .HasMaxLength(40);

            builder.Property(e => e.VoorkeurEmailadresVeldNieuw)
                .HasColumnName("voorkeur_emailadres_veld_nieuw")
                .HasMaxLength(40);

            builder.Property(e => e.Zoeknaam)
                .IsRequired()
                .HasColumnName("zoeknaam")
                .HasMaxLength(150);
        }
    }
}
