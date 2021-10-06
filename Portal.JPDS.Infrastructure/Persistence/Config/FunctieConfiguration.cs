using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class FunctieConfiguration : IEntityTypeConfiguration<Functie>
    {
        public void Configure(EntityTypeBuilder<Functie> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("functie_pk");

            builder.ToTable("functie");

            builder.HasIndex(e => e.Functie1, "index_functie")
                    .IsUnique();

            builder.Property(e => e.Guid)
                            .HasMaxLength(40)
                            .IsUnicode(false)
                            .HasColumnName("guid");

            builder.Property(e => e.Functie1)
                            .IsRequired()
                            .HasMaxLength(40)
                            .HasColumnName("functie");

            builder.Property(e => e.GewijzigdDoor)
                            .HasMaxLength(40)
                            .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                            .HasColumnType("datetime")
                            .HasColumnName("gewijzigd_op");

            builder.Property(e => e.IngevoerdDoor)
                            .HasMaxLength(40)
                            .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                            .HasColumnType("datetime")
                            .HasColumnName("ingevoerd_op");
        }
    }
}
