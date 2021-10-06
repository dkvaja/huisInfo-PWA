using Portal.JPDS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class UserApiModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public Apps[] AvailableApps { get; set; }
        public string EmployeeId { get; set; }
        public string RelationId { get; set; }
        public string BuyerId { get; set; }
        public string PersonId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public DateTime? ResetPasswordLinkCreatedDateTime { get; set; }
        public byte? Role { get; set; }
        public string OrganisationId { get; set; }
    }

    public class UserModuleModel
    {
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ProjectId { get; set; }
        public string BuildingId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
