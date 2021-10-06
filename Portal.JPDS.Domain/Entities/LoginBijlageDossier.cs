
namespace Portal.JPDS.Domain.Entities
{
    public class LoginBijlageDossier : BaseEntity
    {
        public string LoginGuid { get; set; }
        public string BijlageDossierGuid { get; set; }
        public virtual BijlageDossier BijlageDossierGu { get; set; }
        public virtual Login LoginGu { get; set; }
    }
}
