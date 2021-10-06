using Portal.JPDS.AppCore.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Common
{
    public interface IDigitalSigningService
    {
        void SendDocumentToBeDigitallySigned(string quoteId, string userId, string quotationDocumentHeader, string quotationDrawingHeader, Uri callbackUrl);
        byte[] GetDigitallySignedFile(long documentId, string fileId);
    }
}
