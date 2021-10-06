using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class RepairRequestApiModel
    {
        public RepairRequestApiModel()
        {

        }
        public RepairRequestApiModel(Melding entity, string reporterName, string completedByBuyerStatusGuid)
        {
            RequestId = entity.Guid;
            BuildingId = entity.GebouwGuid;
            BuyerRenterName = entity.GebouwGebruikerNaam;
            SurveyId = entity.OpnameGuid;
            CompletionTermId = entity.AfhandelingstermijnGuid;
            TargetCompletionDate = entity.StreefdatumAfhandeling;
            IsRework = entity.Herstelwerkzaamheden;
            RepairRequestType = entity.MeldingSoortGu?.MeldingSoort1;
            CarryOutAsType = entity.MeldingSoortGu?.MeldingSoort1;
            Number = entity.MeldingNummer;
            Reporter = new RepairRequestReporterApiModel
            {
                Role = (BuildingManagerRole)entity.Melder,
                Name = reporterName,
                OrganisationName = entity.MelderOrganisatieGu?.Naam
            };
            Location = entity.MeldingLocatieGu?.Locatie ?? string.Empty;
            Desc = entity.Omschrijving ?? string.Empty;
            DetailDesc = entity.Melding1 ?? string.Empty;
            WorkOrderText = entity.TekstWerkbon ?? string.Empty;
            Date = entity.MeldingDatum;
            Overdue = entity.Afgehandeld != true && entity.StreefdatumAfhandeling.HasValue ? entity.StreefdatumAfhandeling.Value < DateTime.Now.Date : false;
            Priority = (Priority?)entity.Prioriteit;
            Status = entity.MeldingStatusGu?.MeldingStatus1 ?? string.Empty;
            Completed = entity.Afgehandeld == true;
            CompletedByBuyer = entity.MeldingStatusGuid.ToUpperInvariant() == completedByBuyerStatusGuid.ToUpperInvariant();
            if (entity.Bijlage != null)
            {
                Attachments = entity.Bijlage.OrderBy(x => x.Volgorde).Select(x => new RepairRequestAttachmentApiModel(x)).ToList();
            }
            Is48HoursReminder = entity.Afgehandeld != true && entity.StreefdatumAfhandeling.HasValue && !Overdue && (entity.StreefdatumAfhandeling.Value - DateTime.Now).TotalHours <= 48;
            PreferredAppointmentTime = entity.VoorkeurstijdstipAfspraak;
        }

        public RepairRequestApiModel(Melding entity, string reporterName, string completedByBuyerStatusGuid, IEnumerable<RepairRequestResolverApiModel> resolvers) : this(entity, reporterName, completedByBuyerStatusGuid)
        {
            if (resolvers != null && resolvers.Any())
            {
                Resolvers = resolvers.ToList();
            }
        }

        public RepairRequestApiModel(Melding entity, string reporterName, string completedByBuyerStatusGuid, IEnumerable<RepairRequestResolverApiModel> resolvers, Adres address) : this(entity, reporterName, completedByBuyerStatusGuid, resolvers)
        {
            if (address != null)
            {
                Address = new AddressModel(address);
            }
        }

        public RepairRequestApiModel(Melding entity, string reporterName, string completedByBuyerStatusGuid, IEnumerable<RepairRequestResolverApiModel> resolvers, Adres address, RepairRequestContactApiModel contactInfo, DateTime? secondSignatureDate) : this(entity, reporterName, completedByBuyerStatusGuid, resolvers, address)
        {
            SurveyType = (SurveyType?)entity.OpnameGu?.OpnameSoort;
            CarryOutAsTypeId = entity.MeldingSoortGuid;
            LocationId = entity.MeldingLocatieGuid;
            CompletionDate = entity.DatumOplevering;
            MaintenanceEndDate = entity.DatumEindeOnderhoudstermijn;
            WarrantyEndDate = entity.DatumEindeGarantietermijn;
            SettledOn = entity.DatumAfhandeling;
            ReceivedVia = (RepairRequestReceivedVia?)entity.MeldingOntvangenVia;
            ProductServiceId = entity.ProductDienstGuid;
            SubProductService1Id = entity.ProductDienstSub1Guid;
            SubProductService2Id = entity.ProductDienstSub2Guid;
            BookingPeriod = entity.Inboektermijn;
            LeadTime = entity.Doorlooptijd;
            AdoptedBy = entity.AangenomenDoorMedewerkerGu?.Ondertekenaar ?? string.Empty;
            ClientReference = entity.ReferentieOpdrachtgever;
            TypeId = entity.MeldingTypeGuid;
            NatureId = entity.MeldingAardGuid;
            CauseId = entity.MeldingOorzaakGuid;
            CauserId = entity.MeldingVeroorzakerGuid;
            CauserOrganisationId = entity.VeroorzakerOrganisatieGuid;
            PointOfContact = (BuildingManagerRole)entity.AanspreekpuntVoorOplosser;
            ContactInfo = contactInfo;
            SecondSignatureDate = secondSignatureDate;

            bool isAllHandled = entity.Oplosser.All(x => x.GecontroleerdOp != null);
            IsAllWorkOrderCompleted = isAllHandled && entity.Oplosser.Any(x => x.OplosserStatus == (int)ResolverStatus.Completed);
            IsAllWorkOrderDeclined = isAllHandled && !IsAllWorkOrderCompleted;
            CompletionText = entity.Oplossing;
            RejectionText = entity.RedenAfwijzing;
            IsCauserOrganisationVisible = entity.MeldingVeroorzakerGu?.Veroorzaker == "Aannemer" || entity.MeldingVeroorzakerGu?.Veroorzaker == "Onderaannemer";
        }

        public string RequestId { get; set; }
        public string BuildingId { get; set; }
        public string BuyerRenterName { get; set; }
        public string SurveyId { get; set; }
        public SurveyType? SurveyType { get; set; }
        public string CompletionTermId { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public bool IsRework { get; set; }
        public string CarryOutAsTypeId { get; set; }
        public string CarryOutAsType { get; set; }

        [Obsolete("Use CarryOutAsType instead")]
        public string RepairRequestType { get; set; }
        public string Number { get; set; }
        public RepairRequestReporterApiModel Reporter { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string Desc { get; set; }
        public string DetailDesc { get; set; }
        public string WorkOrderText { get; set; }
        public DateTime Date { get; set; }
        public bool Overdue { get; set; }
        public Priority? Priority { get; set; }
        public string Status { get; set; }
        public bool Completed { get; set; }
        public bool CompletedByBuyer { get; set; }
        public IEnumerable<RepairRequestAttachmentApiModel> Attachments { get; set; }
        public IEnumerable<RepairRequestResolverApiModel> Resolvers { get; set; }
        public AddressModel Address { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? MaintenanceEndDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime? SettledOn { get; set; }
        public RepairRequestReceivedVia? ReceivedVia { get; set; }
        public int? BookingPeriod { get; set; }
        public int? LeadTime { get; set; }
        public string ProductServiceId { get; set; }
        public string SubProductService1Id { get; set; }
        public string SubProductService2Id { get; set; }
        public string AdoptedBy { get; set; }
        public string ClientReference { get; set; }
        public string TypeId { get; set; }
        public string NatureId { get; set; }
        public string CauseId { get; set; }
        public string CauserId { get; set; }
        public string CauserOrganisationId { get; set; }
        public BuildingManagerRole? PointOfContact { get; set; }
        public RepairRequestContactApiModel ContactInfo { get; set; }
        public DateTime? SecondSignatureDate { get; set; }
        public bool IsAllWorkOrderCompleted { get; set; }
        public bool IsAllWorkOrderDeclined { get; set; }
        public string CompletionText { get; set; }
        public string RejectionText { get; set; }
        public bool Is48HoursReminder { get; set; }
        public bool IsCauserOrganisationVisible { get; set; }
        public string PreferredAppointmentTime { get; set; }
    }

    public class RepairRequestReporterApiModel
    {
        public BuildingManagerRole Role { get; set; }
        public string Name { get; set; }
        public string OrganisationName { get; set; }
    }

    public class RepairRequestResolverApiModel
    {
        public RepairRequestResolverApiModel() { }
        public RepairRequestResolverApiModel(Oplosser entity, Relatie relatie)
        {
            ResolverId = entity.Guid;
            ProjectId = entity.MeldingGu.WerkGuid;
            OrganisatonId = entity.OrganisatieGuid;
            RelationId = entity.RelatieGuid;          
            Name = entity.OrganisatieGu?.NaamOnderdeel ?? string.Empty;
            Email = entity.OrganisatieGu?.Email;
            Telephone = entity.OrganisatieGu?.Telefoon ?? string.Empty;
            RelationName = relatie?.PersoonGu?.Naam == "N.v.t." ? relatie?.AfdelingGu?.Afdeling1 : relatie?.PersoonGu?.Naam ?? string.Empty;
            RelationTelephone = relatie?.Doorkiesnummer;
            RelationMobile = relatie?.Mobiel ?? string.Empty;
            RelationEmail = relatie?.EmailZakelijk;
            RelationPersonId = relatie?.PersoonGuid;
            RelationFunctionName = relatie?.FunctieGu?.Functie1 ?? string.Empty;
            RelationPersonSex = relatie?.PersoonGu?.Geslacht;
            DateNotified = entity.DatumIngelicht;
            Status = (ResolverStatus?)entity.OplosserStatus;
            WorkOrderNumber = entity.Werkbonnummer;
            TargetCompletionDate = entity.StreefdatumAfhandeling ?? entity.MeldingGu?.StreefdatumAfhandeling;
            Overdue = (entity.OplosserStatus != (int?)ResolverStatus.Completed && entity.OplosserStatus != (int?)ResolverStatus.TurnedDown)
                && entity.StreefdatumAfhandeling.HasValue && entity.StreefdatumAfhandeling.Value < DateTime.Now.Date;
            IsRequiredHandling = (entity.OplosserStatus == (int?)ResolverStatus.Completed || entity.OplosserStatus == (int?)ResolverStatus.TurnedDown) && entity.GecontroleerdOp == null;
            Is48HoursReminder = entity.StreefdatumAfhandeling.HasValue && entity.OplosserStatus != (int?)ResolverStatus.Completed && entity.OplosserStatus != (int?)ResolverStatus.TurnedDown && entity.StreefdatumAfhandeling.Value >= DateTime.Now.Date && (entity.StreefdatumAfhandeling.Value - DateTime.Now).TotalHours <= 48;
        }

        public RepairRequestResolverApiModel(Oplosser entity, Relatie relatie, Gebouw building, RepairRequestContactApiModel contactInfo, DateTime? secondSignatureDate) : this(entity, relatie)
        {
            CompletionTermId = entity.AfhandelingstermijnGuid;
            Explanation = entity.Toelichting;
            Description = entity.Omschrijving ?? string.Empty;
            WorkOrderText = entity.TekstWerkbon;
            DateHandled = entity.DatumAfhandeling;
            if (entity.MeldingGu?.Bijlage != null)
            {
                SolutionImages = entity.MeldingGu?.Bijlage.Where(x => x.OplosserGuid == entity.Guid).OrderBy(x => x.Volgorde).Select(x => new RepairRequestAttachmentApiModel(x)).ToList();
            }
            SolutionText = entity.Oplossing;
            IsOnlyOrAllOthersWorkOrderCompleted = entity.MeldingGu?.Oplosser.All(x => x.Guid == entity.Guid || (x.OplosserStatus == (int)ResolverStatus.Completed || x.OplosserStatus == (int)ResolverStatus.TurnedDown) && x.GecontroleerdOp != null);
            CarryOutAsType = entity.MeldingSoortGu?.MeldingSoort1;
            CarryOutAsTypeId = entity.MeldingSoortGuid;
            RepairRequestCarryOutAsType = entity.MeldingGu?.MeldingSoortGu?.MeldingSoort1;
            RepairRequestRole = (BuildingManagerRole)entity.MeldingGu?.Melder;
            RepairRequestDate = entity.MeldingGu?.MeldingDatum;
            RepairRequestLocationId = entity.MeldingGu?.MeldingLocatieGuid;
            if (entity.MeldingGu?.Bijlage != null)
            {
                RepairRequestImages = entity.MeldingGu?.Bijlage.Where(x => x.OplosserGuid == null).OrderBy(x => x.Volgorde).Select(x => new RepairRequestAttachmentApiModel(x)).ToList();
            }
            CompletionDate = entity.MeldingGu?.DatumOplevering;
            MaintenanceEndDate = entity.MeldingGu?.DatumEindeOnderhoudstermijn;
            WarrantyEndDate = entity.MeldingGu?.DatumEindeGarantietermijn;
            PointOfContact = (BuildingManagerRole)entity.MeldingGu?.AanspreekpuntVoorOplosser;
            if (building?.AdresGu != null)
            {
                Address = new AddressModel(building.AdresGu);
            }
            RepairRequestNo = entity.MeldingGu?.MeldingNummer ?? string.Empty;
            BuildingId = building?.Guid;
            RepairRequestId = entity.MeldingGuid;
            HandledBy = entity.GecontroleerdDoorLoginGu?.Naam;
            HandledOn = entity.GecontroleerdOp;
            IsHandled = (entity.OplosserStatus == (int)ResolverStatus.Completed || entity.OplosserStatus == (int)ResolverStatus.TurnedDown) && entity.GecontroleerdOp != null;
            RejectionText = entity.RedenAfwijzing;
            ContactInfo = contactInfo;
            SecondSignatureDate = secondSignatureDate;
            if (!string.IsNullOrWhiteSpace(entity.VervolgVanWerkbonOplosserGuid) && entity.VervolgVanWerkbonOplosserGu != null)
            {
                PreviousWorkOrderDetails = new PreviousWorkOrderDetails
                {
                    Explantion = entity.VervolgVanWerkbonOplosserGu?.Toelichting,
                    HandledDate = entity.VervolgVanWerkbonOplosserGu?.GecontroleerdOp,
                    Solution = entity.VervolgVanWerkbonOplosserGu?.Oplossing,
                    WorkOrderNumber = entity.VervolgVanWerkbonOplosserGu?.Werkbonnummer,
                    WorkOrderText = entity.VervolgVanWerkbonOplosserGu?.TekstWerkbon,
                };
            }
            RepairRequestCompleted = entity.MeldingGu?.Afgehandeld == true;
        }

        public string ResolverId { get; set; }
        public string OrganisatonId { get; set; }      
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string RelationId { get; set; }
        public string RelationName { get; set; }    
        public string RelationTelephone { get; set; }
        public string RelationMobile { get; set; }
        public string RelationEmail { get; set; }
        public string RelationPersonId { get; set; }
        public byte? RelationPersonSex { get; set; }
        public string RelationFunctionName { get; set; }
        public DateTime? DateNotified { get; set; }
        public ResolverStatus? Status { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CompletionTermId { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public bool Overdue { get; set; }
        public string Explanation { get; set; }
        public string Description { get; set; }
        public string WorkOrderText { get; set; }
        public DateTime? DateHandled { get; set; }
        public string SolutionText { get; set; }
        public IEnumerable<RepairRequestAttachmentApiModel> SolutionImages { get; set; }
        public bool? IsOnlyOrAllOthersWorkOrderCompleted { get; set; }
        public string CarryOutAsType { get; set; }
        public string CarryOutAsTypeId { get; set; }
        public string RepairRequestCarryOutAsType { get; set; }
        public BuildingManagerRole RepairRequestRole { get; set; }
        public DateTime? RepairRequestDate { get; set; }
        public string RepairRequestLocationId { get; set; }
        public IEnumerable<RepairRequestAttachmentApiModel> RepairRequestImages { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? MaintenanceEndDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public BuildingManagerRole? PointOfContact { get; set; }
        public AddressModel Address { get; set; }
        public string RepairRequestNo { get; set; }
        public string BuildingId { get; set; }
        public string RepairRequestId { get; set; }
        public DateTime? HandledOn { get; set; }
        public string HandledBy { get; set; }
        public bool IsHandled { get; set; }
        public string RejectionText { get; set; }
        public RepairRequestContactApiModel ContactInfo { get; set; }
        public DateTime? SecondSignatureDate { get; set; }
        public bool IsRequiredHandling { get; set; }
        public PreviousWorkOrderDetails PreviousWorkOrderDetails { get; set; }
        public bool Is48HoursReminder { get; set; }
        public bool RepairRequestCompleted { get; set; }
        public string ProjectId { get; set; }
    }
    public class PreviousWorkOrderDetails
    {
        public DateTime? HandledDate { get; set; }
        public string Solution { get; set; }
        public string Explantion { get; set; }
        public string WorkOrderNumber { get; set; }
        public string WorkOrderText { get; set; }
    }

    public class NewRepairRequestApiModel
    {
        public Guid BuildingId { get; set; }
        public string Desc { get; set; }
        public string DetailedDesc { get; set; }
        public string PreferredAppointmentTime { get; set; }
        public Guid LocationId { get; set; }
        public Guid? ReporterLoginId { get; set; }
        public Guid? SurveyId { get; set; }
        public bool SendMailToReporter { get; set; }
        public Guid? CompletionTermId { get; set; }
        public List<AddRepairRequestResolverApiModel> ResolverOrganisations { get; set; }
    }

    public class AddRepairRequestResolverApiModel
    {
        public Guid OrganisationId { get; set; }
        public Guid? RelationId { get; set; }
    }

    public class UpdateRepairRequestApiModel
    {
        public Guid RepairRequestId { get; set; }
        public string Desc { get; set; }
        public string DetailedDesc { get; set; }
        public string PreferredAppointmentTime { get; set; }
        public Guid LocationId { get; set; }
        public Guid? CompletionTermId { get; set; }
    }

    public class RepairRequestAttachmentApiModel
    {
        public RepairRequestAttachmentApiModel()
        {

        }
        public RepairRequestAttachmentApiModel(Bijlage bijlage)
        {
            AttachmentId = bijlage.Guid;
            ResolverId = bijlage.OplosserGuid;
        }
        public string AttachmentId { get; set; }
        public string ResolverId { get; set; }
    }

    public class ReWorkApiModel
    {
        public Guid RepairRequestId { get; set; }
        public string Desc { get; set; }
        public List<AddRepairRequestResolverApiModel> ResolverOrganisations { get; set; }
    }

    public class BuyerAgreementApiModel
    {
        public Guid RepairRequestId { get; set; }
        public string SecondSignatureAgreement { get; set; }
    }

    public class ResolverForWorkOrderApiModel
    {
        public ResolverForWorkOrderApiModel(Oplosser result, Gebouw building)
        {
            ResolverId = result.Guid;
            RepairRequestId = result.MeldingGuid;
            RepairRequestNo = result.MeldingGu?.MeldingNummer ?? string.Empty;
            RepairRequestDate = result.MeldingGu?.MeldingDatum;
            BuildingId = building?.Guid;
            OrganisationId = result.OrganisatieGuid;
            OrganisationName = result.OrganisatieGu?.NaamOnderdeel ?? string.Empty;
            OrganisationHasLogo = !string.IsNullOrWhiteSpace(result.OrganisatieGu?.Logo);
            Status = (ResolverStatus?)result.OplosserStatus;
            Address = building?.AdresGu?.VolledigAdres ?? string.Empty;
            WorkOrderNumber = result.Werkbonnummer;
        }

        public string ResolverId { get; set; }
        public string RepairRequestId { get; set; }
        public string RepairRequestNo { get; set; }
        public DateTime? RepairRequestDate { get; set; }
        public string BuildingId { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public bool OrganisationHasLogo { get; set; }
        public ResolverStatus? Status { get; set; }
        public string Address { get; set; }
        public string WorkOrderNumber { get; set; }
    }

    public class WorkOrderApiModel
    {
        public Guid ResolverId { get; set; }
        public string Desc { get; set; }
        public bool IsComplete { get; set; }
    }

    public class RepairRequestContactApiModel
    {
        public RepairRequestContactApiModel(Melding repairRequest, KoperHuurder entity, Relatie clientRelation, Relatie vvERelation, Relatie vvEAdministratorRelation, Relatie propertyManagerRelation, Organisatie generalConfigForOrg)
        {
            if (entity != null)
            {
                Buyer = new BuyersInfoApiModel(entity);
            }
            if (repairRequest.OpdrachtgeverOrganisatieGu != null)
            {
                Client = new OrgInfoApiModel(repairRequest.OpdrachtgeverOrganisatieGu, clientRelation);
            }
            if (repairRequest.VveOrganisatieGu != null)
            {
                VvE = new OrgInfoApiModel(repairRequest.VveOrganisatieGu, vvERelation);
            }
            if (repairRequest.VveBeheerderOrganisatieGu != null)
            {
                VvEAdministrator = new OrgInfoApiModel(repairRequest.VveBeheerderOrganisatieGu, vvEAdministratorRelation);
            }
            if (repairRequest.VastgoedBeheerderOrganisatieGu != null)
            {
                PropertyManager = new OrgInfoApiModel(repairRequest.VastgoedBeheerderOrganisatieGu, propertyManagerRelation);
            }
            if (repairRequest.MelderMedewerkerGu != null)
            {
                Employee = new OrgInfoApiModel(repairRequest.MelderMedewerkerGu?.EigenOrganisatieGu, repairRequest.MelderMedewerkerGu?.RelatieGu);
            }
            else if (generalConfigForOrg != null)
            {
                Employee = new OrgInfoApiModel(generalConfigForOrg, null);
            }
        }

        public RepairRequestContactApiModel(Melding repairRequest, KoperHuurder buyer, Relatie relation, BuildingManagerRole managerRole, Organisatie generalConfigForOrg)
        {
            if (buyer != null && managerRole == BuildingManagerRole.gebouw_gebruiker)
            {
                Buyer = new BuyersInfoApiModel(buyer);
            }
            if (repairRequest.OpdrachtgeverOrganisatieGu != null && managerRole == BuildingManagerRole.opdrachtgever)
            {
                Client = new OrgInfoApiModel(repairRequest.OpdrachtgeverOrganisatieGu, relation);
            }
            if (repairRequest.VveOrganisatieGu != null && managerRole == BuildingManagerRole.vve)
            {
                VvE = new OrgInfoApiModel(repairRequest.VveOrganisatieGu, relation);
            }
            if (repairRequest.VveBeheerderOrganisatieGu != null && managerRole == BuildingManagerRole.vve_beheerder)
            {
                VvEAdministrator = new OrgInfoApiModel(repairRequest.VveBeheerderOrganisatieGu, relation);
            }
            if (repairRequest.VastgoedBeheerderOrganisatieGu != null && managerRole == BuildingManagerRole.vastgoed_beheerder)
            {
                PropertyManager = new OrgInfoApiModel(repairRequest.VastgoedBeheerderOrganisatieGu, relation);
            }
            if (managerRole == BuildingManagerRole.medewerker)
            {
                if (repairRequest.MelderMedewerkerGu != null)
                {
                    Employee = new OrgInfoApiModel(repairRequest.MelderMedewerkerGu?.EigenOrganisatieGu, repairRequest.MelderMedewerkerGu?.RelatieGu);
                }
                else if (generalConfigForOrg != null)
                {
                    Employee = new OrgInfoApiModel(generalConfigForOrg, null);
                }
            }
        }
        public BuyersInfoApiModel Buyer { get; set; }
        public OrgInfoApiModel Client { get; set; }
        public OrgInfoApiModel VvE { get; set; }
        public OrgInfoApiModel VvEAdministrator { get; set; }
        public OrgInfoApiModel PropertyManager { get; set; }
        public OrgInfoApiModel Employee { get; set; }
    }

    public class UpdateWorkOrderApiModel
    {
        public Guid ResolverId { get; set; }
        public string CompleteOrRejectionText { get; set; }
        public bool IsComplete { get; set; }
        public bool ContinuedWorkOrder { get; set; }
        public bool IsCompleteRepairRequest { get; set; }
        public RepairRequestNotificationModel Notification { get; set; }
        public string OrganisationId { get; set; }
    }

    public class RepairRequestNotificationModel
    {
        public bool IsNotify { get; set; }
        public RepairRequestEmailModel ToEmails { get; set; }
        public RepairRequestEmailModel CCEmails { get; set; }
    }

    public class RepairRequestEmailModel
    {
        public RepairRequestEmailModel()
        {
            ClientEmails = new List<string>();
            VvEEmails = new List<string>();
            VvEAdministratorEmails = new List<string>();
            PropertyManagerEmails = new List<string>();
            ResolverEmails = new List<string>();
            ReporterEmails = new List<string>();
            BuyerEmails = new List<string>();
            CustomEmails = new List<string>();
        }
        public List<string> ClientEmails { get; set; }
        public List<string> VvEEmails { get; set; }
        public List<string> VvEAdministratorEmails { get; set; }
        public List<string> PropertyManagerEmails { get; set; }
        public List<string> ResolverEmails { get; set; }
        public List<string> ReporterEmails { get; set; }
        public List<string> BuyerEmails { get; set; }
        public List<string> CustomEmails { get; set; }
    }

    public class RepairRequestUpdateStatus
    {
        public bool IsComplete { get; set; }
        public string CompleteOrRejectionText { get; set; }
        public RepairRequestNotificationModel Notification { get; set; }
    }
    public class ContactDetailsforReceiver
    {
        public ContactAddressee? ContactAddressee { get; set; }
        public string OrganisationId { get; set; }
        public string RelationId { get; set; }
        public string BuyerId { get; set; }
        public string PersonId { get; set; }
        public string FormalSalutation { get; set; }
        public string InformalSalutation { get; set; }
    }

    public class ResolverReminderModel
    {
        public List<Guid> ResolverIdList { get; set; }
        public List<string> CCEmails { get; set; }
    }
}
