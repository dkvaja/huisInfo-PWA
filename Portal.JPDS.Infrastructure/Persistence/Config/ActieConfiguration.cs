using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ActieConfiguration : IEntityTypeConfiguration<Actie>
    {
        public void Configure(EntityTypeBuilder<Actie> builder)
        {
            builder.HasKey(e => e.Guid)
                  .HasName("actie_pk");

            builder.ToTable("actie");

            builder.HasIndex(e => e.ActieMedewerkerGuid)
                .HasName("fk_ref_medewerker_actie_actie");

            builder.HasIndex(e => e.AfgehandeldMedewerkerGuid)
                .HasName("fk_ref_medewerker_actie_afgehandeld");

            builder.HasIndex(e => e.GebouwGuid)
                .HasName("fk_ref_gebouw_actie_integriteit");

            builder.HasIndex(e => new { e.WerkGuid, e.ActieStandaardGuid })
                .HasName("fk_ref_actie_werk_actie");

            builder.HasIndex(e => new { e.WerkGuid, e.GebouwGuid, e.Volgorde, e.ActieMedewerkerGuid, e.ActieDatum })
                .HasName("so_actie");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieDatum)
                .HasColumnName("actie_datum")
                .HasColumnType("date");

            builder.Property(e => e.ActieEindtijd).HasColumnName("actie_eindtijd");

            builder.Property(e => e.ActieMedewerkerGuid)
                .IsRequired()
                .HasColumnName("actie_medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieStandaardGuid)
                .HasColumnName("actie_standaard_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.ActieStarttijd).HasColumnName("actie_starttijd");

            builder.Property(e => e.AfgehandeldMedewerkerGuid)
                .HasColumnName("afgehandeld_medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AfgehandeldOp)
                .HasColumnName("afgehandeld_op")
                .HasColumnType("date");

            builder.Property(e => e.GebouwGuid)
                .HasColumnName("gebouw_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

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

            builder.Property(e => e.Omschrijving)
                .IsRequired()
                .HasColumnName("omschrijving")
                .HasMaxLength(80);

            builder.Property(e => e.OmschrijvingUitgebreid).HasColumnName("omschrijving_uitgebreid");

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");

            builder.Property(e => e.WerkGuid)
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.HasOne(d => d.ActieMedewerkerGu)
                .WithMany(p => p.ActieActieMedewerkerGu)
                .HasForeignKey(d => d.ActieMedewerkerGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_medewerker_actie_actie");

            builder.HasOne(d => d.AfgehandeldMedewerkerGu)
                .WithMany(p => p.ActieAfgehandeldMedewerkerGu)
                .HasForeignKey(d => d.AfgehandeldMedewerkerGuid)
                .HasConstraintName("ref_medewerker_actie_afgehandeld");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.Actie)
                .HasForeignKey(d => d.GebouwGuid)
                .HasConstraintName("ref_gebouw_actie_integriteit");

            builder.HasOne(d => d.WerkGu)
                .WithMany(p => p.Actie)
                .HasForeignKey(d => d.WerkGuid)
                .HasConstraintName("ref_werk_actie");
        }
    }
}
