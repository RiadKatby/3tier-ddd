using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public class ActivitySearchCriteriaModel : SearchCriteria
    {
        [Display(Name = "Process Name")]
        public int? ProcessId { get; set; }

        [Display(Name = "Activity Name")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Activity Description")]
        [StringLength(205, ErrorMessage = "It should be no more than 250 characters")]
        public string Description { get; set; }

        [Display(Name = "Activity Tyep")]
        public int? ActivityTypeId { get; set; }
    }
}