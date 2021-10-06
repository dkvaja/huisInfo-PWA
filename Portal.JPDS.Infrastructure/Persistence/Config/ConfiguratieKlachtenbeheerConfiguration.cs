using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Config
{
    public class ConfiguratieKlachtenbeheerConfiguration : IEntityTypeConfiguration<ConfiguratieKlachtenbeheer>
    {
        public void Configure(EntityTypeBuilder<ConfiguratieKlachtenbeheer> builder)
        {
            builder.HasKey(e => e.Guid)
                    .HasName("configuratie_klachtenbeheer_pk");

            builder.ToTable("configuratie_klachtenbeheer");

            builder.HasIndex(e => e.StandaardAfhandelingstermijnGuid)
                .HasName("fk_ref_afhandelingstermijn_configuratie_klachtenbeheer");

            builder.HasIndex(e => e.StandaardMeldingStatusGuid)
                .HasName("fk_ref_melding_status_configuratie_klachtenbeheer");

            builder.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AardVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("aard_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.AfmeldingsverzoekSturenNa).HasColumnName("afmeldingsverzoek_sturen_na");

            builder.Property(e => e.EmailNazorg)
                .HasColumnName("email_nazorg")
                .HasMaxLength(200)
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

            builder.Property(e => e.LocatieVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("locatie_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.MeldingNummersPerJaar)
                .IsRequired()
                .HasColumnName("melding_nummers_per_jaar")
                .HasDefaultValueSql("('1')");

            builder.Property(e => e.OorzaakVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("oorzaak_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.OpmaakMeldingNummer)
                .HasColumnName("opmaak_melding_nummer")
                .HasMaxLength(20);

            builder.Property(e => e.OpmerkingVersturenOnlineMelding)
                .HasColumnName("opmerking_versturen_online_melding")
                .HasMaxLength(4000);

            builder.Property(e => e.OverdrachtsmomentUitvoeringNazorg)
                    .HasColumnName("overdrachtsmoment_uitvoering_nazorg")
                    .HasDefaultValueSql("((2))")
                    .ValueGeneratedNever();

            builder.Property(e => e.ProductDienstSub1VerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("product_dienst_sub1_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.ProductDienstSub2VerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("product_dienst_sub2_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.ProductDienstVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("product_dienst_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.StandaardAfhandelingstermijnGuid)
                .IsRequired()
                .HasColumnName("standaard_afhandelingstermijn_guid")
                .HasMaxLength(40)
                .IsUnicode(false);


            builder.Property(e => e.StandaardMeldingStatusGuid)
                .HasColumnName("standaard_melding_status_guid")
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.StandaardRegistratiebevestigingSjabloonGuid)
                    .HasColumnName("standaard_registratiebevestiging_sjabloon_guid")
                    .HasMaxLength(40)
                    .IsUnicode(false);

            builder.Property(e => e.TypeVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("type_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.Property(e => e.VeroorzakerVerplichtBijAfhandelingMelding)
                .IsRequired()
                .HasColumnName("veroorzaker_verplicht_bij_afhandeling_melding")
                .HasDefaultValueSql("('0')");

            builder.HasOne(d => d.StandaardAfhandelingstermijnGu)
                .WithMany(p => p.ConfiguratieKlachtenbeheer)
                .HasForeignKey(d => d.StandaardAfhandelingstermijnGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ref_afhandelingstermijn_configuratie_klachtenbeheer");

            builder.HasOne(d => d.StandaardMeldingStatusGu)
                .WithMany(p => p.ConfiguratieKlachtenbeheer)
                .HasForeignKey(d => d.StandaardMeldingStatusGuid)
                .HasConstraintName("ref_melding_status_configuratie_klachtenbeheer");
        }
    }
}
