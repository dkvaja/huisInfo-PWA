using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class SurveyApiModel
    {
        public SurveyApiModel(Opname opname, Gebouw gebouw, bool withSignatures)
        {
            SurveyId = opname.Guid;
            SurveyType = (SurveyType)opname.OpnameSoort;
            BuildingId = opname.GebouwGuid;
            BuildingNoExtern = gebouw.BouwnummerExtern ?? string.Empty;
            BuildingNoIntern = gebouw.BouwnummerIntern ?? string.Empty;
            GuaranteeCertNo = gebouw.CertificaatNummer;
            ProjectId = opname.WerkGuid;
            BuildingBuyerRenterId = gebouw.KoperHuurderGuid ?? string.Empty;
            ProjectNo = opname.WerkGu.Werknummer ?? string.Empty;
            ProjectName = opname.WerkGu.WerknummerWerknaam ?? string.Empty;
            Address = gebouw.AdresGu?.VolledigAdres ?? string.Empty;
            Date = opname.Datum;
            Status = (SurveyStatus)opname.OpnameStatus;
            ExecutedBy = opname.VolledigeNaamUitvoerder ?? string.Empty;
            BuyerRenter1 = opname.VolledigeNaamKh1 ?? string.Empty;
            BuyerRenter2 = opname.VolledigeNaamKh2 ?? string.Empty;
            ExecutorComments = opname.OpmerkingenUitvoerder ?? string.Empty;
            BuyerRenterComments = opname.OpmerkingenKoperHuurder ?? string.Empty;
            MeterReading_Electric1 = opname.MeterstandElektra1;
            MeterReading_Electric2 = opname.MeterstandElektra2;
            MeterReading_ElectricReturn1 = opname.MeterstandElektraRetour1;
            MeterReading_ElectricReturn2 = opname.MeterstandElektraRetour2;
            MeterReading_GasHeat = opname.MeterstandGasWarmte;
            MeterReading_Water = opname.MeterstandWater;
            ReportHeader = opname.KoptekstPvo;
            ReportFooter = opname.VoettekstPvo;
            IsSecondSignatureInitiated = opname.TweedeHandtekeningGestart;
            SecondSignatureDate = opname.DatumTweedeHandtekeningOndertekening;
            if (withSignatures)
            {
                ExecutorSignature = new FileModel
                {
                    Name = opname.HandtekeningUitvoerder ?? string.Empty,
                    Content = opname.HandtekeningUitvoerderFileOpslag
                };
                BuyerRenter1Signature = new FileModel
                {
                    Name = opname.HandtekeningKh1 ?? string.Empty,
                    Content = opname.HandtekeningKh1FileOpslag
                };
                BuyerRenter2Signature = new FileModel
                {
                    Name = opname.HandtekeningKh2 ?? string.Empty,
                    Content = opname.HandtekeningKh2FileOpslag
                };
                if (IsSecondSignatureInitiated)
                {
                    BuyerRenter1SecondSignature = new FileModel
                    {
                        Name = opname.Handtekening2Kh1 ?? string.Empty,
                        Content = opname.Handtekening2Kh1FileOpslag
                    };
                    BuyerRenter2SecondSignature = new FileModel
                    {
                        Name = opname.Handtekening2Kh2 ?? string.Empty,
                        Content = opname.Handtekening2Kh2FileOpslag
                    };
                }
            }

            if (opname.Melding != null)
            {
                RepairRequestsCount = opname.Melding.Count;
            }
        }

        public string SurveyId { get; set; }
        public SurveyType SurveyType { get; set; }
        public string BuildingId { get; set; }
        public string BuildingNoIntern { get; set; }
        public string BuildingNoExtern { get; set; }
        public string GuaranteeCertNo { get; set; }
        public string ProjectId { get; set; }
        public string BuildingBuyerRenterId { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public DateTime? Date { get; set; }
        public SurveyStatus Status { get; set; }
        public string ExecutedBy { get; set; }
        public string BuyerRenter1 { get; set; }
        public string BuyerRenter2 { get; set; }
        public string ExecutorComments { get; set; }
        public string BuyerRenterComments { get; set; }
        public int? MeterReading_Electric1 { get; set; }
        public int? MeterReading_Electric2 { get; set; }
        public int? MeterReading_ElectricReturn1 { get; set; }
        public int? MeterReading_ElectricReturn2 { get; set; }
        public int? MeterReading_GasHeat { get; set; }
        public int? MeterReading_Water { get; set; }
        public string ReportHeader { get; set; }
        public string ReportFooter { get; set; }
        public FileModel ExecutorSignature { get; set; }
        public FileModel BuyerRenter1Signature { get; set; }
        public FileModel BuyerRenter2Signature { get; set; }
        public FileModel BuyerRenter1SecondSignature { get; set; }
        public FileModel BuyerRenter2SecondSignature { get; set; }
        public bool IsSecondSignatureInitiated { get; set; }
        public DateTime? SecondSignatureDate { get; set; }
        public int RepairRequestsCount { get; set; }
    }


    public class AddOrUpdateSurveyApiModel
    {
        public Guid SurveyId { get; set; }
        public SurveyType SurveyType { get; set; }
        public Guid BuildingId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ExecutorEmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public SurveyStatus Status { get; set; }
        public string ExecutorComments { get; set; }
        public string BuyerRenterComments { get; set; }
        public int? MeterReading_Electric1 { get; set; }
        public int? MeterReading_Electric2 { get; set; }
        public int? MeterReading_ElectricReturn1 { get; set; }
        public int? MeterReading_ElectricReturn2 { get; set; }
        public int? MeterReading_GasHeat { get; set; }
        public int? MeterReading_Water { get; set; }
        public string ReportHeader { get; set; }
        public string ReportFooter { get; set; }
        public FileModel ExecutorSignature { get; set; }
        public FileModel BuyerRenter1Signature { get; set; }
        public FileModel BuyerRenter2Signature { get; set; }
        public FileModel OfflineReport { get; set; }
    }

    public class CompleteSecondSignatureApiModel
    {
        public Guid SurveyId { get; set; }
        public FileModel BuyerRenter1Signature { get; set; }
        public FileModel BuyerRenter2Signature { get; set; }
        public FileModel OfflineReport { get; set; }
    }
}
