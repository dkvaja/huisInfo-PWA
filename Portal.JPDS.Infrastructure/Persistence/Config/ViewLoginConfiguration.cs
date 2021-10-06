using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ViewLoginConfiguration : IEntityTypeConfiguration<ViewLogin>
    {
        public void Configure(EntityTypeBuilder<ViewLogin> builder)
        {

            builder.HasNoKey();

            builder.ToView("view_login");

            builder.Property(e => e.Actief).HasColumnName("actief");

            builder.Property(e => e.CentralGuid).HasColumnName("central_guid");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");

            builder.Property(e => e.Gebruikersnaam)
                .HasMaxLength(40)
                .HasColumnName("gebruikersnaam");

            builder.Property(e => e.IsSuperAdmin).HasColumnName("is_super_admin");

            builder.Property(e => e.KoperHuurderGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("koper_huurder_guid");

            builder.Property(e => e.LaatsteLogin)
                .HasColumnType("datetime")
                .HasColumnName("laatste_login");

            builder.Property(e => e.LoginAccountVoor).HasColumnName("login_account_voor");

            builder.Property(e => e.Guid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.MedewerkerGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("medewerker_guid");

            builder.Property(e => e.Naam)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("naam");

            builder.Property(e => e.OptIn).HasColumnName("opt_in");

            builder.Property(e => e.OrganisatieGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("organisatie_guid");

            builder.Property(e => e.PersoonGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("persoon_guid");

            builder.Property(e => e.RelatieGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("relatie_guid");

            builder.Property(e => e.VorigWachtwoord)
                .HasMaxLength(250)
                .HasColumnName("vorig_wachtwoord");

            builder.Property(e => e.VorigWachtwoordGeresetOp)
                .HasColumnType("datetime")
                .HasColumnName("vorig_wachtwoord_gereset_op");

            builder.Property(e => e.VorigWachtwoordHash)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("vorig_wachtwoord_hash");

            builder.Property(e => e.Wachtwoord)
                .HasMaxLength(250)
                .HasColumnName("wachtwoord");

            builder.Property(e => e.WachtwoordHash)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("wachtwoord_hash");

            builder.Property(e => e.WijzigWachtwoordLinkAangemaakt)
                .HasColumnType("datetime")
                .HasColumnName("wijzig_wachtwoord_link_aangemaakt");
        }
    }
}
