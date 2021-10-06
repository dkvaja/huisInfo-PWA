using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OpnameConfiguration : IEntityTypeConfiguration<Opname>
    {
        public void Configure(EntityTypeBuilder<Opname> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("opname_pk");

            builder.ToTable("opname");

            builder.HasIndex(e => e.UitgevoerdDoorMedewerkerGuid)
                .HasName("fk_ref_medewerker_opname");

            builder.HasIndex(e => new { e.WerkGuid, e.GebouwGuid })
                .HasName("fk_ref_gebouw_opname");

            builder.HasIndex(e => new { e.Datum, e.GebouwGuid, e.UitgevoerdDoorMedewerkerGuid })
                .HasName("so_opname");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AantalGebreken).HasColumnName("aantal_gebreken");

            builder.Property(e => e.Datum)
                .HasColumnName("datum")
                .HasColumnType("date");

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.HandtekeningKh1)
                .HasColumnName("handtekening_kh_1")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.HandtekeningKh1FileOpslag).HasColumnName("handtekening_kh_1_file_opslag");

            builder.Property(e => e.HandtekeningKh2)
                .HasColumnName("handtekening_kh_2")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.HandtekeningKh2FileOpslag).HasColumnName("handtekening_kh_2_file_opslag");

            builder.Property(e => e.HandtekeningUitvoerder)
                .HasColumnName("handtekening_uitvoerder")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.HandtekeningUitvoerderFileOpslag).HasColumnName("handtekening_uitvoerder_file_opslag");

            builder.Property(e => e.KoptekstPvo).HasColumnName("koptekst_pvo");

            builder.Property(e => e.MeterstandElektra1).HasColumnName("meterstand_elektra_1");

            builder.Property(e => e.MeterstandElektra2).HasColumnName("meterstand_elektra_2");

            builder.Property(e => e.MeterstandElektraRetour1).HasColumnName("meterstand_elektra_retour_1");

            builder.Property(e => e.MeterstandElektraRetour2).HasColumnName("meterstand_elektra_retour_2");

            builder.Property(e => e.MeterstandGasWarmte).HasColumnName("meterstand_gas_warmte");

            builder.Property(e => e.MeterstandWater).HasColumnName("meterstand_water");

            builder.Property(e => e.OpmerkingenKoperHuurder).HasColumnName("opmerkingen_koper_huurder");

            builder.Property(e => e.OpmerkingenUitvoerder).HasColumnName("opmerkingen_uitvoerder");

            builder.Property(e => e.OpnameSoort)
                .HasColumnName("opname_soort")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OpnameStatus)
                .HasColumnName("opname_status")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Pvo1BijlageGuid)
                .HasColumnName("pvo_1_bijlage_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Pvo2BijlageGuid)
                .HasColumnName("pvo_2_bijlage_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.UitgevoerdDoorMedewerkerGuid)
                .IsRequired()
                .HasColumnName("uitgevoerd_door_medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VoettekstPvo).HasColumnName("voettekst_pvo");

            builder.Property(e => e.VolledigeNaamKh1)
                .HasColumnName("volledige_naam_kh_1")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.VolledigeNaamKh2)
                .HasColumnName("volledige_naam_kh_2")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.VolledigeNaamUitvoerder)
                .HasColumnName("volledige_naam_uitvoerder")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Handtekening2Kh1)
                .HasColumnName("handtekening_2_kh_1")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Handtekening2Kh1FileOpslag).HasColumnName("handtekening_2_kh_1_file_opslag");

            builder.Property(e => e.Handtekening2Kh2)
                .HasColumnName("handtekening_2_kh_2")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Handtekening2Kh2FileOpslag).HasColumnName("handtekening_2_kh_2_file_opslag");

            builder.Property(e => e.DatumTweedeHandtekeningOndertekening)
                .HasColumnName("datum_tweede_handtekening_ondertekening")
                .HasColumnType("date");

            builder.Property(e => e.TweedeHandtekeningGestart)
                .IsRequired()
                .HasColumnName("tweede_handtekening_gestart")
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

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Opname)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_opname");

            builder.HasOne(d => d.Pvo1BijlageGu)
                    .WithMany(p => p.OpnamePvo1BijlageGu)
                    .HasForeignKey(d => d.Pvo1BijlageGuid)
                    .HasConstraintName("ref_bijlage_opname_pvo_1");

            builder.HasOne(d => d.Pvo2BijlageGu)
                .WithMany(p => p.OpnamePvo2BijlageGu)
                .HasForeignKey(d => d.Pvo2BijlageGuid)
                .HasConstraintName("ref_bijlage_opname_pvo_2");
        }
    }
}
