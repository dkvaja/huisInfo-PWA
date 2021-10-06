using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(e => e.Guid)
                  .HasName("contact_pk");

            builder.ToTable("contact");

            builder.HasIndex(e => e.CommunicatieGuid, "fk_ref_communicatie_contact");

            builder.HasIndex(e => e.GebouwGuid, "fk_ref_gebouw_contact");

            builder.HasIndex(e => e.KoperHuurderGuid, "fk_ref_koper_huurder_contact");

            builder.HasIndex(e => e.OrganisatieGuid, "fk_ref_organisatie_contact");

            builder.HasIndex(e => e.PersoonGuid, "fk_ref_persoon_contact");

            builder.HasIndex(e => e.RelatieGuid, "fk_ref_relatie_contact");

            builder.Property(e => e.Guid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("guid");

            builder.Property(e => e.Bestandsnaam)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("bestandsnaam");

            builder.Property(e => e.CommunicatieGuid)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("communicatie_guid");

            builder.Property(e => e.Contact1)
                .HasColumnName("contact")
                .HasDefaultValueSql("('1')")
                .ValueGeneratedNever();

            builder.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("email");

            builder.Property(e => e.EmailBcc)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email_bcc");

            builder.Property(e => e.EmailBericht).HasColumnName("email_bericht");

            builder.Property(e => e.EmailCc)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("email_cc");

            builder.Property(e => e.EmailVerzonden)
                .HasColumnType("datetime")
                .HasColumnName("email_verzonden");

            builder.Property(e => e.Formeel)
                .IsRequired()
                .HasColumnName("formeel")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.GebouwGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("gebouw_guid");

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

            builder.Property(e => e.KoperHuurderGuid)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("koper_huurder_guid");

            builder.Property(e => e.Naam)
                .HasMaxLength(100)
                .HasColumnName("naam")
                .HasComputedColumnSql("([dbo].[cff_proxy_contact_naam]([contact],[organisatie_guid],[relatie_guid],[persoon_guid],[koper_huurder_guid],[gebouw_guid]))", false);

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

            builder.HasOne(d => d.CommunicatieGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CommunicatieGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_communicatie_contact");

            builder.HasOne(d => d.GebouwGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.GebouwGuid)
                .HasConstraintName("ref_gebouw_contact");

            builder.HasOne(d => d.KoperHuurderGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.KoperHuurderGuid)
                .HasConstraintName("ref_koper_huurder_contact");

            builder.HasOne(d => d.OrganisatieGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.OrganisatieGuid)
                .HasConstraintName("ref_organisatie_contact");

            builder.HasOne(d => d.PersoonGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.PersoonGuid)
                .HasConstraintName("ref_persoon_contact");

            builder.HasOne(d => d.RelatieGu)
                .WithMany(p => p.Contacts)
                .HasForeignKey(d => d.RelatieGuid)
                .HasConstraintName("ref_relatie_contact");
        }
    }
}
