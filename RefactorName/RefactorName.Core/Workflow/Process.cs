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
    /// A Process is the collection of all other data that is unique to a group of users and how they want their <see cref="Request"/>s manipulated.
    /// a Process is infrastructure that is used to define and associate all information such as <see cref="Action"/>, <see cref="State"/>, and/or <see cref="Transition"/> etc.
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Gets identity number of <see cref="Process"/> object.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="Process"/>.
        /// </summary>
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
        /// Gets all <see cref="State"/>s which could be passed by throught out <see cref="Process"/>.
        /// </summary>
        [Owned]
        public IList<State> States { get; private set; }

        /// <summary>
        /// Gets all <see cref="Transition"/> which could be followed by <see cref="Process"/>.
        /// </summary>
        [Owned]
        public IList<Transition> Transitions { get; private set; }
    }
}
