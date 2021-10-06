using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ProductDienstConfiguration : IEntityTypeConfiguration<ProductDienst>
    {
        public void Configure(EntityTypeBuilder<ProductDienst> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("product_dienst_pk");

            builder.ToTable("product_dienst");

            builder.HasIndex(e => e.Omschrijving, "index_product_dienst_omschrijving")
                .IsUnique();

            builder.HasIndex(e => new { e.ProductCode, e.Omschrijving }, "so_product_dienst");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.BetrokkenePubliceren)
                .IsRequired()
                .HasColumnName("betrokkene_publiceren")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.BouwdeelCode)
                .HasMaxLength(20)
                .HasColumnName("bouwdeel_code");

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

            builder.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("product_code");

            builder.Property(e => e.Systeemwaarde)
                .IsRequired()
                .HasColumnName("systeemwaarde")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.ToevoegenAlsBetrokkene)
                .IsRequired()
                .HasColumnName("toevoegen_als_betrokkene")
                .HasDefaultValueSql("('0')");
        }
    }
}
