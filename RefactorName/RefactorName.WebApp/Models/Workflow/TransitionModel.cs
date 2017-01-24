using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Models
{
    /// <summary>
    /// A Transition is a path between two <see cref="State"/>s that shows how a <see cref="Request"/> can travel between them.
    /// </summary>
    public class TransitionModel
    {
        /// <summary>
        /// Gets identity number of <see cref="TransitionModel"/> object.
        /// </summary>
        public int TransitionId { get; set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Transition"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets identity number of starting <see cref="State"/> that this <see cref="Transition"/> start from.
        /// </summary>
        public int CurrentStateId { get; private set; }

        /// <summary>
        /// Gets identity number of target <see cref="State"/> that this <see cref="Transition"/> going to.
        /// </summary>
        public int NextStateId { get; private set; }

        /// <summary>
        /// Gets the <see cref="State"/> that this <see cref="Transition"/> start from.
        /// </summary>
        public StateModel CurrentState { get; private set; }

        /// <summary>
        /// Gets the <see cref="State"/> that this <see cref="Transition"/> will take a <see cref="Request"/> to it.
        /// </summary>
        public StateModel NextState { get; private set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s that must be executed when a <see cref="Request"/> followed this <see cref="Transition"/>.
        /// </summary>
        public IList<ActivityModel> Activities { get; private set; }

        /// <summary>
        /// Gets all <see cref="Action"/>s that could be cuase this <see cref="Transition"/> to be followed.
        /// </summary>
        public IList<Action> Actions { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Transition"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public TransitionModel()
        {
            
        }

    }
}
