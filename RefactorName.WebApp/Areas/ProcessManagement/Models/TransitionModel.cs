using RefactorName.Core;
using RefactorName.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class TransitionModel
    {
        [Display(Name = "Process")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int ProcessId { get; set; }

        [Display(Name = "Process Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string ProcessName { get; set; }


        [Display(Name = "Current State")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int CurrentStateId { get; set; }


        [Display(Name = "Next State")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int NextStateId { get; set; }

        [Display(Name = "Current Sate Name")]
        public string CurrentStateName { get; set; }



        [Display(Name = "Next Sate Name")]
        public string NextStateName { get; set; }

        public int TransitionId { get; set; }

        public Dictionary<string, string> StateNames { get; set; }

        public string[] States { get; set; }

        public Dictionary<string, string> ActionNames { get; set; }

        [Display(Name = "Actions")]
        public string[] Actions { get; set; }

        public Dictionary<string, string> ActivityNames { get; set; }

        [Display(Name = "Activities")]
        public string[] Activities { get; set; }

        public List<ActivityAddModel> TransitionActivities{get;set;}
        public List<ActionAddModel> TransitionActions { get; set; }
        public StateAddModel CurrentState { get; set; }
        public StateAddModel NextState { get; set; }

    }
    public class TransitionAddModel : TransitionModel
    {
        public TransitionAddModel FillDDLsWithStates()
        {
            StateNames = StateService.Obj.GetAllStatesDictionary(ProcessId);
            return this;
        }
        public TransitionAddModel FillDDLsWithActions()
        {
            ActionNames = ActionService.Obj.GetAllActionsDictionary(ProcessId);
            return this;
        }
        public TransitionAddModel FillDDLsWithActivities()
        {
            ActivityNames = ActivityService.Obj.GetAllActivitiesDictionary(ProcessId);
            return this;
        }
    }
}