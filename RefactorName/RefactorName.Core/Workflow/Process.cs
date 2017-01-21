using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Process
    {
        public int ProcessId { get; private set; }
        
        public int UserId { get; private set; }

        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Gets all <see cref="User"/>s whom can change the <see cref="Process"/> itself.
        /// </summary>
        [Associated]
        public IList<User> Admins { get; private set; }

        /// <summary>
        /// Gets all <see cref="Group"/>s of <see cref="User"/>s whom can take <see cref="Action"/>s on this <see cref="Process"/>.
        /// </summary>
        [Owned]
        public IList<Group> Groups { get; private set; }

        /// <summary>
        /// Gets all <see cref="Action"/>s which could be taken against this <see cref="Process"/> object.
        /// </summary>
        [Owned]
        public IList<Action> Actions { get; private set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s which could be executed as a result of <see cref="Action"/> taken or <see cref="Transition"/> followed.
        /// </summary>
        [Owned]
        public IList<Activity> Activities { get; private set; }

        /// <summary>
        /// Gets all data <see cref="Field"/>s definition which could be filled up within <see cref="Request"/> throught out this <see cref="Process"/>.
        /// </summary>
        [Owned]
        public IList<Field> Fields { get; private set; }

        /// <summary>
        /// Gets all <see cref="State"/>s which could be 
        /// </summary>
        [Owned]
        public IList<State> States { get; private set; }

        [Owned]
        public IList<Transition> Transitions { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Process"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Process() { } 
    }
}
