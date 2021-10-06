using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class ScriveApiCallbackModel
    {
        public long document_id { get; set; }
        public string document_json { get; set; }
        public bool document_signed_and_sealed { get; set; }
    }

    public class ScriveSigningModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public List<ScriveParty> parties { get; set; }
        public ScriveFile file { get; set; }
        public ScriveFile sealed_file { get; set; }
        public List<object> author_attachments { get; set; }
        public DateTime ctime { get; set; }
        public DateTime mtime { get; set; }
        public object timeout_time { get; set; }
        public object auto_remind_time { get; set; }
        public string status { get; set; }
        public int days_to_sign { get; set; }
        public object days_to_remind { get; set; }
        public ScriveDisplayOptions display_options { get; set; }
        public string invitation_message { get; set; }
        public string confirmation_message { get; set; }
        public string lang { get; set; }
        public string api_callback_url { get; set; }
        public int object_version { get; set; }
        public string access_token { get; set; }
        public string timezone { get; set; }
        public List<object> tags { get; set; }
        public bool is_template { get; set; }
        public bool is_saved { get; set; }
        public bool is_shared { get; set; }
        public bool is_trashed { get; set; }
        public bool is_deleted { get; set; }
        public ScriveViewer viewer { get; set; }
        public object shareable_link { get; set; }
        public object template_id { get; set; }
        public bool from_shareable_link { get; set; }
    }
    public class ScriveField
    {
        public string type { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string value { get; set; }
        public bool is_obligatory { get; set; }
        public bool should_be_filled_by_sender { get; set; }
        public List<object> placements { get; set; }
        public bool? editable_by_signatory { get; set; }
    }

    public class ScriveParty
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public bool is_author { get; set; }
        public bool is_signatory { get; set; }
        public string signatory_role { get; set; }
        public List<ScriveField> fields { get; set; }
        public object consent_module { get; set; }
        public int sign_order { get; set; }
        public DateTime? sign_time { get; set; }
        public object seen_time { get; set; }
        public object read_invitation_time { get; set; }
        public DateTime? rejected_time { get; set; }
        public string rejection_reason { get; set; }
        public object sign_success_redirect_url { get; set; }
        public object reject_redirect_url { get; set; }
        public string email_delivery_status { get; set; }
        public string mobile_delivery_status { get; set; }
        public string confirmation_email_delivery_status { get; set; }
        public bool has_authenticated_to_view { get; set; }
        public object csv { get; set; }
        public string delivery_method { get; set; }
        public string authentication_method_to_view { get; set; }
        public string authentication_method_to_view_archived { get; set; }
        public string authentication_method_to_sign { get; set; }
        public string confirmation_delivery_method { get; set; }
        public string notification_delivery_method { get; set; }
        public bool allows_highlighting { get; set; }
        public bool hide_personal_number { get; set; }
        public bool can_forward { get; set; }
        public List<object> attachments { get; set; }
        public List<object> highlighted_pages { get; set; }
        public object api_delivery_url { get; set; }
    }

    public class ScriveFile
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class ScriveDisplayOptions
    {
        public bool show_header { get; set; }
        public bool show_pdf_download { get; set; }
        public bool show_reject_option { get; set; }
        public bool allow_reject_reason { get; set; }
        public bool show_footer { get; set; }
        public bool document_is_receipt { get; set; }
        public bool show_arrow { get; set; }
    }

    public class ScriveViewer
    {
        public string signatory_id { get; set; }
        public string role { get; set; }
    }
}
