using Portal.JPDS.AppCore.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Common
{
    public  interface IMimeMappingService
    {
        string Map(string fileName);
    }
}
