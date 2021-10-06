using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ProductDienstSub2Configuration : IEntityTypeConfiguration<ProductDienstSub2>
    {
        public void Configure(EntityTypeBuilder<ProductDienstSub2> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("product_dienst_sub2_pk");

            builder.ToTable("product_dienst_sub2");

            builder.HasIndex(e => e.ProductDienstSub1Guid, "fk_ref_product_dienst_sub1_product_dienst_sub2");

            builder.HasIndex(e => e.Omschrijving, "so_product_dienst_sub2");

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

            builder.Property(e => e.ProductDienstSub1Guid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("product_dienst_sub1_guid");

            builder.HasOne(d => d.ProductDienstSub1Gu)
                .WithMany(p => p.ProductDienstSub2s)
                .HasForeignKey(d => d.ProductDienstSub1Guid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_product_dienst_sub1_product_dienst_sub2");
        }
    }
}
