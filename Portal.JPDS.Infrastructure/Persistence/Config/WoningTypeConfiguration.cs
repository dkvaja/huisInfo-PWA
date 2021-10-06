using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class WoningTypeConfiguration : IEntityTypeConfiguration<WoningType>
    {
        public void Configure(EntityTypeBuilder<WoningType> builder)
        {
            builder.HasKey(e => e.Guid)
                  .HasName("woning_type_pk");

            builder.ToTable("woning_type");

            builder.HasIndex(e => new { e.WerkGuid, e.WoningType1 }, "index_werk_guid_woning_type")
                .IsUnique();

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

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

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.Property(e => e.WoningType1)
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnName("woning_type");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.WoningType)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_woning_type");

        }
    }
}
