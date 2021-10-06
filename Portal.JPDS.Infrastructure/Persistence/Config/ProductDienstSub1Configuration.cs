using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ProductDienstSub1Configuration : IEntityTypeConfiguration<ProductDienstSub1>
    {
        public void Configure(EntityTypeBuilder<ProductDienstSub1> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("product_dienst_sub1_pk");

            builder.ToTable("product_dienst_sub1");

            builder.HasIndex(e => e.ProductDienstGuid, "fk_ref_product_dienst_product_dienst_sub1");

            builder.HasIndex(e => e.Omschrijving, "so_product_dienst_sub1");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('1')");

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

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("omschrijving");

            builder.Property(e => e.ProductDienstGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("product_dienst_guid");

            builder.HasOne(d => d.ProductDienstGu)
                .WithMany(p => p.ProductDienstSub1s)
                .HasForeignKey(d => d.ProductDienstGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_product_dienst_product_dienst_sub1");
        }
    }
}
