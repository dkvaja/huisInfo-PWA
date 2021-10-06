using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class KoperHuurderConfiguration : IEntityTypeConfiguration<KoperHuurder>
    {
        public void Configure(EntityTypeBuilder<KoperHuurder> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("koper_huurder_pk");

            builder.ToTable("koper_huurder");

            builder.HasIndex(e => e.BankGuid)
                .HasName("fk_ref_bank_koper_huurder");

            builder.HasIndex(e => e.CorrespondentieadresGuid)
                .HasName("fk_ref_adres_koper_huurder");

            builder.HasIndex(e => e.OrganisatieGuid)
                .HasName("fk_ref_organisatie_koper_huurder");

            builder.HasIndex(e => e.Persoon2Guid)
                .HasName("fk_ref_persoon_koper_huurder_2");

            builder.HasIndex(e => e.RelatieGuid)
                .HasName("fk_ref_relatie_koper_huurder");

            builder.HasIndex(e => e.TaalGuid)
                .HasName("fk_ref_taal_koper_huurder");

            builder.HasIndex(e => e.Zoeknaam)
                .HasName("so_koper_huurder");

            builder.HasIndex(e => new { e.Persoon1Guid, e.Persoon2Guid, e.RelatieGuid })
                .HasName("persoon_1_guid_persoon_2_guid_relatie_guid")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

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

            builder.Property(e => e.CorrespondentieadresGuid)
                .IsRequired()
                .HasColumnName("correspondentieadres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Debiteur)
                .IsRequired()
                .HasColumnName("debiteur")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Debiteurnummer)
                .HasColumnName("debiteurnummer")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.EmailFactuur)
                .HasColumnName("email_factuur")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.EmailNaarAlleEmailadressen)
                .IsRequired()
                .HasColumnName("email_naar_alle_emailadressen")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.FactuurDigitaal)
                .IsRequired()
                .HasColumnName("factuur_digitaal")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

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

            builder.Property(e => e.KoperHuurderAantal)
                .HasColumnName("koper_huurder_aantal")
                .HasComputedColumnSql("([dbo].[cff_proxy_koper_huurder_koper_huurder_aantal]([soort],[persoon_1_guid],[persoon_2_guid],[organisatie_guid],[relatie_guid]))");

            builder.Property(e => e.KoperHuurderStatus)
                .HasColumnName("koper_huurder_status")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.NaamInTweeRegels)
                .IsRequired()
                .HasColumnName("naam_in_twee_regels")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Persoon1EmailWerk)
                .HasColumnName("persoon_1_email_werk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Persoon1Fax)
                .HasColumnName("persoon_1_fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Persoon1Guid)
                .HasColumnName("persoon_1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Persoon1TelefoonWerk)
                .HasColumnName("persoon_1_telefoon_werk")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Persoon2EmailWerk)
                .HasColumnName("persoon_2_email_werk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Persoon2Fax)
                .HasColumnName("persoon_2_fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Persoon2Guid)
                .HasColumnName("persoon_2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Persoon2TelefoonWerk)
                .HasColumnName("persoon_2_telefoon_werk")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Soort)
                .HasColumnName("soort")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.TaalGuid)
                .HasColumnName("taal_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ToestemmingVrijgevenGegevens)
                .IsRequired()
                .HasColumnName("toestemming_vrijgeven_gegevens")
                .HasDefaultValueSql("('0')");

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

            builder.Property(e => e.Zoeknaam)
                .IsRequired()
                .HasColumnName("zoeknaam")
                .HasMaxLength(150);

            //builder.HasOne(d => d.BankGu)
            //    .WithMany(p => p.KoperHuurder)
            //    .HasForeignKey(d => d.BankGuid)
            //    .HasConstraintName("ref_bank_koper_huurder");

            builder.HasOne(d => d.CorrespondentieadresGu)
                .WithMany(p => p.KoperHuurder)
                .HasForeignKey(d => d.CorrespondentieadresGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_adres_koper_huurder");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.KoperHuurder)
                .HasForeignKey(d => d.OrganisatieGuid)
                .HasConstraintName("ref_organisatie_koper_huurder");

            builder.HasOne(d => d.Persoon1Gu)
                .WithMany(p => p.KoperHuurderPersoon1Gu)
                .HasForeignKey(d => d.Persoon1Guid)
                .HasConstraintName("ref_persoon_koper_huurder_1");

            builder.HasOne(d => d.Persoon2Gu)
                .WithMany(p => p.KoperHuurderPersoon2Gu)
                .HasForeignKey(d => d.Persoon2Guid)
                .HasConstraintName("ref_persoon_koper_huurder_2");

            builder.HasOne(d => d.RelatieGu)
                .WithMany(p => p.KoperHuurder)
                .HasForeignKey(d => d.RelatieGuid)
                .HasConstraintName("ref_relatie_koper_huurder");

            //builder.HasOne(d => d.TaalGu)
            //    .WithMany(p => p.KoperHuurder)
            //    .HasForeignKey(d => d.TaalGuid)
            //    .HasConstraintName("ref_taal_koper_huurder");
        }
    }
}
