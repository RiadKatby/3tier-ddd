using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class ActionModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        [Display(Name = "Transition Name")]
        public int TransitionId { get; set; }

        [Display(Name = "Action Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int ActionId { get; set; }

        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Display(Name = "Action Type")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public int ActionTypeID { get; set; }

        [Display(Name = "Action Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Action Description")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(255, ErrorMessage = "It should be no more than 100 characters")]
        public string Description { get; set; }

        [Display(Name = "Action Type")]
        public string ActionTypeName { get; set; }

        public int Order { get; set; }
    }    
    public class ActionAddModel : ActionModel
    {
        public WebGridList<ActionAddModel> Items { get; set; }
    }
    
}