using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    /// <summary>
    /// group of <see cref="State"/>s.
    /// </summary>
    public class StateType
    {
        /// <summary>
        /// Should only be one per process.This state is the state into which a new Request is placed when it is created.
        /// </summary>
        public static readonly StateType Start = new StateType { StateTypeId = 1, Name = "Start" };

        /// <summary>
        /// A regular state with no special designation.
        /// </summary>
        public static readonly StateType Normal = new StateType { StateTypeId = 2, Name = "Normal" };

        /// <summary>
        /// A state signifying that any Request in this state have completed normally.
        /// </summary>
        public static readonly StateType Complete = new StateType { StateTypeId = 3, Name = "Complete" };

        /// <summary>
        /// A state signifying that any Request in this state has been denied (e.g.never got started and will not be worked on).
        /// </summary>
        public static readonly StateType Denied = new StateType { StateTypeId = 4, Name = "Denied" };

        /// <summary>
        /// A state signifying that any Request in this state has been cancelled(e.g.work was started but never completed).
        /// </summary>
        public static readonly StateType Cancelled = new StateType { StateTypeId = 5, Name = "Cancelled" };

        /// <summary>
        /// Gets identity number of <see cref="StateType"/> object.
        /// </summary>
        public int StateTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="StateType"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="StateType"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public StateType() { }

        /// <summary>
        /// Instanciate custom <see cref="StateType"/> object.
        /// </summary>
        /// <param name="name">name of <see cref="StateType"/>.</param>
        public StateType(string name)
        {
            this.Name = name;
        }
    }
}
