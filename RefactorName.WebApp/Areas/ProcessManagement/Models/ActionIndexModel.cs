using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class ActionIndexModel
    {
        public ActionSearchCriteriaModel actionSearchCriteriaModel { get; set; }

        public WebGridList<ActionModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public ActionIndexModel()
        {
            actionSearchCriteriaModel = new ActionSearchCriteriaModel();
        }
    }
}