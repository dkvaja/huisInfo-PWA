using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("login_pk");

            builder.ToTable("login");

            builder.HasIndex(e => e.Email)
                .HasName("email")
                .IsUnique();

            builder.HasIndex(e => e.KoperHuurderGuid)
                .HasName("fk_ref_koper_huurder_login");

            builder.HasIndex(e => e.MedewerkerGuid)
                .HasName("fk_ref_medewerker_login");

            builder.HasIndex(e => e.PersoonGuid)
                .HasName("fk_ref_persoon_login");

            builder.HasIndex(e => e.RelatieGuid)
                .HasName("fk_ref_relatie_login");

            builder.HasIndex(e => new { e.Gebruikersnaam, e.Email })
                .HasName("so_login");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Actief)
                .IsRequired()
                .HasColumnName("actief")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.CentralLoginGuid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("central_login_guid");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Gebruikersnaam)
                .IsRequired()
                .HasColumnName("gebruikersnaam")
                .HasMaxLength(40);

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

            builder.Property(e => e.KoperHuurderGuid)
                .HasColumnName("koper_huurder_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.LaatsteLogin)
                .HasColumnName("laatste_login")
                .HasColumnType("datetime");

            builder.Property(e => e.LoginAccountVoor)
                .HasColumnName("login_account_voor")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.LoginRol)
                .HasColumnName("login_rol")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.MedewerkerGuid)
                .HasColumnName("medewerker_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Naam)
                .HasColumnName("naam")
                .HasMaxLength(100);

            builder.Property(e => e.OptIn)
                .IsRequired()
                .HasColumnName("opt_in")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.OrganisatieGuid)
                .HasColumnName("organisatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.PersoonGuid)
                .HasColumnName("persoon_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.RelatieGuid)
                .HasColumnName("relatie_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.VorigWachtwoord)
                .HasColumnName("vorig_wachtwoord")
                    .HasMaxLength(100);

            builder.Property(e => e.VorigWachtwoordGeresetOp)
                .HasColumnName("vorig_wachtwoord_gereset_op")
                .HasColumnType("datetime");

            builder.Property(e => e.Verwijderd).HasColumnName("verwijderd");

            builder.Property(e => e.Wachtwoord)
                .HasColumnName("wachtwoord")
                .HasMaxLength(100);

            builder.Property(e => e.WijzigWachtwoordLinkAangemaakt)
                   .HasColumnName("wijzig_wachtwoord_link_aangemaakt")
                   .HasColumnType("datetime");

            builder.Property(e => e.OfflineMode).HasColumnName("offline_mode");
            builder.Property(e => e.OfflineInspectieOpslaanAantalDagen).HasColumnName("offline_inspectie_opslaan_aantal_dagen");
            builder.Property(e => e.OfflineOpleveringOpslaanAantalDagen).HasColumnName("offline_oplevering_opslaan_aantal_dagen");
            builder.Property(e => e.OfflineTweedeHandtekeningOpslaanAantalDagen).HasColumnName("offline_tweede_handtekening_opslaan_aantal_dagen");
            builder.Property(e => e.OfflineVoorschouwOpslaanAantalDagen).HasColumnName("offline_voorschouw_opslaan_aantal_dagen");



            builder.HasOne(d => d.KoperHuurderGu)
                .WithMany(p => p.Login)
                .HasForeignKey(d => d.KoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_login");

            builder.HasOne(d => d.MedewerkerGu)
                .WithMany(p => p.Login)
                .HasForeignKey(d => d.MedewerkerGuid)
                .HasConstraintName("ref_medewerker_login");

            builder.HasOne(d => d.PersoonGu)
                .WithMany(p => p.Login)
                .HasForeignKey(d => d.PersoonGuid)
                .HasConstraintName("ref_persoon_login");

            builder.HasOne(d => d.RelatieGu)
                .WithMany(p => p.Login)
                .HasForeignKey(d => d.RelatieGuid)
                .HasConstraintName("ref_relatie_login");
        }
    }
}
