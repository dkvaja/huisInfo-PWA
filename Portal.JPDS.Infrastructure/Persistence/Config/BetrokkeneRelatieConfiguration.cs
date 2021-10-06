using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class BetrokkeneRelatieConfiguration : IEntityTypeConfiguration<BetrokkeneRelatie>
    {
        public void Configure(EntityTypeBuilder<BetrokkeneRelatie> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("betrokkene_relatie_pk");

            builder.ToTable("betrokkene_relatie");

            builder.HasIndex(e => e.FunctieGuid)
                .HasName("fk_ref_functie_betrokkene_relatie");

            builder.HasIndex(e => new { e.BetrokkeneGuid, e.OrganisatieGuid })
                .HasName("fk_ref_betrokkene_betrokkene_relatie_lookup");

            builder.HasIndex(e => new { e.OrganisatieGuid, e.RelatieGuid })
                .HasName("fk_ref_relatie_betrokkene_relatie");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BetrokkeneGuid)
                .IsRequired()
                .HasColumnName("betrokkene_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.FunctieGuid)
                .HasColumnName("functie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);
            
            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.Publiceren)
                .IsRequired()
                .HasColumnName("publiceren")
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.GewijzigdDoor)
                .HasColumnName("gewijzigd_door")
                .HasMaxLength(40);

            builder.Property(e => e.GewijzigdOp)
                .HasColumnName("gewijzigd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.IngevoerdDoor)
                .HasColumnName("ingevoerd_door")
                .HasMaxLength(40);

            builder.Property(e => e.IngevoerdOp)
                .HasColumnName("ingevoerd_op")
                .HasColumnType("datetime");

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .IsRequired()
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.BetrokkeneGu)
                .WithMany(p => p.BetrokkeneRelatie)
                .HasForeignKey(d => d.BetrokkeneGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_betrokkene_betrokkene_relatie_integriteit");

            builder.HasOne(d => d.FunctieGu)
                .WithMany(p => p.BetrokkeneRelaties)
                .HasForeignKey(d => d.FunctieGuid)
                .HasConstraintName("ref_functie_betrokkene_relatie");

            builder.Property(e => e.EmailZakelijk)
                .HasColumnName("email_zakelijk")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Mobiel)
                .HasColumnName("mobiel")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Doorkiesnummer)
                .HasColumnName("doorkiesnummer")
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
