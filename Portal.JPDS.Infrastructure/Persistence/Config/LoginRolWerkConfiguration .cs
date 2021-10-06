using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class LoginRolWerkConfiguration : IEntityTypeConfiguration<LoginRolWerk>
    {
        public void Configure(EntityTypeBuilder<LoginRolWerk> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("login_rol_werk_pk");

            builder.ToTable("login_rol_werk");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Actief).HasColumnName("actief");

            builder.Property(e => e.GebouwGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gebouw_guid");

            builder.Property(e => e.GewijzigdDoor)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("gewijzigd_door");

            builder.Property(e => e.GewijzigdOp)
                .HasColumnType("datetime")
                .HasColumnName("gewijzigd_op");

            builder.Property(e => e.IngevoerdDoor)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("ingevoerd_door");

            builder.Property(e => e.IngevoerdOp)
                .HasColumnType("datetime")
                .HasColumnName("ingevoerd_op");

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("login_guid");

            builder.Property(e => e.ModuleGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("module_guid");

            builder.Property(e => e.RolGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("rol_guid");

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("werk_guid");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.LoginRolWerks)
                .HasForeignKey(d => d.GebouwGuid)
                .HasConstraintName("ref_gebouw_login_rol_werk");

            builder.HasOne(d => d.LoginGu)
                .WithMany(p => p.LoginRolWerks)
                .HasForeignKey(d => d.LoginGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_login_login_rol_werk");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.LoginRolWerks)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_login_rol_werk");
        }
    }
}
