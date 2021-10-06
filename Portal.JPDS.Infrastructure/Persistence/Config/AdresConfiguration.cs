using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class AdresConfiguration : IEntityTypeConfiguration<Adres>
    {
        public void Configure(EntityTypeBuilder<Adres> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("adres_pk");

            builder.ToTable("adres");

            builder.HasIndex(e => new { e.Straat, e.Nummer })
                .HasName("so_adres");

            builder.HasIndex(e => new { e.LandGuid, e.Postcode, e.Nummer, e.NummerToevoeging })
                .HasName("uk_land_postcode_nummer_toevoeging")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
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

            builder.Property(e => e.LandGuid)
                .IsRequired()
                .HasColumnName("land_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Netnummer)
                .HasColumnName("netnummer")
                .HasMaxLength(4)
                .IsUnicode(false);

            builder.Property(e => e.Nummer)
                .IsRequired()
                .HasColumnName("nummer")
                .HasMaxLength(12);

            builder.Property(e => e.NummerToevoeging)
                .HasColumnName("nummer_toevoeging")
                .HasMaxLength(10);

            builder.Property(e => e.Plaats)
                .IsRequired()
                .HasColumnName("plaats")
                .HasMaxLength(40);

            builder.Property(e => e.Postcode)
                .IsRequired()
                .HasColumnName("postcode")
                .HasMaxLength(8);

            builder.Property(e => e.Straat)
                .IsRequired()
                .HasColumnName("straat")
                .HasMaxLength(40);

            builder.Property(e => e.StraatNummerToevoeging)
                .HasColumnName("straat_nummer_toevoeging")
                .HasMaxLength(64)
                .HasComputedColumnSql("((ltrim([straat]+' ')+[nummer])+isnull(' '+[nummer_toevoeging],''))");

            builder.Property(e => e.VolledigAdres)
                .HasColumnName("volledig_adres")
                .HasMaxLength(116)
                .HasComputedColumnSql("((((ltrim([straat]+' ')+[nummer])+isnull(' '+[nummer_toevoeging],''))+isnull(', '+ltrim([postcode]+'  '),''))+[plaats])");

            //builder.HasOne(d => d.LandGu)
            //    .WithMany(p => p.Adres)
            //    .HasForeignKey(d => d.LandGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_land_adres");
        }
    }
}
