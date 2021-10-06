namespace Portal.JPDS.Domain.Entities
{
    public class LoginRolWerk : BaseEntity 
    {
        public string LoginGuid { get; set; }
        public string ModuleGuid { get; set; }
        public string RolGuid { get; set; }
        public string WerkGuid { get; set; }
        public string GebouwGuid { get; set; }
        public bool Actief { get; set; }
        public virtual Gebouw GebouwGu { get; set; }
        public virtual Login LoginGu { get; set; }
        public virtual Werk WerkGu { get; set; }
    }
}
