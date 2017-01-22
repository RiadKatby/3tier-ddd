using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Basis
{
    public abstract class AuditableEntity : BaseEntity
    {
        public int? CreatedByID { get; set; }

        public int? UpdatedByID { get; set; }

        /// <summary>
        /// System creation date time of this entity, set by the system.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// The user who create the entity, set by the system.
        /// </summary>
        public User CreatedBy { get; protected set; }

        /// <summary>
        /// System update date time of this entity, set by the system.
        /// </summary>
        public DateTime? UpdatedAt { get; protected set; }

        /// <summary>
        /// The user who update the entity, set by the system.
        /// </summary>
        public User UpdatedBy { get; protected set; }
    }
}
