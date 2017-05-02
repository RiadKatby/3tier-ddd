using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class TransitionIndexModel
    {
        public TransitionSearchCriteriaModel transitionSearchCriteriaModel { get; set; }

        public WebGridList<TransitionModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public TransitionIndexModel()
        {
            //RoleNames = new Dictionary<string, string>();
            transitionSearchCriteriaModel = new TransitionSearchCriteriaModel();
            CurrentState = new Dictionary<int, string>();
            NextState = new Dictionary<int, string>();
        }        

        public Dictionary<int, string> CurrentState { get; set; }
        public Dictionary<int, string> NextState { get; set; }
    }
}