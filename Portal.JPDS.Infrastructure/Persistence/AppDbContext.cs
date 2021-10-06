using Portal.JPDS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Portal.JPDS.AppCore.ApiModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.Linq;
using System;
using System.Data;
using System.Collections.Generic;

namespace Portal.JPDS.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        private string ExecuteDbFunctionReturnString(string functionName, params object[] sqlParameters)
        {
            var parameters = new List<SqlParameter>();
            if (sqlParameters != null && sqlParameters.Length > 0)
            {
                for (int i = 0; i < sqlParameters.Length; i++)
                {
                    parameters.Add(new SqlParameter("@p" + i, sqlParameters[i] ?? (object)DBNull.Value));
                }
            }

            var outputParameter = new SqlParameter("@result", SqlDbType.NVarChar);
            // Size -1 treats as NVarChar(max)
            outputParameter.Size = -1;
            outputParameter.Direction = ParameterDirection.Output;

            var parameterIndexes = string.Join(',', parameters.Select(x => x.ParameterName));
            parameters.Add(outputParameter);

            this.Database.ExecuteSqlRaw("set @result = " + functionName + "(" + parameterIndexes + ")", parameters);
            return outputParameter.Value?.ToString();
        }

        [DbFunction]
        public string GetEmailsForBuyerRenter(string buyerRenterId)
        {
            return ExecuteDbFunctionReturnString("dbo.get_email_koper_huurder", buyerRenterId);
        }

        [DbFunction]
        public string GetSalutationForEmail(string organisationId, string personId, string relationId, string buyerRenterId, bool? formal)
        {
            return ExecuteDbFunctionReturnString("dbo.get_briefaanhef", organisationId, personId, relationId, buyerRenterId, formal);
        }

        [DbFunction]
        public string GetDomainTranslation(string domainId, string dbValue)
        {
            return ExecuteDbFunctionReturnString("dbo.vertaal_domein", domainId, dbValue);
        }

        [DbFunction]
        public string DecryptPasswordForEmailAccount(string passwordHash, string emailAccountId)
        {
            return ExecuteDbFunctionReturnString("dbo.decrypt_tekst", passwordHash, emailAccountId);
        }

        [DbFunction]
        public string EncryptPassword(string passwordHash, string loginId)
        {
            return ExecuteDbFunctionReturnString("dbo.encrypt_tekst", passwordHash, loginId);
        }


        [DbFunction]
        public string GetOnsKenmark(string onsKenmarkFormat, string initials, DateTime year, int? serialNumber,string department, string workNumber)
        {
            return ExecuteDbFunctionReturnString("dbo.samenstellen_ons_kenmerk", onsKenmarkFormat, initials, year, serialNumber, department, workNumber);
        }

        [DbFunction]
        public DateTime? GetGuaranteeEndDate(string buildingId, string productId, string productSub1Id, string productSub2Id)
        {
            var dateOfEndGuarantee = ExecuteDbFunctionReturnString("dbo.bepaal_datum_einde_garantietermijn", buildingId, productId, productSub1Id, productSub2Id);
            return !string.IsNullOrWhiteSpace(dateOfEndGuarantee) ? Convert.ToDateTime(dateOfEndGuarantee) : null;
        }

        [DbFunction]
        public string GetCarryOutAsTypeId(string buildingId, DateTime meldingDate)
        {
            return ExecuteDbFunctionReturnString("dbo.bepaal_melding_soort", buildingId, meldingDate);
        }

        public string GetPersonName(string employeeId, string relationId, string buyerRenterId)
        {
            if (!string.IsNullOrWhiteSpace(buyerRenterId))
            {
                var buyer = KoperHuurder.Find(buyerRenterId);
                if (buyer != null)
                    return buyer.VolledigeNaam;
            }

            return Persoon.Where(x => x.Medewerker.Any(y => y.Guid == employeeId) || x.Relatie.Any(y => y.Guid == relationId)).Select(x => x.VolledigeNaam).FirstOrDefault() ?? string.Empty;
        }

        public DbSet<Adres> Adres { get; set; }
        public DbSet<Actie> Actie { get; set; }
        public DbSet<Afdeling> Afdelings { get; set; }
        public DbSet<Afhandelingstermijn> Afhandelingstermijn { get; set; }
        public DbSet<Betrokkene> Betrokkene { get; set; }
        public DbSet<BetrokkeneRelatie> BetrokkeneRelatie { get; set; }
        public DbSet<Communicatie> Communicaties { get; set; }
        public DbSet<ConfiguratieAlgemeen> ConfiguratieAlgemeen { get; set; }
        public DbSet<ConfiguratieKlachtenbeheer> ConfiguratieKlachtenbeheer { get; set; }
        public DbSet<ConfiguratieWebPortal> ConfiguratieWebPortal { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Bijlage> Bijlage { get; set; }
        public DbSet<BijlageDossier> BijlageDossiers { get; set; }
        public DbSet<BijlageRubriek> BijlageRubrieks { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatBericht> ChatBericht { get; set; }
        public DbSet<ChatBerichtBelangrijk> ChatBerichtBelangrijk { get; set; }
        public DbSet<ChatDeelnemer> ChatDeelnemer { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<DossierGebouw> DossierGebouws { get; set; }
        public DbSet<DossierLoginLaatstGelezen> DossierLoginLaatstGelezens { get; set; }
        public DbSet<DossierVolgorde> DossierVolgordes { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<FaqRubriek> FaqRubriek { get; set; }
        public DbSet<FaqVraagAntwoordWerk> FaqVraagAntwoordWerk { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Functie> Functies { get; set; }
        public DbSet<Gebouw> Gebouw { get; set; }
        public DbSet<GebouwSoort> GebouwSoorts { get; set; }
        public DbSet<GebouwStatus> GebouwStatuses { get; set; }
        public DbSet<KoperHuurder> KoperHuurder { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<LoginDossierRecht> LoginDossierRechts { get; set; }
        public DbSet<LoginRolWerk> LoginRolWerks { get; set; }
        public DbSet<Medewerker> Medewerker { get; set; }
        public DbSet<Melding> Melding { get; set; }
        public DbSet<MeldingAard> MeldingAard { get; set; }
        public DbSet<MeldingKosten> MeldingKosten { get; set; }
        public DbSet<MeldingLocatie> MeldingLocatie { get; set; }
        public DbSet<MeldingOorzaak> MeldingOorzaak { get; set; }
        public DbSet<MeldingSoort> MeldingSoort { get; set; }
        public DbSet<MeldingStatus> MeldingStatus { get; set; }
        public DbSet<MeldingType> MeldingType { get; set; }
        public DbSet<MeldingVeroorzaker> MeldingVeroorzaker { get; set; }
        public DbSet<Nieuws> Nieuws { get; set; }
        public DbSet<Oplosser> Oplosser { get; set; }
        public DbSet<Opname> Opname { get; set; }
        public DbSet<OptieGekozen> OptieGekozen { get; set; }
        public DbSet<OptieGekozenOfferte> OptieGekozenOfferte { get; set; }
        public DbSet<OptieRubriekWerk> OptieRubriekWerk { get; set; }
        public DbSet<OptieStandaard> OptieStandaards { get; set; }
        public DbSet<Organisatie> Organisatie { get; set; }
        public DbSet<OrganisatieProduct> OrganisatieProducts { get; set; }
        public DbSet<Persoon> Persoon { get; set; }
        public DbSet<ProductDienst> ProductDienst { get; set; }
        public DbSet<ProductDienstSub1> ProductDienstSub1 { get; set; }
        public DbSet<ProductDienstSub2> ProductDienstSub2 { get; set; }
        public DbSet<Relatie> Relatie { get; set; }
        public DbSet<Sjabloon> Sjabloon { get; set; }
        public DbSet<Tekstblok> Tekstblok { get; set; }
        public DbSet<ViewInputKoperHuurder> ViewInputKoperHuurder { get; set; }
        public  DbSet<ViewLogin> ViewLogins { get; set; }
        public DbSet<ViewPortalBetrokkene> ViewPortalBetrokkene { get; set; }
        public DbSet<ViewPortalBetrokkeneRelatieKopersbegeleider> ViewPortalBetrokkeneRelatieKopersbegeleider { get; set; }
        public DbSet<ViewPortalBijlage> ViewPortalBijlage { get; set; }
        public DbSet<ViewPortalChat> ViewPortalChat { get; set; }
        public DbSet<ViewPortalChatBericht> ViewPortalChatBericht { get; set; }
        public DbSet<ViewPortalChatBerichtBelangrijk> ViewPortalChatBerichtBelangrijk { get; set; }
        public DbSet<ViewPortalChatDeelnemer> ViewPortalChatDeelnemer { get; set; }
        public DbSet<ViewPortalChatGebouwStart> ViewPortalChatGebouwStart { get; set; }
        public DbSet<ViewPortalChatRelatieStart> ViewPortalChatRelatieStart { get; set; }
        public DbSet<ViewPortalChatZoeken> ViewPortalChatZoeken { get; set; }
        public DbSet<ViewPortalDashboardPlanningKoperHuurder> ViewPortalDashboardPlanningKoperHuurder { get; set; }
        public DbSet<ViewPortalDashboardPlanningKopersbegeleider> ViewPortalDashboardPlanningKopersbegeleider { get; set; }
        public DbSet<ViewPortalEmailNotification> ViewPortalEmailNotification { get; set; }
        public DbSet<ViewPortalEmailNotificationAfzender> ViewPortalEmailNotificationAfzender { get; set; }
        public DbSet<ViewPortalGebouwAlgemeen> ViewPortalGebouwAlgemeen { get; set; }
        public DbSet<ViewPortalGebouwKoperHuurder> ViewPortalGebouwKoperHuurder { get; set; }
        public DbSet<ViewPortalGebouwRelatie> ViewPortalGebouwRelatie { get; set; }
        public DbSet<ViewPortalMedewerker> ViewPortalMedewerker { get; set; }
        public DbSet<ViewPortalOptieCategorieSluitingsdatum> ViewPortalOptieCategorieSluitingsdatum { get; set; }
        public DbSet<ViewPortalOptieGekozen> ViewPortalOptieGekozen { get; set; }
        public DbSet<ViewPortalOptieStandaardPerGebouw> ViewPortalOptieStandaardPerGebouw { get; set; }
        public DbSet<ViewPortalOptieStandaardPerWerk> ViewPortalOptieStandaardPerWerk { get; set; }
        public DbSet<ViewPortalScriveDigitaalOndertekenen> ViewPortalScriveDigitaalOndertekenen { get; set; }
        public DbSet<ViewPortalWerk> ViewPortalWerk { get; set; }
        public DbSet<ViewModuleRole> ViewModuleRoles { get; set; }
        public DbSet<Werk> Werk { get; set; }
        public DbSet<WerkFase> WerkFase { get; set; }
        public DbSet<WoningType> WoningTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
