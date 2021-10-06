using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class CommunicatieConfiguration : IEntityTypeConfiguration<Communicatie>
    {
        public void Configure(EntityTypeBuilder<Communicatie> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("communicatie_pk");

            builder.ToTable("communicatie");

            builder.HasIndex(e => new { e.WerkGuid, e.ActieStandaard1Guid }, "fk_ref_actie_werk_communicatie_1");

            builder.HasIndex(e => new { e.WerkGuid, e.ActieStandaard2Guid }, "fk_ref_actie_werk_communicatie_2");

            builder.HasIndex(e => e.AfdelingGuid, "fk_ref_afdeling_communicatie");

            builder.HasIndex(e => new { e.MedewerkerGuid, e.AfzenderEmailAccountGuid }, "fk_ref_email_account_communicatie");

            builder.HasIndex(e => e.FactuurGuid, "fk_ref_factuur_communicatie");

            builder.HasIndex(e => new { e.WerkGuid, e.GebouwGuid }, "fk_ref_gebouw_communicatie");

            builder.HasIndex(e => e.HerinneringMedewerkerGuid, "fk_ref_medewerker_communicatie_herinnering");

            builder.HasIndex(e => e.MeldingGuid, "fk_ref_melding_communicatie_lookup");

            builder.HasIndex(e => new { e.SjabloonSoort, e.SjabloonGuid }, "fk_ref_sjabloon_communicatie");

            builder.HasIndex(e => e.SjabloonGuid, "fk_ref_sjabloon_communicatie_integriteit");

            builder.HasIndex(e => e.IngevoerdOp, "so_communicatie");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.ActieStandaard1Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("actie_standaard_1_guid");

            builder.Property(e => e.ActieStandaard2Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("actie_standaard_2_guid");

            builder.Property(e => e.AfdelingGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("afdeling_guid");

            builder.Property(e => e.AfzenderEmailAccountGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("afzender_email_account_guid");

            builder.Property(e => e.Betreft)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnName("betreft");

            builder.Property(e => e.CommunicatieKenmerkVolgnummer).HasColumnName("communicatie_kenmerk_volgnummer");

            builder.Property(e => e.CommunicatieSoort)
                .HasColumnName("communicatie_soort")
                .HasDefaultValueSql("('2')");

            builder.Property(e => e.CommunicatieStatus)
                .HasColumnName("communicatie_status")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.CommunicatieType).HasColumnName("communicatie_type");

            builder.Property(e => e.Datum)
                .HasColumnType("date")
                .HasColumnName("datum");

            builder.Property(e => e.EmailId)
                .HasMaxLength(150)
                .HasColumnName("email_id");

            builder.Property(e => e.FactuurGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("factuur_guid");

            builder.Property(e => e.GebouwGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gebouw_guid");

            builder.Property(e => e.GewijzigdDoor)
                .HasMaxLength(40)
                .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                .HasColumnType("datetime")
                .HasColumnName("gewijzigd_op");

            builder.Property(e => e.Groep).HasColumnName("groep");

            builder.Property(e => e.HerinneringDatum)
                .HasColumnType("date")
                .HasColumnName("herinnering_datum");

            builder.Property(e => e.HerinneringMedewerkerGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("herinnering_medewerker_guid");

            builder.Property(e => e.IngevoerdDoor)
                .HasMaxLength(40)
                .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                .HasColumnType("datetime")
                .HasColumnName("ingevoerd_op");

            builder.Property(e => e.MedewerkerGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("medewerker_guid");

            builder.Property(e => e.MeldingGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("melding_guid");

            builder.Property(e => e.OnsKenmerk)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("ons_kenmerk");

            builder.Property(e => e.OorspronkelijkVerslag).HasColumnName("oorspronkelijk_verslag");

            builder.Property(e => e.SjabloonGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("sjabloon_guid");

            builder.Property(e => e.SjabloonSoort)
                .HasColumnName("sjabloon_soort")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.TijdAanvang).HasColumnName("tijd_aanvang");

            builder.Property(e => e.TijdEinde).HasColumnName("tijd_einde");

            builder.Property(e => e.UwKenmerk)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("uw_kenmerk");

            builder.Property(e => e.Verslag).HasColumnName("verslag");

            builder.Property(e => e.WerkGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.HasOne(d => d.AfdelingGu)
                .WithMany(p => p.Communicaties)
                .HasForeignKey(d => d.AfdelingGuid)
                .HasConstraintName("ref_afdeling_communicatie");

            //builder.HasOne(d => d.FactuurGu)
            //    .WithMany(p => p.Communicaties)
            //    .HasForeignKey(d => d.FactuurGuid)
            //    .HasConstraintName("ref_factuur_communicatie");

            builder.HasOne(d => d.HerinneringMedewerkerGu)
                .WithMany(p => p.CommunicatieHerinneringMedewerkerGus)
                .HasForeignKey(d => d.HerinneringMedewerkerGuid)
                .HasConstraintName("ref_medewerker_communicatie_herinnering");

            builder.HasOne(d => d.MedewerkerGu)
                .WithMany(p => p.CommunicatieMedewerkerGus)
                .HasForeignKey(d => d.MedewerkerGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_medewerker_communicatie");

            builder.HasOne(d => d.MeldingGu)
                .WithMany(p => p.Communicaties)
                .HasForeignKey(d => d.MeldingGuid)
                .HasConstraintName("ref_melding_communicatie_lookup");

            builder.HasOne(d => d.SjabloonGu)
                .WithMany(p => p.Communicaties)
                .HasForeignKey(d => d.SjabloonGuid)
                .HasConstraintName("ref_sjabloon_communicatie_integriteit");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Communicaties)
                .HasForeignKey(d => d.WerkGuid)
                .HasConstraintName("ref_werk_communicatie");
        }
    }
}
