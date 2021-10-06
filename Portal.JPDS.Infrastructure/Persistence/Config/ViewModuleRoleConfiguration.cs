using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewModuleRoleConfiguration : IEntityTypeConfiguration<ViewModuleRole>
    {
        public void Configure(EntityTypeBuilder<ViewModuleRole> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_module_role");

            builder.Property(e => e.Active).HasColumnName("active");

            builder.Property(e => e.AdministrationGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("administration_guid");

            builder.Property(e => e.ModuleGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("module_guid");

            builder.Property(e => e.ModuleName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("module_name");

            builder.Property(e => e.RoleEnum).HasColumnName("role_enum");

            builder.Property(e => e.RoleGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("role_guid");

            builder.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("role_name");
        }
    }
}
