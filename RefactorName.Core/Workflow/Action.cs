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
    /// Actions are things a user can perform upon a Request. which can cause the Request to go to the next state
    /// </summary>
    public class Action
    {
        public static readonly Action Submit = new Action("Submit Application", "Submitting new application", ActionType.Approve) { ActionId = 1 };

        /// <summary>
        /// Gets identity number of <see cref="Action"/> object.
        /// </summary>
        public int ActionId { get;  set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Action"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        public Process Process { get; private set; }

        /// <summary>
        /// Gets idenitity number of the <see cref="ActionType"/> that this <see cref="Action"/> belong to.
        /// </summary>
        public int ActionTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="Action"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set;}

        /// <summary>
        /// Gets description of this <see cref="Action"/>.
        /// </summary>
        [StringLength(255)]
        public string Description { get; private set; }

        /// <summary>
        /// Gets <see cref="ActionType"/> object that determin the group of this <see cref="Action"/>.
        /// </summary>
        [Associated]
        public ActionType ActionType { get; private set; }

        /// <summary>
        /// Gets all <see cref="Transition"/>s that may be followed as a result of performing this <see cref="Action"/>.
        /// </summary>
        [Associated]
        public IList<Transition> Transitions { get; private set; }

        //[Associated]
        public IList<ActionTarget> ActionTargets { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Action"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Action()
        {
        }

        /// <summary>
        /// Instanciate custom <see cref="Action"/> object.
        /// </summary>
        /// <param name="name">name of action</param>
        /// <param name="description">description of action</param>
        /// <param name="actionType">actionType of action.</param>
        public Action(string name, string description, ActionType actionType):this()
        {
            this.Name = name;
            this.Description = description;

            this.ActionType = actionType;
            this.ActionTypeId = actionType.ActionTypeId;            
        }

        /// <summary>
        /// Modify <see cref="Action"/> object.
        /// </summary>
        /// <param name="name">new name of action.</param>
        /// <param name="description">new description of action.</param>
        /// <param name="actionType">new actionType of action.</param>
        /// <returns>Current instance of <see cref="Action"/> object.</returns>
        public Action Update(string name, string description, ActionType actionType)
        {
            this.Name = name;
            this.Description = description;

            this.ActionType = actionType;
            this.ActionTypeId = actionType.ActionTypeId;

            return this;
        }

        public override bool Equals(object obj)
        {
            Action entity = obj as Action;

            if (entity == null) return false;
            if (entity.ActionId != this.ActionId) return false;

            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public Action addActionTarget(ActionTarget actionTarget)
        {
            this.ActionTargets.Add(actionTarget);
            return this;
        }
    }
}
