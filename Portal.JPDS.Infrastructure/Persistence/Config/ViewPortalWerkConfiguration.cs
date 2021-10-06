using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalWerkConfiguration : IEntityTypeConfiguration<ViewPortalWerk>
    {
        public void Configure(EntityTypeBuilder<ViewPortalWerk> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_werk");

            builder.Property(e => e.AantalObjecten).HasColumnName("aantal_objecten");

            builder.Property(e => e.AchtergrondWebPortal)
                .HasColumnName("achtergrond_web_portal")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.AlgemeneInfo).HasColumnName("algemene_info");

            builder.Property(e => e.DatumEindBouw)
                .HasColumnName("datum_eind_bouw")
                .HasColumnType("date");

            builder.Property(e => e.DatumStartBouw)
                .HasColumnName("datum_start_bouw")
                .HasColumnType("date");

            builder.Property(e => e.DatumStartVerkoop)
                .HasColumnName("datum_start_verkoop")
                .HasColumnType("date");

            builder.Property(e => e.Logo)
                .HasColumnName("logo")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Plaats)
                .HasColumnName("plaats")
                .HasMaxLength(40);

            builder.Property(e => e.WerkFase)
                .IsRequired()
                .HasColumnName("werk_fase")
                .HasMaxLength(40);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.WerkSoort)
                .HasColumnName("werk_soort")
                .HasMaxLength(250);

            builder.Property(e => e.WerkType)
                .HasColumnName("werk_type")
                .HasMaxLength(250);

            builder.Property(e => e.Werknaam)
                .IsRequired()
                .HasColumnName("werknaam")
                .HasMaxLength(100);

            builder.Property(e => e.Werknummer)
                .IsRequired()
                .HasColumnName("werknummer")
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
