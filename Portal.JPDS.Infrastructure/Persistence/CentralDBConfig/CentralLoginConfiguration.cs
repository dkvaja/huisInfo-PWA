using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.CentralDBEntities;

namespace Portal.JPDS.Infrastructure.Persistence.CentralDBConfig
{
    public class CentralLoginConfiguration : IEntityTypeConfiguration<CentralLogin>
    {
        public void Configure(EntityTypeBuilder<CentralLogin> builder)
        {
            builder.HasKey(e => e.Guid)
                   .HasName("PK_login");

            builder.ToTable("central_login");

            builder.Property(e => e.Guid)
                .ValueGeneratedNever()
                .HasColumnName("guid");

            builder.Property(e => e.Active).HasColumnName("active");

            builder.Property(e => e.CreatedBy).HasColumnName("created_by");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("created_on");

            builder.Property(e => e.CustomerLoginModifiedOn)
                .HasColumnType("datetime")
                .HasColumnName("customer_login_modified_on");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");

            builder.Property(e => e.EmailApproval)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email_approval");

            builder.Property(e => e.EmailVerificationDateTime)
                .HasColumnType("datetime")
                .HasColumnName("email_verification_date_time");

            builder.Property(e => e.EmailVerificationOtp).HasColumnName("email_verification_otp");

            builder.Property(e => e.IsSuperAdmin).HasColumnName("is_super_admin");

            builder.Property(e => e.LastLoginOn)
                .HasColumnType("datetime")
                .HasColumnName("last_login_on");

            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");

            builder.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasColumnName("modified_on");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            builder.Property(e => e.OldPasswordHash)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("old_password_hash");

            builder.Property(e => e.OldPasswordResetDate)
                .HasColumnType("datetime")
                .HasColumnName("old_password_reset_date");

            builder.Property(e => e.OptIn).HasColumnName("opt_in");

            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("password_hash");

            builder.Property(e => e.ResetPasswordLinkCreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("reset_password_link_created_on");

            builder.Property(e => e.Username)
                .HasMaxLength(40)
                .HasColumnName("username");

            builder.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_login_created_by");

            builder.HasOne(d => d.ModifiedByNavigation)
                .WithMany(p => p.InverseModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_login_modified_by");
        }
    }
}
