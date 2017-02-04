using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// Actions are things a user can perform upon a Request. which can cause the Request to go to the next state
    /// </summary>
    public class ActionModel
    {
        //public static readonly Action Submit = new Action("Submit Application", "Submitting new application", ActionType.Approve) { ActionId = 1 };

        /// <summary>
        /// Gets identity number of <see cref="Action"/> object.
        /// </summary>
        public int ActionId { get;  set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Action"/>.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Gets idenitity number of the <see cref="ActionType"/> that this <see cref="Action"/> belong to.
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        /// Gets the name of this <see cref="Action"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; set;}

        /// <summary>
        /// Gets description of this <see cref="Action"/>.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Gets <see cref="ActionType"/> object that determin the group of this <see cref="Action"/>.
        /// </summary>
        public ActionTypeModel ActionType { get; set; }

        /// <summary>
        /// Gets all <see cref="Transition"/>s that may be followed as a result of performing this <see cref="Action"/>.
        /// </summary>
        public IList<TransitionModel> Transitions { get; set; }


    }
}
