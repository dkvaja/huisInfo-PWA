using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.Domain.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using Portal.JPDS.AppCore.Models;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class RepairRequestRepository : BaseRepository, IRepairRequestRepository
    {
        private string _defaultStatusGuid, _completedByResolverStatusGuid, _completedByBuyerStatusGuid;
        private readonly string _declinedStatusText, _inProgressStatusText, _siteUrl, _notApplicableRelationName;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RepairRequestRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration appSettingConfig) : base(dbContext)
        {
            var configuration = dbContext.ConfiguratieKlachtenbeheer.SingleOrDefault();
            var configWeb = dbContext.ConfiguratieWebPortal.SingleOrDefault();
            _defaultStatusGuid = configuration?.StandaardMeldingStatusGuid;
            _completedByBuyerStatusGuid = configWeb?.AfgehandeldKoperHuurderMeldingStatusGuid;
            _completedByResolverStatusGuid = configWeb?.AfgehandeldOplosserMeldingStatusGuid;
            _httpContextAccessor = httpContextAccessor;
            _declinedStatusText = "Afgewezen";
            _inProgressStatusText = "In behandeling";
            _siteUrl = appSettingConfig["AppSettings:SiteUrl"];
            _notApplicableRelationName = "N.v.t.";
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<RepairRequestApiModel> GetRepairRequestsByBuildingId(string buildingId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var login = _dbContext.ViewLogins.Where(x => x.Guid == currentUserId).FirstOrDefault();
            bool isBuyer = login.LoginAccountVoor == (int)AccountType.Buyer;

            return _dbContext.Melding
                .Where(x => x.GebouwGuid == buildingId && x.OpnameGu.OpnameSoort != (int)SurveyType.Inspection && x.MeldingSoortGu.MeldingSoort1 != "Inspectie"
                && (
                    !isBuyer ||
                    (x.OpnameGu.OpnameSoort != (int)SurveyType.PreDelivery && x.MeldingSoortGu.MeldingSoort1 != "Voorschouw")
                    || !x.DatumOplevering.HasValue || x.DatumOplevering > DateTime.Today)
                    )
                //Inspection & PreDelivery(Voorschouw) check hardcoded for now, need to improve in future by creating a list of all available system values for this field.
                .Include(x => x.MeldingStatusGu)
                .Include(x => x.MeldingLocatieGu)
                .Include(x => x.MeldingSoortGu)
                .Include(x => x.Bijlage)
                .Include(x => x.MelderOrganisatieGu)
                .OrderByDescending(x => x.MeldingDatum)
                .ThenByDescending(x => x.GewijzigdOp)
                .Select(x => new RepairRequestApiModel
                {
                    RequestId = x.Guid,
                    BuildingId = x.GebouwGuid,
                    BuyerRenterName = x.GebouwGebruikerNaam,
                    SurveyId = x.OpnameGuid,
                    CompletionTermId = x.AfhandelingstermijnGuid,
                    TargetCompletionDate = x.StreefdatumAfhandeling,
                    IsRework = x.Herstelwerkzaamheden,
                    RepairRequestType = x.MeldingSoortGu.MeldingSoort1,
                    CarryOutAsType = x.MeldingSoortGu.MeldingSoort1,
                    Number = x.MeldingNummer,
                    Reporter = new RepairRequestReporterApiModel
                    {
                        Role = (BuildingManagerRole)x.Melder,
                        Name = _dbContext.GetPersonName(x.MelderMedewerkerGuid, x.MelderRelatieGuid, x.MelderKoperHuurderGuid),
                        OrganisationName = x.MelderOrganisatieGu.Naam
                    },
                    Location = x.MeldingLocatieGu.Locatie ?? string.Empty,
                    Desc = x.Omschrijving ?? string.Empty,
                    DetailDesc = x.Melding1 ?? string.Empty,
                    WorkOrderText = x.TekstWerkbon ?? string.Empty,
                    Date = x.MeldingDatum,
                    Priority = (Priority?)x.Prioriteit,
                    Status = x.MeldingStatusGu.MeldingStatus1 ?? string.Empty,
                    Completed = x.Afgehandeld == true,
                    CompletedByBuyer = x.MeldingStatusGuid.ToUpperInvariant() == _completedByBuyerStatusGuid.ToUpperInvariant(),
                    Attachments = x.Bijlage.OrderBy(x => x.Volgorde).Take(1)//only first image needed in Grid
                      .Select(x => new RepairRequestAttachmentApiModel
                      {
                          AttachmentId = x.Guid,
                          ResolverId = x.OplosserGuid
                      })
                }).ToList();
        }

        public IEnumerable<RepairRequestApiModel> GetRepairRequestsByProjectId(string projectId)
        {
            var result = _dbContext.Melding
                  .Where(x => x.WerkGuid == projectId)
                  .Include(x => x.MeldingStatusGu)
                  .Include(x => x.MeldingLocatieGu)
                  .Include(x => x.MeldingSoortGu)
                  .Include(x => x.Bijlage)
                  .Include(x => x.Oplosser).ThenInclude(x => x.OrganisatieGu)
                  .OrderByDescending(x => x.MeldingDatum)
                  .ThenByDescending(x => x.GewijzigdOp)
                  .Join(_dbContext.Gebouw.Include(x => x.AdresGu)
                  , x => x.GebouwGuid,
                  y => y.Guid, (melding, gebouw) => new { melding, gebouw.AdresGu })
                  .Select(x => new RepairRequestApiModel
                  {
                      RequestId = x.melding.Guid,
                      BuildingId = x.melding.GebouwGuid,
                      BuyerRenterName = x.melding.GebouwGebruikerNaam,
                      SurveyId = x.melding.OpnameGuid,
                      CompletionTermId = x.melding.AfhandelingstermijnGuid,
                      TargetCompletionDate = x.melding.StreefdatumAfhandeling,
                      IsRework = x.melding.Herstelwerkzaamheden,
                      CarryOutAsType = x.melding.MeldingSoortGu.MeldingSoort1,
                      Number = x.melding.MeldingNummer,
                      Location = x.melding.MeldingLocatieGu.Locatie ?? string.Empty,
                      Desc = x.melding.Omschrijving ?? string.Empty,
                      DetailDesc = x.melding.Melding1 ?? string.Empty,
                      WorkOrderText = x.melding.TekstWerkbon ?? string.Empty,
                      Date = x.melding.MeldingDatum,
                      Overdue = x.melding.Afgehandeld != true && x.melding.StreefdatumAfhandeling.HasValue ? x.melding.StreefdatumAfhandeling.Value < DateTime.Now.Date : false,
                      Priority = (Priority?)x.melding.Prioriteit,
                      Status = x.melding.MeldingStatusGu.MeldingStatus1 ?? string.Empty,
                      Completed = x.melding.Afgehandeld == true,
                      CompletedByBuyer = x.melding.MeldingStatusGuid.ToUpperInvariant() == _completedByBuyerStatusGuid.ToUpperInvariant(),
                      Attachments = x.melding.Bijlage.OrderBy(x => x.Volgorde).Take(1)//only first image needed in Grid
                      .Select(x => new RepairRequestAttachmentApiModel
                      {
                          AttachmentId = x.Guid,
                          ResolverId = x.OplosserGuid
                      }),
                      Is48HoursReminder = x.melding.Afgehandeld != true && x.melding.StreefdatumAfhandeling.HasValue && x.melding.StreefdatumAfhandeling.Value >= DateTime.Now.Date && (x.melding.StreefdatumAfhandeling.Value - DateTime.Now).TotalHours <= 48,
                      Resolvers = x.melding.Oplosser.Select(x => new RepairRequestResolverApiModel
                      {
                          ResolverId = x.Guid,
                          Name = x.OrganisatieGu.NaamOnderdeel,
                          Status = (ResolverStatus?)x.OplosserStatus,
                          IsRequiredHandling = (x.OplosserStatus == (int?)ResolverStatus.Completed || x.OplosserStatus == (int?)ResolverStatus.TurnedDown) && x.GecontroleerdOp == null
                      }),
                      Address = new AddressModel
                      {
                          Street = x.AdresGu.Straat,
                          HouseNo = x.AdresGu.Nummer,
                          HouseNoAddition = x.AdresGu.NummerToevoeging,
                          Postcode = x.AdresGu.Postcode,
                          Place = x.AdresGu.Plaats,
                      },
                      PreferredAppointmentTime = x.melding.VoorkeurstijdstipAfspraak
                  }).ToList();

            return result;
        }

        public IEnumerable<RepairRequestApiModel> GetRepairRequestsForSurvey(string surveyId)
        {
            var survey = _dbContext.Opname.Find(surveyId.ToUpper());

            return _dbContext.Melding.Where(x => x.OpnameGuid == survey.Guid)
                    .Include(x => x.MeldingStatusGu)
                    .Include(x => x.MeldingLocatieGu)
                    .Include(x => x.MeldingSoortGu)
                    .Include(x => x.Bijlage)
                    .Include(x => x.Oplosser).ThenInclude(x => x.OrganisatieGu)
                    .Include(x => x.MelderOrganisatieGu)
                    .OrderByDescending(x => x.MeldingDatum)
                    .ThenByDescending(x => x.GewijzigdOp)
                    .Select(x => new RepairRequestApiModel(
                        x,
                        _dbContext.GetPersonName(x.MelderMedewerkerGuid, x.MelderRelatieGuid, x.MelderKoperHuurderGuid),
                        _completedByBuyerStatusGuid,
                        x.Oplosser.Select(
                            y => new RepairRequestResolverApiModel(
                                y,
                                _dbContext.Relatie.Include(x => x.PersoonGu).SingleOrDefault(z => z.Guid == y.RelatieGuid)
                                )
                            )
                        )
                    );
        }

        List<CommonKeyValueApiModel> locations = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestLocations()
        {
            if (locations == null)
            {
                List<CommonKeyValueApiModel> locs = new List<CommonKeyValueApiModel>();
                foreach (var location in _dbContext.MeldingLocatie.Where(x => x.Actief == true).OrderBy(x => x.Locatie))
                {
                    locs.Add(new CommonKeyValueApiModel(location.Guid, location.Locatie));
                }
                locations = locs;
            }
            return locations;
        }

        public RepairRequestApiModel AddRepairRequest(NewRepairRequestApiModel newRepairRequest)
        {
            if (newRepairRequest.ReporterLoginId.HasValue)
            {
                var login = _dbContext.ViewLogins.Where(x => x.Guid == newRepairRequest.ReporterLoginId.Value.ToUpperString()).FirstOrDefault();
                if (login != null)
                {
                    var projectId = _dbContext.Gebouw.Find(newRepairRequest.BuildingId.ToUpperString())?.WerkGuid;
                    var availableModules = GetAvailableModulesWithRoles(login.Guid, projectId, newRepairRequest.BuildingId.ToUpperString());

                    List<string> rolesList = new List<string>();

                    if (availableModules != null)
                    {
                        rolesList = availableModules.Select(x => x.RoleName).ToList();
                    }

                    var role = Helper.GetBuildingManagerRoleFromLoginRole((AccountType)login.LoginAccountVoor, rolesList);

                    var spName = "melding_toevoegen";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@gebouw_guid", newRepairRequest.BuildingId.ToUpperString()));
                    parameters.Add(new SqlParameter("@melder", role));
                    parameters.Add(new SqlParameter("@melder_relatie_guid", login.RelatieGuid ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melder_organisatie_guid", login.OrganisatieGuid ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melder_koper_huurder_guid", login.KoperHuurderGuid ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melder_medewerker_guid", login.MedewerkerGuid ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@omschrijving", newRepairRequest.Desc ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melding", newRepairRequest.DetailedDesc ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melding_locatie_guid", newRepairRequest.LocationId.ToUpperString()));
                    parameters.Add(new SqlParameter("@product_dienst_guid", DBNull.Value));
                    parameters.Add(new SqlParameter("@product_dienst_sub1_guid", DBNull.Value));
                    parameters.Add(new SqlParameter("@product_dienst_sub2_guid", DBNull.Value));
                    parameters.Add(new SqlParameter("@voorkeurstijdstip_afspraak", newRepairRequest.PreferredAppointmentTime ?? (object)DBNull.Value));
                    parameters.Add(new SqlParameter("@melding_ontvangen_via", newRepairRequest.SurveyId.HasValue ? RepairRequestReceivedVia.App : RepairRequestReceivedVia.Website));

                    if (newRepairRequest.SurveyId.HasValue)
                    {
                        spName = "opname_melding_toevoegen";
                        parameters.Add(new SqlParameter("@opname_guid", newRepairRequest.SurveyId.HasValue ? newRepairRequest.SurveyId.Value.ToUpperString() : (object)DBNull.Value));
                        parameters.Add(new SqlParameter("@afhandelingstermijn_guid", newRepairRequest.CompletionTermId.HasValue ? newRepairRequest.CompletionTermId.Value.ToUpperString() : (object)DBNull.Value));
                    }
                    parameters.Add(new SqlParameter("@login_guid", login.Guid));

                    var repairRequestNo = new SqlParameter
                    {
                        ParameterName = "@melding_nummer",
                        DbType = DbType.String,
                        Size = 20,
                        Direction = ParameterDirection.Output
                    };
                    parameters.Add(repairRequestNo);

                    var repairRequestId = new SqlParameter
                    {
                        ParameterName = "@melding_guid",
                        DbType = DbType.String,
                        Size = 40,
                        Direction = ParameterDirection.Output
                    };
                    parameters.Add(repairRequestId);

                    var sql = spName + " " + string.Join(", ", parameters.Select(x => x.ParameterName + (x.Direction == ParameterDirection.Output ? " OUTPUT" : "")));
                    _dbContext.Database.ExecuteSqlRaw(sql, parameters);

                    return new RepairRequestApiModel
                    {
                        RequestId = repairRequestId.Value.ToString(),
                        Number = repairRequestNo.Value.ToString()
                    };
                }
            }
            return null;
        }

        public bool UpdateRepairRequest(UpdateRepairRequestApiModel updateRepairRequest)
        {
            var repairRequest = _dbContext.Melding.Find(updateRepairRequest.RepairRequestId.ToUpperString());
            //only update when not marked as completed
            if (repairRequest != null && repairRequest.Afgehandeld != true)
            {
                repairRequest.Omschrijving = updateRepairRequest.Desc;
                repairRequest.Melding1 = updateRepairRequest.DetailedDesc;
                repairRequest.MeldingLocatieGuid = updateRepairRequest.LocationId.ToUpperString();
                if (updateRepairRequest.CompletionTermId.HasValue)
                    repairRequest.AfhandelingstermijnGuid = updateRepairRequest.CompletionTermId.Value.ToUpperString();
                return true;
            }
            return false;
        }

        public bool UpdateRework(ReWorkApiModel reWork)
        {
            var repairRequest = _dbContext.Melding.Find(reWork.RepairRequestId.ToUpperString());
            if (repairRequest != null)
            {
                var lineSeperator = "\r\n====================\r\n";
                repairRequest.TekstWerkbon = reWork.Desc + lineSeperator + (repairRequest.TekstWerkbon ?? string.Empty);
                repairRequest.Herstelwerkzaamheden = true;
                repairRequest.MeldingStatusGuid = _defaultStatusGuid;

                return true;
            }
            return false;
        }

        public bool DeleteRepairRequest(string repairRequestId, out string[] filesToDelete)
        {
            var repairRequest = _dbContext.Melding.Include(x => x.Oplosser).Include(x => x.Bijlage).SingleOrDefault(x => x.Guid == repairRequestId);
            if (repairRequest != null && repairRequest.Afgehandeld != true && repairRequest.Oplosser.All(x => x.OplosserStatus == (int)ResolverStatus.New))
            {
                filesToDelete = repairRequest.Bijlage.Select(x => x.Bijlage1).ToArray();

                if (repairRequest.Bijlage.Any())
                    _dbContext.Bijlage.RemoveRange(repairRequest.Bijlage);
                if (repairRequest.Oplosser.Any())
                    _dbContext.Oplosser.RemoveRange(repairRequest.Oplosser);

                _dbContext.Melding.Remove(repairRequest);
                return true;
            }
            filesToDelete = null;
            return false;
        }

        public void AddRepairRequestResolvers(string repairRequestId, List<AddRepairRequestResolverApiModel> lstModel)
        {
            if (lstModel != null)
            {
                foreach (var item in lstModel)
                {
                    AddRepairRequestResolver(repairRequestId, item.OrganisationId.ToUpperString(), item.RelationId?.ToUpperString());
                }
            }
        }

        public bool UpdateRepairRequestResolverRelation(string resolverId, string relationId)
        {
            var resolver = _dbContext.Oplosser.Find(resolverId);
            if (resolver != null && resolver.OplosserStatus == (int)ResolverStatus.New)
            {
                if (resolver.RelatieGuid != relationId)
                {
                    var valid = _dbContext.Relatie.Any(x => x.Guid == relationId && x.OrganisatieGuid == resolver.OrganisatieGuid);

                    if (valid == true)
                    {
                        resolver.RelatieGuid = relationId;
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteRepairRequestResolver(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Find(resolverId);
            if (resolver != null && resolver.OplosserStatus == (int)ResolverStatus.New && string.IsNullOrEmpty(resolver.Werkbonnummer))
            {
                var resolverImages = _dbContext.Bijlage.Where(x => x.OplosserGuid == resolver.Guid && x.MeldingGuid == resolver.MeldingGuid);
                if (resolverImages.Any())
                    _dbContext.Bijlage.RemoveRange(resolverImages);

                _dbContext.Oplosser.Remove(resolver);
                return true;
            }
            return false;
        }

        public List<RepairRequestAttachmentApiModel> GetRepairRequestAttachments(string repairRequestId)
        {
            return _dbContext.Bijlage.Where(x => x.MeldingGuid == repairRequestId).OrderBy(x => x.Volgorde)
                .Select(x => new RepairRequestAttachmentApiModel
                {
                    AttachmentId = x.Guid,
                    ResolverId = x.OplosserGuid
                }).ToList();
        }

        public bool DeleteAttachment(string repairRequestId, string attachmentId, out string fileToDelete)
        {
            var repairRequest = _dbContext.Melding.Include(x => x.Bijlage).SingleOrDefault(x => x.Guid == repairRequestId);
            if (repairRequest != null && repairRequest.Afgehandeld != true)
            {
                var attachmentToDelete = repairRequest.Bijlage.FirstOrDefault(x => x.Guid == attachmentId);
                if (attachmentToDelete != null)
                {
                    fileToDelete = attachmentToDelete.Bijlage1;
                    repairRequest.Bijlage.Remove(attachmentToDelete);
                    return true;
                }
            }
            fileToDelete = null;
            return false;
        }

        public RepairRequestApiModel GetRepairRequestById(string repairRequestId)
        {
            var repairRequest = _dbContext.Melding.Include(x => x.MelderOrganisatieGu).Where(x => x.Guid == repairRequestId).FirstOrDefault();
            if (repairRequest != null)
                return new RepairRequestApiModel(repairRequest, _dbContext.GetPersonName(repairRequest.MelderMedewerkerGuid, repairRequest.MelderRelatieGuid, repairRequest.MelderKoperHuurderGuid), _completedByBuyerStatusGuid);
            return null;
        }

        public RepairRequestApiModel GetRepairRequestDetails(string repairRequestId)
        {
            var repairRequest = _dbContext.Melding.Where(x => x.Guid == repairRequestId)
                .Include(x => x.MeldingStatusGu)
                .Include(x => x.MeldingLocatieGu)
                .Include(x => x.MeldingSoortGu)
                .Include(x => x.Bijlage)
                .Include(x => x.Oplosser).ThenInclude(x => x.OrganisatieGu)
                .Include(x => x.MelderOrganisatieGu)
                .Include(x => x.AangenomenDoorMedewerkerGu)
                .Include(x => x.OpnameGu)
                .Include(x => x.OpdrachtgeverOrganisatieGu)
                .Include(x => x.VveOrganisatieGu)
                .Include(x => x.VveBeheerderOrganisatieGu)
                .Include(x => x.VastgoedBeheerderOrganisatieGu)
                .Include(x => x.MelderMedewerkerGu)
                .Include(x => x.MeldingVeroorzakerGu)
                .SingleOrDefault();

            var generalConfigOrg = string.IsNullOrWhiteSpace(repairRequest.MelderMedewerkerGuid) ?
                _dbContext.ConfiguratieAlgemeen.Include(x => x.HoofdvestigingEigenOrganisatieGu)
                .Select(x => x.HoofdvestigingEigenOrganisatieGu).SingleOrDefault() : null;

            var secondSignatureDate = _dbContext.Opname.Where(x => x.GebouwGuid == repairRequest.GebouwGuid && x.OpnameSoort == (int)SurveyType.Delivery)
                .Select(x => x.DatumTweedeHandtekeningOndertekening).SingleOrDefault();

            var result = new RepairRequestApiModel(
                    repairRequest,
                    _dbContext.GetPersonName(repairRequest.MelderMedewerkerGuid, repairRequest.MelderRelatieGuid, repairRequest.MelderKoperHuurderGuid),
                    _completedByBuyerStatusGuid,
                    repairRequest.Oplosser.Select(
                        y => new RepairRequestResolverApiModel(
                             y,
                            _dbContext.Relatie.Include(z => z.PersoonGu).Include(x => x.FunctieGu).Include(x => x.AfdelingGu)
                            .SingleOrDefault(z => z.Guid == y.RelatieGuid)
                            )
                        ),
                    _dbContext.Gebouw.Where(z => z.Guid == repairRequest.GebouwGuid).Select(z => z.AdresGu).SingleOrDefault(),
                    new RepairRequestContactApiModel(repairRequest,
                    _dbContext.KoperHuurder.Where(y => y.Guid == repairRequest.GebouwGebruikerKoperHuurderGuid)
                        .Include(y => y.Persoon1Gu).Include(y => y.Persoon2Gu).Include(y => y.OrganisatieGu)
                        .Include(y => y.RelatieGu).ThenInclude(y => y.PersoonGu)
                        .Include(y => y.RelatieGu).ThenInclude(y => y.FunctieGu)
                        .Include(y => y.RelatieGu).ThenInclude(y => y.AfdelingGu)
                        .Include(y => y.Login).SingleOrDefault(),
                    _dbContext.Relatie.Where(y => y.Guid == repairRequest.OpdrachtgeverRelatieGuid)
                        .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(z => z.AfdelingGu).SingleOrDefault(),
                    _dbContext.Relatie.Where(y => y.Guid == repairRequest.VveRelatieGuid)
                        .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(z => z.AfdelingGu).SingleOrDefault(),
                    _dbContext.Relatie.Where(y => y.Guid == repairRequest.VveBeheerderRelatieGuid)
                        .Include(z => z.FunctieGu).Include(x => x.PersoonGu).Include(z => z.AfdelingGu).SingleOrDefault(),
                    _dbContext.Relatie.Where(y => y.Guid == repairRequest.VastgoedBeheerderRelatieGuid)
                        .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(z => z.AfdelingGu).SingleOrDefault(),
                    generalConfigOrg), secondSignatureDate);

            return result;
        }

        public ResolverForWorkOrderApiModel GetResolverDetailsForWorkOrder(string resolverId)
        {
            var result = _dbContext.Oplosser
                .Include(x => x.MeldingGu)
                .Include(x => x.OrganisatieGu)
                .SingleOrDefault(x => x.Guid == resolverId);

            if (result != null)
            {
                var building = _dbContext.Gebouw.Where(x => x.Guid == result.MeldingGu.GebouwGuid).Include(x => x.AdresGu).SingleOrDefault();
                return new ResolverForWorkOrderApiModel(result, building);
            }
            return null;
        }

        public Dictionary<string, string> GetDefaultEmailTokens(string repairRequestId, string employeeId = null)
        {
            var repairRequest = _dbContext.Melding
                .Include(x => x.MeldingLocatieGu)
                .Include(x => x.Bijlage)
                .SingleOrDefault(x => x.Guid == repairRequestId);
            var building = _dbContext.Gebouw.Where(x => x.Guid == repairRequest.GebouwGuid).Include(x => x.AdresGu).SingleOrDefault();

            var resultDict = new Dictionary<string, string>();
            resultDict["[geachte]"] = _dbContext.GetSalutationForEmail(repairRequest.MelderOrganisatieGuid, null, repairRequest.MelderRelatieGuid, repairRequest.MelderKoperHuurderGuid, true);
            resultDict["[geachte_informeel]"] = _dbContext.GetSalutationForEmail(repairRequest.MelderOrganisatieGuid, null, repairRequest.MelderRelatieGuid, repairRequest.MelderKoperHuurderGuid, false);
            resultDict["[bouwnummer_intern]"] = building?.BouwnummerIntern;
            resultDict["[bouwnummer_extern]"] = building?.BouwnummerExtern;
            resultDict["[hoofdaannemer]"] = building?.WerkGu?.HoofdaannemerOrganisatieGu?.Naam ?? string.Empty;

            resultDict["[melding_datum]"] = repairRequest.MeldingDatum.ToString("dd-MM-yyyy");
            resultDict["[melding_datum_afgehandeld]"] = repairRequest.DatumAfhandeling?.ToString("dd-MM-yyyy") ?? string.Empty;
            resultDict["[melding_korte_omschrijving]"] = repairRequest.Omschrijving;
            resultDict["[melding_locatie]"] = repairRequest.MeldingLocatieGu?.Locatie;
            resultDict["[melding_melder]"] = _dbContext.GetDomainTranslation("gebouw_beheerder_rol", repairRequest.Melder.ToString());
            resultDict["[melding_nummer]"] = repairRequest.MeldingNummer;
            resultDict["[melding_tekst]"] = repairRequest.Melding1?.Replace("\n", "<br />");
            resultDict["[melding_tekst_werkbon]"] = repairRequest.TekstWerkbon;
            resultDict["[object_straat]"] = building?.AdresGu?.Straat;
            resultDict["[object_huisnummer]"] = string.Format("{0} {1}", building?.AdresGu?.Nummer ?? string.Empty, building?.AdresGu?.NummerToevoeging ?? string.Empty);
            resultDict["[object_postcode]"] = building?.AdresGu?.Postcode;
            resultDict["[object_plaats]"] = building?.AdresGu?.Plaats;

            string bijlage = string.Empty;
            if (repairRequest.Bijlage.Any())
            {
                bijlage = string.Format("<ul><li>{0}</li></ul>", string.Join("</li><li>", repairRequest.Bijlage.Select(x => x.Omschrijving + System.IO.Path.GetExtension(x.Bijlage1))));
            }
            resultDict["[melding_bijlagen]"] = bijlage;
            resultDict["[slotgroet]"] = string.Empty;
            resultDict["[ondertekenaar_naam]"] = string.Empty;
            resultDict["[organisatie_naamonderdeel]"] = string.Empty;
            resultDict["[organisatie_logo]"] = string.Empty;
            if (!string.IsNullOrWhiteSpace(employeeId))
            {
                var employeeData = _dbContext.Medewerker.Where(x => x.Guid == employeeId).Select(x => new { x.Slotgroet, x.Ondertekenaar }).SingleOrDefault();
                if (employeeData != null)
                {
                    resultDict["[slotgroet]"] = employeeData.Slotgroet;
                    resultDict["[ondertekenaar_naam]"] = employeeData.Ondertekenaar;
                    var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    if (!string.IsNullOrWhiteSpace(currentUserId))
                    {
                        var organisationId = _dbContext.Login.Where(x => x.Guid == currentUserId).Select(x => x.OrganisatieGuid).SingleOrDefault();
                        if (!string.IsNullOrWhiteSpace(organisationId))
                        {
                            var organisationName = _dbContext.Organisatie.Find(organisationId)?.NaamOnderdeel;
                            resultDict["[organisatie_naamonderdeel]"] = !string.IsNullOrWhiteSpace(organisationName) ? organisationName : string.Empty;
                            var organisationLogo = _siteUrl + "api/Organisation/GetOrganisationLogo/" + organisationId;
                            resultDict["[organisatie_logo]"] = "<a href=\"" + organisationLogo + "\" ><img width=100 style=\"margin:5px\" src =\"" + organisationLogo + " \"/></a>";
                        }
                    }
                }
            }
            return resultDict;
        }

        public bool MarkCompletedByBuyer(BuyerAgreementApiModel buyerAgreement)
        {
            var repairRequest = _dbContext.Melding.Find(buyerAgreement.RepairRequestId.ToUpperString());
            if (repairRequest != null)
            {
                if (repairRequest.Afgehandeld != true)
                {
                    repairRequest.MeldingStatusGuid = _dbContext.MeldingStatus.Find(_completedByBuyerStatusGuid)?.Guid;
                    repairRequest.DatumAfhandeling = DateTime.Now;//Maybe in next release we will make user to fill the date in the form.
                }
                repairRequest.KoperHuurderIsAkkoord = true;
                repairRequest.AfsprakenTweedeHandtekening = buyerAgreement.SecondSignatureAgreement;

                return true;
            }
            return false;
        }

        List<CommonKeyValueApiModel> repairRequestTypeList = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestTypeList()
        {
            if (repairRequestTypeList == null)
            {
                List<CommonKeyValueApiModel> lstType = new List<CommonKeyValueApiModel>();
                foreach (var type in _dbContext.MeldingType.Where(x => x.Actief == true).OrderBy(x => x.MeldingType1))
                {
                    lstType.Add(new CommonKeyValueApiModel(type.Guid, type.MeldingType1));
                }
                repairRequestTypeList = lstType;
            }
            return repairRequestTypeList;
        }

        List<CommonKeyValueApiModel> repairRequestNatureList = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestNatureList()
        {
            if (repairRequestNatureList == null)
            {
                List<CommonKeyValueApiModel> lstNature = new List<CommonKeyValueApiModel>();
                foreach (var nature in _dbContext.MeldingAard.Where(x => x.Actief == true).OrderBy(x => x.Aard))
                {
                    lstNature.Add(new CommonKeyValueApiModel(nature.Guid, nature.Aard));
                }
                repairRequestNatureList = lstNature;
            }
            return repairRequestNatureList;
        }

        List<CommonKeyValueApiModel> repairRequestCauseList = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestCauseList()
        {
            if (repairRequestCauseList == null)
            {
                List<CommonKeyValueApiModel> lstCause = new List<CommonKeyValueApiModel>();
                foreach (var cause in _dbContext.MeldingOorzaak.Where(x => x.Actief == true).OrderBy(x => x.Oorzaak))
                {
                    lstCause.Add(new CommonKeyValueApiModel(cause.Guid, cause.Oorzaak));
                }
                repairRequestCauseList = lstCause;
            }
            return repairRequestCauseList;
        }

        List<CommonKeyValueApiModel> repairRequestCauserList = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestCauserList()
        {
            if (repairRequestCauserList == null)
            {
                List<CommonKeyValueApiModel> lstCauser = new List<CommonKeyValueApiModel>();
                foreach (var causer in _dbContext.MeldingVeroorzaker.Where(x => x.Actief == true).OrderBy(x => x.Veroorzaker))
                {
                    lstCauser.Add(new CommonKeyValueApiModel(causer.Guid, causer.Veroorzaker));
                }
                repairRequestCauserList = lstCauser;
            }
            return repairRequestCauserList;
        }

        List<CommonKeyValueApiModel> carryOutAsTypeList = null;
        public IEnumerable<CommonKeyValueApiModel> GetRepairRequestCarryOutAsTypeList()
        {
            if (carryOutAsTypeList == null)
            {
                List<CommonKeyValueApiModel> lstSoort = new List<CommonKeyValueApiModel>();
                foreach (var causer in _dbContext.MeldingSoort.Where(x => x.Actief == true).OrderBy(x => x.MeldingSoort1))
                {
                    lstSoort.Add(new CommonKeyValueApiModel(causer.Guid, causer.MeldingSoort1));
                }
                carryOutAsTypeList = lstSoort;
            }
            return carryOutAsTypeList;
        }

        public bool UpdateRepairRequestKeyValue(string repairRequestId, List<CommonKeyValueApiModel> lstKeyValues)
        {
            var repairRequest = _dbContext.Melding.Find(repairRequestId);
            //only update when not marked as completed
            if (repairRequest != null && repairRequest.Afgehandeld != true)
            {
                if (lstKeyValues.Count > 0)
                {
                    foreach (var item in lstKeyValues)
                    {
                        string carryOutAsTypeId = null;
                        switch (item.Id.ToLower())
                        {
                            case "desc":
                                repairRequest.Omschrijving = item.Name;
                                break;
                            case "productserviceid":
                                if (repairRequest.ProductDienstGuid != item.Name)
                                {
                                    repairRequest.ProductDienstGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                    if (!lstKeyValues.Where(x => x.Id.ToLower().Equals("subproductservice1id") && x.Name != null).Any())
                                    {
                                        repairRequest.ProductDienstSub1Guid = null;
                                        repairRequest.ProductDienstSub2Guid = null;
                                    }
                                    carryOutAsTypeId = GetForecastingCarryOutAsTypeId(repairRequestId, repairRequest.ProductDienstGuid, repairRequest.ProductDienstSub1Guid, repairRequest.ProductDienstSub2Guid);
                                    if (!string.IsNullOrWhiteSpace(carryOutAsTypeId))
                                    {
                                        repairRequest.MeldingSoortGuid = carryOutAsTypeId;
                                    }
                                }
                                break;
                            case "subproductservice1id":
                                if (repairRequest.ProductDienstSub1Guid != item.Name)
                                {
                                    repairRequest.ProductDienstSub1Guid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                    if (!lstKeyValues.Where(x => x.Id.ToLower().Equals("subproductservice2id") && x.Name != null).Any())
                                    {
                                        repairRequest.ProductDienstSub2Guid = null;
                                    }
                                    carryOutAsTypeId = GetForecastingCarryOutAsTypeId(repairRequestId, repairRequest.ProductDienstGuid, repairRequest.ProductDienstSub1Guid, repairRequest.ProductDienstSub2Guid);
                                    if (!string.IsNullOrWhiteSpace(carryOutAsTypeId))
                                    {
                                        repairRequest.MeldingSoortGuid = carryOutAsTypeId;
                                    }
                                }
                                break;
                            case "subproductservice2id":
                                repairRequest.ProductDienstSub2Guid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                carryOutAsTypeId = GetForecastingCarryOutAsTypeId(repairRequestId, repairRequest.ProductDienstGuid, repairRequest.ProductDienstSub1Guid, repairRequest.ProductDienstSub2Guid);
                                if (!string.IsNullOrWhiteSpace(carryOutAsTypeId))
                                {
                                    repairRequest.MeldingSoortGuid = carryOutAsTypeId;
                                }
                                break;
                            case "carryoutastypeid":
                                repairRequest.MeldingSoortGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "workordertext":
                                repairRequest.TekstWerkbon = item.Name;
                                break;
                            case "priority":
                                if (item.Name != null)
                                {
                                    int priority = 0;
                                    if (int.TryParse(item.Name, out priority))
                                    {
                                        if (Enum.IsDefined(typeof(Priority), priority) == true)
                                        {
                                            repairRequest.Prioriteit = (byte?)(priority);
                                        }
                                    }
                                }
                                break;
                            case "completiontermid":
                                repairRequest.AfhandelingstermijnGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "typeid":
                                repairRequest.MeldingTypeGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "natureid":
                                repairRequest.MeldingAardGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "causeid":
                                repairRequest.MeldingOorzaakGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "causerid":
                                repairRequest.MeldingVeroorzakerGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "causerorganisationid":
                                repairRequest.VeroorzakerOrganisatieGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "clientreference":
                                repairRequest.ReferentieOpdrachtgever = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            case "pointofcontact":
                                int pointOfContact = 0;
                                if (int.TryParse(item.Name, out pointOfContact))
                                {
                                    if (Enum.IsDefined(typeof(BuildingManagerRole), pointOfContact) == true)
                                    {
                                        repairRequest.AanspreekpuntVoorOplosser = (byte)(pointOfContact);
                                    }
                                }
                                break;
                            case "locationid":
                                repairRequest.MeldingLocatieGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                break;
                            default:
                                return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public string AddRepairRequestResolver(string repairRequestId, string organisationId, string relationId = null)
        {
            var repairRequest = _dbContext.Melding
                .SingleOrDefault(x => x.Guid == repairRequestId);

            if (repairRequest != null && repairRequest.Afgehandeld != true)
            {
                var resolverEntity = new Oplosser
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    MeldingGuid = repairRequestId,
                    OrganisatieGuid = organisationId,
                    MeldingSoortGuid = repairRequest.MeldingSoortGuid,
                    StreefdatumAfhandeling = repairRequest.StreefdatumAfhandeling,
                    RelatieGuid = relationId ?? null,
                    TekstWerkbon = repairRequest.TekstWerkbon,
                    Omschrijving = repairRequest.Omschrijving,
                    OplosserStatus = (int)ResolverStatus.New
                };

                _dbContext.Oplosser.Add(resolverEntity);
                return resolverEntity.Guid;
            }
            return null;
        }

        public bool UpdateWorkOrder(string resolverId, List<CommonKeyValueApiModel> lstKeyValues)
        {
            var resolver = _dbContext.Oplosser.Find(resolverId);
            //only update when work order status is New
            if (resolver != null && !resolver.WerkbonnummerVolgnummer.HasValue && resolver.OplosserStatus == (int)ResolverStatus.New)
            {
                if (lstKeyValues.Count > 0)
                {
                    foreach (var item in lstKeyValues)
                    {
                        switch (item.Id.ToLower())
                        {
                            case "carryoutastypeid":
                                if (string.IsNullOrWhiteSpace(resolver.MeldingGu?.OpnameGuid))
                                {
                                    resolver.MeldingSoortGuid = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null;
                                }
                                break;
                            case "explanation":
                                resolver.Toelichting = item.Name;
                                break;
                            case "targetcompletiondate":
                                if (item.Name != null)
                                {
                                    if (DateTime.TryParse(item.Name, out DateTime targetDate) == true)
                                        resolver.StreefdatumAfhandeling = targetDate;
                                }
                                break;
                            default:
                                return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public RepairRequestResolverApiModel GetWorkOrderDetails(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                 .Include(x => x.OrganisatieGu)
                 .Include(x => x.MeldingGu).ThenInclude(x => x.Bijlage)
                 .Include(x => x.MeldingGu.MeldingSoortGu)
                 .Include(x => x.MeldingGu.Oplosser)
                 .Include(x => x.MeldingSoortGu)
                 .Include(x => x.GecontroleerdDoorLoginGu)
                 .Include(x => x.VervolgVanWerkbonOplosserGu)
                 .OrderByDescending(x => x.DatumAfhandeling).SingleOrDefault();

            if (resolver != null)
            {
                Relatie relation = null;
                KoperHuurder buyer = null;
                var pointOfContact = (BuildingManagerRole)resolver.MeldingGu?.AanspreekpuntVoorOplosser;

                var repairRequest = _dbContext.Melding.Where(x => x.Guid == resolver.MeldingGuid)
                .Include(x => x.Bijlage)
                .Include(x => x.OpdrachtgeverOrganisatieGu)
                .Include(x => x.VveOrganisatieGu)
                .Include(x => x.VveBeheerderOrganisatieGu)
                .Include(x => x.VastgoedBeheerderOrganisatieGu)
                .Include(x => x.MelderMedewerkerGu).SingleOrDefault();

                var generalConfigOrg = string.IsNullOrWhiteSpace(repairRequest.MelderMedewerkerGuid) ?
                _dbContext.ConfiguratieAlgemeen.Include(x => x.HoofdvestigingEigenOrganisatieGu)
                .Select(x => x.HoofdvestigingEigenOrganisatieGu).SingleOrDefault() : null;

                var secondSignatureDate = _dbContext.Opname.Where(x => x.GebouwGuid == repairRequest.GebouwGuid && x.OpnameSoort == (int)SurveyType.Delivery)
                .Select(x => x.DatumTweedeHandtekeningOndertekening).SingleOrDefault();

                switch (pointOfContact)
                {
                    case BuildingManagerRole.opdrachtgever:
                        relation = _dbContext.Relatie.Where(y => y.Guid == repairRequest.OpdrachtgeverRelatieGuid)
                            .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(x => x.AfdelingGu).SingleOrDefault();
                        break;
                    case BuildingManagerRole.vve:
                        relation = _dbContext.Relatie.Where(y => y.Guid == repairRequest.VveRelatieGuid)
                            .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(x => x.AfdelingGu).SingleOrDefault();
                        break;
                    case BuildingManagerRole.vve_beheerder:
                        relation = _dbContext.Relatie.Where(y => y.Guid == repairRequest.VveBeheerderRelatieGuid)
                            .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(x => x.AfdelingGu).SingleOrDefault();
                        break;
                    case BuildingManagerRole.vastgoed_beheerder:
                        relation = _dbContext.Relatie.Where(y => y.Guid == repairRequest.VastgoedBeheerderRelatieGuid)
                            .Include(z => z.FunctieGu).Include(z => z.PersoonGu).Include(x => x.AfdelingGu).SingleOrDefault();
                        break;
                    case BuildingManagerRole.gebouw_gebruiker:
                        buyer = _dbContext.KoperHuurder.Where(y => y.Guid == repairRequest.GebouwGebruikerKoperHuurderGuid)
                        .Include(y => y.Persoon1Gu)
                        .Include(y => y.Persoon2Gu)
                        .Include(y => y.OrganisatieGu)
                        .Include(y => y.RelatieGu).ThenInclude(y => y.PersoonGu)
                        .Include(y => y.RelatieGu.FunctieGu)
                        .Include(y => y.RelatieGu.AfdelingGu)
                        .Include(y => y.Login).SingleOrDefault();
                        break;
                    default:
                        break;
                }

                return new RepairRequestResolverApiModel(resolver,
                  _dbContext.Relatie.Include(z => z.PersoonGu).Include(x => x.FunctieGu).Include(x => x.AfdelingGu).SingleOrDefault(z => z.Guid == resolver.RelatieGuid),
                 _dbContext.Gebouw.Where(y => y.Guid == resolver.MeldingGu.GebouwGuid).Include(z => z.AdresGu).SingleOrDefault(),
                 new RepairRequestContactApiModel(repairRequest, buyer, relation, pointOfContact, generalConfigOrg),
                 secondSignatureDate);
            }
            return null;
        }

        public bool CreateWorkOrder(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Include(x => x.MeldingGu).SingleOrDefault(x => x.Guid == resolverId);

            var repairRequest = _dbContext.Melding.Include(x => x.Oplosser)
                .Where(x => x.Guid == resolver.MeldingGu.Guid).SingleOrDefault();

            if (resolver != null && resolver.OplosserStatus == (int)ResolverStatus.New && resolver.WerkbonnummerVolgnummer == null)
            {
                int nextWorkOrderId = repairRequest.Oplosser.Max(x => x.WerkbonnummerVolgnummer ?? 0) + 1;
                resolver.WerkbonnummerVolgnummer = nextWorkOrderId;
                if (repairRequest.MeldingStatusGuid == _defaultStatusGuid)
                {
                    var inprogressStatusGuid = _dbContext.MeldingStatus.Where(x => x.MeldingStatus1.Equals(_inProgressStatusText)).Select(x => x.Guid).SingleOrDefault();
                    repairRequest.MeldingStatusGuid = inprogressStatusGuid;
                }
                return true;
            }
            return false;
        }

        public ResolverForWorkOrderApiModel UpdateWorkOrderByDirectLink(WorkOrderApiModel model)
        {
            if (model != null)
            {
                var resolver = _dbContext.Oplosser
                                   .Include(x => x.MeldingGu)
                                   .Include(x => x.OrganisatieGu)
                                   .SingleOrDefault(x => x.Guid == model.ResolverId.ToUpperString());

                if (resolver != null && resolver.OplosserStatus != (int)ResolverStatus.Completed && resolver.OplosserStatus != (int)ResolverStatus.TurnedDown)
                {
                    resolver.OplosserStatus = model.IsComplete ? (byte)ResolverStatus.Completed : (byte)ResolverStatus.TurnedDown;
                    resolver.DatumAfhandeling = DateTime.Now;

                    if (model.IsComplete)
                        resolver.Oplossing = model.Desc;
                    else
                        resolver.RedenAfwijzing = model.Desc;

                    var building = _dbContext.Gebouw.Where(x => x.Guid == resolver.MeldingGu.GebouwGuid).Include(x => x.AdresGu).SingleOrDefault();
                    return new ResolverForWorkOrderApiModel(resolver, building);
                }
            }
            return null;
        }

        public string UpdateWorkOrderStatus(UpdateWorkOrderApiModel workOrder)
        {
            //when resolver logic is updated please update same in method "UpdateWorkOrderByDirectLink" for Resolver
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == workOrder.ResolverId.ToUpperString())
                .Include(x => x.OrganisatieGu)
                .Include(x => x.MeldingGu)
                .SingleOrDefault();

            if (resolver != null)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var login = !string.IsNullOrWhiteSpace(currentUserId) ? _dbContext.Login.Where(x => x.Guid == currentUserId && !x.Verwijderd).FirstOrDefault() : null;
                if (login != null)
                {
                    var repairRequest = resolver.MeldingGu;

                    var availableModuleWithRoles = GetAvailableModulesWithRoles(login.Guid, resolver.MeldingGu.WerkGuid, resolver.MeldingGu.GebouwGuid);
                    var availableRoles = availableModuleWithRoles?.Where(x => x.ModuleName == LoginModules.ConstructionQuality).Select(x => x.RoleName);
                    if (availableRoles.Any())
                    {
                        if (availableRoles.Any(x => x == LoginRoles.SubContractor || x == LoginRoles.SiteManager))
                        {
                            if (workOrder.IsComplete || workOrder.ContinuedWorkOrder)
                            {
                                resolver.Oplossing = workOrder.CompleteOrRejectionText;
                            }
                            else
                            {
                                resolver.RedenAfwijzing = workOrder.CompleteOrRejectionText;
                            }
                        }

                        if (availableRoles.Any(x => x == LoginRoles.SubContractor))
                        {
                            resolver.OplosserStatus = workOrder.IsComplete ? (byte)ResolverStatus.Completed : (byte)ResolverStatus.TurnedDown;
                            resolver.DatumAfhandeling = DateTime.Now;
                        }

                        if (availableRoles.Any(x => x == LoginRoles.SiteManager))
                        {
                            if (resolver.DatumAfhandeling == null)
                            {
                                resolver.DatumAfhandeling = DateTime.Now;
                            }
                            resolver.GecontroleerdOp = DateTime.Now;
                            resolver.GecontroleerdDoorLoginGuid = login.Guid;
                            if (workOrder.IsComplete && !workOrder.ContinuedWorkOrder)
                            {
                                var solution = string.Format(
                                   "{0} {1}:\r\n{2}{3}{4}",
                                   DateTime.Now.Date.ToString("dd-MM-yyyy"),
                                   (resolver.OrganisatieGu?.NaamOnderdeel ?? string.Empty),
                                   workOrder.CompleteOrRejectionText,
                                   (string.IsNullOrWhiteSpace(repairRequest.Oplossing) ? string.Empty : "\r\n\r\n"),
                                   repairRequest.Oplossing ?? string.Empty
                                   );
                                repairRequest.Oplossing = solution;
                                resolver.OplosserStatus = (byte?)ResolverStatus.Completed;
                                var isAllWorkOrderResolved = resolver.MeldingGu.Oplosser.All(x => x.Guid == workOrder.ResolverId.ToUpperString() || (x.OplosserStatus == (int)ResolverStatus.Completed || x.OplosserStatus == (int)ResolverStatus.TurnedDown) && x.GecontroleerdOp != null);
                                if (isAllWorkOrderResolved && workOrder.IsCompleteRepairRequest)
                                {
                                    repairRequest.MeldingStatusGuid = _completedByBuyerStatusGuid;
                                    repairRequest.DatumAfhandeling = DateTime.Now;
                                }
                            }
                            if (!workOrder.IsComplete && !workOrder.ContinuedWorkOrder)
                            {
                                var declineText = string.Format(
                                  "{0} {1}:\r\n{2}{3}{4}",
                                  DateTime.Now.Date.ToString("dd-MM-yyyy"),
                                  (resolver.OrganisatieGu?.NaamOnderdeel ?? string.Empty),
                                  workOrder.CompleteOrRejectionText,
                                  (string.IsNullOrWhiteSpace(repairRequest.RedenAfwijzing) ? string.Empty : "\r\n\r\n"),
                                  repairRequest.RedenAfwijzing ?? string.Empty
                                  );

                                repairRequest.RedenAfwijzing = declineText;
                                resolver.OplosserStatus = (byte?)ResolverStatus.TurnedDown;
                            }
                            if (workOrder.ContinuedWorkOrder)
                            {
                                resolver.VervolgWerkbon = true;
                                if (resolver.OplosserStatus != (int)ResolverStatus.TurnedDown)
                                {
                                    resolver.OplosserStatus = (int)ResolverStatus.Completed;
                                }
                                var resolverEntity = new Oplosser
                                {
                                    Guid = Guid.NewGuid().ToUpperString(),
                                    MeldingGuid = resolver.MeldingGu?.Guid,
                                    OrganisatieGuid = string.IsNullOrWhiteSpace(workOrder.OrganisationId) ? resolver.OrganisatieGuid : workOrder.OrganisationId,
                                    RelatieGuid = string.IsNullOrWhiteSpace(workOrder.OrganisationId) || resolver.OrganisatieGuid.Equals(workOrder.OrganisationId) ? resolver.RelatieGuid : null,
                                    Omschrijving = resolver.Omschrijving,
                                    OplosserStatus = (int)ResolverStatus.New,
                                    VervolgVanWerkbonOplosserGuid = resolver.Guid,
                                    Toelichting = resolver.Toelichting,
                                    MeldingSoortGuid = resolver.MeldingSoortGuid,
                                    TekstWerkbon = resolver.TekstWerkbon,
                                };
                                _dbContext.Oplosser.Add(resolverEntity);
                                return resolverEntity.Guid;
                            }
                        }
                        return resolver.Guid;
                    }
                }
            }
            return null;
        }

        public bool UpdateRepairRequestStatus(string repairRequestId, bool isComplete, string completeOrRejectionText)
        {
            var repairRequest = _dbContext.Melding.Include(x => x.Oplosser).Where(x => x.Guid == repairRequestId)
                .SingleOrDefault();
            if (repairRequest != null && repairRequest.Afgehandeld != true)
            {
                if (isComplete && repairRequest.Oplosser.All(x => x.GecontroleerdOp != null && (x.OplosserStatus == (int)ResolverStatus.Completed || x.OplosserStatus == (int)ResolverStatus.TurnedDown)))
                {
                    repairRequest.Oplossing = completeOrRejectionText;
                    repairRequest.MeldingStatusGuid = _completedByBuyerStatusGuid;
                }
                if (!isComplete && repairRequest.Oplosser.All(x => x.OplosserStatus == (int)ResolverStatus.TurnedDown))
                {
                    var declinedStatusGuid = _dbContext.MeldingStatus.Where(x => x.MeldingStatus1.Equals(_declinedStatusText)).Select(x => x.Guid).SingleOrDefault();
                    repairRequest.MeldingStatusGuid = declinedStatusGuid;
                    repairRequest.RedenAfwijzing = completeOrRejectionText;
                }
                repairRequest.DatumAfhandeling = DateTime.Now;
                return true;
            }
            return false;
        }

        public Dictionary<string, string> GetDefaultEmailTokensForWorkOrder(string resolverId, string employeeId)
        {
            var resolver = GetWorkOrderDetails(resolverId);

            if (resolver != null)
            {
                var repairRequest = _dbContext.Melding.Where(x => x.Guid == resolver.RepairRequestId)
                    .Include(x => x.MeldingLocatieGu)
                    .Include(x => x.WerkGu)
                    .Select(x => new
                    {
                        x.WerkGu,
                        x.MeldingLocatieGu,
                        x.OpdrachtgeverOrganisatieGuid,
                        x.OpdrachtgeverRelatieGuid,
                        x.AanspreekpuntVoorOplosser,
                        x.VoorkeurstijdstipAfspraak
                    }).SingleOrDefault();

                var building = _dbContext.Gebouw.Where(x => x.Guid == resolver.BuildingId)
                    .Include(x => x.AdresGu).Include(x => x.WerkGu.HoofdaannemerOrganisatieGu)
                    .Select(x => new
                    {
                        x.BouwnummerIntern,
                        x.BouwnummerExtern,
                        OrganisationName = x.WerkGu.HoofdaannemerOrganisatieGu.Naam,
                        x.AdresGu
                    }).SingleOrDefault();

                var resultDict = new Dictionary<string, string>();
                resultDict["[bouwnummer_intern]"] = building?.BouwnummerIntern;
                resultDict["[bouwnummer_extern]"] = building?.BouwnummerExtern;
                resultDict["[hoofdaannemer]"] = building.OrganisationName ?? string.Empty;
                resultDict["[werkbonnummer]"] = resolver.WorkOrderNumber;
                resultDict["[werknaam]"] = repairRequest.WerkGu?.Werknummer + " - " + repairRequest.WerkGu?.Werknaam;
                resultDict["[melding_datum]"] = resolver.RepairRequestDate?.ToString("dd-MM-yyyy");
                resultDict["[oplosser_datum_afgehandeld]"] = resolver.DateHandled?.ToString("dd-MM-yyyy") ?? string.Empty;
                resultDict["[oplosser_Toelichting]"] = resolver.Explanation?.Replace("\n", "<br />");
                resultDict["[oplosser_omschrijving]"] = resolver.Description;
                resultDict["[melding_locatie]"] = repairRequest.MeldingLocatieGu?.Locatie;
                resultDict["[melding_soort]"] = resolver.CarryOutAsType;
                resultDict["[melding_melder]"] = _dbContext.GetDomainTranslation("gebouw_beheerder_rol", resolver.RepairRequestRole.ToString());
                resultDict["[melding_nummer]"] = resolver.RepairRequestNo;
                resultDict["[oplosser_tekst_werkbon]"] = resolver.WorkOrderText?.Replace("\n", "<br />");
                resultDict["[object_straat]"] = building?.AdresGu?.Straat;
                resultDict["[object_huisnummer]"] = string.Format("{0} {1}", building?.AdresGu?.Nummer ?? string.Empty, building?.AdresGu?.NummerToevoeging ?? string.Empty);
                resultDict["[object_postcode]"] = building?.AdresGu?.Postcode;
                resultDict["[object_plaats]"] = building?.AdresGu?.Plaats;
                resultDict["[melding_datumoplevering]"] = resolver.CompletionDate?.ToString("dd-MM-yyyy") ?? string.Empty;
                resultDict["[melding_datumeindegarantietermijn]"] = resolver.WarrantyEndDate?.ToString("dd-MM-yyyy") ?? string.Empty;
                resultDict["[melding_datumeindeonderhoudstermijn]"] = resolver.MaintenanceEndDate?.ToString("dd-MM-yyyy") ?? string.Empty;
                resultDict["[oplosser_datum_streefdatumafhandeling]"] = resolver.TargetCompletionDate?.ToString("dd-MM-yyyy") ?? string.Empty;
                var contactInfo = resolver.ContactInfo;
                resultDict["[aanspreekpunt_naam]"] = string.Empty;
                resultDict["[aanspreekpunt_telefoon]"] = string.Empty;
                resultDict["[aanspreekpunt_email]"] = string.Empty;
                resultDict["[aanspreekpunt_telefoon_privé]"] = string.Empty;
                resultDict["[aanspreekpunt_email_privé]"] = string.Empty;
                resultDict["[aanspreekpunt_voor_oplosser]"] = string.Empty;
                resultDict["[client_organisatie_relation_naam]"] = string.Empty;
                if (contactInfo != null)
                {
                    if (contactInfo.Client != null || contactInfo.Buyer != null || contactInfo.Employee != null || contactInfo.VvE != null || contactInfo.VvEAdministrator != null || contactInfo.PropertyManager != null)
                    {
                        resultDict["[aanspreekpunt_voor_oplosser]"] = _dbContext.GetDomainTranslation("gebouw_beheerder_rol", repairRequest.AanspreekpuntVoorOplosser.ToString());
                    }
                    if (contactInfo.Client != null && resolver.PointOfContact == BuildingManagerRole.opdrachtgever)
                    {
                        var relationId = contactInfo.Client.RelationId;
                        var OrgName = contactInfo.Client.Name;
                        resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                        resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.Client.RelationMobile) ? contactInfo.Client.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.Client.Telephone) ? contactInfo.Client.Telephone : string.Empty;
                        resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.Client.RelationEmail) ? contactInfo.Client.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.Client.Email) ? contactInfo.Client.Email : string.Empty;
                    }
                    else if (contactInfo.VvE != null && resolver.PointOfContact == BuildingManagerRole.vve)
                    {
                        var relationId = contactInfo.VvE.RelationId;
                        var OrgName = contactInfo.VvE.Name;
                        resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                        resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.VvE.RelationMobile) ? contactInfo.VvE.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.VvE.Telephone) ? contactInfo.VvE.Telephone : string.Empty;
                        resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.VvE.RelationEmail) ? contactInfo.VvE.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.VvE.Email) ? contactInfo.VvE.Email : string.Empty;
                    }
                    else if (contactInfo.VvEAdministrator != null && resolver.PointOfContact == BuildingManagerRole.vve_beheerder)
                    {
                        var relationId = contactInfo.VvEAdministrator.RelationId;
                        var OrgName = contactInfo.VvEAdministrator.Name;
                        resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                        resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.VvEAdministrator.RelationMobile) ? contactInfo.VvEAdministrator.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.VvEAdministrator.Telephone) ? contactInfo.VvEAdministrator.Telephone : string.Empty;
                        resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.VvEAdministrator.RelationEmail) ? contactInfo.VvEAdministrator.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.VvEAdministrator.Email) ? contactInfo.VvEAdministrator.Email : string.Empty;
                    }
                    else if (contactInfo.PropertyManager != null && resolver.PointOfContact == BuildingManagerRole.vastgoed_beheerder)
                    {
                        var relationId = contactInfo.PropertyManager.RelationId;
                        var OrgName = contactInfo.PropertyManager.Name;
                        resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                        resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.PropertyManager.RelationMobile) ? contactInfo.PropertyManager.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.PropertyManager.Telephone) ? contactInfo.PropertyManager.Telephone : string.Empty;
                        resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.PropertyManager.RelationEmail) ? contactInfo.PropertyManager.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.PropertyManager.Email) ? contactInfo.PropertyManager.Email : string.Empty;
                    }
                    else if (contactInfo.Employee != null && resolver.PointOfContact == BuildingManagerRole.medewerker)
                    {
                        var relationId = contactInfo.Employee.RelationId;
                        var OrgName = contactInfo.Employee.Name;
                        resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                        resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.Employee.RelationMobile) ? contactInfo.Employee.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.Employee.Telephone) ? contactInfo.Employee.Telephone : string.Empty;
                        resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.Employee.RelationEmail) ? contactInfo.Employee.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.Employee.Email) ? contactInfo.Employee.Email : string.Empty;
                    }
                    else if (contactInfo.Buyer != null && resolver.PointOfContact == BuildingManagerRole.gebouw_gebruiker)
                    {
                        if (contactInfo.Buyer.P1 != null && contactInfo.Buyer.P2 != null)
                        {
                            resultDict["[aanspreekpunt_naam]"] = string.Join(", ", new string[] { contactInfo.Buyer.P1.Name, contactInfo.Buyer.P2.Name }.Where(c => !string.IsNullOrEmpty(c)));
                            resultDict["[aanspreekpunt_email]"] = string.Join(", ", new string[] { contactInfo.Buyer.P1.EmailWork, contactInfo.Buyer.P2.EmailWork }.Where(c => !string.IsNullOrEmpty(c)));
                            resultDict["[aanspreekpunt_telefoon]"] = string.Join(", ", new string[] { contactInfo.Buyer.P1.TelephoneWork, contactInfo.Buyer.P1.Mobile, contactInfo.Buyer.P2.TelephoneWork, contactInfo.Buyer.P2.Mobile }.Where(c => !string.IsNullOrEmpty(c)));
                            resultDict["[aanspreekpunt_telefoon_privé]"] = string.Join(", ", new string[] { contactInfo.Buyer.P1.TelephonePrivate, contactInfo.Buyer.P2.TelephonePrivate }.Where(c => !string.IsNullOrEmpty(c)));
                            resultDict["[aanspreekpunt_email_privé]"] = string.Join(", ", new string[] { contactInfo.Buyer.P1.EmailPrivate, contactInfo.Buyer.P2.EmailPrivate }.Where(c => !string.IsNullOrEmpty(c)));
                        }
                        else if (contactInfo.Buyer.P1 != null)
                        {
                            resultDict["[aanspreekpunt_naam]"] = contactInfo.Buyer.P1.Name ?? string.Empty;
                            resultDict["[aanspreekpunt_email]"] = contactInfo.Buyer.P1.EmailWork ?? string.Empty;
                            resultDict["[aanspreekpunt_telefoon]"] = contactInfo.Buyer.P1.TelephoneWork ?? contactInfo.Buyer.P1.Mobile ?? string.Empty;
                            resultDict["[aanspreekpunt_telefoon_privé]"] = contactInfo.Buyer.P1.TelephonePrivate ?? string.Empty;
                            resultDict["[aanspreekpunt_email_privé]"] = contactInfo.Buyer.P1.EmailPrivate ?? string.Empty;
                        }
                        else if (contactInfo.Buyer.P2 != null)
                        {
                            resultDict["[aanspreekpunt_naam]"] = contactInfo.Buyer.P2.Name ?? string.Empty;
                            resultDict["[aanspreekpunt_email]"] = contactInfo.Buyer.P2.EmailWork ?? string.Empty;
                            resultDict["[aanspreekpunt_telefoon]"] = contactInfo.Buyer.P2.TelephoneWork ?? contactInfo.Buyer.P2.Mobile ?? string.Empty;
                            resultDict["[aanspreekpunt_telefoon_privé]"] = contactInfo.Buyer.P2.TelephonePrivate;
                            resultDict["[aanspreekpunt_email_privé]"] = contactInfo.Buyer.P2.EmailPrivate;
                        }
                        if (contactInfo.Buyer.Org != null)
                        {
                            var relationId = contactInfo.Buyer.Org.RelationId;
                            var OrgName = contactInfo.Buyer.Org.Name;
                            resultDict["[aanspreekpunt_naam]"] = !string.IsNullOrWhiteSpace(relationId) ? OrgName + " - " + GetOrgRelationName(relationId) : OrgName ?? string.Empty;
                            resultDict["[aanspreekpunt_telefoon]"] = !string.IsNullOrWhiteSpace(contactInfo.Buyer.Org.RelationMobile) ? contactInfo.Buyer.Org.RelationMobile : !string.IsNullOrWhiteSpace(contactInfo.Buyer.Org.Telephone) ? contactInfo.Buyer.Org.Telephone : string.Empty;
                            resultDict["[aanspreekpunt_email]"] = !string.IsNullOrWhiteSpace(contactInfo.Buyer.Org.RelationEmail) ? contactInfo.Buyer.Org.RelationEmail : !string.IsNullOrWhiteSpace(contactInfo.Buyer.Org.Email) ? contactInfo.Buyer.Org.Email : string.Empty;
                        }
                    }
                }
                resultDict["[oplosser_naam]"] = !string.IsNullOrWhiteSpace(resolver.RelationId) ? resolver.Name + " - " + GetOrgRelationName(resolver.RelationId) : resolver.Name;
                resultDict["[datumtweedehandtekeningondertekening]"] = resolver.SecondSignatureDate?.ToString("dd-MM-yyyy") ?? string.Empty;
                resultDict["[datum_ingelicht]"] = resolver.DateNotified?.ToString("dd-MM-yyyy") ?? string.Empty;
                string bijlage = string.Empty;
                resultDict["[slotgroet]"] = string.Empty;
                resultDict["[ondertekenaar_naam]"] = string.Empty;
                resultDict["[organisatie_naamonderdeel]"] = string.Empty;
                resultDict["[organisatie_logo]"] = string.Empty;
                if (resolver.SolutionImages.Any() || resolver.RepairRequestImages.Any())
                {
                    var builder = new StringBuilder();
                    var contentId = string.Empty;
                    foreach (var images in resolver.RepairRequestImages)
                    {
                        contentId = _siteUrl + "api/RepairRequest/GetImage/" + resolver.RepairRequestId + "/" + images.AttachmentId;
                        builder.Append("<a href=\"" + contentId + "\" ><img width=100 style=\"margin:5px\" src =\"" + contentId + " \"/></a>");
                    }
                    foreach (var images in resolver.SolutionImages)
                    {
                        contentId = _siteUrl + "api/RepairRequest/GetImage/" + resolver.RepairRequestId + "/" + images.AttachmentId;
                        builder.Append("<a href=\"" + contentId + "\" ><img width=100 style=\"margin:5px\" src =\"" + contentId + " \"/></a>");
                    }
                    resultDict["[bijlagen]"] = builder.ToString();
                }
                resultDict["[workorder_direct_link]"] = _siteUrl + "werkbon/" + resolverId;
                resultDict["[workorder_login_link]"] = _siteUrl + "werk/" + repairRequest.WerkGu?.Werknummer + "/werkbon/" + resolverId;
                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    var employeeData = _dbContext.Medewerker.Where(x => x.Guid == employeeId).Select(x => new { x.Slotgroet, x.Ondertekenaar }).SingleOrDefault();
                    if (employeeData != null)
                    {
                        resultDict["[slotgroet]"] = employeeData.Slotgroet;
                        resultDict["[ondertekenaar_naam]"] = employeeData.Ondertekenaar;
                        var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                        if (!string.IsNullOrWhiteSpace(currentUserId))
                        {
                            var organisationId = _dbContext.Login.Where(x => x.Guid == currentUserId && !x.Verwijderd).Select(x => x.OrganisatieGuid).SingleOrDefault();
                            if (!string.IsNullOrWhiteSpace(organisationId))
                            {
                                var organisationName = _dbContext.Organisatie.Find(organisationId)?.NaamOnderdeel;
                                resultDict["[organisatie_naamonderdeel]"] = !string.IsNullOrWhiteSpace(organisationName) ? organisationName : string.Empty;
                                var organisationLogo = _siteUrl + "api/Organisation/GetOrganisationLogo/" + organisationId;
                                resultDict["[organisatie_logo]"] = "<a href=\"" + organisationLogo + "\" ><img width=100 style=\"margin:5px\" src =\"" + organisationLogo + " \"/></a>";
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(repairRequest.OpdrachtgeverOrganisatieGuid))
                {
                    resultDict["[client_organisatie_relation_naam]"] = GetOrgRelationName(repairRequest.OpdrachtgeverRelatieGuid, repairRequest.OpdrachtgeverOrganisatieGuid);
                }
                resultDict["[melding_voorkeurstijdstip_afspraak]"] = repairRequest.VoorkeurstijdstipAfspraak ?? string.Empty;
                return resultDict;
            }
            return null;
        }

        public RepairRequestEmailModel GetEmailsForWorkOrderResolver(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                .Include(x => x.MeldingGu)
                .Select(x => new
                {
                    x.MeldingGu,
                    x.RelatieGuid,
                    x.OrganisatieGuid,
                }).SingleOrDefault();

            if (resolver != null)
            {
                var resolverEmailsModel = new RepairRequestEmailModel();

                var repairRequest = resolver.MeldingGu;

                string resolverEmails = GetEmailForWorkOrder(resolver.RelatieGuid, resolver.OrganisatieGuid);
                if (resolverEmails != null)
                    resolverEmailsModel.ResolverEmails.Add(resolverEmails);

                if (repairRequest != null)
                {
                    string clientEmails = GetEmailForWorkOrder(repairRequest.OpdrachtgeverRelatieGuid, repairRequest.OpdrachtgeverOrganisatieGuid);
                    if (clientEmails != null)
                        resolverEmailsModel.ClientEmails.Add(clientEmails);

                    string vveEmails = GetEmailForWorkOrder(repairRequest.VveRelatieGuid, repairRequest.VveOrganisatieGuid);
                    if (vveEmails != null)
                        resolverEmailsModel.VvEEmails.Add(vveEmails);

                    string vveAdminEmails = GetEmailForWorkOrder(repairRequest.VveBeheerderRelatieGuid, repairRequest.VveBeheerderOrganisatieGuid);
                    if (vveAdminEmails != null)
                        resolverEmailsModel.VvEAdministratorEmails.Add(vveAdminEmails);

                    string propertyEmails = GetEmailForWorkOrder(repairRequest.VastgoedBeheerderRelatieGuid, repairRequest.VastgoedBeheerderOrganisatieGuid);
                    if (propertyEmails != null)
                        resolverEmailsModel.PropertyManagerEmails.Add(propertyEmails);

                    if (!string.IsNullOrWhiteSpace(repairRequest.MelderKoperHuurderGuid))
                    {
                        resolverEmailsModel.ReporterEmails = (_dbContext.GetEmailsForBuyerRenter(repairRequest.MelderKoperHuurderGuid) ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                    {
                        string reporterEmails = GetEmailForWorkOrder(repairRequest.MelderRelatieGuid, repairRequest.MelderOrganisatieGuid);
                        if (reporterEmails != null)
                            resolverEmailsModel.ReporterEmails.Add(reporterEmails);
                    }

                    if (!string.IsNullOrWhiteSpace(repairRequest.GebouwGebruikerKoperHuurderGuid))
                    {
                        resolverEmailsModel.BuyerEmails = (_dbContext.GetEmailsForBuyerRenter(repairRequest.GebouwGebruikerKoperHuurderGuid) ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                }
                return resolverEmailsModel;
            }
            return null;
        }

        public bool MarkResolverStatusAsInformed(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                   .SingleOrDefault();
            if (resolver != null && resolver.OplosserStatus == (int)ResolverStatus.New && resolver.Werkbonnummer != null)
            {
                resolver.OplosserStatus = (int)ResolverStatus.Informed;
                resolver.DatumIngelicht = DateTime.Now;
                return true;
            }
            return false;
        }

        public string GetResolverEmail(string resolverId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                .Include(x => x.OrganisatieGu)
                .Include(x => x.RelatieGu)
                .Select(x => new { x.OrganisatieGu.Email, x.RelatieGu.EmailZakelijk }).SingleOrDefault();

            if (resolver != null)
            {
                var resolverEmail = resolver.EmailZakelijk ?? resolver.Email;
                return resolverEmail;
            }
            return null;
        }

        public List<string> GetSiteManagerEmails(string resolverId)
        {
            var usersData = GetSiteManagerDetails(resolverId);
            List<string> userEmails = new List<string>();
            foreach (var user in usersData)
            {
                userEmails.Add(user.Email);
            }
            return userEmails;
        }

        public IEnumerable<RepairRequestResolverApiModel> GetWorkOrdersByProjectId(string projectId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var organisationId = _dbContext.Login.Where(x => x.Guid == currentUserId && !x.Verwijderd).Select(x => x.OrganisatieGuid).SingleOrDefault();

            var result = _dbContext.Oplosser
                .Include(x => x.OrganisatieGu)
                .Include(x => x.RelatieGu).ThenInclude(x => x.PersoonGu)
                .Include(x => x.RelatieGu.AfdelingGu)
                .Include(x => x.MeldingGu).ThenInclude(x => x.Bijlage)
                .Where(x => x.MeldingGu.WerkGuid == projectId && x.OrganisatieGuid == organisationId && !string.IsNullOrWhiteSpace(x.Werkbonnummer))
                .OrderBy(x => x.OplosserStatus)
                .ThenByDescending(x => x.Werkbonnummer)
                .ThenByDescending(x => x.DatumAfhandeling)
                .Join(_dbContext.Gebouw.Include(x => x.AdresGu)
                  , z => z.MeldingGu.GebouwGuid,
                  y => y.Guid, (oplosser, gebouw) => new { oplosser, gebouw.AdresGu })
                .Select(x => new RepairRequestResolverApiModel
                {
                    ResolverId = x.oplosser.Guid,
                    Name = x.oplosser.OrganisatieGu.NaamOnderdeel ?? string.Empty,
                    RelationName = x.oplosser.RelatieGu.PersoonGu.Naam == _notApplicableRelationName ? x.oplosser.RelatieGu.AfdelingGu.Afdeling1 : x.oplosser.RelatieGu.PersoonGu.Naam ?? string.Empty,
                    Status = (ResolverStatus?)x.oplosser.OplosserStatus,
                    WorkOrderNumber = x.oplosser.Werkbonnummer,
                    Explanation = x.oplosser.Toelichting,
                    WorkOrderText = x.oplosser.TekstWerkbon,
                    TargetCompletionDate = x.oplosser.StreefdatumAfhandeling ?? x.oplosser.MeldingGu.StreefdatumAfhandeling,
                    Description = x.oplosser.Omschrijving ?? string.Empty,
                    BuildingId = x.oplosser.MeldingGu.GebouwGuid,
                    RepairRequestImages = x.oplosser.MeldingGu.Bijlage != null ? x.oplosser.MeldingGu.Bijlage.Where(y => y.OplosserGuid == null || y.OplosserGuid == x.oplosser.Guid).OrderBy(x => x.Volgorde).Take(1)//only first image needed in Grid
                    .Select(x => new RepairRequestAttachmentApiModel
                    {
                        AttachmentId = x.Guid,
                        ResolverId = x.OplosserGuid
                    }) : null,
                    Address = x.AdresGu != null ?
                    new AddressModel
                    {
                        Street = x.AdresGu.Straat,
                        HouseNo = x.AdresGu.Nummer,
                        HouseNoAddition = x.AdresGu.NummerToevoeging,
                        Postcode = x.AdresGu.Postcode,
                        Place = x.AdresGu.Plaats,
                    } : null
                }).ToList();

            return result;
        }

        public RepairRequestEmailModel GetEmailsForRepairRequest(string repairRequestId)
        {
            var repairRequest = _dbContext.Melding.Where(x => x.Guid == repairRequestId)
                .Include(x => x.Oplosser)
                .Select(x => new
                {
                    x.OpdrachtgeverOrganisatieGuid,
                    x.OpdrachtgeverRelatieGuid,
                    x.VveOrganisatieGuid,
                    x.VveRelatieGuid,
                    x.VveBeheerderOrganisatieGuid,
                    x.VveBeheerderRelatieGuid,
                    x.VastgoedBeheerderOrganisatieGuid,
                    x.VastgoedBeheerderRelatieGuid,
                    x.MelderKoperHuurderGuid,
                    x.MelderOrganisatieGuid,
                    x.MelderRelatieGuid,
                    x.GebouwGebruikerKoperHuurderGuid,
                    x.Oplosser
                }).SingleOrDefault();

            if (repairRequest != null)
            {
                var resolverEmailsModel = new RepairRequestEmailModel();

                if (repairRequest.Oplosser.Any())
                {
                    foreach (var resolver in repairRequest.Oplosser)
                    {
                        string resolverEmails = GetEmailForWorkOrder(resolver.RelatieGuid, resolver.OrganisatieGuid);
                        if (resolverEmails != null)
                            resolverEmailsModel.ResolverEmails.Add(resolverEmails);
                    }
                }

                string clientEmails = GetEmailForWorkOrder(repairRequest.OpdrachtgeverRelatieGuid, repairRequest.OpdrachtgeverOrganisatieGuid);
                if (clientEmails != null)
                    resolverEmailsModel.ClientEmails.Add(clientEmails);

                string vveEmails = GetEmailForWorkOrder(repairRequest.VveRelatieGuid, repairRequest.VveOrganisatieGuid);
                if (vveEmails != null)
                    resolverEmailsModel.VvEEmails.Add(vveEmails);

                string vveAdminEmails = GetEmailForWorkOrder(repairRequest.VveBeheerderRelatieGuid, repairRequest.VveBeheerderOrganisatieGuid);
                if (vveAdminEmails != null)
                    resolverEmailsModel.VvEAdministratorEmails.Add(vveAdminEmails);

                string propertyEmails = GetEmailForWorkOrder(repairRequest.VastgoedBeheerderRelatieGuid, repairRequest.VastgoedBeheerderOrganisatieGuid);
                if (propertyEmails != null)
                    resolverEmailsModel.PropertyManagerEmails.Add(propertyEmails);

                if (!string.IsNullOrWhiteSpace(repairRequest.MelderKoperHuurderGuid))
                {
                    resolverEmailsModel.ReporterEmails = (_dbContext.GetEmailsForBuyerRenter(repairRequest.MelderKoperHuurderGuid) ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                {
                    string reporterEmails = GetEmailForWorkOrder(repairRequest.MelderRelatieGuid, repairRequest.MelderOrganisatieGuid);
                    if (reporterEmails != null)
                        resolverEmailsModel.ReporterEmails.Add(reporterEmails);
                }

                if (!string.IsNullOrWhiteSpace(repairRequest.GebouwGebruikerKoperHuurderGuid))
                {
                    resolverEmailsModel.BuyerEmails = (_dbContext.GetEmailsForBuyerRenter(repairRequest.GebouwGebruikerKoperHuurderGuid) ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                return resolverEmailsModel;
            }
            return null;
        }

        private string GetEmailForWorkOrder(string relationId, string organisationId)
        {
            string email = null;
            if (!string.IsNullOrWhiteSpace(relationId))
            {
                email = _dbContext.Relatie.Where(y => y.Guid == relationId).Select(x => x.EmailZakelijk).SingleOrDefault();
            }
            if (string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(organisationId))
            {
                email = _dbContext.Organisatie.Where(x => x.Guid == organisationId).Select(x => x.Email).SingleOrDefault();
            }
            return email;
        }

        public Dictionary<string, List<string>> GetRepairRequestToEmailsAndSalutation(string repairRequestId, RepairRequestEmailModel repairRequestEmails)
        {
            Dictionary<string, List<string>> emailWithSalutation = new Dictionary<string, List<string>>();
            List<string> emailList = new List<string>();
            if (repairRequestEmails != null)
            {
                if (repairRequestEmails.ClientEmails != null && repairRequestEmails.ClientEmails.Any())
                {
                    foreach (string clientEmail in repairRequestEmails.ClientEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(clientEmail))
                        {
                            emailList.Add(clientEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "client"), emailList);
                    }
                }
                if (repairRequestEmails.VvEEmails != null && repairRequestEmails.VvEEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string vvEEmail in repairRequestEmails.VvEEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(vvEEmail))
                        {
                            emailList.Add(vvEEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "vve"), emailList);
                    }
                }
                if (repairRequestEmails.VvEAdministratorEmails != null && repairRequestEmails.VvEAdministratorEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string vvEAdministratorEmail in repairRequestEmails.VvEAdministratorEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(vvEAdministratorEmail))
                        {
                            emailList.Add(vvEAdministratorEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "vveadministrator"), emailList);
                    }
                }
                if (repairRequestEmails.PropertyManagerEmails != null && repairRequestEmails.PropertyManagerEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string propertyManagerEmail in repairRequestEmails.PropertyManagerEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(propertyManagerEmail))
                        {
                            emailList.Add(propertyManagerEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "propertymanager"), emailList);
                    }
                }
                if (repairRequestEmails.ReporterEmails != null && repairRequestEmails.ReporterEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string reporterEmail in repairRequestEmails.ReporterEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(reporterEmail))
                        {
                            emailList.Add(reporterEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "reporter"), emailList);
                    }
                }
                if (repairRequestEmails.BuyerEmails != null && repairRequestEmails.BuyerEmails.Any())
                {
                    emailList = new List<string>();
                    foreach (string buyerEmail in repairRequestEmails.BuyerEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(buyerEmail))
                        {
                            emailList.Add(buyerEmail);
                        }
                    }
                    if (emailList != null && emailList.Any())
                    {
                        emailWithSalutation.Add(GetSalutationForRepairRequestEmail(repairRequestId, "buyer"), emailList);
                    }
                }
            }
            return emailWithSalutation;
        }

        public string GetSalutationForEmail(string resolverId, string notifyTo, string siteManagerEmail = null)
        {
            var salutation = string.Empty;
            if (notifyTo.Equals("sitemanager"))
            {
                var usersData = GetSiteManagerDetails(resolverId);
                foreach (var user in usersData)
                {
                    if (user.Email.Equals(siteManagerEmail))
                    {
                        salutation = GetSalutation(user.RelationId, user.OrganisationId);
                    }
                }
            }
            return salutation;
        }

        private string GetSalutationForRepairRequestEmail(string repairRequestId, string notifyTo)
        {
            var salutation = string.Empty;
            var repairRequest = _dbContext.Melding.Where(x => x.Guid == repairRequestId)
                .Select(x => new
                {
                    x.OpdrachtgeverOrganisatieGuid,
                    x.OpdrachtgeverRelatieGuid,
                    x.VveOrganisatieGuid,
                    x.VveRelatieGuid,
                    x.VveBeheerderOrganisatieGuid,
                    x.VveBeheerderRelatieGuid,
                    x.VastgoedBeheerderOrganisatieGuid,
                    x.VastgoedBeheerderRelatieGuid,
                    x.MelderKoperHuurderGuid,
                    x.MelderOrganisatieGuid,
                    x.MelderRelatieGuid,
                    x.GebouwGebruikerKoperHuurderGuid
                }).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(salutation))
            {
                if (repairRequest != null)
                {
                    switch (notifyTo)
                    {
                        case "client":
                            salutation = GetSalutation(repairRequest.OpdrachtgeverRelatieGuid, repairRequest.OpdrachtgeverOrganisatieGuid);
                            break;
                        case "vve":
                            salutation = GetSalutation(repairRequest.VveRelatieGuid, repairRequest.VveOrganisatieGuid);
                            break;
                        case "vveadministrator":
                            salutation = GetSalutation(repairRequest.VveBeheerderRelatieGuid, repairRequest.VveBeheerderOrganisatieGuid);
                            break;
                        case "propertymanager":
                            salutation = GetSalutation(repairRequest.VastgoedBeheerderRelatieGuid, repairRequest.VastgoedBeheerderOrganisatieGuid);
                            break;
                        case "reporter":
                            salutation = !string.IsNullOrWhiteSpace(repairRequest.MelderKoperHuurderGuid) ?
                                GetSalutation(null, null, repairRequest.MelderKoperHuurderGuid) :
                                GetSalutation(repairRequest.MelderRelatieGuid, repairRequest.MelderOrganisatieGuid);
                            break;
                        case "buyer":
                            salutation = GetSalutation(null, null, repairRequest.GebouwGebruikerKoperHuurderGuid);
                            break;
                        default:
                            return null;
                    }
                    return salutation;
                }
            }
            return null;
        }

        private string GetSalutation(string relationId, string organisationId, string buyerId = null, bool isFormal = true)
        {
            var salutation = string.Empty;
            if (!string.IsNullOrWhiteSpace(relationId))
            {
                salutation = _dbContext.GetSalutationForEmail(null, null, relationId, null, isFormal);
            }
            if (string.IsNullOrWhiteSpace(salutation) && !string.IsNullOrWhiteSpace(organisationId))
            {
                salutation = _dbContext.GetSalutationForEmail(organisationId, null, null, null, isFormal); ;
            }
            if (string.IsNullOrWhiteSpace(salutation) && !string.IsNullOrWhiteSpace(buyerId))
            {
                salutation = _dbContext.GetSalutationForEmail(null, null, null, buyerId, isFormal);
            }
            return salutation;
        }

        public string CreateCommunicationForWorkOrder(string resolverId, string emailTemplateWithoutToken, string employeeId, string emailAccountId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                .Include(x => x.MeldingGu).ThenInclude(x => x.WerkGu)
                .Select(x => new
                {
                    x.MeldingGuid,
                    x.MeldingGu.WerkGuid,
                    x.MeldingGu.GebouwGuid,
                    x.Werkbonnummer,
                    x.MeldingGu.WerkGu.Werknummer
                }).SingleOrDefault();

            if (resolver != null)
            {
                string kenmarkformat = _dbContext.ConfiguratieAlgemeen.Select(x => x.CommunicatieKenmerkOpmaak).SingleOrDefault();
                var employeeData = !string.IsNullOrWhiteSpace(employeeId) ? _dbContext.Medewerker.Where(x => x.Guid == employeeId).Select(x => new { x.Initialen, x.AfdelingGuid }).SingleOrDefault() : null;
                int kenmarkSequentialNumber = (_dbContext.Communicaties.Where(r => r.Datum.Year == DateTime.Today.Year).Max(x => x.CommunicatieKenmerkVolgnummer) ?? 0) + 1;
                string departmentCode = _dbContext.Afdelings.Where(x => x.Guid == employeeData.AfdelingGuid).Select(x => x.CodeTbvCommunicatieKenmerk).SingleOrDefault();
                var kenmarkValue = _dbContext.GetOnsKenmark(kenmarkformat, employeeData.Initialen, DateTime.Now, kenmarkSequentialNumber, departmentCode, resolver.Werknummer);

                var communication = new Communicatie
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    MeldingGuid = resolver.MeldingGuid,
                    CommunicatieSoort = (int)CommunicationMethod.Email,
                    CommunicatieType = (int)CommunicationType.Outgoing,
                    WerkGuid = resolver.WerkGuid,
                    GebouwGuid = resolver.GebouwGuid,
                    Verslag = emailTemplateWithoutToken,
                    Betreft = "Werkbon " + resolver.Werkbonnummer,
                    Datum = DateTime.Now,
                    OnsKenmerk = kenmarkValue,
                    MedewerkerGuid = employeeId,
                    AfdelingGuid = employeeData?.AfdelingGuid,
                    AfzenderEmailAccountGuid = emailAccountId,
                    CommunicatieKenmerkVolgnummer = kenmarkSequentialNumber,
                    CommunicatieStatus = (int)CommunicationStatus.Sent
                };
                _dbContext.Communicaties.Add(communication);
                return communication.Guid;
            }
            return null;
        }

        public void CreateContactsForWorkorder(string resolverId, string communicationId, string emailTemplatewithToken, string toEmails, string ccEmails, ContactDetailsforReceiver receiverContactDetails)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                .Include(x => x.MeldingGu).Select(x => new { x.MeldingGu.GebouwGuid })
                .SingleOrDefault();

            if (resolver != null)
            {
                if (receiverContactDetails != null && receiverContactDetails.ContactAddressee != null)
                {
                    var contact = new Contact
                    {
                        Guid = Guid.NewGuid().ToUpperString(),
                        CommunicatieGuid = communicationId,
                        Contact1 = (byte)receiverContactDetails.ContactAddressee,
                        OrganisatieGuid = receiverContactDetails.OrganisationId,
                        RelatieGuid = receiverContactDetails.RelationId,
                        KoperHuurderGuid = receiverContactDetails.BuyerId,
                        PersoonGuid = receiverContactDetails.PersonId,
                        GebouwGuid = resolver.GebouwGuid,
                        EmailBericht = emailTemplatewithToken,
                        Formeel = true,
                        Email = toEmails,
                        EmailCc = ccEmails,
                        EmailVerzonden = DateTime.Now
                    };
                    _dbContext.Contacts.Add(contact);
                }
            }
        }

        public ContactDetailsforReceiver GetReceiverContactDetails(string resolverId, string toEmailContactBuildingRole)
        {
            string relationId = null;
            string organisationId = null;
            string buyerId = null;
            string personId = null;
            string formalSalutation = null;
            string inFormalSalutation = null;

            var contactDetails = new ContactDetailsforReceiver();
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                .Include(x => x.MeldingGu).Select(x => new
                {
                    x.RelatieGuid,
                    x.OrganisatieGuid,
                    x.MeldingGu.OpdrachtgeverOrganisatieGuid,
                    x.MeldingGu.OpdrachtgeverRelatieGuid,
                    x.MeldingGu.VveOrganisatieGuid,
                    x.MeldingGu.VveRelatieGuid,
                    x.MeldingGu.VveBeheerderOrganisatieGuid,
                    x.MeldingGu.VveBeheerderRelatieGuid,
                    x.MeldingGu.VastgoedBeheerderOrganisatieGuid,
                    x.MeldingGu.VastgoedBeheerderRelatieGuid,
                    x.MeldingGu.MelderKoperHuurderGuid,
                    x.MeldingGu.MelderRelatieGuid,
                    x.MeldingGu.MelderOrganisatieGuid,
                    x.MeldingGu.GebouwGebruikerKoperHuurderGuid
                }).SingleOrDefault();

            if (resolver != null)
            {
                switch (toEmailContactBuildingRole)
                {
                    case "client":
                        if (!string.IsNullOrWhiteSpace(resolver.OpdrachtgeverRelatieGuid))
                        {
                            relationId = resolver.OpdrachtgeverRelatieGuid;
                            organisationId = resolver.OpdrachtgeverOrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.OpdrachtgeverOrganisatieGuid))
                        {
                            organisationId = resolver.OpdrachtgeverOrganisatieGuid;
                        }
                        formalSalutation = GetSalutation(resolver.OpdrachtgeverRelatieGuid, resolver.OpdrachtgeverOrganisatieGuid, null, true);
                        inFormalSalutation = GetSalutation(resolver.OpdrachtgeverRelatieGuid, resolver.OpdrachtgeverOrganisatieGuid, null, false);
                        break;
                    case "vve":
                        if (!string.IsNullOrWhiteSpace(resolver.VveRelatieGuid))
                        {
                            relationId = resolver.VveRelatieGuid;
                            organisationId = resolver.VveOrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.VveOrganisatieGuid))
                        {
                            organisationId = resolver.VveOrganisatieGuid;
                        }
                        formalSalutation = GetSalutation(resolver.VveRelatieGuid, resolver.VveOrganisatieGuid, null, true);
                        inFormalSalutation = GetSalutation(resolver.VveRelatieGuid, resolver.VveOrganisatieGuid, null, false);
                        break;
                    case "vveadministrator":
                        if (!string.IsNullOrWhiteSpace(resolver.VveBeheerderRelatieGuid))
                        {
                            relationId = resolver.VveBeheerderRelatieGuid;
                            organisationId = resolver.VveBeheerderOrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.VveBeheerderOrganisatieGuid))
                        {
                            organisationId = resolver.VveBeheerderOrganisatieGuid;
                        }
                        formalSalutation = GetSalutation(resolver.VveBeheerderRelatieGuid, resolver.VveBeheerderOrganisatieGuid, null, true);
                        inFormalSalutation = GetSalutation(resolver.VveBeheerderRelatieGuid, resolver.VveBeheerderOrganisatieGuid, null, false);
                        break;
                    case "propertymanager":
                        if (!string.IsNullOrWhiteSpace(resolver.VastgoedBeheerderRelatieGuid))
                        {
                            relationId = resolver.VastgoedBeheerderRelatieGuid;
                            organisationId = resolver.VastgoedBeheerderOrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.VastgoedBeheerderOrganisatieGuid))
                        {
                            organisationId = resolver.VastgoedBeheerderOrganisatieGuid;
                        }
                        formalSalutation = GetSalutation(resolver.VastgoedBeheerderRelatieGuid, resolver.VastgoedBeheerderOrganisatieGuid, null, true);
                        inFormalSalutation = GetSalutation(resolver.VastgoedBeheerderRelatieGuid, resolver.VastgoedBeheerderOrganisatieGuid, null, false);
                        break;
                    case "reporter":
                        if (!string.IsNullOrWhiteSpace(resolver.MelderKoperHuurderGuid))
                        {
                            buyerId = resolver.MelderKoperHuurderGuid;
                            formalSalutation = GetSalutation(null, null, resolver.MelderKoperHuurderGuid, true);
                            inFormalSalutation = GetSalutation(null, null, resolver.MelderKoperHuurderGuid, false);
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.MelderRelatieGuid))
                        {
                            relationId = resolver.MelderRelatieGuid;
                            organisationId = resolver.MelderOrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.MelderOrganisatieGuid))
                        {
                            organisationId = resolver.MelderOrganisatieGuid;
                        }
                        if (string.IsNullOrWhiteSpace(resolver.MelderKoperHuurderGuid))
                        {
                            formalSalutation = GetSalutation(resolver.MelderRelatieGuid, resolver.MelderOrganisatieGuid, null, true);
                            inFormalSalutation = GetSalutation(resolver.MelderRelatieGuid, resolver.MelderOrganisatieGuid, null, false);
                        }
                        break;
                    case "buyer":
                        if (!string.IsNullOrWhiteSpace(resolver.GebouwGebruikerKoperHuurderGuid))
                        {
                            buyerId = resolver.GebouwGebruikerKoperHuurderGuid;
                            formalSalutation = GetSalutation(null, null, resolver.GebouwGebruikerKoperHuurderGuid, true);
                            inFormalSalutation = GetSalutation(null, null, resolver.GebouwGebruikerKoperHuurderGuid, false);
                        }
                        break;
                    case "resolver":
                        if (!string.IsNullOrWhiteSpace(resolver.RelatieGuid))
                        {
                            relationId = resolver.RelatieGuid;
                            organisationId = resolver.OrganisatieGuid;
                        }
                        else if (!string.IsNullOrWhiteSpace(resolver.OrganisatieGuid))
                        {
                            organisationId = resolver.OrganisatieGuid;
                        }
                        formalSalutation = GetSalutation(resolver.RelatieGuid, resolver.OrganisatieGuid, null, true);
                        inFormalSalutation = GetSalutation(resolver.RelatieGuid, resolver.OrganisatieGuid, null, false);
                        break;
                    default:
                        break;
                }
            }
            contactDetails.ContactAddressee = !string.IsNullOrWhiteSpace(relationId) ? ContactAddressee.Relation :
                                              !string.IsNullOrWhiteSpace(organisationId) ? ContactAddressee.Organisation :
                                              !string.IsNullOrWhiteSpace(buyerId) ? ContactAddressee.Buyer :
                                              !string.IsNullOrWhiteSpace(personId) ? ContactAddressee.Person : null;
            contactDetails.RelationId = relationId;
            contactDetails.OrganisationId = organisationId;
            contactDetails.BuyerId = buyerId;
            contactDetails.FormalSalutation = formalSalutation;
            contactDetails.InformalSalutation = inFormalSalutation;
            return contactDetails;

        }

        private string GetForecastingCarryOutAsTypeId(string repairRequestId, string productId, string productSub1Id, string productSub2Id)
        {
            string newCarryOutAsTypeId = null;
            string orderCarryOutAsTypeId = _dbContext.MeldingSoort.Where(x => x.MeldingSoort1.Equals("opdracht"))
                .Select(x => x.Guid).SingleOrDefault();
            string couponCarryOutAsTypeId = _dbContext.MeldingSoort.Where(x => x.MeldingSoort1.Equals("coulance"))
                .Select(x => x.Guid).SingleOrDefault();
            string otherCarryOutAsTypeId = _dbContext.MeldingSoort.Where(x => x.MeldingSoort1.Equals("overige"))
                .Select(x => x.Guid).SingleOrDefault();

            var repairRequest = _dbContext.Melding.Where(x => x.Guid == repairRequestId).
                Select(x => new
                {
                    x.GebouwGuid,
                    x.MeldingSoortGuid,
                    x.MeldingDatum
                }).SingleOrDefault();

            if (repairRequest != null)
            {
                string carryOutAsTypeId = repairRequest.MeldingSoortGuid;
                if (!string.IsNullOrWhiteSpace(repairRequest.GebouwGuid))
                {
                    var repairRequestDate = repairRequest.MeldingDatum;
                    var buildingData = _dbContext.Gebouw.Where(x => x.Guid == repairRequest.GebouwGuid)
                        .Select(x => new { x.DatumOplevering, x.DatumEindeOnderhoudstermijn }).SingleOrDefault();
                    var dateOfEndGuarantee = _dbContext.GetGuaranteeEndDate(repairRequest.GebouwGuid, productId, productSub1Id, productSub2Id);
                    if (buildingData.DatumEindeOnderhoudstermijn != null)
                    {
                        if (dateOfEndGuarantee != null)
                        {
                            if (repairRequestDate <= dateOfEndGuarantee)
                            {
                                newCarryOutAsTypeId = _dbContext.GetCarryOutAsTypeId(repairRequest.GebouwGuid, repairRequestDate);
                            }
                            // Opdracht
                            else if (carryOutAsTypeId == null || (!carryOutAsTypeId.Equals(couponCarryOutAsTypeId) && !carryOutAsTypeId.Equals(orderCarryOutAsTypeId)))
                            {
                                newCarryOutAsTypeId = orderCarryOutAsTypeId;
                            }
                        }
                        else if (repairRequestDate > buildingData.DatumEindeOnderhoudstermijn)
                        {
                            newCarryOutAsTypeId = orderCarryOutAsTypeId;
                        }
                        else
                        {
                            newCarryOutAsTypeId = _dbContext.GetCarryOutAsTypeId(repairRequest.GebouwGuid, repairRequestDate);
                        }
                    }
                    else
                    {
                        // Overige
                        if (carryOutAsTypeId == null || (!carryOutAsTypeId.Equals(couponCarryOutAsTypeId) && !carryOutAsTypeId.Equals(orderCarryOutAsTypeId)))
                        {
                            newCarryOutAsTypeId = otherCarryOutAsTypeId;
                        }
                    }
                    return newCarryOutAsTypeId;
                }
                return null;
            }
            return null;
        }

        public string GetOrgRelationName(string relationId, string organisationId = null)
        {
            string orgRelationName = null;
            var relationData = _dbContext.Relatie.Where(x => x.Guid == relationId)
                .Select(x => new { x.PersoonGu.Naam, x.AfdelingGu.Afdeling1 })
                .SingleOrDefault();
            if (relationData != null)
            {
                if (!string.IsNullOrWhiteSpace(relationData.Naam) && relationData.Naam.Equals(_notApplicableRelationName))
                    orgRelationName = relationData.Afdeling1;
                else
                    orgRelationName = relationData.Naam;
            }

            if (!string.IsNullOrWhiteSpace(organisationId))
            {
                string orgName = _dbContext.Organisatie.Where(x => x.Guid == organisationId).Select(x => x.Naam)
                       .SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(orgRelationName))
                {
                    orgRelationName = orgName + " - " + orgRelationName;
                }
                else
                {
                    orgRelationName = orgName;
                }
            }
            return orgRelationName;
        }

        public string CopyWorkOrder(string organisationId, string resolverId)
        {
            var resolver = _dbContext.Oplosser.Where(x => x.Guid == resolverId)
                    .Select(x => new
                    {
                        x.MeldingGuid,
                        x.MeldingSoortGuid,
                        x.StreefdatumAfhandeling,
                        x.TekstWerkbon,
                        x.Omschrijving,
                        x.Oplossing,
                        x.RedenAfwijzing,
                        x.Toelichting,
                        x.AfhandelingstermijnGuid,
                        RepairRequestCompleted = x.MeldingGu.Afgehandeld
                    }).SingleOrDefault();

            if (resolver != null && resolver.RepairRequestCompleted != true)
            {
                var resolverEntity = new Oplosser
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    MeldingGuid = resolver.MeldingGuid,
                    OrganisatieGuid = organisationId,
                    MeldingSoortGuid = resolver.MeldingSoortGuid,
                    StreefdatumAfhandeling = resolver.StreefdatumAfhandeling,
                    RelatieGuid = null,
                    TekstWerkbon = resolver.TekstWerkbon,
                    Omschrijving = resolver.Omschrijving,
                    Oplossing = resolver.Oplossing,
                    RedenAfwijzing = resolver.RedenAfwijzing,
                    Toelichting = resolver.Toelichting,
                    AfhandelingstermijnGuid = resolver.AfhandelingstermijnGuid
                };
                _dbContext.Oplosser.Add(resolverEntity);
                return resolverEntity.Guid;
            }
            return null;
        }

        public List<UserApiModel> GetSiteManagerDetails(string resolverId)
        {
            var usersData = _dbContext.ViewLogins.Join(_dbContext.LoginRolWerks,
                x => x.Guid,
                y => y.LoginGuid,
                (login, loginrolwerk) => new
                {
                    login,
                    loginrolwerk
                }).Join(_dbContext.ViewModuleRoles,
                x => new { x.loginrolwerk.ModuleGuid, RoleGuid = x.loginrolwerk.RolGuid },
                y => new { y.ModuleGuid, y.RoleGuid },
                (x, y) => new
                {
                    x.login.OrganisatieGuid,
                    x.login.Email,
                    x.login.RelatieGuid,
                    x.loginrolwerk.WerkGuid,
                    x.loginrolwerk.GebouwGuid,
                    y.ModuleName,
                    y.RoleName,
                    y.Active,
                    x.loginrolwerk.Actief
                }).Join(_dbContext.Oplosser.Include(x => x.MeldingGu),
                x => new { x.OrganisatieGuid, x.WerkGuid },
                y => new { y.OrganisatieGuid, y.MeldingGu.WerkGuid },
                (x, y) => new
                {
                    UserOrgnisationId = x.OrganisatieGuid,
                    UserEmail = x.Email,
                    UserRelationId = x.RelatieGuid,
                    ModuleBuildingId = x.GebouwGuid,
                    ResolverBuildingId = y.MeldingGu.GebouwGuid,
                    ResolverId = y.Guid,
                    x.ModuleName,
                    x.RoleName,
                    x.Active,
                    x.Actief
                }).Where(y => (y.ModuleBuildingId == null || y.ModuleBuildingId == y.ResolverBuildingId) && y.ResolverId == resolverId && y.ModuleName == LoginModules.ConstructionQuality && y.RoleName == LoginRoles.SiteManager && y.Active == true && y.Actief == true)
                .Select(x => new UserApiModel
                {
                    Email = x.UserEmail,
                    OrganisationId = x.UserOrgnisationId,
                    RelationId = x.UserRelationId
                }).Distinct();

            return usersData?.ToList();
        }
    }
}
