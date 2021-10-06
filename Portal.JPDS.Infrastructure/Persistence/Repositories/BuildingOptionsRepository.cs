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
    public class BuildingOptionsRepository : BaseRepository, IBuildingOptionsRepository
    {
        public BuildingOptionsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void AddOrUpdateToCartIndividualOption(AddToCartApiModel model, string userId)
        {
            var optionToUpdate = _dbContext.OptieGekozen.Find(model.OptionId);

            optionToUpdate.OptieStatus = (int)SelectedOptionStatus.Provisional;
            optionToUpdate.OptieSubstatus = (int)SelectedOptionSubStatus.InShoppingCart;

            optionToUpdate.ViaWebsite = true;
            //Commented below as requested in https://jpds.atlassian.net/browse/WPJPBM-295
            //optionToUpdate.AanvullendeOmschrijving = model.Comment;
        }

        public void AddOrUpdateToCartStandardOption(AddToCartApiModel model, string userId)
        {
            var sql = "optie_standaard_toevoegen_aan_optie_gekozen @optie_standaard_guid, @gebouw_guid, @aantal, @aanvullende_omschrijving, @via_website, @login_guid";
            _dbContext.Database.ExecuteSqlRaw(sql,
                new SqlParameter("@optie_standaard_guid", model.OptionId),
                new SqlParameter("@gebouw_guid", model.BuildingId),
                new SqlParameter("@aantal", model.Quantity),
                new SqlParameter("@aanvullende_omschrijving", string.IsNullOrWhiteSpace(model.Comment) ? (object)DBNull.Value : model.Comment),
                new SqlParameter("@via_website", true),
                new SqlParameter("@login_guid", userId)
                );
        }

        public void DeleteSelectedOption(string selectedOptionId)
        {
            var selectedOption = _dbContext.OptieGekozen.Find(selectedOptionId);
            if (selectedOption != null)
            {
                if (selectedOption.Soort == (int)SelectedOptionType.Standard)
                {
                    if (selectedOption.OptieStatus == (int)SelectedOptionStatus.Provisional)
                    {
                        //Remove only when standard option
                        _dbContext.OptieGekozen.Remove(selectedOption);
                    }
                }
                else if (selectedOption.Soort == (int)SelectedOptionType.Individual)
                {
                    //Reset status when individual option
                    selectedOption.OptieStatus = (int)SelectedOptionStatus.Provisional;
                    selectedOption.OptieSubstatus = (int)SelectedOptionSubStatus.ToBeDisplayedForSelection;
                }
            }
        }

        public bool RequestSelectedOptions(string buildingId)
        {
            var hasAdditionalDescription = false;

            var optionsInShoppingCart = _dbContext.OptieGekozen
                .Where(x =>
                    x.GebouwGuid == buildingId
                    && x.OptieStatus == (int)SelectedOptionStatus.Provisional
                    && x.OptieSubstatus == (int)SelectedOptionSubStatus.InShoppingCart
                    );
            foreach (var selectedOption in optionsInShoppingCart)
            {
                selectedOption.OptieStatus = (int)SelectedOptionStatus.New;
                selectedOption.OptieSubstatus = (int)SelectedOptionSubStatus.NewOrToBeJudged;

                if (!hasAdditionalDescription && !string.IsNullOrEmpty(selectedOption.AanvullendeOmschrijving))
                {
                    hasAdditionalDescription = true;
                }
            }
            return hasAdditionalDescription;
        }

        public IEnumerable<SelectedOptionApiModel> GetAvailableIndividualOptions(string buildingId)
        {
            return _dbContext.ViewPortalOptieGekozen
                .Where(x =>
                    x.GebouwGuid == buildingId
                    && x.Soort == (int)SelectedOptionType.Individual
                    && x.OptieStatus == (int)SelectedOptionStatus.Provisional
                    && x.OptieSubstatus == (int)SelectedOptionSubStatus.ToBeDisplayedForSelection
                    )
                .OrderBy(x => x.Optienummer)
                .Select(x => new SelectedOptionApiModel(x));
        }

        public QuotationApiModel GetQuotationById(string quoteId)
        {
            var quotation = _dbContext.OptieGekozenOfferte.Find(quoteId);
            return new QuotationApiModel
            {
                QuoteId = quotation.Guid,
                QuoteNo = quotation.Offertenummer,
                ClosingDate = quotation.Sluitingsdatum,
                BuildingId = quotation.GebouwGuid,
                DigitalDocumentId = quotation.ScriveDocumentId,
                DigitalDocumentStatus = quotation.ScriveDocumentStatus,
                IsDigitalSigning = quotation.Ondertekening == (int)QuotationSignatureType.DigitalStandard || quotation.Ondertekening == (int)QuotationSignatureType.Digital_iDIN,
            };
        }

        public IEnumerable<QuotationApiModel> GetQuotationsForBuilding(string buildingId)
        {
            var options = _dbContext.ViewPortalOptieGekozen
                .Where(x =>
                x.GebouwGuid == buildingId
                && !string.IsNullOrEmpty(x.OptieGekozenOfferteGuid)
                && x.OptieStatus == (int)SelectedOptionStatus.Quotation
                && x.OptieSubstatus == (int)SelectedOptionSubStatus.Quotation)
                .OrderBy(x => x.CategorieVolgorde)
                .ThenBy(x => x.Categorie)
                .ThenBy(x => x.RubriekVolgorde)
                .ThenBy(x => x.Rubriek)
                .ThenBy(x => x.Optienummer).ToList();

            var quotations = _dbContext.OptieGekozenOfferte
                .Where(x =>
                x.GebouwGuid == buildingId
                && x.Sluitingsdatum >= DateTime.Now.Date)
                .ToList();

            return quotations.Select(x => new QuotationApiModel
            {
                QuoteId = x.Guid,
                QuoteNo = x.Offertenummer,
                ClosingDate = x.Sluitingsdatum,
                BuildingId = x.GebouwGuid,
                IsDigitalSigning = x.Ondertekening == (int)QuotationSignatureType.DigitalStandard || x.Ondertekening == (int)QuotationSignatureType.Digital_iDIN,
                Options = options.Where(y => y.OptieGekozenOfferteGuid == x.Guid).Select(y => new SelectedOptionApiModel(y))
            }).Where(x => x.Options.Count() > 0);
        }

        public int GetQuotationsCountForBuilding(string buildingId)
        {
            return _dbContext.OptieGekozenOfferte
                .Where(x =>
                    x.GebouwGuid == buildingId
                    && x.Sluitingsdatum >= DateTime.Now.Date
                    && x.OptieGekozen.Any(y => y.OptieStatus == (int)SelectedOptionStatus.Quotation && y.OptieSubstatus == (int)SelectedOptionSubStatus.Quotation)
                ).Count();
        }

        public IEnumerable<QuotationApiModel> GetQuotationsByBuildingIdAndStatuses(string buildingId, SelectedOptionStatus status, SelectedOptionSubStatus? subStatus)
        {
            var options = _dbContext.ViewPortalOptieGekozen
                .Where(x =>
                x.GebouwGuid == buildingId
                && !string.IsNullOrEmpty(x.OptieGekozenOfferteGuid)
                && x.OptieStatus == (int)status
                && (subStatus.HasValue ? x.OptieSubstatus == (int)subStatus.Value : true))
                .OrderBy(x => x.CategorieVolgorde)
                .ThenBy(x => x.Categorie)
                .ThenBy(x => x.RubriekVolgorde)
                .ThenBy(x => x.Rubriek)
                .ThenBy(x => x.Optienummer).ToList();

            var quotations = _dbContext.OptieGekozenOfferte
                .Where(x =>
                x.GebouwGuid == buildingId)
                .ToList();

            return quotations.Select(x => new QuotationApiModel
            {
                QuoteId = x.Guid,
                QuoteNo = x.Offertenummer,
                ClosingDate = x.Sluitingsdatum,
                BuildingId = x.GebouwGuid,
                IsDigitalSigning = x.Ondertekening == (int)QuotationSignatureType.DigitalStandard || x.Ondertekening == (int)QuotationSignatureType.Digital_iDIN,
                Options = options.Where(y => y.OptieGekozenOfferteGuid == x.Guid).Select(y => new SelectedOptionApiModel(y))
            }).Where(x => x.Options.Count() > 0);
        }

        public IEnumerable<SelectedOptionApiModel> GetSelectedOptionsByBuildingIdAndStatuses(string buildingId, SelectedOptionStatus status, SelectedOptionSubStatus subStatus)
        {
            return _dbContext.ViewPortalOptieGekozen
                .Where(x => x.GebouwGuid == buildingId && x.OptieStatus == (int)status && x.OptieSubstatus == (int)subStatus)
                .OrderBy(x => x.Optienummer)
                .Select(x => new SelectedOptionApiModel(x));
        }

        public int GetCountOfItemsInShoppingCart(string buildingId)
        {
            return _dbContext.ViewPortalOptieGekozen
                   .Count(x => x.GebouwGuid == buildingId && x.OptieStatus == (int)SelectedOptionStatus.Provisional && x.OptieSubstatus == (int)SelectedOptionSubStatus.InShoppingCart);
        }

        public IEnumerable<OptionCategoryApiModel> GetStandardOptionCategoriesByBuildingId(string buildingId)
        {
            return _dbContext.ViewPortalOptieCategorieSluitingsdatum.Where(x => x.GebouwGuid == buildingId).Select(x => new OptionCategoryApiModel(x)).ToList();
        }

        public IEnumerable<OptionHeaderApiModel> GetStandardOptionHeadersByProjectId(string projectId)
        {
            return _dbContext.OptieRubriekWerk.Where(x => x.WerkGuid == projectId).Select(x => new OptionHeaderApiModel(x)).ToList();
        }

        public IEnumerable<StandardOptionApiModel> GetStandardOptionsByBuildingId(string buildingId)
        {
            return _dbContext.ViewPortalOptieStandaardPerGebouw
                .Where(x => x.GebouwGuid == buildingId)
                .OrderBy(x => x.Optienummer)
                .Select(x => new StandardOptionApiModel(x)).ToList();
        }

        public bool UpdateQuotationStatus(string quoteId, SelectedOptionStatus optionStatus, SelectedOptionSubStatus optionSubStatus, string documentStatus = null, long? documentId = null)
        {
            var quotation = _dbContext.OptieGekozenOfferte.Find(quoteId);

            if (!string.IsNullOrWhiteSpace(documentStatus))
            {
                quotation.ScriveDocumentStatus = documentStatus;
            }

            if (documentId.HasValue)
            {
                quotation.ScriveDocumentId = documentId;
            }

            foreach (var quotedOption in _dbContext.OptieGekozen.Where(x => x.OptieGekozenOfferteGuid == quoteId))
            {
                if (quotedOption.OptieStatus != (byte)optionStatus)
                {
                    quotedOption.OptieStatus = (byte)optionStatus;
                    if (optionStatus == SelectedOptionStatus.Definite)
                    {
                        quotedOption.DatumDefinitief = DateTime.Now;
                    }
                    else if (optionStatus == SelectedOptionStatus.Cancelled)
                    {
                        quotedOption.DatumVervallen = DateTime.Now;
                    }
                }

                quotedOption.OptieSubstatus = (int)optionSubStatus;
            }

            return true;
        }

        public IEnumerable<SelectedOptionApiModel> GetRequestedOptionsByBuildingId(string buildingId)
        {
            return _dbContext.OptieGekozen
                .Include(x => x.EenheidGu)
                .Where(x => x.GebouwGuid == buildingId && x.OptieStatus == (int)SelectedOptionStatus.New && x.OptieSubstatus == (int)SelectedOptionSubStatus.NewOrToBeJudged)
                .OrderBy(x => x.Optienummer)
                .Select(x => new SelectedOptionApiModel(x));
        }

        public IEnumerable<SelectedOptionApiModel> GetSeletedDefiniteOptionsByBuildingId(string buildingId)
        {
            return _dbContext.OptieGekozen
                .Include(x => x.EenheidGu)
                .Where(x => x.GebouwGuid == buildingId && x.OptieStatus == (int)SelectedOptionStatus.Definite)
                .OrderBy(x => x.Optienummer)
                .Select(x => new SelectedOptionApiModel(x));
        }

        public IEnumerable<StandardOptionProjectApiModel> GetStandardOptionsByProjectId(string projectId)
        {
            return _dbContext.ViewPortalOptieStandaardPerWerk
                .Where(x => x.WerkGuid == projectId)
                .OrderBy(x => x.Optienummer)
                .Select(x => new StandardOptionProjectApiModel(x)).ToList();
        }

        public DigitalSignInfoModel GetSigningInfoForQuotation(string quoteId, string userId)
        {
            var quotationSigningInfo = _dbContext.ViewPortalScriveDigitaalOndertekenen.SingleOrDefault(x => x.OptieGekozenOfferteGuid == quoteId);

            SigningParty signingParty1 = null, signingParty2 = null;

            var isSecondPartyCurrentUser = string.Equals(userId, quotationSigningInfo.Login2Guid, StringComparison.InvariantCulture);

            if (isSecondPartyCurrentUser)
            {
                signingParty1 = new SigningParty { Email = quotationSigningInfo.Login2Email, Name = quotationSigningInfo.Login2Naam };
            }
            else
            {
                signingParty1 = new SigningParty { Email = quotationSigningInfo.Login1Email, Name = quotationSigningInfo.Login1Naam };
            }

            if (quotationSigningInfo.OndertekeningAantalPersonen != 1)
            {
                if (isSecondPartyCurrentUser)
                {
                    signingParty2 = new SigningParty { Email = quotationSigningInfo.Login1Email, Name = quotationSigningInfo.Login1Naam };
                }
                else if (!string.IsNullOrWhiteSpace(quotationSigningInfo.Login2Email))
                {
                    signingParty2 = new SigningParty { Email = quotationSigningInfo.Login2Email, Name = quotationSigningInfo.Login2Naam };
                }
            }

            DigitalSignInfoModel digitalSignInfo = new DigitalSignInfoModel
            {
                ClosingDate = quotationSigningInfo.Sluitingsdatum,
                QuoteId = quotationSigningInfo.OptieGekozenOfferteGuid,
                QuoteNo = quotationSigningInfo.Offertenummer,
                SignatureType = quotationSigningInfo.Ondertekening,
                SigningParty1 = signingParty1,
                SigningParty2 = signingParty2
            };

            return digitalSignInfo;
        }

        public string GetProjectIdForStandardOption(string optionStandardId)
        {
            return _dbContext.OptieStandaards.Find(optionStandardId)?.WerkGuid;
        }
    }
}
