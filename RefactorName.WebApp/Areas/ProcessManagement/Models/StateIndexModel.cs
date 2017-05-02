using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class StateIndexModel
    {
        public StateSearchCriteriaModel stateSearchCriteriaModel { get; set; }

        public WebGridList<StateModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public StateIndexModel()
        {
            stateSearchCriteriaModel = new StateSearchCriteriaModel();
        }
    }
}