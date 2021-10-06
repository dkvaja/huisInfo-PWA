using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BetrokkeneConfiguration : IEntityTypeConfiguration<Betrokkene>
    {
        public void Configure(EntityTypeBuilder<Betrokkene> builder)
        {
            builder.HasKey(e => e.Guid)
                     .HasName("betrokkene_pk");

            builder.ToTable("betrokkene");

            builder.HasIndex(e => e.BetrokkeneSoortGuid)
                .HasName("fk_ref_betrokkene_soort_betrokkene");

            builder.HasIndex(e => new { e.ProductDienstGuid, e.OrganisatieGuid })
                .HasName("fk_ref_organisatie_product_betrokkene");

            builder.HasIndex(e => new { e.WerkGuid, e.ProductDienstGuid, e.BetrokkeneSoortGuid })
                .HasName("so_betrokkene");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BetrokkeneSoortGuid)
                .IsRequired()
                .HasColumnName("betrokkene_soort_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Hoofdaannemer)
                .IsRequired()
                .HasColumnName("hoofdaannemer")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.LaatsteOrderbevestiging)
                .HasColumnName("laatste_orderbevestiging")
                .HasColumnType("date");

            builder.Property(e => e.Notities).HasColumnName("notities");

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ProductDienstGuid)
                .IsRequired()
                .HasColumnName("product_dienst_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Publiceren)
                .IsRequired()
                .HasColumnName("publiceren")
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.VoettekstOrderbevestiging)
                .HasColumnName("voettekst_orderbevestiging")
                .HasMaxLength(4000);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            //builder.HasOne(d => d.BetrokkeneSoortGu)
            //    .WithMany(p => p.Betrokkene)
            //    .HasForeignKey(d => d.BetrokkeneSoortGuid)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("ref_betrokkene_soort_betrokkene");

            builder.HasOne(d => d.ProductDienstGu)
                .WithMany(p => p.Betrokkenes)
                .HasForeignKey(d => d.ProductDienstGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_product_dienst_betrokkene");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Betrokkene)
                .HasForeignKey(d => d.WerkGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_werk_betrokkene");
        }
    }
}
