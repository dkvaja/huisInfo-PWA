using System;

namespace Portal.JPDS.Domain.CentralDBEntities
{
   public class CentralBaseEntity
    {
        public Guid Guid { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
