using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    /// <summary>
    /// Group of <see cref="Activity"/>.
    /// </summary>
    public class ActivityType
    {
        /// <summary>
        /// Specifies that we should automatically add a note to a <see cref="Request"/>.
        /// </summary>
        public static readonly ActivityType AddNote = new ActivityType { ActivityTypeId = 1, Name = "Add Note" };

        /// <summary>
        /// Specifies that we should send an email to one or more recipients.
        /// </summary>
        public static readonly ActivityType SendEmail = new ActivityType { ActivityTypeId = 1, Name = "Send Email" };

        /// <summary>
        /// Specifies that we should add one or more persons as Stakeholders on this <see cref="Request"/>.
        /// </summary>
        public static readonly ActivityType AddStakeholders = new ActivityType { ActivityTypeId = 1, Name = "Add Stakeholders" };

        /// <summary>
        /// Specifies that we should remove one or more stakeholders from this <see cref="Request"/>.
        /// </summary>
        public static readonly ActivityType RemoveStakeholders = new ActivityType { ActivityTypeId = 1, Name = "Remove Stakeholders" };

        /// <summary>
        /// Gets identity number of this <see cref="ActivityType"/> object.
        /// </summary>
        public int ActivityTypeId { get; private set; }

        /// <summary>
        /// Gets <see cref="ActivityType"/> name.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="ActivityType"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public ActivityType() { }

        /// <summary>
        /// Instanciate custom <see cref="ActivityType"/> object.
        /// </summary>
        /// <param name="name">name of actionType.</param>
        public ActivityType(string name)
        {
            this.Name = name;
        }
    }
}
