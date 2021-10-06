using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.CentralDBEntities
{
    public class CentralLogin : CentralBaseEntity
    {
        public CentralLogin()
        {
            //AdministrationCreatedByNavigations = new HashSet<Administration>();
            //AdministrationModifiedByNavigations = new HashSet<Administration>();
            //AdministrationModuleCreatedByNavigations = new HashSet<AdministrationModule>();
            //AdministrationModuleModifiedByNavigations = new HashSet<AdministrationModule>();
            InverseCreatedByNavigation = new HashSet<CentralLogin>();
            InverseModifiedByNavigation = new HashSet<CentralLogin>();
            //LoginAdministrationCreatedByNavigations = new HashSet<LoginAdministration>();
            //LoginAdministrationLoginGus = new HashSet<LoginAdministration>();
            //LoginAdministrationModifiedByNavigations = new HashSet<LoginAdministration>();
            //LoginSessionCreatedByNavigations = new HashSet<LoginSession>();
            //LoginSessionLoginGus = new HashSet<LoginSession>();
            //LoginSessionModifiedByNavigations = new HashSet<LoginSession>();
            //ModuleCreatedByNavigations = new HashSet<Module>();
            //ModuleModifiedByNavigations = new HashSet<Module>();
            //ModuleRoleCreatedByNavigations = new HashSet<ModuleRole>();
            //ModuleRoleModifiedByNavigations = new HashSet<ModuleRole>();
            //RoleCreatedByNavigations = new HashSet<Role>();
            //RoleModifiedByNavigations = new HashSet<Role>();
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public bool OptIn { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string OldPasswordHash { get; set; }
        public DateTime? OldPasswordResetDate { get; set; }
        public DateTime? ResetPasswordLinkCreatedOn { get; set; }
        public int? EmailVerificationOtp { get; set; }
        public DateTime? EmailVerificationDateTime { get; set; }
        public string EmailApproval { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public DateTime? CustomerLoginModifiedOn { get; set; }

        public virtual CentralLogin CreatedByNavigation { get; set; }
        public virtual CentralLogin ModifiedByNavigation { get; set; }
        //public virtual ICollection<Administration> AdministrationCreatedByNavigations { get; set; }
        //public virtual ICollection<Administration> AdministrationModifiedByNavigations { get; set; }
        //public virtual ICollection<AdministrationModule> AdministrationModuleCreatedByNavigations { get; set; }
        //public virtual ICollection<AdministrationModule> AdministrationModuleModifiedByNavigations { get; set; }
        public virtual ICollection<CentralLogin> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<CentralLogin> InverseModifiedByNavigation { get; set; }
        //public virtual ICollection<LoginAdministration> LoginAdministrationCreatedByNavigations { get; set; }
        //public virtual ICollection<LoginAdministration> LoginAdministrationLoginGus { get; set; }
        //public virtual ICollection<LoginAdministration> LoginAdministrationModifiedByNavigations { get; set; }
        //public virtual ICollection<LoginSession> LoginSessionCreatedByNavigations { get; set; }
        //public virtual ICollection<LoginSession> LoginSessionLoginGus { get; set; }
        //public virtual ICollection<LoginSession> LoginSessionModifiedByNavigations { get; set; }
        //public virtual ICollection<Module> ModuleCreatedByNavigations { get; set; }
        //public virtual ICollection<Module> ModuleModifiedByNavigations { get; set; }
        //public virtual ICollection<ModuleRole> ModuleRoleCreatedByNavigations { get; set; }
        //public virtual ICollection<ModuleRole> ModuleRoleModifiedByNavigations { get; set; }
        //public virtual ICollection<Role> RoleCreatedByNavigations { get; set; }
        //public virtual ICollection<Role> RoleModifiedByNavigations { get; set; }
    }
}
