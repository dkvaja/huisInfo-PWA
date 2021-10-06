using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class RelatieConfiguration : IEntityTypeConfiguration<Relatie>
    {
        public void Configure(EntityTypeBuilder<Relatie> builder)
        {
            builder.HasKey(e => e.Guid)
                       .HasName("relatie_pk");

            builder.ToTable("relatie");

            builder.HasIndex(e => e.AfdelingGuid)
                .HasName("fk_ref_afdeling_relatie");

            builder.HasIndex(e => e.FunctieGuid)
                .HasName("fk_ref_functie_relatie");

            builder.HasIndex(e => new { e.PersoonGuid, e.OrganisatieGuid })
                .HasName("so_relatie");

            builder.HasIndex(e => new { e.OrganisatieGuid, e.PersoonGuid, e.AfdelingGuid, e.FunctieGuid })
                .HasName("index_organisatie_persoon_afdeling_functie")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AanwezigDinsdagMiddag)
                .IsRequired()
                .HasColumnName("aanwezig_dinsdag_middag")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigDinsdagOchtend)
                .IsRequired()
                .HasColumnName("aanwezig_dinsdag_ochtend")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigDonderdagMiddag)
                .IsRequired()
                .HasColumnName("aanwezig_donderdag_middag")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigDonderdagOchtend)
                .IsRequired()
                .HasColumnName("aanwezig_donderdag_ochtend")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigMaandagMiddag)
                .IsRequired()
                .HasColumnName("aanwezig_maandag_middag")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigMaandagOchtend)
                .IsRequired()
                .HasColumnName("aanwezig_maandag_ochtend")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigVrijdagMiddag)
                .IsRequired()
                .HasColumnName("aanwezig_vrijdag_middag")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigVrijdagOchtend)
                .IsRequired()
                .HasColumnName("aanwezig_vrijdag_ochtend")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigWoensdagMiddag)
                .IsRequired()
                .HasColumnName("aanwezig_woensdag_middag")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AanwezigWoensdagOchtend)
                .IsRequired()
                .HasColumnName("aanwezig_woensdag_ochtend")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AdresGuid)
                .HasColumnName("adres_guid")
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[cff_proxy_relatie_adres_guid]([persoon_guid],[organisatie_guid],[gebruik_adres_organisatie]))");

            builder.Property(e => e.Adresblok)
                .HasColumnName("adresblok")
                .HasMaxLength(800);

            builder.Property(e => e.AfdelingGuid)
                .HasColumnName("afdeling_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AfdelingInAdressering)
                .IsRequired()
                .HasColumnName("afdeling_in_adressering")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.AfdelingVoorAfdelingsnaam)
                .IsRequired()
                .HasColumnName("afdeling_voor_afdelingsnaam")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Doorkiesnummer)
                .HasColumnName("doorkiesnummer")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Einddatum)
                .HasColumnName("einddatum")
                .HasColumnType("date");

            builder.Property(e => e.EmailZakelijk)
                .HasColumnName("email_zakelijk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.FunctieGuid)
                .HasColumnName("functie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GebruikAdresOrganisatie)
                .IsRequired()
                .HasColumnName("gebruik_adres_organisatie")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.GebruikAdresPersoon)
                .IsRequired()
                .HasColumnName("gebruik_adres_persoon")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.GecontroleerdOp)
                .HasColumnName("gecontroleerd_op")
                .HasColumnType("date");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Ingangsdatum)
                .HasColumnName("ingangsdatum")
                .HasColumnType("date");

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

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonGuid)
                .IsRequired()
                .HasColumnName("persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.UitDienst)
                .IsRequired()
                .HasColumnName("uit_dienst")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Zoeknaam)
                .HasColumnName("zoeknaam")
                .HasMaxLength(150);

            builder.HasOne(d => d.AfdelingGu)
                .WithMany(p => p.Relaties)
                .HasForeignKey(d => d.AfdelingGuid)
                .HasConstraintName("ref_afdeling_relatie");

            builder.HasOne(d => d.FunctieGu)
                .WithMany(p => p.Relaties)
               .HasForeignKey(d => d.FunctieGuid)
               .HasConstraintName("ref_functie_relatie");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.Relatie)
                .HasForeignKey(d => d.OrganisatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_organisatie_relatie");

            builder.HasOne(d => d.PersoonGu)
                .WithMany(p => p.Relatie)
                .HasForeignKey(d => d.PersoonGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_persoon_relatie");
        }
    }
}
