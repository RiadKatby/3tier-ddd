using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// The <see cref="Action"/>s that can be performed at any given time upon a <see cref="Request"/>.
    /// </summary>
    public class RequestAction
    {
        /// <summary>
        /// Gets identity number of <see cref="RequestAction"/> object.
        /// </summary>
        public int RequestActionId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Request"/> which has this <see cref="RequestAction"/>.
        /// </summary>
        public int RequestId { get; private set; }

        public int ActionId { get; private set; }

        public int TransitionId { get; private set; }

        public bool IsActive { get; set; }

        public bool IsComplete { get; set; }

        public Action Action { get; private set; }

        public Request Request { get; private set; }

        public Transition Transition { get; private set; }

        public RequestAction(Action action, Transition transition)
        {
            this.Action = action;
            this.ActionId = action.ActionId;

            this.Transition = transition;
            this.TransitionId = transition.TransitionId;

            this.IsActive = true;
            this.IsComplete = false;
        }

        public void Complete()
        {
            this.IsActive = false;
            this.IsComplete = true;
        }
    }
}
