using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.Domain.Entities
{
    /// <summary>
    /// Author : Abhishek Saini
    /// This is the Base Entity class. We can add/modify the
    /// properties as per the application requirements
    /// </summary>
   public class BaseEntity
    {
        public string Guid { get; set; }
        public DateTime? IngevoerdOp { get; set; }
        public string IngevoerdDoor { get; set; }
        public DateTime? GewijzigdOp { get; set; }
        public string GewijzigdDoor { get; set; }
    }
}
