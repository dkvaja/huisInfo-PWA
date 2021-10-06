using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Repositories
{
    /// <summary>
    /// Author : Abhishek Saini
    /// This is interface which should be implemented in outer layer.
    /// </summary>
    public interface ISurveyRepository
    {
        IEnumerable<SurveyApiModel> GetSurveys(string userId, SurveyType surveyType, int? noOfDaysForOffline, bool? isSecondSignature);
        SurveyApiModel GetSurveyDetails(string surveyId);
        FileModel GetSurveySignature(string surveyId, SurveySignatoryType surveySignatoryType);
        string AddOrUpdateSurvey(AddOrUpdateSurveyApiModel surveyApiModel);
        bool? MarkSurveyComplete(string surveyId);
        bool MarkSurveySent(string surveyId);
        string GetOfficialReportOfCompletion(string surveyId, string attachmentHeaderId);
        string GetOfficialReportOfCompletionSecondSignature(string surveyId, string attachmentHeaderId);
        bool SetSecondSignatureAsInitiated(string surveyId);
        bool UpdateSecondSignature(CompleteSecondSignatureApiModel completeSecondSignature);
        bool CompleteSecondSignature(string surveyId);
        void UpdateDeliveryReport(string surveyId, string attachmentId);
        void UpdateSecondSignatureReport(string surveyId, string attachmentId);
    }
}
