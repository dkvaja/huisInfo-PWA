using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    /// <summary>
    /// 0: Huisinfo App (Kopersbegeleiding/Bouwkeuzes)
    /// 1: Aftercare App (MeldingenRegistratie)
    /// 2: Survey App (Opname)
    /// </summary>
    public enum Apps
    {
        BuyerGuide = 0,
        Aftercare = 1,
        Survey = 2,
        ConstructionQuality = 3,
        ResolverModule = 4
    }

    public enum AccountType
    {
        Relation = 0,
        Buyer = 1,
        Employee = 2
    }

    /// <summary>
    /// Login login_rol
    /// </summary>
    public enum LoginRole
    {
        //For now in dutch.. need to translate soon
        geinteresseerde = 0,

        optant = 1,

        koper = 2,

        huurder = 3,

        gebouw_gebruiker = 4,

        makelaar = 5,

        kopersbegeleider = 6,

        opdrachtgever = 7,

        showroom = 8,

        onderaannemer = 9,

        toeschouwer = 10,

        vastgoedbeheerder = 11,

        vve = 12,

        vve_beheerder = 13,

        servicedienst = 14,
    }


    /// <summary>
    /// Prioriteit
    /// </summary>
    public enum Priority
    {
        Low = 0,
        Normal = 1,
        High = 2,
    }

    /// <summary>
    /// Melder gebouw_beheerder_rol
    /// </summary>
    public enum BuildingManagerRole
    {
        //Need to change in English
        gebouw_gebruiker = 0,
        opdrachtgever = 1,
        vve = 2,
        vve_beheerder = 3,
        vastgoed_beheerder = 4,
        medewerker = 5,
        overige = 9
    }

    /// <summary>
    /// Werk werk_soort
    /// </summary>
    public enum ConstructionType
    {
        //Need to change in English
        nieuwbouw = 0,
        onderhoud = 1,
        renovatie = 2,
        beheer = 3,
        aan_en_verbouw = 4,
        transformatie = 5
    }

    public enum SurveyType
    {
        /// <summary>
        /// opname_soort_voorschouw (2 weeks before the actual delivery)
        /// </summary>
        PreDelivery = 0,
        /// <summary>
        /// opname_soort_oplevering 
        /// </summary>
        Delivery = 1,
        /// <summary>
        /// opname_soort_inspectie
        /// </summary>
        Inspection = 2
    }

    public enum SurveyStatus
    {
        /// <summary>
        /// opname_status_nieuw
        /// </summary>
        New = 0,
        /// <summary>
        /// opname_status_in_behandeling
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// opname_status_voltooid
        /// </summary>
        Completed = 2,
        /// <summary>
        /// opname_status_verzonden (only applicable to delivery(oplevering))
        /// </summary>
        Sent = 3
    }

    public enum SurveySignatoryType
    {
        Executor = 0,
        BuyerRenter1 = 1,
        BuyerRenter2 = 2
    }

    public enum RepairRequestReceivedVia
    {
        Telefoon = 0,
        Email = 1,
        Website = 2,
        App = 3,
        Brief = 4,
        Formulier = 5,
        Fax = 6,
        Medewerker = 7,
    }

    public enum ResolverStatus
    {
        /// <summary>
        /// nieuw
        /// </summary>
        New = 0,
        /// <summary>
        /// ingelicht
        /// </summary>
        Informed = 1,
        /// <summary>
        /// in_afwachting
        /// </summary>
        Pending = 2,
        /// <summary>
        /// afgewezen
        /// </summary>
        TurnedDown = 3,
        /// <summary>
        /// afgehandeld
        /// </summary>
        Completed = 4
    }

    ////For linking attachment to different tables... see ticket https://jpds.atlassian.net/browse/WPJPBM-78 for list
    public enum AttachmentLinkedTo
    {
        /// <summary>
        /// 0 = organisatie (fill foreign key: organisatie_guid)
        /// </summary>
        Organisation = 0,
        /// <summary>
        /// 1 = werk (fill foreign key: werk_guid)
        /// </summary>
        Project = 1,
        /// <summary>
        /// 2 = gebouw (fill foreign key: gebouw_guid)
        /// </summary>
        Building = 2,
        /// <summary>
        /// 3 = optie_standaard (fill foreign key: optie_standaard_guid)
        /// </summary>
        StandardOption = 3,
        /// <summary>
        /// 4 = optie_gekozen (fill foreign key: optie_gekozen_guid)
        /// </summary>
        SelectedOption = 4,
        /// <summary>
        /// 5 = melding (fill foreign key: melding_guid)
        /// </summary>
        RepairRequest = 5,
        /// <summary>
        /// 6 = communicatie (fill foreign key: communicatie_guid
        /// </summary>
        Communication = 6,
        /// <summary>
        /// 7 = software_verbeterpunt (not used anymore)
        /// </summary>
        SoftwareImporvementPoint = 7,
        /// <summary>
        /// 8 = persoon (fill foreign key: persoon_guid)
        /// </summary>
        Person = 8,
        /// <summary>
        /// 9 = chat_bericht (fill foreign key: chat_bericht_guid)
        /// </summary>
        ChatMessage = 9,
        /// <summary>
        /// 10 = Dossier
        /// </summary>
        Dossier = 10
    }

    public enum SelectedOptionType
    {
        Standard = 0,
        Individual = 1
    }

    public enum SelectedOptionStatus
    {
        /// <summary>
        /// 0 = voorlopig
        /// </summary>
        Provisional = 0,
        /// <summary>
        /// 1 = nieuw 
        /// </summary>
        New = 1,
        /// <summary>
        /// 2 = offerte
        /// </summary>
        Quotation = 2,
        /// <summary>
        /// 3 = definitief
        /// </summary>
        Definite = 3,
        /// <summary>
        /// 4 = vervallen
        /// </summary>
        Cancelled = 4
    }

    public enum SelectedOptionSubStatus
    {
        Provisional = 0,
        ToBeDisplayedForSelection = 10,
        InShoppingCart = 20,
        NewOrToBeJudged = 100,
        Quotation = 200,
        OrderedOnline = 210,
        SentToBeDigitallySigned = 220,
        SignedDocumentsToBeReviewed = 230,
        Definite = 300,
        Cancelled = 400,
        Denied = 410
    }

    public enum QuotationSignatureType
    {
        Manual = 10,
        DigitalStandard = 20,
        Digital_iDIN = 30,
        NotApplicable = 99
    }

    public enum OrganisationSearchMethod
    {
        /// <summary>
        /// 0 = Betrokkene_van_het_werk
        /// </summary>
        InvolvedInTheProject = 0,
        /// <summary>
        /// 1 = Eigen_organisatie
        /// </summary>
        Own = 1,
        /// <summary>
        /// 2 = Vrij te selecteren organisatie
        /// </summary>
        All = 2
    }


    /// <summary>
    /// communicatie_soort
    /// </summary>
    public enum CommunicationMethod
    {
        /// <summary>
        /// Correspondentie
        /// </summary>
        Correspondence = 0,
        /// <summary>
        /// Email
        /// </summary>
        Email = 1,
        /// <summary>
        /// Telefoongesprek
        /// </summary>
        Telephone_Call = 2,
        /// <summary>
        /// Gesprek
        /// </summary>
        Conversation = 3,
        /// <summary>
        /// Bezoek
        /// </summary>
        Visit = 4,
        /// <summary>
        /// Vergadering
        /// </summary>
        Meeting = 5,
        /// <summary>
        /// Upload
        /// </summary>
        Upload = 6,
        /// <summary>
        /// Overige
        /// </summary>
        Others = 7
    }

    /// <summary>
    /// communicatie_type
    /// </summary>
    public enum CommunicationType
    {
        /// <summary>
        /// Inkomend
        /// </summary>
        Incoming = 0,
        /// <summary>
        /// Uitgaand
        /// </summary>
        Outgoing = 1,
        /// <summary>
        /// N.v.t
        /// </summary>
        Not_Applicable = 2,
    }

    /// <summary>
    /// communicatie_status
    /// </summary>
    public enum CommunicationStatus
    {
        /// <summary>
        /// Concept
        /// </summary>
        Draft = 0,
        /// <summary>
        /// Verzonden
        /// </summary>
        Sent = 1,
        /// <summary>
        /// Geregistreerd
        /// </summary>
        Registered = 2,
    }

    /// <summary>
    /// contact_geadresseerde
    /// </summary>
    public enum ContactAddressee
    {
        /// <summary>
        /// Organisatie
        /// </summary>
        Organisation = 0,
        /// <summary>
        /// relatie
        /// </summary>
        Relation = 1,
        /// <summary>
        /// persoon
        /// </summary>
        Person = 2,
        /// <summary>
        /// koper/huurder
        /// </summary>
        Buyer = 3,
        /// <summary>
        /// gebouw
        /// </summary>
        Building = 4
    }

    /// <summary>
    /// Dossier Status
    /// </summary>
    public enum DossierStatus
    {
        Draft = 0,
        Open = 1,
        Closed = 2,
    }

    /// <summary>
    /// Dossier Or File Actions
    /// </summary>
    public enum DossierOrFileActions
    {
        DossierCreate = 0,
        DossierEdit = 1,
        DossierArchive = 2,
        DossierComplete = 3,
        FileUpload = 4,
        FileDownload = 5,
        FileArchive = 6
    }

    public static class ScriveDocumentStatus
    {
        public const string Preparation = "preparation";
        public const string Pending = "pending";
        public const string Closed = "closed";
        public const string Canceled = "canceled";
        public const string Timedout = "timedout";
        public const string Rejected = "rejected";
        public const string DocumentError = "document_error";
    }

    public static class PolicyConstants
    {
        public const string FullAccess = "FullAccess";
        public const string ViewOnlyAccess = "ViewOnlyAccess";
        public const string ResetPasswordOnly = "ResetPasswordOnly";
    }
    public static class PolicyClaimType
    {
        public const string Access = "Access";
    }

    public static class PolicyClaimValue
    {
        public const string ViewOnly = "ViewOnly";
        public const string FullAccess = "Full";
        public const string ResetPasswordOnly = "ResetPasswordOnly";
    }

    public static class WorkOrderEmailNotificationText
    {
        public const string Create = "Hierbij ontvangt u van ons een werkbon i.v.m. uit te voeren werkzaamheden.";
        public const string Reminder = "Hierbij ontvangt u van ons een herinnering voor een eerder gestuurde werkbon voor uit te voeren werkzaamheden.";
        public const string Complete = "De genoemde werkzaamheden van de werkbon zijn afgerond.";
        public const string TurnedDown = "De genoemde werkzaamheden zullen niet worden uitgevoerd want de werkbon is afgewezen.";
        public const string Rework = "Er is meer werk nodig om de werkzaamheden van de werkbon te voltooien.";
    }

    public static class LoginRoles
    {
        public const string SiteManager = "Uitvoerder";
        public const string SubContractor = "Onderaannemer";
        public const string BuyerOrRenter = "Koper/Huurder";
        public const string BuyersGuide = "Kopers-/Huurders-/Klantbegeleider";
        public const string PropertyManager  = "Vastgoedbeheerder";
        public const string AftercareEmployee = "Nazorg medewerker";
        public const string Showroom = "Showroom";
        public const string Spectator = "Toeschouwer";
    }

    public static class LoginModules
    {
        public const string ConstructionQuality = "Bouwkwaliteit";
        public const string BuyerGuide = "Kopersbegeleiding";
        public const string Survey = "Opname App";
        public const string Aftercare = "Nazorg & Service";
    }

    public enum DossierResponseTypes
    {
        Success = 0,
        Unauthorized = 1,
        UpdateFailed = 2
    }

    /// <summary>
    /// overdrachtsmoment_uitvoering_nazorg
    /// </summary>
    public enum AfterCareTransferMoment 
    {
        /// <summary>
        /// na_datum_oplevering
        /// </summary>
        AfterDateOfDelivery = 0,
        /// <summary>
        /// na_datum_tweede_handtekening 
        /// </summary>
        AfterDateOfSecondSignature = 1,
        /// <summary>
        /// na_datum_einde_onderhoudstermijn
        /// </summary>
        AfterEndDateOfMaintenancePeriod = 2
    }
}
