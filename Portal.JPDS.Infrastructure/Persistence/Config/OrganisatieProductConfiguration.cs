using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class OrganisatieProductConfiguration : IEntityTypeConfiguration<OrganisatieProduct>
    {
        public void Configure(EntityTypeBuilder<OrganisatieProduct> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("organisatie_product_pk");

            builder.ToTable("organisatie_product");

            builder.HasIndex(e => new { e.OrganisatieGuid, e.ProductDienstGuid }, "organisatie_guid_product_dienst_guid")
                .IsUnique();

            builder.HasIndex(e => new { e.ProductDienstGuid, e.OrganisatieGuid }, "so_organisatie_product");

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

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("organisatie_guid");

            builder.Property(e => e.ProductDienstGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("product_dienst_guid");

            builder.Property(e => e.Waardering).HasColumnName("waardering");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.OrganisatieProducts)
                .HasForeignKey(d => d.OrganisatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_organisatie_organisatie_product");

            builder.HasOne(d => d.ProductDienstGu)
                .WithMany(p => p.OrganisatieProducts)
                .HasForeignKey(d => d.ProductDienstGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_product_dienst_organisatie_product");
        }
    }
}
