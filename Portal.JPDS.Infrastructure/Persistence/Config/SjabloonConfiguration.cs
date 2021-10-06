using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class SjabloonConfiguration : IEntityTypeConfiguration<Sjabloon>
    {
        public void Configure(EntityTypeBuilder<Sjabloon> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("sjabloon_pk");

            builder.ToTable("sjabloon");

            builder.HasIndex(e => e.Omschrijving)
                .HasName("so_sjabloon");

            builder.HasIndex(e => new { e.WerkGuid, e.ActieStandaard1Guid })
                .HasName("fk_ref_actie_werk_sjabloon_1");

            builder.HasIndex(e => new { e.WerkGuid, e.ActieStandaard2Guid })
                .HasName("fk_ref_actie_werk_sjabloon_2");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieStandaard1Guid)
                .HasColumnName("actie_standaard_1_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieStandaard2Guid)
                .HasColumnName("actie_standaard_2_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Betreft)
                .HasColumnName("betreft")
                .HasMaxLength(60);

            builder.Property(e => e.Documentcode)
                .HasColumnName("documentcode")
                .HasMaxLength(20);

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

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.Sjabloon1)
                .HasColumnName("sjabloon")
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValueSql("('.\\')");

            builder.Property(e => e.SjabloonBestand)
                .HasColumnName("sjabloon_bestand")
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.SjabloonHtml).HasColumnName("sjabloon_html");

            builder.Property(e => e.SjabloonSoort)
                .HasColumnName("sjabloon_soort")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.Standaard)
                .IsRequired()
                .HasColumnName("standaard")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.Submap)
                .HasColumnName("submap")
                .HasMaxLength(40);

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Sjabloon)
                .HasForeignKey(d => d.WerkGuid)
                .HasConstraintName("ref_werk_sjabloon");
        }
    }
}
