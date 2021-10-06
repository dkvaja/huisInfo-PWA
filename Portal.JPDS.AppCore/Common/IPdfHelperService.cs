using Portal.JPDS.AppCore.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Portal.JPDS.AppCore.Common
{
    public interface IPdfHelperService
    {
        byte[] MergePdf(List<string> filesToMerge);
        int TotalPageCount(string file);
    }
}
