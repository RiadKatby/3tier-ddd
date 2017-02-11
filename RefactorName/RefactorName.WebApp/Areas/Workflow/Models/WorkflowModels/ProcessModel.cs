using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// A Process is the collection of all other data that is unique to a group of users and how they want their <see cref="Request"/>s manipulated.
    /// a Process is infrastructure that is used to define and associate all information such as <see cref="Action"/>, <see cref="State"/>, and/or <see cref="Transition"/> etc.
    /// </summary>
    public class ProcessModel
    {
        /// <summary>
        /// Gets identity number of <see cref="ProcessModel"/> object.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Gets the name of this <see cref="ProcessModel"/>.
        /// </summary>
        [Display(Name = "Process Name")]
        [Required, StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets all <see cref="User"/>s whom can change the <see cref="Process"/> itself.
        /// </summary>
        public IList<UserModel> Admins { get; set; }

        /// <summary>
        /// Gets all <see cref="Group"/>s of <see cref="User"/>s whom can take <see cref="Action"/>s on this <see cref="Process"/>.
        /// </summary>
        public IList<GroupModel> Groups { get; set; }

        /// <summary>
        /// Gets all <see cref="Action"/>s which could be taken against this <see cref="Process"/> object.
        /// </summary>
        public IList<ActionModel> Actions { get; set; }

        /// <summary>
        /// Gets all <see cref="Activity"/>s which could be executed as a result of <see cref="Action"/> taken or <see cref="Transition"/> followed.
        /// </summary>
        public IList<ActivityModel> Activities { get; set; }

        /// <summary>
        /// Gets all data <see cref="FieldModel"/>s definition which could be filled up within <see cref="Request"/> throught out this <see cref="Process"/>.
        /// </summary>
        public List<FieldModel> Fields { get; set; }

        /// <summary>
        /// Gets all <see cref="StateModel"/>s which could be passed by throught out <see cref="Process"/>.
        /// </summary>
        public IList<StateModel> States { get; set; }

        /// <summary>
        /// Gets all <see cref="Transition"/> which could be followed by <see cref="Process"/>.
        /// </summary>
        public IList<TransitionModel> Transitions { get; set; }

        /// <summary>
        /// Instanciate empty <see cref="Process"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public ProcessModel()
        {
            States = new List<StateModel>();
            Groups = new List<GroupModel>();
            Admins = new List<UserModel>();
            Actions = new List<ActionModel>();
            Activities = new List<ActivityModel>();
            Transitions = new List<TransitionModel>();
            Fields = new List<FieldModel>();
        }

    }
}
