using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// A Transition is a path between two <see cref="State"/>s that shows how a <see cref="Request"/> can travel between them.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Gets identity number of <see cref="Transition"/> object.
        /// </summary>
        public int TransitionId { get; set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Transition"/>.
        /// </summary>
        public int ProcessId { get; private set; }


        public Process Process { get; private set; }
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
        [Associated]
        [ForeignKey("CurrentStateId")]
        public State CurrentState { get; private set; }

        /// <summary>
        /// Gets the <see cref="State"/> that this <see cref="Transition"/> will take a <see cref="Request"/> to it.
        /// </summary>
        [Associated]
        [ForeignKey("NextStateId")]
        public State NextState { get; private set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s that must be executed when a <see cref="Request"/> followed this <see cref="Transition"/>.
        /// </summary>
        /// 
        [Associated]
        public IList<Activity> Activities { get; private set; }

        /// <summary>
        /// Gets all <see cref="Action"/>s that could be cuase this <see cref="Transition"/> to be followed.
        /// </summary>
        [Associated]
        public IList<Action> Actions { get; private set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Instanciate empty <see cref="Transition"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Transition()
        {
            this.Activities = new List<Activity>();
            this.Actions = new List<Action>();
        }

        /// <summary>
        /// Instanciate custom <see cref="Transition"/> object.
        /// </summary>
        /// <param name="currentState">the starting <see cref="State"/> of this <see cref="Transition"/>.</param>
        /// <param name="nextState">the ending <see cref="State"/> of this <see cref="Transition"/>.</param>
        public Transition(State currentState, State nextState)
            : this()
        {

            this.CurrentState = currentState;
            this.CurrentStateId = currentState.StateId;

            this.NextState = nextState;
            this.NextStateId = nextState.StateId;
        }

        public Transition AddAction(Action action)
        {
            var result = (from transitionActions in this.Actions where transitionActions.ActionId == action.ActionId select transitionActions).ToList();
            if (result.Count == 0)
                this.Actions.Add(action);
            return this;
        }

        /// <summary>
        /// Add <see cref="Activity"/> that would be executed when this <see cref="Transition"/> is followed.
        /// </summary>
        /// <param name="activity"><see cref="Activity"/> object to be added.</param>
        /// <returns>Current instance of <see cref="Transition"/> object.</returns>
        public Transition AddActivity(Activity activity)
        {
            var result = (from transitionActiviyt in this.Activities where transitionActiviyt.ActivityId == activity.ActivityId select transitionActiviyt).ToList();
            if (result.Count == 0)
                this.Activities.Add(activity);

            return this;
        }

        /// <summary>
        /// Delete <see cref="Activity"/> from those which will be executed when this <see cref="Transition"/> is going to be followed.
        /// </summary>
        /// <param name="activity"><see cref="Activity"/> object to delete.</param>
        /// <returns>Current instance of <see cref="Transition"/> object.</returns>
        public Transition DeleteActivity(Activity activity)
        {
            this.Activities.Remove(activity);

            return this;
        }

        public override bool Equals(object obj)
        {
            Transition entity = obj as Transition;

            if (entity == null) return false;
            if (entity.TransitionId != this.TransitionId) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}] -> [{1}]", CurrentState.Name, NextState.Name);
        }

        public Transition RemoveActivity(Activity activity)
        {
            this.Activities.Remove(activity);
            return this;
        }

        public Transition RemoveAction(Action action)
        {
            this.Actions.Remove(action);
            return this;
        }

        public Transition Update(State currentState, State nextState)
        {
            this.CurrentState = currentState;
            //if (this.CurrentState != null)
            //{
            //    if (this.CurrentState.StateId != currentState.StateId)
            //    {
            //        this.CurrentStateId = currentState.StateId;
            //        this.CurrentState = currentState;
            //    }
            //}
            this.NextState = nextState;
            //if (this.NextState != null)
            //{
            //    if (this.NextState.StateId != nextState.StateId)
            //    {
            //        this.NextStateId = nextState.StateId;
            //        this.NextState = nextState;
            //    }
            //}

            return this;
        }

        public Transition RemoveActions(List<Action> actions)
        {
            foreach (Action action in actions)
                this.Actions.Remove(action);
            return this;
        }
        public Transition RemoveActivities(List<Activity> activities)
        {
            foreach (Activity activity in activities)
                this.Activities.Remove(activity);
            return this;
        }
    }
}
