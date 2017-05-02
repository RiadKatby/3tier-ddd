using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class StateSearchCriteriaModel : SearchCriteria
    {
        public int? ProcessId { get; set; }

        [Display(Name = "State Name")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "State Description")]
        [StringLength(205, ErrorMessage = "It should be no more than 250 characters")]
        public string Description { get; set; }

        [Display(Name = "State Tyep")]
        public int? StateTypeId { get; set; }
    }
}