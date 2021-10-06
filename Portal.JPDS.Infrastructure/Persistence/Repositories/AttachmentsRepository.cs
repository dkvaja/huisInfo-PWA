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
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Portal.JPDS.Infrastructure.Persistence.Repositories
{
    public class AttachmentsRepository : BaseRepository, IAttachmentsRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AttachmentsRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AppDbContext AppDbContext
        {
            get { return _dbContext as AppDbContext; }
        }

        public string AddAttachmentForBuilding(string buildingId, string attachmentHeaderId, string fileName, string storeLocation)
        {
            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation);
            bijlage.GebouwGuid = buildingId;
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.Building;

            _dbContext.Bijlage.Add(bijlage);
            return bijlage.Bijlage1;
        }

        public string AddNewChatMessageAttachment(string chatMessageId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation)
        {
            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation);
            bijlage.ChatBerichtGuid = chatMessageId;
            bijlage.GebouwGuid = buildingId;
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.ChatMessage;

            _dbContext.Bijlage.Add(bijlage);
            return bijlage.Bijlage1;
        }

        public string AddNewSignedDocumentAttachment(out string attachmentId, string quoteId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation, bool appendTimestampToFileName)
        {
            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation, appendTimestampToFileName);
            bijlage.OptieGekozenOfferteGuid = quoteId;
            bijlage.GebouwGuid = buildingId;
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.Building;

            _dbContext.Bijlage.Add(bijlage);
            attachmentId = bijlage.Guid;
            return bijlage.Bijlage1;
        }

        public string AddNewStandardOptionImage(string optionStandardId, string attachmentHeaderId, string fileName, string storeLocation)
        {
            short? position = _dbContext.Bijlage.Local.Where(x => x.OptieStandaardGuid == optionStandardId).Max(x => x.Volgorde);

            if (!position.HasValue || position == 0)
            {
                position = _dbContext.Bijlage.Where(x => x.OptieStandaardGuid == optionStandardId).Max(x => x.Volgorde);
            }

            if (!position.HasValue)
            {
                position = 0;
            }

            position++;

            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation);
            bijlage.OptieStandaardGuid = optionStandardId;
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.StandardOption;
            bijlage.Volgorde = position;

            _dbContext.Bijlage.Add(bijlage);
            return bijlage.Bijlage1;
        }

        public string AddNewRepairRequestAttachment(string repairRequestId, string attachmentHeaderId, string fileName, string storeLocation, string resolverId)
        {
            short? position = _dbContext.Bijlage.Local.Where(x => x.MeldingGuid == repairRequestId).Max(x => x.Volgorde);

            if (!position.HasValue || position == 0)
            {
                position = _dbContext.Bijlage.Where(x => x.MeldingGuid == repairRequestId).Max(x => x.Volgorde);
            }

            if (!position.HasValue)
            {
                position = 0;
            }

            position++;

            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation);
            bijlage.MeldingGuid = repairRequestId;
            if (!string.IsNullOrWhiteSpace(resolverId))
            {
                bijlage.OplosserGuid = resolverId;
            }
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.RepairRequest;
            bijlage.Volgorde = position;

            _dbContext.Bijlage.Add(bijlage);
            return bijlage.Bijlage1;
        }

        public string GetAttachmentLocation(string attachmentId, bool updateModifyTimeStamp)
        {
            var attachment = _dbContext.Bijlage.Where(x => x.Guid == attachmentId && x.Publiceren)
                             .SingleOrDefault();
            if (attachment != null)
            {
                if (updateModifyTimeStamp)
                {
                    attachment.GewijzigdOp = DateTime.Now;
                }

                return attachment?.Bijlage1;
            }
            return null;
        }

        public IEnumerable<AttachmentWithHeaderApiModel> GetAttachmentsByBuildingId(string buildingId)
        {
            return _dbContext.Bijlage.Include(x => x.BijlageRubriekGu)
                .Where(x => x.Bijlage1 != null && x.Publiceren == true && x.GebouwGuid == buildingId && x.KoppelenAan != (int)AttachmentLinkedTo.Dossier)
                .Select(x => new AttachmentWithHeaderApiModel(x));
        }

        public IEnumerable<AttachmentWithHeaderApiModel> GetCommonAttachmentsForProject(string projectId)
        {
            return _dbContext.Bijlage.Include(x => x.BijlageRubriekGu)
                .Where(x => x.Bijlage1 != null && x.Publiceren == true && x.WerkGuid == projectId && x.GebouwGuid == null && x.KoppelenAan != (int)AttachmentLinkedTo.Dossier)
                .Select(x => new AttachmentWithHeaderApiModel(x));
        }

        public IEnumerable<AttachmentWithHeaderApiModel> GetDossierAttachmentsForBuilding(string buildingId)
        {
            var projectId = _dbContext.Gebouw.Find(buildingId).WerkGuid;

            return _dbContext.BijlageDossiers.Include(x => x.BijlageGu).ThenInclude(x => x.BijlageRubriekGu)
                                .Include(x => x.DossierGu).ThenInclude(x => x.DossierGebouws)
                                .Where(x =>
                                    x.BijlageGu.Bijlage1 != null && x.BijlageGu.Publiceren == true
                                    && !x.Intern && !x.Gearchiveerd && !x.Verwijderd && x.DossierGu.Status != (byte)DossierStatus.Draft
                                    && x.DossierGu.WerkGuid == projectId && (x.GebouwGuid == buildingId || x.GebouwGuid == null)
                                    && x.DossierGu.DossierGebouws.Any(y => y.Actief == true && y.Status != (byte)DossierStatus.Draft && x.DossierGu.Extern && y.GebouwGuid == buildingId)
                                    )
                           .Select(x => new AttachmentWithHeaderApiModel(x.BijlageGu));
        }

        private Bijlage FillNewAttachmentObject(string attachmentHeaderId, string fileName, string storeDirectory, bool appendTimestampToFileName = true, string description = null)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileExtension = Path.GetExtension(fileName);
            string fileNameToStore = fileNameWithoutExtension
                + (appendTimestampToFileName ? DateTime.Now.ToString("_yyyyMMddhhmmss", System.Globalization.CultureInfo.InvariantCulture) : "")
                + fileExtension;

            if (!Directory.Exists(storeDirectory))
            {
                Directory.CreateDirectory(storeDirectory);
            }

            string fileLocation = Path.Combine(storeDirectory, fileNameToStore);

            Bijlage bijlage = new Bijlage();
            bijlage.Guid = Guid.NewGuid().ToUpperString();
            bijlage.BijlageRubriekGuid = attachmentHeaderId;
            bijlage.Omschrijving = description ?? fileNameWithoutExtension;
            bijlage.Datum = DateTime.Now;
            bijlage.Publiceren = true;
            bijlage.BijlageVerwijderenBijVerwijderenRecord = true;
            bijlage.Bijlage1 = fileLocation;
            bijlage.BijlageOrigineel = fileLocation;
            return bijlage;
        }

        public string GetChatAttachmentLocation(string chatMessageId)
        {
            return _dbContext.Bijlage.Where(x => x.ChatBerichtGuid == chatMessageId).Select(x => x.Bijlage1).FirstOrDefault();
        }

        public IEnumerable<AttachmentApiModel> GetQuotationDocuments(string quoteId, string attachmentHeaderId)
        {
            return _dbContext.Bijlage
                .Where(x => x.OptieGekozenOfferteGuid == quoteId && x.BijlageRubriekGuid == attachmentHeaderId)
                .Select(x => new AttachmentApiModel
                {
                    Id = x.Guid,
                    DateTime = x.Datum,
                    Description = x.Omschrijving,
                    Order = x.Volgorde
                }).OrderBy(x => x.Order);
        }

        public IEnumerable<AttachmentApiModel> GetSelectedOptionAttachments(string selectedOptionId, bool imagesOnly)
        {
            var results = _dbContext.Bijlage
                .Where(x => x.OptieGekozenGuid == selectedOptionId).ToList();

            //check for imagesOnly
            if (imagesOnly)
                results = results.Where(x => x.Bijlage1.IsImage()).ToList();

            return results
                .Select(x => new AttachmentApiModel
                {
                    Id = x.Guid,
                    DateTime = x.Datum,
                    Description = x.Omschrijving,
                    Order = x.Volgorde
                }).OrderBy(x => x.Order);
        }

        public IEnumerable<AttachmentApiModel> GetStandardOptionAttachments(string standardOptionId, bool imagesOnly = false)
        {
            var results = _dbContext.Bijlage
                .Where(x => x.OptieStandaardGuid == standardOptionId).ToList();

            //check for imagesOnly
            if (imagesOnly)
                results = results.Where(x => x.Bijlage1.IsImage()).ToList();

            return results
                .Select(x => new AttachmentApiModel
                {
                    Id = x.Guid,
                    DateTime = x.Datum,
                    Description = x.Omschrijving,
                    Order = x.Volgorde
                }).OrderBy(x => x.Order);
        }

        /// <summary>
        /// Deletes the attachment record from the database.
        /// When <para>deleteFile</para> is true then the delete operation will be committed immediately.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="deleteFile"></param>
        /// <returns></returns>
        public bool DeleteAttachment(string attachmentId, out string fileToDelete)
        {
            var itemToDelete = _dbContext.Bijlage.Find(attachmentId);
            if (itemToDelete != null)
            {
                fileToDelete = itemToDelete.Bijlage1;
                _dbContext.Bijlage.Remove(itemToDelete);

                return true;
            }
            fileToDelete = null;
            return false;
        }

        public void SortAttachments(List<Guid> lstIds)
        {
            short count = 0;
            foreach (var id in lstIds)
            {
                var bijlage = _dbContext.Bijlage.Find(id.ToUpperString());
                if (bijlage != null)
                {
                    count++;
                    bijlage.Volgorde = count;
                }
            }
        }

        public string AddDossierFileAttachment(out string attachmentId, string projectId, string buildingId, string attachmentHeaderId, string fileName, string storeLocation)
        {
            var bijlage = FillNewAttachmentObject(attachmentHeaderId, fileName, storeLocation);
            bijlage.WerkGuid = projectId;
            if (!string.IsNullOrWhiteSpace(buildingId))
            {
                bijlage.GebouwGuid = buildingId;
            }
            bijlage.KoppelenAan = (int)AttachmentLinkedTo.Dossier;
            _dbContext.Bijlage.Add(bijlage);
            attachmentId = bijlage.Guid;

            return bijlage.Bijlage1;
        }

        public string GetStandardOptionId(string attachmentId)
        {
            return _dbContext.Bijlage.Find(attachmentId)?.OptieStandaardGuid;
        }

        public string GetStandardOptionId(List<string> attachmentIds)
        {
            var optionStandardIds = _dbContext.Bijlage.Where(x => attachmentIds.Contains(x.Guid)).Select(x => x.OptieStandaardGuid).Distinct();
            return optionStandardIds.Count() == 1 ? optionStandardIds?.SingleOrDefault() : null;
        }
    }
}
