using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public class ActionSearchCriteriaModel : SearchCriteria
    {
        public int? ProcessId { get; set; }

        [Display(Name = "Action Name")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Action Description")]
        [StringLength(205, ErrorMessage = "It should be no more than 250 characters")]
        public string Description { get; set; }

        [Display(Name = "Action Tyep")]
        public int? ActionTypeId { get; set; }

        public int? TransitionId { get; set; }
    }
}