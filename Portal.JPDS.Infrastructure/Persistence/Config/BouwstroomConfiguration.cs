using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BouwstroomConfiguration : IEntityTypeConfiguration<Bouwstroom>
    {
        public void Configure(EntityTypeBuilder<Bouwstroom> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("bouwstroom_pk");

            builder.ToTable("bouwstroom");

            builder.HasIndex(e => new { e.WerkGuid, e.Bouwstroom1 })
                .HasName("index_werk_guid_bouwstroom")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Bouwstroom1)
                .IsRequired()
                .HasColumnName("bouwstroom")
                .HasMaxLength(40);

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

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Bouwstroom)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_bouwstroom");
        }
    }
}
