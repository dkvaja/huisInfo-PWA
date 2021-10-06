using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public AppSettingAttachmentHeaders AttachmentHeaders { get; set; }
        public AppSettingsFileStore FileStoreSettings { get; set; }
        public SendDailyEmailNotificationsSetting SendDailyEmailNotificationsSetting { get; set; }
        public string SiteUrl { get; set; }
        public string AppName { get; set; }
        public string AppShortName { get; set; }
        public string FeedbackEmail { get; set; }
        public bool CorsAllowAll { get; set; }
    }

    public class SendDailyEmailNotificationsSetting
    {
        public bool Enabled { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan ResolverOrSiteManagerNotificationTime { get; set; }
    }

    public class AppSettingsFileStore
    {
        public string BasePath { get; set; }
        public string StandardOptionAttachmentsLocation { get; set; }
    }

    public class AppSettingAttachmentHeaders
    {
        public string PictureUpload { get; set; }
        public string DocumentUpload { get; set; }
        public string ChatMessageUpload { get; set; }
        public string Quotations { get; set; }
        public string QuotationDrawing { get; set; }
        public string SignedDocumentUpload { get; set; }
    }

}
