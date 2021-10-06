using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OrganisatieConfiguration : IEntityTypeConfiguration<Organisatie>
    {
        public void Configure(EntityTypeBuilder<Organisatie> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("organisatie_pk");

            builder.ToTable("organisatie");

            builder.HasIndex(e => e.BankGuid)
                .HasName("fk_ref_bank_organisatie");

            builder.HasIndex(e => e.BezoekadresGuid)
                .HasName("fk_ref_adres_organisatie_bezoekadres");

            builder.HasIndex(e => e.BrancheGuid)
                .HasName("fk_ref_branche_organisatie");

            builder.HasIndex(e => e.FactuuradresGuid)
                .HasName("fk_ref_adres_organisatie_factuuradres");

            builder.HasIndex(e => e.OrganisatieSoortGuid)
                .HasName("fk_ref_organisatie_soort_organisatie");

            builder.HasIndex(e => e.OrganisatieTypeGuid)
                .HasName("fk_ref_organisatie_type_organisatie");

            builder.HasIndex(e => e.PostadresGuid)
                .HasName("fk_ref_adres_organisatie_postadres");

            builder.HasIndex(e => e.Zoeknaam)
                .HasName("so_organisatie");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Adresblok)
                .HasColumnName("adresblok")
                .HasMaxLength(800);

            builder.Property(e => e.BankGuid)
                .HasColumnName("bank_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Betaalwijze).HasColumnName("betaalwijze");

            builder.Property(e => e.Betalingstermijn).HasColumnName("betalingstermijn");

            builder.Property(e => e.BezoekadresGuid)
                .HasColumnName("bezoekadres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Bic)
                .HasColumnName("bic")
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.BrancheGuid)
                .HasColumnName("branche_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BtwNummer)
                .HasColumnName("btw_nummer")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Debiteur)
                .IsRequired()
                .HasColumnName("debiteur")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Debiteurnummer)
                .HasColumnName("debiteurnummer")
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.EigenOrganisatie)
                .IsRequired()
                .HasColumnName("eigen_organisatie")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.EmailFactuur)
                .HasColumnName("email_factuur")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.FactuurDigitaal)
                .IsRequired()
                .HasColumnName("factuur_digitaal")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.FactuuradresGuid)
                .HasColumnName("factuuradres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Fax)
                .HasColumnName("fax")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.GecontroleerdOp)
                .HasColumnName("gecontroleerd_op")
                .HasColumnType("date");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.HoofdvestigingOrganisatieGuid)
                .HasColumnName("hoofdvestiging_organisatie_guid")
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

            builder.Property(e => e.IsoCertificaat)
                .IsRequired()
                .HasColumnName("iso_certificaat")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.IsoGeldigTm)
                .HasColumnName("iso_geldig_tm")
                .HasColumnType("date");

            builder.Property(e => e.KvkNummer)
                .HasColumnName("kvk_nummer")
                .HasMaxLength(8)
                .IsUnicode(false);

            builder.Property(e => e.Logo)
                .HasColumnName("logo")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .IsRequired()
                .HasColumnName("naam")
                .HasMaxLength(100);

            builder.Property(e => e.NaamFactuur)
                .HasColumnName("naam_factuur")
                .HasMaxLength(100);

            builder.Property(e => e.NaamOnderdeel)
                .IsRequired()
                .HasColumnName("naam_onderdeel")
                .HasMaxLength(203)
                .HasComputedColumnSql("([naam]+isnull(' - '+[onderdeel],''))");

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.Onderdeel)
                .HasColumnName("onderdeel")
                .HasMaxLength(100);

            builder.Property(e => e.OnderdeelInAdresblok)
                .IsRequired()
                .HasColumnName("onderdeel_in_adresblok")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OrganisatieSoortGuid)
                .HasColumnName("organisatie_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.OrganisatieTypeGuid)
                .HasColumnName("organisatie_type_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PostadresGuid)
                .IsRequired()
                .HasColumnName("postadres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StartFactuurnummer)
                .HasColumnName("start_factuurnummer")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Telefoon)
                .HasColumnName("telefoon")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.VcaCertificaat)
                .IsRequired()
                .HasColumnName("vca_certificaat")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.VcaGeldigTm)
                .HasColumnName("vca_geldig_tm")
                .HasColumnType("date");

            builder.Property(e => e.Website)
                .HasColumnName("website")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Wka)
                .IsRequired()
                .HasColumnName("wka")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.WkaGeldigTm)
                .HasColumnName("wka_geldig_tm")
                .HasColumnType("date");

            builder.Property(e => e.Zoeknaam)
                .IsRequired()
                .HasColumnName("zoeknaam")
                .HasMaxLength(150);

            //builder.HasOne(d => d.BankGu)
            //    .WithMany(p => p.Organisatie)
            //    .HasForeignKey(d => d.BankGuid)
            //    .HasConstraintName("ref_bank_organisatie");

            builder.HasOne(d => d.BezoekadresGu)
                .WithMany(p => p.OrganisatieBezoekadresGu)
                .HasForeignKey(d => d.BezoekadresGuid)
                .HasConstraintName("ref_adres_organisatie_bezoekadres");

            //builder.HasOne(d => d.BrancheGu)
            //    .WithMany(p => p.Organisatie)
            //    .HasForeignKey(d => d.BrancheGuid)
            //    .HasConstraintName("ref_branche_organisatie");

            builder.HasOne(d => d.FactuuradresGu)
                .WithMany(p => p.OrganisatieFactuuradresGu)
                .HasForeignKey(d => d.FactuuradresGuid)
                .HasConstraintName("ref_adres_organisatie_factuuradres");

            //builder.HasOne(d => d.OrganisatieSoortGu)
            //    .WithMany(p => p.Organisatie)
            //    .HasForeignKey(d => d.OrganisatieSoortGuid)
            //    .HasConstraintName("ref_organisatie_soort_organisatie");

            //builder.HasOne(d => d.OrganisatieTypeGu)
            //    .WithMany(p => p.Organisatie)
            //    .HasForeignKey(d => d.OrganisatieTypeGuid)
            //    .HasConstraintName("ref_organisatie_type_organisatie");

            builder.HasOne(d => d.PostadresGu)
                .WithMany(p => p.OrganisatiePostadresGu)
                .HasForeignKey(d => d.PostadresGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_adres_organisatie_postadres");
        }
    }
}
