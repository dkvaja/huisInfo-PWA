using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class EmailAccountConfiguration : IEntityTypeConfiguration<EmailAccount>
    {
        public void Configure(EntityTypeBuilder<EmailAccount> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("email_account_pk");

            builder.ToTable("email_account");

            builder.HasIndex(e => e.MedewerkerGuid, "fk_ref_medewerker_email_account");

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

            builder.Property(e => e.MedewerkerGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("medewerker_guid");

            builder.Property(e => e.PrimairEmailAccount)
                .IsRequired()
                .HasColumnName("primair_email_account")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.SmtpFromEmail)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("smtp_from_email");

            builder.Property(e => e.SmtpPassword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("smtp_password");

            builder.Property(e => e.SmtpPasswordHash)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("smtp_password_hash");

            builder.Property(e => e.SmtpUseSsl)
                .IsRequired()
                .HasColumnName("smtp_use_ssl")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.SmtpUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("smtp_user");

            builder.HasOne(d => d.MedewerkerGu)
                .WithMany(p => p.EmailAccounts)
                .HasForeignKey(d => d.MedewerkerGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_medewerker_email_account");
        }
    }
}
