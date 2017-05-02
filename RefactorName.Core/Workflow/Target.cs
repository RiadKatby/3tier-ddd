using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class Target
    {
        public static readonly Target Requester = new Target { TargetId = 1, Name = "Requester" };

        public static readonly Target Stakeholders = new Target { TargetId = 2, Name = "Stakeholders" };

        public static readonly Target GroupMemebers = new Target { TargetId = 3, Name = "Group Memebers" };

        public static readonly Target ProcessAdmins = new Target { TargetId = 4, Name = "Process Admins" };

        /// <summary>
        /// Gets identity number of this <see cref="Target"/> object.
        /// </summary>
        public int TargetId { get; private set; }

        /// <summary>
        /// Gets name of this <see cref="Target"/> object.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        [StringLength(250)]
        public string Description { get; private set; }



        public IList<ActivityTarget> ActivityTargets { get; private set; }

        public IList<ActionTarget> ActionTargets { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Target"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Target() { }

        /// <summary>
        /// Instanciate custom <see cref="Target"/> object.
        /// </summary>
        /// <param name="name">name of Target.</param>
        public Target(string name,string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
