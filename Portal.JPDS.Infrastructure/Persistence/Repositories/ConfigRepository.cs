using Portal.JPDS.AppCore.ApiModels;
using Portal.JPDS.AppCore.Repositories;
using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.JPDS.AppCore.Models;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class ConfigRepository : BaseRepository, IConfigRepository
    {
        private readonly ConfiguratieWebPortal _webPortalConfig;
        private readonly ConfiguratieKlachtenbeheer _complaintsManagementConfig;
        public ConfigRepository(AppDbContext dbContext) : base(dbContext)
        {
            _webPortalConfig = _dbContext.ConfiguratieWebPortal.FirstOrDefault();
            _complaintsManagementConfig = _dbContext.ConfiguratieKlachtenbeheer.FirstOrDefault();
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string GetBrowserTabNameInternal()
        {
            return _webPortalConfig.NaamTabBrowserIntern;
        }

        public string GetBrowserTabNameExternal()
        {
            return _webPortalConfig.NaamTabBrowserExtern;
        }

        public string GetWebPortalLogo()
        {
            return _webPortalConfig.StandaardLogoWebPortal;
        }

        public string GetWebPortalLoginBackground()
        {
            return _webPortalConfig.AchtergrondLoginWebPortal;
        }

        public SmtpModel GetSmtpConfig()
        {
            return new SmtpModel(_dbContext.ViewPortalEmailNotificationAfzender.SingleOrDefault(x => x.EmailAccountGuid == _webPortalConfig.AfzenderEmailAccountGuid));
        }

        public EmailTemplateModel GetStandardNotificationEmailTemplate()
        {
            return new EmailTemplateModel(_dbContext.Sjabloon.Find(_webPortalConfig.StandaardNotificatieEmailSjabloonGuid));
        }

        public DigitalSigningApiConfigModel GetDigitalSigningApiConfig()
        {
            return new DigitalSigningApiConfigModel(_webPortalConfig);
        }

        public EmailTemplateModel GetForgotPasswordEmailTemplate()
        {
            var template = _dbContext.Sjabloon.Find(_webPortalConfig.StandaardResetWachtwoordSjabloonGuid);
            if (template != null)
                return new EmailTemplateModel(template);
            else return new EmailTemplateModel
            {
                Subject = "Wachtwoord herstel link voor Huisinfo",
                Template = string.Empty,
                TemplateHtml = @"[geachte]
                                <br />
                                <br />
                                U heeft een aanvraag gedaan voor een nieuw wachtwoord voor Huisinfo. U kunt deze wijzigen/herstellen via onderstaande link:
                                <br />
                                <br />
                                <a href='[nieuw_wachtwoord_url]'>klik hier om het wachtwoord bij te werken</a>
                                <br />
                                <br />
                                Als u deze aanvraag niet zelf heeft gedaan, klik dan niet op deze link.
                                <br />
                                <br />                                
                                Met vriendelijke groet,
                                <br />
                                [bedrijf_naam]"
            };
        }

        public SettingsModel GetConfigSettings()
        {
            var general = _dbContext.ConfiguratieAlgemeen
                .Include(x => x.HoofdvestigingEigenOrganisatieGu)
                .ThenInclude(x => x.BezoekadresGu)
                .SingleOrDefault();

            if (general != null && general.HoofdvestigingEigenOrganisatieGu != null)
            {
                return new SettingsModel
                {
                    CompanyName = general.HoofdvestigingEigenOrganisatieGu.Naam,
                    BusinessUnit = general.HoofdvestigingEigenOrganisatieGu.Onderdeel,
                    VisitingAddress = general.HoofdvestigingEigenOrganisatieGu.BezoekadresGu != null ? new AddressModel(general.HoofdvestigingEigenOrganisatieGu.BezoekadresGu) : null,
                    Telephone = general.HoofdvestigingEigenOrganisatieGu.Telefoon,
                    EmailGeneral = general.HoofdvestigingEigenOrganisatieGu.Email,
                    EmailRepairRequest = _complaintsManagementConfig?.EmailNazorg,
                    NoteSendOnlineRepairRequest = _complaintsManagementConfig?.OpmerkingVersturenOnlineMelding
                };
            }

            return null;
        }

        public EmailTemplateModel GetOfficialReportOfCompletionEmailTemplate()
        {
            var template = _dbContext.Sjabloon.Find(_webPortalConfig.StandaardPvoSjabloonGuid);
            if (template != null)
                return new EmailTemplateModel(template);
            else return new EmailTemplateModel
            {
                Subject = "Proces-verbaal van Oplevering [bouwnummer_extern]",
                Template = string.Empty,
                TemplateHtml = @"[geachte]
                                <br />
                                <br />
                                Hierbij ontvangt u van ons het volgende document:
                                <br />
                                <br />
                                <ul>
                                    <li>Proces-verbaal van Oplevering [bouwnummer_extern]</li>
                                </ul>
                                <br />
                                <br />
                                Met vriendelijke groet,
                                <br />
                                [hoofdaannemer]"
            };
        }
        public EmailTemplateModel RepairRequestConfirmationEmailTemplate()
        {
            var templateId = _complaintsManagementConfig?.StandaardRegistratiebevestigingSjabloonGuid;
            if (templateId != null)
            {
                var template = _dbContext.Sjabloon.Find(templateId);
                if (template != null)
                    return new EmailTemplateModel(template);
            }

            return new EmailTemplateModel
            {
                Subject = "Melding [melding_nummer] van [bedrijf_naam]",
                Template = string.Empty,
                TemplateHtml = @"Geachte heer/mevrouw,<br />
                                <br />
                                Hierbij ontvangt u een kopie van de melding die u via onze website heeft ingevoerd.<br />
                                <br />
                                <table>
                                    <tr>
                                        <td><b><u>Algemeen</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Nummer:</td>
                                        <td>[melding_nummer]</td>
                                    </tr>
                                    <tr>
                                        <td>Datum:</td>
                                        <td>[melding_datum]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Object</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Objectcode:</td>
                                        <td>[bouwnummer_intern]</td>
                                    </tr>
                                    <tr>
                                        <td>Adres:</td>
                                        <td>[object_straat] [object_huisnummer]</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>[object_postcode]  [object_plaats]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Toelichting</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Locatie:</td>
                                        <td>[melding_locatie]</td>
                                    </tr>
                                    <tr>
                                        <td>Melding:</td>
                                        <td>[melding_korte_omschrijving]</td>
                                    </tr>
                                    <tr>
                                        <td>Toelichting:</td>
                                        <td>[melding_tekst]</td>
                                    </tr>
                                    <tr>
                                        <td>Voorkeurstijdstip:</td>
                                        <td>[melding_voorkeurstijdstip_afspraak]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Bijlagen</u></b>
                                            [melding_bijlagen]
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                Met vriendelijke groet,<br />
                                <br />
                                Afdeling Nazorg<br />
                                [bedrijf_naam]"
            };
        }

        public IEnumerable<SettlementTermApiModel> GetSettlementTerms()
        {
            return _dbContext.Afhandelingstermijn.Select(x => new SettlementTermApiModel(x, _complaintsManagementConfig.StandaardAfhandelingstermijnGuid == x.Guid));
        }

        public OfflineConfigApiModel GetOfflineConfig(string userId)
        {
            var offlineConfig = new OfflineConfigApiModel
            {
                Mode = _webPortalConfig.StandaardOfflineMode,
                DaysForDelivery = _webPortalConfig.StandaardOfflineOpleveringOpslaanAantalDagen,
                DaysForPreDelivery = _webPortalConfig.StandaardOfflineVoorschouwOpslaanAantalDagen,
                DaysForInspection = _webPortalConfig.StandaardOfflineInspectieOpslaanAantalDagen,
                DaysForSecondSignature = _webPortalConfig.StandaardOfflineTweedeHandtekeningOpslaanAantalDagen
            };

            var user = _dbContext.Login.Where(x => x.Guid == userId && !x.Verwijderd).SingleOrDefault();
            if (user != null)
            {
                offlineConfig.Mode = user.OfflineMode ?? offlineConfig.Mode;
                offlineConfig.DaysForDelivery = user.OfflineOpleveringOpslaanAantalDagen ?? offlineConfig.DaysForDelivery;
                offlineConfig.DaysForInspection = user.OfflineInspectieOpslaanAantalDagen ?? offlineConfig.DaysForInspection;
                offlineConfig.DaysForPreDelivery = user.OfflineVoorschouwOpslaanAantalDagen ?? offlineConfig.DaysForPreDelivery;
                offlineConfig.DaysForSecondSignature = user.OfflineTweedeHandtekeningOpslaanAantalDagen ?? offlineConfig.DaysForSecondSignature;
            }

            return offlineConfig;
        }

        public string GetRepairRequestAddWarningText()
        {
            return _complaintsManagementConfig.OpmerkingVersturenOnlineMelding ?? string.Empty;
        }

        public EmailTemplateModel WorkOrderEmailTemplate()
        {
            // In future we need to update this to configuration
            //var template = _dbContext.Sjabloon.Where(x => x.Guid == emailTemplateId).SingleOrDefault();
            //if (template != null)
            //    return new EmailTemplateModel(template);


            return new EmailTemplateModel
            {
                Subject = "Werkbon [werkbonnummer]",
                Template = string.Empty,
                TemplateHtml = @"[geachte]<br/ >
                                 <br />
                                 <b>[belangrijk_tekst]</b>
                                <br />
                                <br/>
                                <table>
                                    <tr style=""vertical-align:top"">
                                        <td><b><u>Project</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Nummer en naam:</td>
                                        <td>[werknaam]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td><b><u>Werkbon</b></u></td>
                                        <td></td>
                                    </tr>
                                     <tr style=""vertical-align:top"">
                                        <td>Werkbonnummer:</td>
                                        <td>[werkbonnummer]</td>
                                    </tr> 
                                    <tr style=""vertical-align:top"">
                                        <td>Oplosser:</td>
                                        <td>[oplosser_naam]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Streefdatum afhandeling:</td>
                                        <td><b>[oplosser_datum_streefdatumafhandeling]</b></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Locatie:</td>
                                        <td>[melding_locatie]</td>
                                    </tr>
                                     <tr style=""vertical-align:top"">
                                        <td>Omschrijving:</td>
                                        <td>[oplosser_omschrijving]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Werkbontekst:</td>
                                        <td>[oplosser_tekst_werkbon]</td>
                                    </tr>
                                     <tr style=""vertical-align:top"">
                                        <td>Toelichting:</td>
                                        <td>[oplosser_Toelichting]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Soort:</td>
                                        <td>[melding_soort]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Melding datum:</td>
                                        <td>[melding_datum]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Datum ingelicht:</td>
                                        <td>[datum_ingelicht]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Voorkeurstijdstip afspraak:</td>
                                        <td>[melding_voorkeurstijdstip_afspraak]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td><b><u>Object informatie</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Straat en huisnummer:</td>
                                        <td>[object_straat] [object_huisnummer]</td>
                                    </tr>
                                     <tr style=""vertical-align:top"">
                                        <td>Postcode en plaats:</td>
                                        <td>[object_postcode]  [object_plaats]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Bouwnummer:</td>
                                        <td>[bouwnummer_intern]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Datum oplevering:</td>
                                        <td>[melding_datumoplevering]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Datum tweede handtekening:</td>
                                        <td>[datumtweedehandtekeningondertekening]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Einde onderhoudstermijn:</td>
                                        <td>[melding_datumeindeonderhoudstermijn]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Einde garantietermijn:</td>
                                        <td>[melding_datumeindegarantietermijn]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td><b><u>Betrokkenen</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Opdrachtgever:</td>
                                        <td>[client_organisatie_relation_naam]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Aanspreekpunt:</td>
                                        <td>[aanspreekpunt_voor_oplosser]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Naam:</td>
                                        <td>[aanspreekpunt_naam]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>E-mail:</td>
                                        <td>[aanspreekpunt_email]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Telefoon:</td>
                                        <td>[aanspreekpunt_telefoon]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>E-mail (privé):</td>
                                        <td>[aanspreekpunt_email_privé]</td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Telefoon (privé):</td>
                                        <td>[aanspreekpunt_telefoon_privé]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td><b><u>Links</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Werkbon afmeld link:</td>
                                        <td><a href='[workorder_direct_link]'>[workorder_direct_link]</a></td>
                                    </tr>
                                    <tr style=""vertical-align:top"">
                                        <td>Werkbon link:</td>
                                        <td><a href='[workorder_login_link]'>[workorder_login_link]</a></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                [slotgroet]<br />
                                [ondertekenaar_naam]<br />
                                [organisatie_naamonderdeel]<br />
                                [organisatie_logo]
                                <br />
                                <br />
                                [bijlagen]"
            };
        }

        public EmailTemplateModel RepairRequestEmailTemplate()
        {
            // In future we need to update this to configuration
            //var template = _dbContext.Sjabloon.Where(x => x.Guid == emailTemplateId).SingleOrDefault();
            //if (template != null)
            //    return new EmailTemplateModel(template);


            return new EmailTemplateModel
            {
                Subject = "Melding [melding_nummer]",
                Template = string.Empty,
                TemplateHtml = @"[geachte]<br />
                                <br />
                                <br />
                                 <b>[belangrijk_tekst]</b>
                                <br />
                                <br/>
                                <table>
                                    <tr>
                                        <td><b><u>Melding</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Melding Nummer:</td>
                                        <td>[melding_nummer]</td>
                                    </tr>
                                    <tr>
                                        <td>Datum:</td>
                                        <td>[melding_datum]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Object</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Objectcode:</td>
                                        <td>[bouwnummer_intern]</td>
                                    </tr>
                                    <tr>
                                        <td>Adres:</td>
                                        <td>[object_straat] [object_huisnummer]</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>[object_postcode]  [object_plaats]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Toelichting</u></b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Hoofdaannemer:</td>
                                        <td>[hoofdaannemer]</td>
                                    </tr>
                                    <tr>
                                        <td>Locatie:</td>
                                        <td>[melding_locatie]</td>
                                    </tr>
                                    <tr>
                                        <td>Omschrijving:</td>
                                        <td>[melding_korte_omschrijving]</td>
                                    </tr>
                                    <tr>
                                        <td>Afgehandelddatum:</td>
                                        <td>[melding_datum_afgehandeld]</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><b><u>Bijlagen</u></b>
                                            [melding_bijlagen]
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                [slotgroet]<br />
                                [ondertekenaar_naam]<br />
                                [organisatie_naamonderdeel]<br />
                                [organisatie_logo]"
            };
        }

        public SmtpModel GetConfigSettingForSender(string employeeId)
        {
            var employeeEmailSetting = _dbContext.EmailAccounts
                .Include(x => x.MedewerkerGu).ThenInclude(x => x.EigenOrganisatieGu)
                .Where(x => x.MedewerkerGuid == employeeId && x.PrimairEmailAccount == true)
               .FirstOrDefault();

            if (employeeEmailSetting != null)
            {
                return new SmtpModel(
                    new ViewPortalEmailNotificationAfzender
                    {
                        EmailAccountGuid = employeeEmailSetting.Guid,
                        MedewerkerGuid = employeeEmailSetting.MedewerkerGuid,
                        SmtpFromEmail = employeeEmailSetting.SmtpFromEmail,
                        SmtpUser = employeeEmailSetting.SmtpUser,
                        SmtpUseSsl = employeeEmailSetting.SmtpUseSsl ?? false,
                        SmtpPasswordDecrypted = _dbContext.DecryptPasswordForEmailAccount(employeeEmailSetting.SmtpPasswordHash, employeeEmailSetting.Guid)
                    }
                );
            }
            return null;
        }

        public EmailTemplateModel DossierNotificationEmailTemplate()
        {
            return new EmailTemplateModel
            {
                Subject = "Dossier melding [werknaam]",
                Template = string.Empty,
                TemplateHtml = @"[geachte]
                                <br />
                                <br />
                                Graag uw aandacht voor het volgende dossier: 
                                <br />
                                <br />
                                <b>[werknaam] </b>
                                <br />
                                <br />
                                   <a href='[dossier_link]'>[dossier_link]</a>
                                <br />
                                <br />
                                   [dossier_gebouw_link]
                                <br />
                                <br />
                                 <b>[verplichte_tekst]</b>
                                <br />
                                <br/>
                                [slotgroet]<br />
                                [ondertekenaar_naam]<br />
                                [organisatie_naamonderdeel]<br />
                                [organisatie_logo]"
            };
        }
    }
}
