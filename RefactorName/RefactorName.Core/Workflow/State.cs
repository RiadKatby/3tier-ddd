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
    /// A State is a position in the <see cref="Process"/> that a given <see cref="Request"/> can be in at any given moment. 
    /// </summary>
    public class State
    {
        /// <summary>
        /// Gets identity number of <see cref="State"/> object.
        /// </summary>
        public int StateId { get;  set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="State"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets idenitity number of the <see cref="StateType"/> that this <see cref="State"/> belong to.
        /// </summary>
        public int StateTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="State"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Gets description of this <see cref="State"/>.
        /// </summary>
        [StringLength(255)]
        public string Description { get; private set; }

        /// <summary>
        /// Gets <see cref="StateType"/> object that determin the group of this <see cref="State"/>.
        /// </summary>
        [Associated]
        public StateType StateType { get; private set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s that must be executed when a <see cref="Request"/> enters this <see cref="State"/>.
        /// </summary>
        [Associated]
        public IList<Activity> Activities { get; private set; }

        /// <summary>
        /// Gets all <see cref="Field"/>s that need to be filled up when this <see cref="State"/> is entered.
        /// </summary>
        [Owned]
        public IList<StateField> Fields { get; private set; }

        public IList<Transition> Outgoing { get; private set; }

        public IList<Transition> Ingoing { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="State"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public State()
        {
            this.Activities = new List<Activity>();
            this.Outgoing = new List<Transition>();
            this.Ingoing = new List<Transition>();
        }

        /// <summary>
        /// Instanciate custom <see cref="Transition"/> object.
        /// </summary>
        /// <param name="name">name of <see cref="State"/>.</param>
        /// <param name="description">description of <see cref="State"/>.</param>
        /// <param name="stateType">type of <see cref="State"/>.</param>
        public State(string name, string description, StateType stateType)
            : this()
        {
            this.Name = name;
            this.Description = description;

            this.StateType = stateType;
            this.StateTypeId = stateType.StateTypeId;
        }

        /// <summary>
        /// Add <see cref="Activity"/> that would be executed when this <see cref="State"/> is entered.
        /// </summary>
        /// <param name="activity"><see cref="Activity"/> object to be added.</param>
        /// <returns>Current instance of <see cref="State"/> object.</returns>
        public State AddActivity(Activity activity)
        {
            this.Activities.Add(activity);

            return this;
        }

        /// <summary>
        /// Delete <see cref="Activity"/> from those which will be executed when this <see cref="State"/> is entered.
        /// </summary>
        /// <param name="activity"><see cref="Activity"/> object to delete.</param>
        /// <returns>Current instance of <see cref="State"/> object.</returns>
        public State DeleteActivity(Activity activity)
        {
            this.Activities.Remove(activity);

            return this;
        }

        /// <summary>
        /// Add <see cref="Field"/> object that need to be filled up when this <see cref="State"/> is entered.
        /// </summary>
        /// <param name="field"><see cref="Field"/> info.</param>
        /// <param name="isRequired">flag determine that this <see cref="Field"/> is required.</param>
        /// <param name="isEditable">flag determine that this <see cref="Field"/> is editable.</param>
        /// <returns>Current instance of <see cref="State"/> object.</returns>
        public State AddField(Field field, bool isRequired, bool isEditable)
        {
            var stateField = new StateField(field, isRequired, isEditable);
            this.Fields.Add(stateField);

            return this;
        }

        /// <summary>
        /// Delete <see cref="StateField"/> from this <see cref="State"/>.
        /// </summary>
        /// <param name="stateFiled"><see cref="StateField"/> which will be deleted.</param>
        /// <returns>Current instance of <see cref="State"/> object.</returns>
        public State DeleteField(StateField stateFiled)
        {
            this.Fields.Remove(stateFiled);

            return this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
