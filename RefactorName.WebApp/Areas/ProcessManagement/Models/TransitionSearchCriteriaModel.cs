using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class TransitionSearchCriteriaModel : SearchCriteria
    {
        [Display(Name = "Process Name")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public int? ProcessId { get; set; }

        //[Display(Name = "User")]
        //public int? User { get; set; }
    }
}