using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OptieRubriekWerkConfiguration : IEntityTypeConfiguration<OptieRubriekWerk>
    {
        public void Configure(EntityTypeBuilder<OptieRubriekWerk> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("optie_rubriek_werk_pk");

            builder.ToTable("optie_rubriek_werk");

            builder.HasIndex(e => e.OptieRubriekStandaardGuid)
                .HasName("fk_ref_optie_rubriek_standaard_optie_rubriek_werk");

            builder.HasIndex(e => new { e.WerkGuid, e.Rubriek })
                .HasName("werk_guid_rubriek")
                .IsUnique();

            builder.HasIndex(e => new { e.WerkGuid, e.Volgorde, e.Rubriek })
                .HasName("so_optie_rubriek_werk");

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

            builder.Property(e => e.OptieRubriekStandaardGuid)
                .HasColumnName("optie_rubriek_standaard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Rubriek)
                .IsRequired()
                .HasColumnName("rubriek")
                .HasMaxLength(40);

            builder.Property(e => e.Toelichting).HasColumnName("toelichting");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            //builder.HasOne(d => d.OptieRubriekStandaardGu)
            //    .WithMany(p => p.OptieRubriekWerk)
            //    .HasForeignKey(d => d.OptieRubriekStandaardGuid)
            //    .HasConstraintName("ref_optie_rubriek_standaard_optie_rubriek_werk");

            //builder.HasOne(d => d.WerkGu)
            //    .WithMany(p => p.OptieRubriekWerk)
            //    .HasForeignKey(d => d.WerkGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_werk_optie_rubriek_werk");
        }
    }
}
