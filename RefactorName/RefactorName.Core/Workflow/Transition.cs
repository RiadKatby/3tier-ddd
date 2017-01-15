using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Transition
    {
        public int TransitionId { get; private set; }

        public int ProcessId { get; private set; }

        public int CurrentStateId { get; private set; }

        public int NextStateId { get; private set; }

        public State CurrentState { get; private set; }

        public State NextState { get; private set; }

        public IList<Activity> Activities { get; private set; }

        public IList<Action> Actions { get; private set; }
    }
}
