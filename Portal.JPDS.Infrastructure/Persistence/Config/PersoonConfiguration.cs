using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class PersoonConfiguration : IEntityTypeConfiguration<Persoon>
    {
        public void Configure(EntityTypeBuilder<Persoon> builder)
        {
            builder.HasKey(e => e.Guid)
                  .HasName("persoon_pk");

            builder.ToTable("persoon");

            builder.HasIndex(e => e.BankGuid)
                .HasName("fk_ref_bank_persoon");

            builder.HasIndex(e => e.PartnerPersoonGuid)
                .HasName("fk_ref_persoon_persoon_partner");

            builder.HasIndex(e => e.PersoonSoortGuid)
                .HasName("fk_ref_persoon_soort_persoon");

            builder.HasIndex(e => e.PriveAdresGuid)
                .HasName("fk_ref_adres_persoon");

            builder.HasIndex(e => e.TaalGuid)
                .HasName("fk_ref_taal_persoon");

            builder.HasIndex(e => e.TitulatuurAchterGuid)
                .HasName("fk_ref_titulatuur_achter_persoon");

            builder.HasIndex(e => e.TitulatuurVoorGuid)
                .HasName("fk_ref_titulatuur_voor_persoon");

            builder.HasIndex(e => e.TussenvoegselGuid)
                .HasName("fk_ref_tussenvoegsel_persoon");

            builder.HasIndex(e => new { e.Achternaam, e.Voorletters })
                .HasName("so_persoon");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Achternaam)
                .IsRequired()
                .HasColumnName("achternaam")
                .HasMaxLength(40);

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

            builder.Property(e => e.EmailPrive)
                .HasColumnName("email_prive")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.FactuurDigitaal)
                .IsRequired()
                .HasColumnName("factuur_digitaal")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Foto)
                .HasColumnName("foto")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Geboortedatum)
                .HasColumnName("geboortedatum")
                .HasColumnType("date");

            builder.Property(e => e.Geboorteplaats)
                .HasColumnName("geboorteplaats")
                .HasMaxLength(40);

            builder.Property(e => e.GecontroleerdOp)
                .HasColumnName("gecontroleerd_op")
                .HasColumnType("date");

            builder.Property(e => e.Geslacht).HasColumnName("geslacht");

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

            builder.Property(e => e.Mobiel)
                .HasColumnName("mobiel")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100)
                .HasComputedColumnSql("([dbo].[cff_proxy_persoon_naam]([achternaam],[tussenvoegsel_guid],[voorletters],[voornaam]))");

            builder.Property(e => e.Notitie).HasColumnName("notitie");

            builder.Property(e => e.Overlijdensdatum)
                .HasColumnName("overlijdensdatum")
                .HasColumnType("date");

            builder.Property(e => e.PartnerPersoonGuid)
                .HasColumnName("partner_persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonSoortGuid)
                .HasColumnName("persoon_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonType).HasColumnName("persoon_type");

            builder.Property(e => e.PriveAdresGuid)
                .HasColumnName("prive_adres_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.TaalGuid)
                .IsRequired()
                .HasColumnName("taal_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Telefoon)
                .HasColumnName("telefoon")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.TitulatuurAchterGuid)
                .HasColumnName("titulatuur_achter_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.TitulatuurVoorGuid)
                .HasColumnName("titulatuur_voor_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.TussenvoegselGuid)
                .HasColumnName("tussenvoegsel_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VolledigeNaam)
                .HasColumnName("volledige_naam")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Voorletters)
                .HasColumnName("voorletters")
                .HasMaxLength(15);

            builder.Property(e => e.Voornaam)
                .HasColumnName("voornaam")
                .HasMaxLength(40);

            builder.Property(e => e.Zoeknaam)
                .HasColumnName("zoeknaam")
                .HasMaxLength(150)
                .HasComputedColumnSql("([dbo].[cff_proxy_persoon_zoeknaam]([achternaam],[tussenvoegsel_guid],[voorletters]))");

            //builder.HasOne(d => d.BankGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.BankGuid)
            //    .HasConstraintName("ref_bank_persoon");

            builder.HasOne(d => d.PartnerPersoonGu)
                .WithMany(p => p.InversePartnerPersoonGu)
                .HasForeignKey(d => d.PartnerPersoonGuid)
                .HasConstraintName("ref_persoon_persoon_partner");

            //builder.HasOne(d => d.PersoonSoortGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.PersoonSoortGuid)
            //    .HasConstraintName("ref_persoon_soort_persoon");

            builder.HasOne(d => d.PriveAdresGu)
                .WithMany(p => p.Persoon)
                .HasForeignKey(d => d.PriveAdresGuid)
                .HasConstraintName("ref_adres_persoon");

            //builder.HasOne(d => d.TaalGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.TaalGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_taal_persoon");

            //builder.HasOne(d => d.TitulatuurAchterGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.TitulatuurAchterGuid)
            //    .HasConstraintName("ref_titulatuur_achter_persoon");

            //builder.HasOne(d => d.TitulatuurVoorGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.TitulatuurVoorGuid)
            //    .HasConstraintName("ref_titulatuur_voor_persoon");

            //builder.HasOne(d => d.TussenvoegselGu)
            //    .WithMany(p => p.Persoon)
            //    .HasForeignKey(d => d.TussenvoegselGuid)
            //    .HasConstraintName("ref_tussenvoegsel_persoon");
        }
    }
}
