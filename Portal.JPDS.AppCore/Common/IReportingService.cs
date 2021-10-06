using Portal.JPDS.AppCore.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Common
{
    public interface IReportingService
    {
        Task<byte[]> GetSurveyReportAsync(string surveyId);
    }
}
