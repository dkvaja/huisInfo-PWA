using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class DossierVolgordeConfiguration : IEntityTypeConfiguration<DossierVolgorde>
    {
        public void Configure(EntityTypeBuilder<DossierVolgorde> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("dossier_volgorde_pk");

            builder.ToTable("dossier_volgorde");

            builder.HasIndex(e => e.DossierGuid, "fk_ref_dossier_dossier_volgorde");

            builder.HasIndex(e => e.WerkGuid, "fk_ref_werk_dossier_volgorde");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.DossierGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("dossier_guid");

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

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.HasOne(d => d.DossierGu)
                .WithMany(p => p.DossierVolgordes)
                .HasForeignKey(d => d.DossierGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_dossier_dossier_volgorde");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.DossierVolgordes)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_dossier_volgorde");
        }
    }
}
