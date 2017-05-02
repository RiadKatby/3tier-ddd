using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class ActivityIndexModel
    {
        public ActivitySearchCriteriaModel activitySearchCriteriaModel { get; set; }

        public WebGridList<ActivityModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public ActivityIndexModel()
        {
            activitySearchCriteriaModel = new ActivitySearchCriteriaModel();
        }
    }
}