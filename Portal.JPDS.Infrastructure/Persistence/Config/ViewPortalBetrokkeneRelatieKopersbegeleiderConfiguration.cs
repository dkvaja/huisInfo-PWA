using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewPortalBetrokkeneRelatieKopersbegeleiderConfiguration : IEntityTypeConfiguration<ViewPortalBetrokkeneRelatieKopersbegeleider>
    {
        public void Configure(EntityTypeBuilder<ViewPortalBetrokkeneRelatieKopersbegeleider> builder)
        {
            builder.HasNoKey();

            builder.ToView("view_portal_betrokkene_relatie_kopersbegeleider");

            builder.Property(e => e.BetrokkeneGuid)
                .IsRequired()
                .HasColumnName("betrokkene_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.BetrokkeneRelatieGuid)
                .IsRequired()
                .HasColumnName("betrokkene_relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Foto)
                .HasColumnName("foto")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Functie)
                .HasColumnName("functie")
                .HasMaxLength(40);

            builder.Property(e => e.LoginGuid)
                .IsRequired()
                .HasColumnName("login_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Mobiel)
                .HasColumnName("mobiel")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100);

            builder.Property(e => e.OrganisatieGuid)
                .IsRequired()
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonGuid)
                .IsRequired()
                .HasColumnName("persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .IsRequired()
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Telefoon)
                .HasColumnName("telefoon")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.VolledigeNaam)
                .HasColumnName("volledige_naam")
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.WerkGuid)
                .IsRequired()
                .HasColumnName("werk_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Volgorde).HasColumnName("volgorde");
        }
    }
}
