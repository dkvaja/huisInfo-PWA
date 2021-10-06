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
using Portal.JPDS.AppCore.Models;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class SurveyRepository : BaseRepository, ISurveyRepository
    {
        public SurveyRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public IEnumerable<SurveyApiModel> GetSurveys(string userId, SurveyType surveyType, int? noOfDaysForOffline, bool? isSecondSignature)
        {
            var employeeId = _dbContext.Login.Where(x => x.Guid == userId && !x.Verwijderd).Select(x => x.MedewerkerGuid).SingleOrDefault();

            var query = _dbContext.Opname
                .Include(x => x.WerkGu)
                .Include(x => x.Melding)
                .Where(x => x.UitgevoerdDoorMedewerkerGuid == employeeId && x.OpnameSoort == (int)surveyType);

            if (isSecondSignature == true && surveyType == SurveyType.Delivery)
            {
                query = query.Where(x => x.OpnameStatus == (int)SurveyStatus.Completed || x.OpnameStatus == (int)SurveyStatus.Sent);
            }

            if (noOfDaysForOffline.HasValue && noOfDaysForOffline > 0)
            {
                if (isSecondSignature == true)
                {
                    query = query.Where(x => Math.Abs(EF.Functions.DateDiffDay(x.DatumTweedeHandtekeningOndertekening, DateTime.Now) ?? 0) <= noOfDaysForOffline.Value);
                }
                else
                {
                    query = query.Where(x => Math.Abs(EF.Functions.DateDiffDay(x.Datum, DateTime.Now) ?? 0) <= noOfDaysForOffline.Value);
                }
            }

            return query
                .Join(_dbContext.Gebouw.Include(x => x.AdresGu), p => p.GebouwGuid, e => e.Guid, (opname, gebouw) => new SurveyApiModel(opname, gebouw, false))
                .ToList()
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.SecondSignatureDate)
                .ThenBy(x => x.BuildingNoExtern)
                .ThenBy(x => x.Address);
        }

        public string AddOrUpdateSurvey(AddOrUpdateSurveyApiModel surveyApiModel)
        {
            if (surveyApiModel.SurveyId != Guid.Empty)
            {
                var survey = _dbContext.Opname.Find(surveyApiModel.SurveyId.ToUpperString());
                if (survey != null && survey.OpnameStatus != (int)SurveyStatus.Completed && survey.OpnameStatus != (int)SurveyStatus.Sent)
                {
                    if (survey.OpnameSoort == (int)SurveyType.Inspection)
                    {
                        if (surveyApiModel.Date.HasValue && surveyApiModel.Date != DateTime.MinValue)
                        {
                            survey.Datum = surveyApiModel.Date;
                        }
                    }
                    else if (survey.OpnameSoort == (int)SurveyType.Delivery)
                    {
                        if (survey.Datum?.Date != DateTime.Now.Date)
                        {
                            survey.Datum = DateTime.Now;
                        }
                    }

                    survey.OpmerkingenUitvoerder = surveyApiModel.ExecutorComments;
                    survey.OpmerkingenKoperHuurder = surveyApiModel.BuyerRenterComments;
                    survey.MeterstandElektra1 = surveyApiModel.MeterReading_Electric1;
                    survey.MeterstandElektra2 = surveyApiModel.MeterReading_Electric2;
                    survey.MeterstandElektraRetour1 = surveyApiModel.MeterReading_ElectricReturn1;
                    survey.MeterstandElektraRetour2 = surveyApiModel.MeterReading_ElectricReturn2;
                    survey.MeterstandGasWarmte = surveyApiModel.MeterReading_GasHeat;
                    survey.MeterstandWater = surveyApiModel.MeterReading_Water;

                    if (!string.IsNullOrWhiteSpace(surveyApiModel.ReportHeader))
                    {
                        survey.KoptekstPvo = surveyApiModel.ReportHeader;
                    }
                    if (!string.IsNullOrWhiteSpace(surveyApiModel.ReportFooter))
                    {
                        survey.VoettekstPvo = surveyApiModel.ReportFooter;
                    }

                    //Need to improve this below...
                    if (surveyApiModel.ExecutorSignature != null && surveyApiModel.ExecutorSignature.Content != null && surveyApiModel.ExecutorSignature.Content.Length > 0)
                    {
                        survey.HandtekeningUitvoerder = surveyApiModel.ExecutorSignature.Name;
                        survey.HandtekeningUitvoerderFileOpslag = surveyApiModel.ExecutorSignature.Content;
                    }
                    if (surveyApiModel.BuyerRenter1Signature != null && surveyApiModel.BuyerRenter1Signature.Content != null && surveyApiModel.BuyerRenter1Signature.Content.Length > 0)
                    {
                        survey.HandtekeningKh1 = surveyApiModel.BuyerRenter1Signature.Name;
                        survey.HandtekeningKh1FileOpslag = surveyApiModel.BuyerRenter1Signature.Content;
                    }
                    if (surveyApiModel.BuyerRenter2Signature != null && surveyApiModel.BuyerRenter2Signature.Content != null && surveyApiModel.BuyerRenter2Signature.Content.Length > 0)
                    {
                        survey.HandtekeningKh2 = surveyApiModel.BuyerRenter2Signature.Name;
                        survey.HandtekeningKh2FileOpslag = surveyApiModel.BuyerRenter2Signature.Content;
                    }

                    if (survey.OpnameSoort != (int)SurveyType.Delivery || (surveyApiModel.Status != SurveyStatus.Completed && surveyApiModel.Status != SurveyStatus.Sent))
                    {
                        survey.OpnameStatus = (byte)surveyApiModel.Status;
                    }
                }
                return surveyApiModel.SurveyId.ToUpperString();
            }
            else
            {
                var executorId = surveyApiModel.ExecutorEmployeeId?.ToUpperString();

                string kh1Name = null, kh2Name = null;
                var building = _dbContext.Gebouw.Find(surveyApiModel.BuildingId.ToUpperString());
                var kh = _dbContext.KoperHuurder.Find(building.KoperHuurderGuid);
                if (kh != null)
                {
                    kh1Name = kh?.Persoon1Gu?.VolledigeNaam;
                    kh2Name = kh?.Persoon2Gu?.VolledigeNaam;
                }
                else
                {
                    kh1Name = _dbContext.Organisatie.Find(building.GebouwGebruikerOrganisatieGuid)?.Naam;
                }

                Opname survey = new Opname
                {
                    Guid = Guid.NewGuid().ToUpperString(),
                    WerkGuid = surveyApiModel.ProjectId.ToUpperString(),
                    GebouwGuid = surveyApiModel.BuildingId.ToUpperString(),
                    OpnameSoort = (byte)surveyApiModel.SurveyType,
                    Datum = surveyApiModel.Date,
                    UitgevoerdDoorMedewerkerGuid = executorId,
                    OpmerkingenUitvoerder = surveyApiModel.ExecutorComments,
                    OpmerkingenKoperHuurder = surveyApiModel.BuyerRenterComments,
                    MeterstandElektra1 = surveyApiModel.MeterReading_Electric1,
                    MeterstandElektra2 = surveyApiModel.MeterReading_Electric2,
                    MeterstandElektraRetour1 = surveyApiModel.MeterReading_ElectricReturn1,
                    MeterstandElektraRetour2 = surveyApiModel.MeterReading_ElectricReturn2,
                    MeterstandGasWarmte = surveyApiModel.MeterReading_GasHeat,
                    MeterstandWater = surveyApiModel.MeterReading_Water,
                    //Need to improve this below...
                    VolledigeNaamUitvoerder = _dbContext.ViewPortalMedewerker.Where(x => x.Guid == executorId).Select(x => x.PersoonVolledigeNaam).FirstOrDefault(),
                    HandtekeningUitvoerder = surveyApiModel.ExecutorSignature.Name,
                    HandtekeningUitvoerderFileOpslag = surveyApiModel.ExecutorSignature.Content,
                    VolledigeNaamKh1 = kh1Name,
                    HandtekeningKh1 = surveyApiModel.BuyerRenter1Signature.Name,
                    HandtekeningKh1FileOpslag = surveyApiModel.BuyerRenter1Signature.Content,
                    VolledigeNaamKh2 = kh2Name,
                    HandtekeningKh2 = surveyApiModel.BuyerRenter2Signature.Name,
                    HandtekeningKh2FileOpslag = surveyApiModel.BuyerRenter2Signature.Content,
                    OpnameStatus = (byte)surveyApiModel.Status
                };
                _dbContext.Opname.Add(survey);
                return survey.Guid;
            }
        }

        public string GetOfficialReportOfCompletion(string surveyId, string attachmentHeaderId)
        {
            return _dbContext.Opname.Where(x => x.Guid == surveyId).Select(x => x.Pvo1BijlageGu.Bijlage1).FirstOrDefault();
        }

        public string GetOfficialReportOfCompletionSecondSignature(string surveyId, string attachmentHeaderId)
        {
            return _dbContext.Opname.Where(x => x.Guid == surveyId).Select(x => x.Pvo2BijlageGu.Bijlage1).FirstOrDefault();
        }

        public SurveyApiModel GetSurveyDetails(string surveyId)
        {
            var survey = _dbContext.Opname.Include(x => x.Melding).Include(x => x.WerkGu).SingleOrDefault(x => x.Guid == surveyId);
            var gebouw = _dbContext.Gebouw.Include(x => x.AdresGu).SingleOrDefault(x => x.Guid == survey.GebouwGuid);

            return new SurveyApiModel(survey, gebouw, true);
        }

        public FileModel GetSurveySignature(string surveyId, SurveySignatoryType surveySignatoryType)
        {
            var survey = _dbContext.Opname.Find(surveyId);

            if (survey != null)
            {
                switch (surveySignatoryType)
                {
                    case SurveySignatoryType.BuyerRenter1:
                        return new FileModel { Name = survey.HandtekeningKh1, Content = survey.HandtekeningKh1FileOpslag };
                    case SurveySignatoryType.BuyerRenter2:
                        return new FileModel { Name = survey.HandtekeningKh2, Content = survey.HandtekeningKh2FileOpslag };
                    case SurveySignatoryType.Executor:
                        return new FileModel { Name = survey.HandtekeningUitvoerder, Content = survey.HandtekeningUitvoerderFileOpslag };
                }
            }

            return null;
        }

        public bool? MarkSurveyComplete(string surveyId)
        {
            var survey = _dbContext.Opname.Find(surveyId);
            if (survey != null)
            {
                if (survey.OpnameStatus != (int)SurveyStatus.Completed && survey.OpnameStatus != (int)SurveyStatus.Sent)
                {
                    survey.OpnameStatus = (int)SurveyStatus.Completed;
                    return true;
                }
                return false;
            }
            return null;
        }

        public bool MarkSurveySent(string surveyId)
        {
            var survey = _dbContext.Opname.Find(surveyId);
            if (survey != null)
            {
                survey.OpnameStatus = (int)SurveyStatus.Sent;
                return true;
            }
            return false;
        }

        public bool SetSecondSignatureAsInitiated(string surveyId)
        {
            var survey = _dbContext.Opname.Find(surveyId);
            if (
                survey != null
                && survey.OpnameSoort == (int)SurveyType.Delivery
                && !survey.DatumTweedeHandtekeningOndertekening.HasValue
                && (survey.OpnameStatus == (int)SurveyStatus.Completed || survey.OpnameStatus == (int)SurveyStatus.Sent))
            {
                survey.TweedeHandtekeningGestart = true;
                return true;
            }

            return false;
        }

        public bool UpdateSecondSignature(CompleteSecondSignatureApiModel completeSecondSignature)
        {
            var survey = _dbContext.Opname.Find(completeSecondSignature.SurveyId.ToUpperString());
            if (
                survey != null
                && survey.OpnameSoort == (int)SurveyType.Delivery
                && !survey.DatumTweedeHandtekeningOndertekening.HasValue
                && (survey.OpnameStatus == (int)SurveyStatus.Completed || survey.OpnameStatus == (int)SurveyStatus.Sent)
                && survey.TweedeHandtekeningGestart)
            {
                if (completeSecondSignature.BuyerRenter1Signature != null && completeSecondSignature.BuyerRenter1Signature.Content != null && completeSecondSignature.BuyerRenter1Signature.Content.Length > 0)
                {
                    survey.Handtekening2Kh1 = completeSecondSignature.BuyerRenter1Signature.Name;
                    survey.Handtekening2Kh1FileOpslag = completeSecondSignature.BuyerRenter1Signature.Content;
                }
                if (completeSecondSignature.BuyerRenter2Signature != null && completeSecondSignature.BuyerRenter2Signature.Content != null && completeSecondSignature.BuyerRenter2Signature.Content.Length > 0)
                {
                    survey.Handtekening2Kh2 = completeSecondSignature.BuyerRenter2Signature.Name;
                    survey.Handtekening2Kh2FileOpslag = completeSecondSignature.BuyerRenter2Signature.Content;
                }

                return true;
            }

            return false;
        }

        public bool CompleteSecondSignature(string surveyId)
        {
            var survey = _dbContext.Opname.Find(surveyId.ToUpperInvariant());
            if (
                survey != null
                && survey.OpnameSoort == (int)SurveyType.Delivery
                && !survey.DatumTweedeHandtekeningOndertekening.HasValue
                && (survey.OpnameStatus == (int)SurveyStatus.Completed || survey.OpnameStatus == (int)SurveyStatus.Sent)
                && survey.TweedeHandtekeningGestart)
            {
                survey.DatumTweedeHandtekeningOndertekening = DateTime.Now;

                return true;
            }

            return false;
        }

        public void UpdateDeliveryReport(string surveyId, string attachmentId)
        {
            var survey = _dbContext.Opname.Find(surveyId.ToUpperInvariant());
            survey.Pvo1BijlageGuid = attachmentId.ToUpperInvariant();
        }

        public void UpdateSecondSignatureReport(string surveyId, string attachmentId)
        {
            var survey = _dbContext.Opname.Find(surveyId.ToUpperInvariant());
            survey.Pvo2BijlageGuid = attachmentId.ToUpperInvariant();
        }
    }
}
