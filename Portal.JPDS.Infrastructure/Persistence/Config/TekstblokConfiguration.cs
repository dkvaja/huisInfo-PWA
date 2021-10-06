using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class TekstblokConfiguration : IEntityTypeConfiguration<Tekstblok>
    {
        public void Configure(EntityTypeBuilder<Tekstblok> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("tekstblok_pk");

            builder.ToTable("tekstblok");

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

            builder.Property(e => e.LoginGuid)
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardTekstblokGuid)
                .HasColumnName("standaard_tekstblok_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Tekstblok1)
                .IsRequired()
                .HasColumnName("tekstblok")
                .HasMaxLength(4000);

            builder.Property(e => e.TekstblokIsHandtekening).HasColumnName("tekstblok_is_handtekening");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Zoekterm)
                .IsRequired()
                .HasColumnName("zoekterm")
                .HasMaxLength(40);
        }
    }
}
