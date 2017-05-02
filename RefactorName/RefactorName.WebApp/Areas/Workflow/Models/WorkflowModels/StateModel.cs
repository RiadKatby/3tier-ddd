using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// A State is a position in the <see cref="Process"/> that a given <see cref="Request"/> can be in at any given moment. 
    /// </summary>
    public class StateModel
    {
        /// <summary>
        /// Gets identity number of <see cref="State"/> object.
        /// </summary>
        public int StateId { get;  set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="State"/>.
        /// </summary>
        public int ProcessId { get;  set; }

        /// <summary>
        /// Gets idenitity number of the <see cref="StateType"/> that this <see cref="State"/> belong to.
        /// </summary>
        public int StateTypeId { get;  set; }

        /// <summary>
        /// Gets the name of this <see cref="State"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get;  set; }

        /// <summary>
        /// Gets description of this <see cref="State"/>.
        /// </summary>
        [StringLength(255)]
        public string Description { get;  set; }

        /// <summary>
        /// Gets <see cref="StateType"/> object that determin the group of this <see cref="State"/>.
        /// </summary>
        public StateTypeModel StateType { get;  set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s that must be executed when a <see cref="Request"/> enters this <see cref="State"/>.
        /// </summary>
        public IList<ActivityModel> Activities { get;  set; }

        /// <summary>
        /// Gets all <see cref="Field"/>s that need to be filled up when this <see cref="State"/> is entered.
        /// </summary>
        //public IList<StateField> Fields { get;  set; }

        public IList<TransitionModel> Outgoing { get;  set; }

        public IList<TransitionModel> Ingoing { get;  set; }

        /// <summary>
        /// Instanciate empty <see cref="State"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public StateModel()
        {
            this.Activities = new List<ActivityModel>();
            this.Outgoing = new List<TransitionModel>();
            this.Ingoing = new List<TransitionModel>();
        }

        
    }
}
