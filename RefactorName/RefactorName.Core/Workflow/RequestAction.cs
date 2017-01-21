using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class RequestAction
    {
        public int RequestActionId { get; private set; }

        public int RequestId { get; private set; }

        public int ActionId { get; private set; }

        public int TransitionId { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsComplete { get; private set; }

        public Action Action { get; private set; }

        public Transition Transition { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="RequestAction"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public RequestAction() { }
    }
}
