using RefactorName.Core;
using RefactorName.Domain;
using RefactorName.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class StateModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        [Display(Name = "State Type ID")]
        [Required(ErrorMessage = "Please Enter")]
        public int StateTypeId { get; set; }        

        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Please Enter")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "State Description")]
        [Required(ErrorMessage = "Please Enter")]
        [StringLength(255, ErrorMessage = "It should be no more than 100 characters")]
        public string Description { get; set; }

        [Display(Name = "State Type Name")]
        public string StateTypeName { get; set; }

        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        public Dictionary<string, string> ActivityNames { get; set; }

        [Display(Name = "Activities")]
        public List<ActivityAddModel> Activities { get; set; }
        public int StateId { get; set; }
    }
    public class StateAddModel : StateModel
    {
        public StateAddModel FillDDLsWithActivities()
        {
            ActivityNames = ActivityService.Obj.GetAllActivitiesDictionary(this.ProcessId);
            return this;
        }
    }
}