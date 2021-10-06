using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class MeldingKostenConfiguration : IEntityTypeConfiguration<MeldingKosten>
    {
        public void Configure(EntityTypeBuilder<MeldingKosten> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("melding_kosten_pk");

            builder.ToTable("melding_kosten");

            builder.HasIndex(e => e.CrediteurOrganisatieGuid)
                .HasName("fk_ref_organisatie_melding_kosten_crediteur");

            builder.HasIndex(e => e.DebiteurKoperHuurderGuid)
                .HasName("fk_ref_koper_huurder_melding_kosten_debiteur");

            builder.HasIndex(e => e.DebiteurOrganisatieGuid)
                .HasName("fk_ref_organisatie_melding_kosten_debiteur");

            builder.HasIndex(e => e.FactuurGuid)
                .HasName("fk_ref_factuur_melding_kosten");

            builder.HasIndex(e => e.InkoopEenheidGuid)
                .HasName("fk_ref_eenheid_melding_kosten_inkoop");

            builder.HasIndex(e => e.VerkoopEenheidGuid)
                .HasName("fk_ref_eenheid_melding_kosten_verkoop");

            builder.HasIndex(e => new { e.MeldingGuid, e.Omschrijving })
                .HasName("so_melding_kosten");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.CrediteurOrganisatieGuid)
                .HasColumnName("crediteur_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DebiteurKoperHuurderGuid)
                .HasColumnName("debiteur_koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.DebiteurOrganisatieGuid)
                .HasColumnName("debiteur_organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Doorberekenen)
                .IsRequired()
                .HasColumnName("doorberekenen")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.DoorberekenenAan).HasColumnName("doorberekenen_aan");

            builder.Property(e => e.FactuurGuid)
                .HasColumnName("factuur_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Gefactureerd)
                .IsRequired()
                .HasColumnName("gefactureerd")
                .HasDefaultValueSql("('0')");

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

            builder.Property(e => e.InkoopAantal)
                .HasColumnName("inkoop_aantal")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.InkoopBedragExclBtw)
                .HasColumnName("inkoop_bedrag_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasComputedColumnSql("(CONVERT([numeric](12,2),[inkoop_aantal]*[inkoop_prijs_per_eenheid_excl_btw],0))");

            builder.Property(e => e.InkoopBtwTarief).HasColumnName("inkoop_btw_tarief");

            builder.Property(e => e.InkoopEenheidGuid)
                .HasColumnName("inkoop_eenheid_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.InkoopFactuurnummer)
                .HasColumnName("inkoop_factuurnummer")
                .HasMaxLength(20);

            builder.Property(e => e.InkoopPrijsPerEenheidExclBtw)
                .HasColumnName("inkoop_prijs_per_eenheid_excl_btw")
                .HasColumnType("numeric(12, 2)");

            builder.Property(e => e.MeldingGuid)
                .IsRequired()
                .HasColumnName("melding_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.NaamDebiteur)
                .HasColumnName("naam_debiteur")
                .HasMaxLength(100)
                .HasComputedColumnSql("([dbo].[cff_proxy_melding_kosten_naam_debiteur]([doorberekenen_aan],[debiteur_koper_huurder_guid],[debiteur_organisatie_guid]))");

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(60);

            builder.Property(e => e.VerkoopAantal)
                .HasColumnName("verkoop_aantal")
                .HasColumnType("numeric(10, 2)");

            builder.Property(e => e.VerkoopBedragExclBtw)
                .HasColumnName("verkoop_bedrag_excl_btw")
                .HasColumnType("numeric(12, 2)")
                .HasComputedColumnSql("(CONVERT([numeric](12,2),[verkoop_aantal]*[verkoop_prijs_per_eenheid_excl_btw],0))");

            builder.Property(e => e.VerkoopBtwTarief).HasColumnName("verkoop_btw_tarief");

            builder.Property(e => e.VerkoopEenheidGuid)
                .HasColumnName("verkoop_eenheid_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VerkoopFactuurnummer)
                .HasColumnName("verkoop_factuurnummer")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.VerkoopPrijsPerEenheidExclBtw)
                .HasColumnName("verkoop_prijs_per_eenheid_excl_btw")
                .HasColumnType("numeric(12, 2)");

            builder.HasOne(d => d.CrediteurOrganisatieGu)
                .WithMany(p => p.MeldingKostenCrediteurOrganisatieGu)
                .HasForeignKey(d => d.CrediteurOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_kosten_crediteur");

            builder.HasOne(d => d.DebiteurKoperHuurderGu)
                .WithMany(p => p.MeldingKosten)
                .HasForeignKey(d => d.DebiteurKoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_melding_kosten_debiteur");

            builder.HasOne(d => d.DebiteurOrganisatieGu)
                .WithMany(p => p.MeldingKostenDebiteurOrganisatieGu)
                .HasForeignKey(d => d.DebiteurOrganisatieGuid)
                .HasConstraintName("ref_organisatie_melding_kosten_debiteur");

            //builder.HasOne(d => d.FactuurGu)
            //    .WithMany(p => p.MeldingKosten)
            //    .HasForeignKey(d => d.FactuurGuid)
            //    .HasConstraintName("ref_factuur_melding_kosten");

            builder.HasOne(d => d.InkoopEenheidGu)
                .WithMany(p => p.MeldingKostenInkoopEenheidGu)
                .HasForeignKey(d => d.InkoopEenheidGuid)
                .HasConstraintName("ref_eenheid_melding_kosten_inkoop");

            builder.HasOne(d => d.MeldingGu)
                .WithMany(p => p.MeldingKosten)
                .HasForeignKey(d => d.MeldingGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_melding_melding_kosten");

            builder.HasOne(d => d.VerkoopEenheidGu)
                .WithMany(p => p.MeldingKostenVerkoopEenheidGu)
                .HasForeignKey(d => d.VerkoopEenheidGuid)
                .HasConstraintName("ref_eenheid_melding_kosten_verkoop");
        }
    }
}
