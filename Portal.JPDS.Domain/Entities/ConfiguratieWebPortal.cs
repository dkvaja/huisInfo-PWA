using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ConfiguratieWebPortal : BaseEntity
    {
        public string NaamTabBrowserIntern { get; set; }
        public string NaamTabBrowserExtern { get; set; }
        public string StandaardLogoWebPortal { get; set; }
        public string AchtergrondLoginWebPortal { get; set; }
        public string StandaardTekstAanvragenOpties { get; set; }
        public string StandaardTekstBestellenOpties { get; set; }
        public string StandaardTekstBestellenOptiesDigitaalOndertekenen { get; set; }
        public string StandaardTekstBestellenOptiesDigitaalOndertekenenIdin { get; set; }
        public string StandaardTekstBedanktVoorAanvraag { get; set; }
        public string StandaardTekstBedanktVoorBestelling { get; set; }
        public string StandaardTekstBedanktVoorBestellingDigitaalOndertekenen { get; set; }
        public string StandaardTekstNieuweOfferte { get; set; }
        public string StandaardNotificatieEmailSjabloonGuid { get; set; }
        public string StandaardInloggegevensSjabloonGuid { get; set; }
        public string StandaardPvoSjabloonGuid { get; set; }
        public string StandaardResetWachtwoordSjabloonGuid { get; set; }
        public string AfzenderEmailAccountGuid { get; set; }
        public byte? DigitaalOndertekenenProvider { get; set; }
        public string DigitaalOndertekenenApiUrl { get; set; }
        public string DigitaalOndertekenenApiToken { get; set; }
        public string DigitaalOndertekenenApiSecret { get; set; }
        public string DigitaalOndertekenenAccessToken { get; set; }
        public string DigitaalOndertekenenAccessSecret { get; set; }

        public string AfgehandeldKoperHuurderMeldingStatusGuid { get; set; }
        public string AfgehandeldOplosserMeldingStatusGuid { get; set; }

        public bool StandaardOfflineMode { get; set; }
        public int StandaardOfflineInspectieOpslaanAantalDagen { get; set; }
        public int StandaardOfflineOpleveringOpslaanAantalDagen { get; set; }
        public int StandaardOfflineTweedeHandtekeningOpslaanAantalDagen { get; set; }
        public int StandaardOfflineVoorschouwOpslaanAantalDagen { get; set; }


        public virtual EmailAccount AfzenderEmailAccountGu { get; set; }
        public virtual Sjabloon StandaardNotificatieEmailSjabloonGu { get; set; }
        public virtual Sjabloon StandaardResetWachtwoordSjabloonGu { get; set; }
    }
}