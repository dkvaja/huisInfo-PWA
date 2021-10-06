using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Infrastructure.Connected_Services.SSRSServiceReference;
using Portal.JPDS.Infrastructure.Persistence;
using SSRSServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.Infrastructure.Services
{
    public class ReportingService : IReportingService
    {
        private readonly string _connectionString;
        private readonly string _reportingServiceUrl;
        private readonly string _surveyReportPath;
        public ReportingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _reportingServiceUrl = configuration["ReportingServiceUrl"];
            _surveyReportPath = configuration["SurveyReportPath"];
        }

        public async Task<byte[]> GetSurveyReportAsync(string surveyId)
        {
            var bytes = File.ReadAllBytes(_surveyReportPath);
            Stream stream = new MemoryStream(bytes);
            DataSet surveyDatasets = GetSurveyDataSets(surveyId);
            LocalReport report = new LocalReport();
            report.LoadReportDefinition(stream);
            report.EnableExternalImages = true;
            if (surveyDatasets != null && surveyDatasets.Tables.Count > 0 && surveyDatasets.Tables[0].Rows.Count > 0)
            {
                var pathToFile = surveyDatasets.Tables[0].Rows[0].ItemArray.GetValue(3).ToString();
                var columnnew = new DataColumn("logo_binary", Type.GetType("System.Byte[]"));
                surveyDatasets.Tables[0].Columns.Add(columnnew);
                if (File.Exists(pathToFile))
                {
                    var fileBytes = File.ReadAllBytes(pathToFile);
                    surveyDatasets.Tables[0].Rows[0]["logo_binary"] = fileBytes;
                }
                surveyDatasets.Tables[0].AcceptChanges();
            }
            report.DataSources.Add(new ReportDataSource("opname", surveyDatasets.Tables[0]));
            report.DataSources.Add(new ReportDataSource("melding", surveyDatasets.Tables[1]));
            byte[] file = report.Render("PDF");
            return file;
        }


        /// <summary>
        /// </summary>
        /// <param name="reportPath">
        /// SSRS report path. Note: Need to include parent folder directory and report name.
        /// Such as value = "/[report folder]/[report name]".
        /// </param>
        /// <param name="parameters">report's required parameters</param>
        /// <param name="exportFormat">value = "PDF" or "EXCEL". By default it is pdf.</param>
        /// <param name="languageCode">
        ///   value = 'en-us', 'fr-ca', 'es-us', 'zh-chs'. 
        /// </param>
        /// <returns></returns>
        private async Task<byte[]> RenderReport(string reportPath, IDictionary<string, object> parameters, string languageCode, string exportFormat)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.MaxReceivedMessageSize = 10485760; //I wanted a 10MB size limit on response to allow for larger PDFs
            //binding.MaxReceivedMessageSize = this.ConfigSettings.ReportingServiceReportMaxSize; //It is 10MB size limit on response to allow for larger PDFs

            //Create the execution service SOAP Client
            ReportExecutionServiceSoapClient reportClient = new ReportExecutionServiceSoapClient(binding, new EndpointAddress(_reportingServiceUrl));

            //Setup access credentials. Here use windows credentials.
            //var clientCredentials = new NetworkCredential("avimi", "", "AVIGHNA-PC");
            reportClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            //reportClient.ClientCredentials.Windows.ClientCredential = clientCredentials;

            //This handles the problem of "Missing session identifier"
            reportClient.Endpoint.EndpointBehaviors.Add(new ReportingServiceEndPointBehavior());

            string historyID = null;
            TrustedUserHeader trustedUserHeader = new TrustedUserHeader();
            ExecutionHeader execHeader = new ExecutionHeader();

            //trustedUserHeader.UserName = clientCredentials.UserName;

            //
            // Load the report
            //
            var taskLoadReport = await reportClient.LoadReportAsync(trustedUserHeader, reportPath, historyID);
            // Fixed the exception of "session identifier is missing".
            execHeader.ExecutionID = taskLoadReport.executionInfo.ExecutionID;

            //
            //Set the parameteres asked for by the report
            //
            ParameterValue[] reportParameters = null;
            if (parameters != null && parameters.Count > 0)
            {
                reportParameters = taskLoadReport.executionInfo.Parameters.Where(x => parameters.ContainsKey(x.Name)).Select(x => new ParameterValue() { Name = x.Name, Value = parameters[x.Name].ToString() }).ToArray();
            }

            await reportClient.SetExecutionParametersAsync(execHeader, trustedUserHeader, reportParameters, languageCode);
            // run the report
            const string deviceInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

            var response = await reportClient.RenderAsync(new RenderRequest(execHeader, trustedUserHeader, exportFormat ?? "PDF", deviceInfo));

            //spit out the result
            return response.Result;
        }

        private DataSet GetSurveyDataSets(string surveyId)
        {
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(_connectionString);
            sqlConn.Open();
            DataTable opname = new DataTable();
            DataTable melding = new DataTable();
            string opnameQuery = string.Format(@"SELECT o.guid, 
                                           o.werk_guid,
                                           o.gebouw_guid,
                                           w.logo,
                                           w.werknummer,
                                           w.werknaam,
                                           g.bouwnummer_intern,
                                           g.bouwnummer_extern,
                                           volledig_adres = a.straat + ' ' + a.nummer + ISNULL(' ' + a.nummer_toevoeging, '') + CHAR(10) + a.postcode + ' ' + a.plaats,
                                           a.straat,
                                           a.nummer,
                                           a.nummer_toevoeging,
                                           a.postcode,
                                           a.plaats,
                                           g.adresblok,
                                           g.certificaat_nummer,
                                           o.opname_soort,
                                           o.datum,
                                           o.uitgevoerd_door_medewerker_guid,
                                           o.koptekst_pvo,
                                           o.voettekst_pvo,
                                           o.opmerkingen_uitvoerder,
                                           o.opmerkingen_koper_huurder,
                                           o.meterstand_elektra_1,
                                           o.meterstand_elektra_2,
                                           o.meterstand_elektra_retour_1,
                                           o.meterstand_elektra_retour_2,
                                           o.meterstand_gas_warmte,
                                           o.meterstand_water,
                                           o.aantal_gebreken,
                                           o.volledige_naam_uitvoerder,
                                           o.handtekening_uitvoerder_file_opslag,
                                           o.handtekening_uitvoerder,
                                           o.volledige_naam_kh_1,
                                           o.handtekening_kh_1_file_opslag,
                                           o.handtekening_kh_1,
                                           o.volledige_naam_kh_2,
                                           o.handtekening_kh_2_file_opslag,
                                           o.handtekening_kh_2,
                                           o.opname_status,
                                           o.tweede_handtekening_gestart,
                                           o.handtekening_2_kh_1_file_opslag,
                                           o.handtekening_2_kh_2_file_opslag
                                           FROM opname o
                                           JOIN werk w ON w.guid = o.werk_guid
                                           JOIN gebouw g ON g.guid = o.gebouw_guid
                                           LEFT JOIN adres a ON a.guid = g.adres_guid
                                           WHERE  o.guid ='" + surveyId + "';");
            System.Data.SqlClient.SqlDataAdapter adOpname = new System.Data.SqlClient.SqlDataAdapter(opnameQuery, sqlConn);
            string meldingQuery = string.Format(@"select m.*, l.locatie FROM melding m LEFT JOIN melding_locatie l ON l.guid = m.melding_locatie_guid
                                        WHERE opname_guid = '" + surveyId + "';");
            System.Data.SqlClient.SqlDataAdapter adMelding = new System.Data.SqlClient.SqlDataAdapter(meldingQuery, sqlConn);
            DataSet dsSurvey = new DataSet();
            adOpname.Fill(opname);
            adMelding.Fill(melding);
            dsSurvey.Tables.Add(opname);
            dsSurvey.Tables.Add(melding);
            sqlConn.Close();
            return dsSurvey;
        }
    }
}
