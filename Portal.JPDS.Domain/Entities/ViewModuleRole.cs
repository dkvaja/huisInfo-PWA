using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    public class ViewModuleRole
    {
        public string ModuleGuid { get; set; }
        public string RoleGuid { get; set; }
        public string ModuleName { get; set; }
        public string RoleName { get; set; }
        public byte? RoleEnum { get; set; }
        public string AdministrationGuid { get; set; }
        public bool Active { get; set; }
    }
}
