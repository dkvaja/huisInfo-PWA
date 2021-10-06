using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class DigitalSigningApiConfigModel
    {
        public DigitalSigningApiConfigModel() { }
        public DigitalSigningApiConfigModel(ConfiguratieWebPortal entity)
        {
            BaseUrl = entity.DigitaalOndertekenenApiUrl;
            ApiToken = entity.DigitaalOndertekenenApiToken;
            ApiSecret = entity.DigitaalOndertekenenApiSecret;
            AccessToken = entity.DigitaalOndertekenenAccessToken;
            AccessSecret = entity.DigitaalOndertekenenAccessSecret;
        }

        public string BaseUrl { get; set; }
        public string ApiToken { get; set; }
        public string ApiSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
    }
}
