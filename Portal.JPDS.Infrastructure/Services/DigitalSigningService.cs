using Newtonsoft.Json;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.AppCore.Models;
using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.Infrastructure.Services
{
    public class DigitalSigningService : IDigitalSigningService
    {
        private readonly IRepoSupervisor _repoSupervisor;
        private readonly IPdfHelperService _pdfHelper;
        private readonly DigitalSigningApiConfigModel _digitalSigningApiConfig;
        public DigitalSigningService(IRepoSupervisor repoSupervisor,
            IPdfHelperService pdfHelper)
        {
            _repoSupervisor = repoSupervisor;
            _pdfHelper = pdfHelper;
            _digitalSigningApiConfig = _repoSupervisor.Config.GetDigitalSigningApiConfig();
        }

        public void SendDocumentToBeDigitallySigned(string quoteId, string userId, string quotationDocumentHeader, string quotationDrawingHeader, Uri callbackUrl)
        {
            var digitalSigningInfo = _repoSupervisor.BuildingOptions.GetSigningInfoForQuotation(quoteId, userId);
            if (digitalSigningInfo.SignatureType == (int)QuotationSignatureType.DigitalStandard || digitalSigningInfo.SignatureType == (int)QuotationSignatureType.Digital_iDIN)
            {
                var quotationDoc = _repoSupervisor.Attachments.GetQuotationDocuments(quoteId, quotationDocumentHeader).FirstOrDefault();
                var documentPath = _repoSupervisor.Attachments.GetAttachmentLocation(quotationDoc.Id);
                var fileName = Path.GetFileName(documentPath);
                int quotationFilePageCount = 0, drawingFilePageCount = 0;
                quotationFilePageCount = _pdfHelper.TotalPageCount(documentPath);

                var quotationDrawing = _repoSupervisor.Attachments.GetQuotationDocuments(quoteId, quotationDrawingHeader).FirstOrDefault();

                byte[] fileToBeSigned;
                if (quotationDrawing != null)
                {
                    var drawingPath = _repoSupervisor.Attachments.GetAttachmentLocation(quotationDrawing.Id);
                    drawingFilePageCount = _pdfHelper.TotalPageCount(drawingPath);
                    var filesToMerge = new List<string>();
                    filesToMerge.Add(documentPath);
                    filesToMerge.Add(drawingPath);

                    fileToBeSigned = _pdfHelper.MergePdf(filesToMerge);
                }
                else
                {
                    fileToBeSigned = File.ReadAllBytes(documentPath);
                }

                var parties = GetPartiesConfiguredWithFiledInfo(digitalSigningInfo, quotationFilePageCount, drawingFilePageCount);

                string scriveOAuthToken = GetScriveOAuthToken();

                var model = New(fileName, fileToBeSigned, scriveOAuthToken);//_ondertekend
                foreach (var party in model.parties)
                {
                    party.is_signatory = false;
                    party.confirmation_delivery_method = "none";
                }

                foreach (var party in parties)
                {
                    model.parties.Add(party);
                }

                model.api_callback_url = callbackUrl.ToString();

                model.days_to_sign = (digitalSigningInfo.ClosingDate - DateTime.Now.Date).Days;

                model = Update(model, scriveOAuthToken);
                Start(model.id, scriveOAuthToken);
            }
        }

        public byte[] GetDigitallySignedFile(long documentId, string fileId)
        {
            using (var client = new HttpClient())
            {
                var scriveOAuthToken = GetScriveOAuthToken();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", scriveOAuthToken);

                client.BaseAddress = new Uri(_digitalSigningApiConfig.BaseUrl);


                var responseTask = client.GetByteArrayAsync("documents/" + documentId + "/files/" + fileId);
                responseTask.Wait();

                return responseTask.Result;
            }
        }

        private ScriveSigningModel New(string fileName, byte[] image, string scriveOAuthToken)
        {
            using (var client = new HttpClient())
            {
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", scriveOAuthToken);

                    client.BaseAddress = new Uri(_digitalSigningApiConfig.BaseUrl);

                    content.Add(new StreamContent(new MemoryStream(image)), "file", fileName);

                    var responseTask = client.PostAsync("documents/new", content);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        var resultObj = JsonConvert.DeserializeObject<ScriveSigningModel>(readTask);

                        return resultObj;
                    }

                    return null;
                }
            }
        }

        private ScriveSigningModel Update(ScriveSigningModel model, string scriveOAuthToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", scriveOAuthToken);

                client.BaseAddress = new Uri(_digitalSigningApiConfig.BaseUrl);


                var formContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("document", JsonConvert.SerializeObject(model))
                    });

                var responseTask = client.PostAsync("documents/" + model.id + "/update", formContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    var resultObj = JsonConvert.DeserializeObject<ScriveSigningModel>(readTask);

                    return resultObj;
                }
            }

            return null;
        }


        private ScriveSigningModel Start(long documentId, string scriveOAuthToken)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", scriveOAuthToken);

                client.BaseAddress = new Uri(_digitalSigningApiConfig.BaseUrl);


                var responseTask = client.PostAsync("documents/" + documentId + "/start", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    var resultObj = JsonConvert.DeserializeObject<ScriveSigningModel>(readTask);

                    return resultObj;
                }
            }

            return null;
        }

        private List<ScriveParty> GetPartiesConfiguredWithFiledInfo(DigitalSignInfoModel digitalSignInfo, int quotationFilePageCount, int drawingFilePageCount)
        {
            List<ScriveParty> parties = new List<ScriveParty>();

            var firstPagePlacements = new
            {
                differenceX = 0.1915789473684209,
                header = new
                {
                    xrel = 0.5989473684210527,
                    yrel = 0.16083395383469842,
                    wrel = 0.16421052631578947,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                name = new
                {
                    xrel = 0.5989473684210527,
                    yrel = 0.21891288160833955,
                    wrel = 0.16421052631578947,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                signature = new
                {
                    xrel = 0.5989473684210527,
                    yrel = 0.17647058823529413,
                    wrel = 0.16421052631578947,
                    hrel = 0.03425167535368578,
                    fsrel = 0.0168
                }
            };

            var generalPlacements = new
            {
                differenceX = 0.2810526315789474,
                name = new
                {
                    xrel = 0.1863157894736842,
                    yrel = 0.945,
                    wrel = 0.16421052631578947,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                label = new
                {
                    xrel = 0.1863157894736842,
                    yrel = 0.955,
                    wrel = 0.15157894736842106,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                checkbox = new
                {
                    xrel = 0.33473684210526317,
                    yrel = 0.95,
                    wrel = 0.021153,
                    hrel = 0,
                    fsrel = 0
                }
            };


            var drawingsPlacements = new
            {
                differenceX = 0.3,
                name = new
                {
                    xrel = 0.1,
                    yrel = 0.05,
                    wrel = 0.16421052631578947,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                label = new
                {
                    xrel = 0.1,
                    yrel = 0.07,
                    wrel = 0.15157894736842106,
                    hrel = 0.022338049143708117,
                    fsrel = 0.01263157894736842
                },
                checkbox = new
                {
                    xrel = 0.24842105263,
                    yrel = 0.064,
                    wrel = 0.021153,
                    hrel = 0,
                    fsrel = 0
                }
            };


            ScriveField nameFieldParty1 = null, emailFieldParty1 = null, signatureFieldParty1 = null,
                customHeaderParty1 = null, customLabelParty1 = null,
                nameFieldParty2 = null, emailFieldParty2 = null, signatureFieldParty2 = null,
                customHeaderParty2 = null, customLabelParty2 = null;

            List<ScriveField> lstCheckboxParty1 = new List<ScriveField>(), lstCheckboxParty2 = new List<ScriveField>();

            var hasTwoSignatories = digitalSignInfo.SigningParty2 != null;

            nameFieldParty1 = new ScriveField
            {
                type = "name",
                order = 1,
                value = digitalSignInfo.SigningParty1.Name,
                is_obligatory = true,
                should_be_filled_by_sender = true,
                editable_by_signatory = false,
                placements = new List<object>()
            };
            emailFieldParty1 = new ScriveField
            {
                type = "email",
                value = digitalSignInfo.SigningParty1.Email,
                is_obligatory = true,
                should_be_filled_by_sender = true,
                editable_by_signatory = false,
                placements = new List<object>()
            };

            customHeaderParty1 = new ScriveField
            {
                type = "text",
                name = "Header1",
                value = "Handtekening voor akkoord:",
                is_obligatory = true,
                should_be_filled_by_sender = true,
                editable_by_signatory = false,
                placements = new List<object>()
            };

            customLabelParty1 = new ScriveField
            {
                type = "text",
                name = "Label1",
                value = "Gecontroleerd en akkoord",
                is_obligatory = true,
                should_be_filled_by_sender = true,
                editable_by_signatory = false,
                placements = new List<object>()
            };

            signatureFieldParty1 = new ScriveField
            {
                type = "signature",
                name = "Signature 1",
                is_obligatory = true,
                should_be_filled_by_sender = false,
                editable_by_signatory = false,
                placements = new List<object>()
            };

            if (hasTwoSignatories)
            {
                nameFieldParty2 = new ScriveField
                {
                    type = "name",
                    order = 1,
                    value = digitalSignInfo.SigningParty2.Name,
                    is_obligatory = true,
                    should_be_filled_by_sender = true,
                    editable_by_signatory = false,
                    placements = new List<object>()
                };
                emailFieldParty2 = new ScriveField
                {
                    type = "email",
                    value = digitalSignInfo.SigningParty2.Email,
                    is_obligatory = true,
                    should_be_filled_by_sender = true,
                    editable_by_signatory = false,
                    placements = new List<object>()
                };

                customHeaderParty2 = new ScriveField
                {
                    type = "text",
                    name = "Header2",
                    value = "Handtekening voor akkoord:",
                    is_obligatory = true,
                    should_be_filled_by_sender = true,
                    editable_by_signatory = false,
                    placements = new List<object>()
                };

                customLabelParty2 = new ScriveField
                {
                    type = "text",
                    name = "Label2",
                    value = "Gecontroleerd en akkoord",
                    is_obligatory = true,
                    should_be_filled_by_sender = true,
                    editable_by_signatory = false,
                    placements = new List<object>()
                };

                signatureFieldParty2 = new ScriveField
                {
                    type = "signature",
                    name = "Signature 2",
                    is_obligatory = true,
                    should_be_filled_by_sender = false,
                    editable_by_signatory = false,
                    placements = new List<object>()
                };

                customHeaderParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.header.xrel,
                        yrel = firstPagePlacements.header.yrel,
                        wrel = firstPagePlacements.header.wrel,
                        hrel = firstPagePlacements.header.hrel,
                        fsrel = firstPagePlacements.header.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                customHeaderParty2.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.header.xrel + firstPagePlacements.differenceX,
                        yrel = firstPagePlacements.header.yrel,
                        wrel = firstPagePlacements.header.wrel,
                        hrel = firstPagePlacements.header.hrel,
                        fsrel = firstPagePlacements.header.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                signatureFieldParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.signature.xrel,
                        yrel = firstPagePlacements.signature.yrel,
                        wrel = firstPagePlacements.signature.wrel,
                        hrel = firstPagePlacements.signature.hrel,
                        fsrel = firstPagePlacements.signature.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                signatureFieldParty2.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.signature.xrel + firstPagePlacements.differenceX,
                        yrel = firstPagePlacements.signature.yrel,
                        wrel = firstPagePlacements.signature.wrel,
                        hrel = firstPagePlacements.signature.hrel,
                        fsrel = firstPagePlacements.signature.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                nameFieldParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.name.xrel,
                        yrel = firstPagePlacements.name.yrel,
                        wrel = firstPagePlacements.name.wrel,
                        hrel = firstPagePlacements.name.hrel,
                        fsrel = firstPagePlacements.name.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                nameFieldParty2.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.name.xrel + firstPagePlacements.differenceX,
                        yrel = firstPagePlacements.name.yrel,
                        wrel = firstPagePlacements.name.wrel,
                        hrel = firstPagePlacements.name.hrel,
                        fsrel = firstPagePlacements.name.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });
            }
            else
            {
                var halfOfDifference = firstPagePlacements.differenceX / 2;
                customHeaderParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.header.xrel + halfOfDifference,
                        yrel = firstPagePlacements.header.yrel,
                        wrel = firstPagePlacements.header.wrel,
                        hrel = firstPagePlacements.header.hrel,
                        fsrel = firstPagePlacements.header.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                signatureFieldParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.signature.xrel + halfOfDifference,
                        yrel = firstPagePlacements.signature.yrel,
                        wrel = firstPagePlacements.signature.wrel,
                        hrel = firstPagePlacements.signature.hrel,
                        fsrel = firstPagePlacements.signature.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });

                nameFieldParty1.placements.Add(
                    new
                    {
                        xrel = firstPagePlacements.name.xrel + halfOfDifference,
                        yrel = firstPagePlacements.name.yrel,
                        wrel = firstPagePlacements.name.wrel,
                        hrel = firstPagePlacements.name.hrel,
                        fsrel = firstPagePlacements.name.fsrel,
                        page = 1,
                        tip = "right",
                        anchors = new List<string>()
                    });
            }

            for (var pageNumber = 1; pageNumber <= quotationFilePageCount + drawingFilePageCount; pageNumber++)
            {
                var placements = pageNumber <= quotationFilePageCount ? generalPlacements : drawingsPlacements;

                nameFieldParty1.placements.Add(
                    new
                    {
                        xrel = placements.name.xrel,
                        yrel = placements.name.yrel,
                        wrel = placements.name.wrel,
                        hrel = placements.name.hrel,
                        fsrel = placements.name.fsrel,
                        page = pageNumber,
                        tip = "right",
                        anchors = new List<string>()
                    });

                customLabelParty1.placements.Add(
                    new
                    {
                        xrel = placements.label.xrel,
                        yrel = placements.label.yrel,
                        wrel = placements.label.wrel,
                        hrel = placements.label.hrel,
                        fsrel = placements.label.fsrel,
                        page = pageNumber,
                        tip = "right",
                        anchors = new List<string>()
                    });


                var checkboxParty1 = new ScriveField
                {
                    type = "checkbox",
                    name = "Checkbox 1-" + pageNumber,
                    is_obligatory = true,
                    should_be_filled_by_sender = false,
                    placements = new List<object>()
                };

                checkboxParty1.placements.Add(
                    new
                    {
                        xrel = placements.checkbox.xrel,
                        yrel = placements.checkbox.yrel,
                        wrel = placements.checkbox.wrel,
                        hrel = placements.checkbox.hrel,
                        fsrel = placements.checkbox.fsrel,
                        page = pageNumber,
                        tip = "right",
                        anchors = new List<string>()
                    });

                lstCheckboxParty1.Add(checkboxParty1);

                if (hasTwoSignatories)
                {
                    nameFieldParty2.placements.Add(
                    new
                    {
                        xrel = placements.name.xrel + placements.differenceX,
                        yrel = placements.name.yrel,
                        wrel = placements.name.wrel,
                        hrel = placements.name.hrel,
                        fsrel = placements.name.fsrel,
                        page = pageNumber,
                        tip = "right",
                        anchors = new List<string>()
                    });

                    customLabelParty2.placements.Add(
                        new
                        {
                            xrel = placements.label.xrel + placements.differenceX,
                            yrel = placements.label.yrel,
                            wrel = placements.label.wrel,
                            hrel = placements.label.hrel,
                            fsrel = placements.label.fsrel,
                            page = pageNumber,
                            tip = "right",
                            anchors = new List<string>()
                        });

                    var checkboxParty2 = new ScriveField
                    {
                        type = "checkbox",
                        name = "Checkbox 2-" + pageNumber,
                        is_obligatory = true,
                        should_be_filled_by_sender = false,
                        placements = new List<object>()
                    };
                    checkboxParty2.placements.Add(
                        new
                        {
                            xrel = placements.checkbox.xrel + placements.differenceX,
                            yrel = placements.checkbox.yrel,
                            wrel = placements.checkbox.wrel,
                            hrel = placements.checkbox.hrel,
                            fsrel = placements.checkbox.fsrel,
                            page = pageNumber,
                            tip = "right",
                            anchors = new List<string>()
                        });
                    lstCheckboxParty2.Add(checkboxParty2);
                }
            }

            var party1 = new ScriveParty();
            party1.is_author = false;
            party1.is_signatory = true;
            party1.fields = new List<ScriveField>();
            party1.fields.Add(nameFieldParty1);
            party1.fields.Add(emailFieldParty1);
            party1.fields.Add(customHeaderParty1);
            party1.fields.Add(signatureFieldParty1);
            party1.fields.Add(customLabelParty1);
            foreach (var checkboxField in lstCheckboxParty1)
            {
                party1.fields.Add(checkboxField);
            }

            parties.Add(party1);

            if (hasTwoSignatories)
            {
                var party2 = new ScriveParty();
                party2.is_author = false;
                party2.is_signatory = true;
                party2.fields = new List<ScriveField>();
                party2.fields.Add(nameFieldParty2);
                party2.fields.Add(emailFieldParty2);
                party2.fields.Add(customHeaderParty2);
                party2.fields.Add(signatureFieldParty2);
                party2.fields.Add(customLabelParty2);

                foreach (var checkboxField in lstCheckboxParty2)
                {
                    party2.fields.Add(checkboxField);
                }

                parties.Add(party2);
            }

            if (digitalSignInfo.SignatureType == (int)QuotationSignatureType.Digital_iDIN)
            {
                foreach (var party in parties)
                {
                    party.authentication_method_to_view = "nl_idin";
                }
            }

            return parties;
        }

        private string GetScriveOAuthToken()
        {
            var ScriveApiToken = _digitalSigningApiConfig.ApiToken;
            var ScriveApiSecret = _digitalSigningApiConfig.ApiSecret;
            var ScriveAccessToken = _digitalSigningApiConfig.AccessToken;
            var ScriveAccessSecret = _digitalSigningApiConfig.AccessSecret;
            string scriveOAuthToken = string.Format("oauth_signature_method=\"PLAINTEXT\",oauth_consumer_key=\"{0}\",oauth_token=\"{1}\",oauth_signature=\"{2}&{3}\"", ScriveApiToken, ScriveAccessToken, ScriveApiSecret, ScriveAccessSecret);
            return scriveOAuthToken;
        }
    }
}
