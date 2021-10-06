using iTextSharp.text;
using iTextSharp.text.pdf;
using Portal.JPDS.AppCore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.Infrastructure.Services
{
    public class PdfHelperService : IPdfHelperService
    {
        public byte[] MergePdf(List<string> filesToMerge)
        {
            PdfReader reader = null;
            PdfImportedPage importedPage;

            MemoryStream os = new MemoryStream();
            Document sourceDocument = new Document();
            PdfCopy pdfCopyProvider = new PdfCopy(sourceDocument, os);

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            foreach (var file in filesToMerge)
            {
                reader = new PdfReader(file);

                //Add pages in new file  
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();

            return os.ToArray();
        }

        public int TotalPageCount(string file)
        {
            using (var reader = new PdfReader(file))
            {
                var totalPages = reader.NumberOfPages;
                reader.Close();
                return totalPages;
            }
        }
    }
}
