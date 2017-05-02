using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    /// <summary>
    /// Activities are things that can happen as a result of a <see cref="Request"/> entering a <see cref="State"/> or following a <see cref="Transition"/>.
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Gets identity number of <see cref="Activity"/> object.
        /// </summary>
        public int ActivityId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Activity"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets idenitity number of the group that this <see cref="Activity"/> belong to.
        /// </summary>
        public int ActivityTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="Activity"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Gets description of this <see cref="Activity"/>.
        /// </summary>
        [StringLength(255)]
        public string Description { get; private set; }

        /// <summary>
        /// Gets <see cref="ActivityType"/> object that determin the group of this <see cref="Activity"/>.
        /// </summary>
        [Associated]
        public ActivityType ActivityType { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Activity"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Activity() { }

        /// <summary>
        /// Instanciate custom <see cref="Activity"/> object.
        /// </summary>
        /// <param name="name">name of action</param>
        /// <param name="description">description of action</param>
        /// <param name="activityType">actionType of action.</param>
        public Activity(string name, string description, ActivityType activityType)
        {
            this.Name = name;
            this.Description = description;

            this.ActivityType = activityType;
            this.ActivityTypeId = activityType.ActivityTypeId;
        }

        /// <summary>
        /// Modify <see cref="Activity"/> object.
        /// </summary>
        /// <param name="name">new name of activity.</param>
        /// <param name="description">new description of activity.</param>
        /// <param name="activityType">new actionType of activity.</param>
        /// <returns>Current instance of <see cref="Activity"/> object.</returns>
        public Activity Update(string name, string description, ActivityType activityType)
        {
            this.Name = name;
            this.Description = description;

            this.ActivityType = activityType;
            this.ActivityTypeId = activityType.ActivityTypeId;

            return this;
        }
    }
}
