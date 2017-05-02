using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// A Process is the collection of all other data that is unique to a group of users and how they want their <see cref="Request"/>s manipulated.
    /// a Process is infrastructure that is used to define and associate all information such as <see cref="Action"/>, <see cref="State"/>, and/or <see cref="Transition"/> etc.
    /// </summary>
    [Serializable]
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

        //public IList<User> Admins { get; private set; }
        [Associated]
        public IList<User> Users { get; private set; }

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

        /// <summary>
        /// Instanciate empty <see cref="Process"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Process()
        {
            States = new List<State>();
            Groups = new List<Group>();
            Users = new List<User>();
            Actions = new List<Action>();
            Activities = new List<Activity>();
            Transitions = new List<Transition>();
            Fields = new List<Field>();
        }

        /// <summary>
        /// Instanciate empty <see cref="Process"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        /// <param name="name"></param>
        public Process(string name, int? userId) : this()
        {
            this.Name = name;
            //States = new List<State>();
        }

        // Methods

        public Process AddState(State state)
        {
            for (int i = 0; i < this.States.Count; i++)
            {
                if (this.States[i].StateId == state.StateId)
                {
                    this.States[i] = state;
                    return this;
                }
            }

            this.States.Add(state);
            return this;
        }

        public Process AddGroup(Group group)
        {
            for(int i=0;i<this.Groups.Count;i++)
            {
                if(this.Groups[i].GroupId==group.GroupId)
                {
                    this.Groups[i] = group;
                    return this;
                }
            }

            this.Groups.Add(group);
            return this;
        }

        public Process AddAction(Action action)
        {
            for (int i = 0; i < this.Actions.Count; i++)
            {
                if (this.Actions[i].ActionId == action.ActionId)
                {
                    this.Actions[i] = action;
                    return this;
                }
            }

            this.Actions.Add(action);
            return this;
        }

        public Process AddActivity(Activity activity)
        {
            for (int i = 0; i < this.Activities.Count; i++)
            {
                if (this.Activities[i].ActivityId == activity.ActivityId)
                {
                    this.Activities[i] = activity;
                    return this;
                }
            }

            this.Activities.Add(activity);
            return this;
        }

        public Process AddTransition(Transition transition)
        {
            for (int i = 0; i < this.Transitions.Count; i++)
            {
                if (this.Transitions[i].TransitionId == transition.TransitionId)
                {
                    this.Transitions[i] = transition;
                    return this;
                }
            }

            this.Transitions.Add(transition);
            return this;

        }

        public Process AddField(Field field)
        {
            this.Fields.Add(field);
            return this;
        }

        public Process AddAdmin(User admin)
        {
            //var result = (from a in this.Admins where admin.Id == a.Id select a).ToList();
            //if (result.Count == 0)
            //    this.Admins.Add(admin);
            var result = (from a in this.Users where admin.Id == a.Id select a).ToList();
            if (result.Count == 0)
                this.Users.Add(admin);
            return this;
        }
        
        public Process RemoveAdmin(User admin)
        {
            //this.Admins.Remove(admin);
            this.Users.Remove(admin);
            return this;
        }
        public Process UpdateName(string name)
        {
            this.Name = name;
            return this;
        }

        public Process RemoveGroup(Group group)
        {
            this.Groups.Remove(group);
            return this;
        }

        public Process RemoveActivity(Activity activity)
        {
            this.Activities.Remove(activity);
            return this;
        }

        public Process RemoveAction(Action action)
        {
            this.Actions.Remove(action);
            return this;
        }

        public Process RemoveState(State state)
        {
            this.States.Remove(state);
            return this;
        }

        public Process RemoveTransition(Transition transition)
        {
            this.Transitions.Remove(transition);
            return this;
        }
    }
}
