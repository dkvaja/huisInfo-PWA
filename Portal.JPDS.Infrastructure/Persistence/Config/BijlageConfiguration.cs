using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BijlageConfiguration : IEntityTypeConfiguration<Bijlage>
    {
        public void Configure(EntityTypeBuilder<Bijlage> builder)
        {
            builder.HasKey(e => e.Guid);

            builder.ToTable("bijlage");

            builder.HasIndex(e => e.BijlageRubriekGuid)
                .HasName("fk_ref_bijlage_rubriek_bijlage");

            builder.HasIndex(e => e.ChatBerichtGuid)
                .HasName("fk_ref_chat_bericht_bijlage");

            builder.HasIndex(e => e.CommunicatieGuid)
                .HasName("fk_ref_communicatie_bijlage");

            builder.HasIndex(e => e.FaqVraagAntwoordGuid)
                .HasName("fk_ref_faq_vraag_antwoord_bijlage");

            builder.HasIndex(e => e.FaqVraagAntwoordWerkGuid)
                .HasName("fk_ref_faq_vraag_antwoord_werk_bijlage");

            builder.HasIndex(e => e.GebouwGuid)
                .HasName("fk_ref_gebouw_bijlage");

            builder.HasIndex(e => e.MeldingGuid)
                .HasName("fk_ref_melding_bijlage");

            builder.HasIndex(e => e.OptieGekozenGuid)
                .HasName("fk_ref_optie_gekozen_bijlage");

            builder.HasIndex(e => e.OptieGekozenOfferteGuid)
                .HasName("fk_ref_optie_gekozen_offerte_bijlage");

            builder.HasIndex(e => e.OptieStandaardGuid)
                .HasName("fk_ref_optie_standaard_bijlage");

            builder.HasIndex(e => e.OrganisatieGuid)
                .HasName("fk_ref_organisatie_bijlage");

            builder.HasIndex(e => e.SoftwareVerbeterpuntGuid)
                .HasName("fk_ref_software_verbeterpunt_bijlage");

            builder.HasIndex(e => e.WerkGuid)
                .HasName("fk_ref_werk_bijlage");

            builder.HasIndex(e => new { e.Volgorde, e.Datum, e.Bijlage1, e.Omschrijving })
                .HasName("so_bijlage");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BestandsopslagGuid)
                .HasColumnName("bestandsopslag_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Bijlage1)
                .HasColumnName("bijlage")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageBinaireData).HasColumnName("bijlage_binaire_data");

            builder.Property(e => e.BijlageOrigineel)
                .HasColumnName("bijlage_origineel")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageRubriekGuid)
                .HasColumnName("bijlage_rubriek_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BijlageUpload)
                .HasColumnName("bijlage_upload")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.BijlageVerwijderenBijVerwijderenRecord)
                .IsRequired()
                .HasColumnName("bijlage_verwijderen_bij_verwijderen_record")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.ChatBerichtGuid)
                .HasColumnName("chat_bericht_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.CommunicatieGuid)
                .HasColumnName("communicatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Datum)
                .HasColumnName("datum")
                .HasColumnType("date");

            builder.Property(e => e.FaqVraagAntwoordGuid)
                .HasColumnName("faq_vraag_antwoord_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.FaqVraagAntwoordWerkGuid)
                .HasColumnName("faq_vraag_antwoord_werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

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

            builder.Property(e => e.KoppelenAan).HasColumnName("koppelen_aan");

            builder.Property(e => e.MeldingGuid)
                .HasColumnName("melding_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OplosserGuid)
                .HasColumnName("oplosser_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(100);

            builder.Property(e => e.OptieGekozenGuid)
                .HasColumnName("optie_gekozen_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieGekozenOfferteGuid)
                .HasColumnName("optie_gekozen_offerte_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OptieStandaardGuid)
                .HasColumnName("optie_standaard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.SoftwareVerbeterpuntGuid)
                .HasColumnName("software_verbeterpunt_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Publiceren).HasColumnName("publiceren");

            builder.Property(e => e.Versie)
                .HasColumnName("versie")
                .HasMaxLength(10);

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.BijlageRubriekGu)
                .WithMany(p => p.Bijlages)
                .HasForeignKey(d => d.BijlageRubriekGuid)
                .HasConstraintName("ref_bijlage_rubriek_bijlage");

            builder.HasOne(d => d.ChatBerichtGu)
                .WithMany(p => p.Bijlage)
                .HasForeignKey(d => d.ChatBerichtGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ref_chat_bericht_bijlage");

            //builder.HasOne(d => d.CommunicatieGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.CommunicatieGuid)
            //    .HasConstraintName("ref_communicatie_bijlage");

            //builder.HasOne(d => d.FaqVraagAntwoordGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.FaqVraagAntwoordGuid)
            //    .HasConstraintName("ref_faq_vraag_antwoord_bijlage");

            //builder.HasOne(d => d.FaqVraagAntwoordWerkGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.FaqVraagAntwoordWerkGuid)
            //    .HasConstraintName("ref_faq_vraag_antwoord_werk_bijlage");

            //builder.HasOne(d => d.GebouwGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.GebouwGuid)
            //    .HasConstraintName("ref_gebouw_bijlage");

            builder.HasOne(d => d.MeldingGu)
                .WithMany(p => p.Bijlage)
                .HasForeignKey(d => d.MeldingGuid)
                .HasConstraintName("ref_melding_bijlage");

            //builder.HasOne(d => d.OptieGekozenGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.OptieGekozenGuid)
            //    .HasConstraintName("ref_optie_gekozen_bijlage");

            //builder.HasOne(d => d.OptieGekozenOfferteGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.OptieGekozenOfferteGuid)
            //    .HasConstraintName("ref_optie_gekozen_offerte_bijlage");

            builder.HasOne(d => d.OptieStandaardGu)
                .WithMany(p => p.Bijlages)
                .HasForeignKey(d => d.OptieStandaardGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ref_optie_standaard_bijlage");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.Bijlage)
                .HasForeignKey(d => d.OrganisatieGuid)
                .HasConstraintName("ref_organisatie_bijlage");

            //builder.HasOne(d => d.SoftwareVerbeterpuntGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.SoftwareVerbeterpuntGuid)
            //    .HasConstraintName("ref_software_verbeterpunt_bijlage");

            //builder.HasOne(d => d.WerkGu)
            //    .WithMany(p => p.Bijlage)
            //    .HasForeignKey(d => d.WerkGuid)
            //    .HasConstraintName("ref_werk_bijlage");
        }
    }
}
