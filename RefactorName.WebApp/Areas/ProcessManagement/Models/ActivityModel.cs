using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class ActivityModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        [Display(Name = "Activity Name")]
        public int ActivityId { get; set; }

        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Display(Name = "Activity Type ID")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int ActivityTypeID { get; set; }

        [Display(Name = "Activity Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Activity Description")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(255, ErrorMessage = "It should be no more than 100 characters")]
        public string Description { get; set; }

        [Display(Name = "Activity Type")]
        [StringLength(255, ErrorMessage = "It should be no more than 100 characters")]
        public string ActivityTypeName { get; set; }

        [Display(Name = "Transition Name")]
        public int TransitionId { get; set; }

        [Display(Name = "State Name")]
        public int StateId { get; set; }

    }
    public class ActivityAddModel : ActivityModel
    {
    }
    
}