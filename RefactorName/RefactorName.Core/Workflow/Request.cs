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
    /// Requests are unique to Processes; a Request may only exist in a single Process
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Gets identity number of <see cref="Request"/> object.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets identity number of <see cref="User"/> who made this <see cref="Request"/>.
        /// </summary>
        public int UserRequestedId { get; private set; }

        /// <summary>
        /// Gets identity number of current <see cref="State"/>.
        /// </summary>
        public int CurrentStateId { get; private set; }

        /// <summary>
        /// Gets the title of this <see cref="Request"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Title { get; private set; }

        /// <summary>
        /// Gets date and time of Creation of this <see cref="Request"/>.
        /// </summary>
        public DateTime DateRequested { get; private set; }

        /// <summary>
        /// Gets <see cref="User"/> who created this <see cref="Request"/>.
        /// </summary>
        public User UserRequested { get; private set; }

        /// <summary>
        /// Gets current <see cref="State"/> object.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Gets all highly-variable set of data that pertains to this <see cref="Request"/>.
        /// </summary>
        [Owned]
        public IList<RequestData> Data { get; private set; }

        /// <summary>
        /// Gets all notes entered by <see cref="User"/>s pertaining to this <see cref="Request"/>.
        /// </summary>
        [Owned]
        public IList<RequestNote> Notes { get; private set; }

        /// <summary>
        /// Gets list of Users that are to receive periodic updates about the Request.
        /// </summary>
        [Associated]
        public IList<User> Stakeholders { get; private set; }

        /// <summary>
        /// Gets all physical files that relate to this <see cref="Request"/>.
        /// </summary>
        [Owned]
        public IList<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Gets all <see cref="Action"/>s that user can perform to invoke <see cref="Transition"/>s.
        /// </summary>
        [Owned]
        public IList<RequestAction> RequestActions { get; private set; }

        public Request()
        {
            this.RequestActions = new List<RequestAction>();
        }

        public Request(string title, State initialState)
            : this()
        {
            this.Title = title;
            ChangeState(initialState);
        }

        public void Perform(Action action)
        {
            /// 3. When an Action is submitted, we check the RequestActions. If the submitted Action 
            ///    matches one of the active RequestActions(where IsActive = 1), we set that entry's IsActive = 0 and IsCompleted = 1.
            var activeRequestActions = from x in RequestActions
                                       where action.Equals(x.Action) && x.IsActive
                                       select x;

            List<RequestAction> temp = new List<Workflow.RequestAction>();

            foreach (var activeAction in activeRequestActions)
            {
                activeAction.Complete();
                temp.AddRange(RequestActions.Where(x => activeAction.Transition.Equals(x.Transition)));
            }

            /// 4. After marking the submitted Action as completed, we check all Actions for that Transition.
            ///    If all RequestActions are marked as Completed, then we Deactivate all remaining actions
            ///    (by setting IsActive = 0, e.g.all actions for Transitions that were not matched).
            
            if (temp.All(x => x.IsComplete == true))
            {
                var nextTransition = temp.FirstOrDefault();

                ChangeState(nextTransition.Transition.NextState);
            }
        }

        private void ChangeState(State newState)
        {
            this.CurrentState = newState;
            this.CurrentStateId = newState.StateId;

            foreach (var transition in CurrentState.Outgoing)
                foreach (var action in transition.Actions)
                    RequestActions.Add(new RequestAction(action, transition));
        }

        public override string ToString()
        {
            return string.Format("Title={0}, CurrentState={1}", Title, CurrentState.Name);
        }
    }

    public class Client
    {


        public void Test()
        {

        }
    }
}
