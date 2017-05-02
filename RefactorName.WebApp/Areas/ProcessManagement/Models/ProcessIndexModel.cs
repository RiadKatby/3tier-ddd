using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class ProcessIndexModel
    {
        public ProcessSearchCriteriaModel processSearchCriteriaModel { get; set; }

        public WebGridList<ProcessAddModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public ProcessIndexModel()
        {
            //RoleNames = new Dictionary<string, string>();
            processSearchCriteriaModel = new ProcessSearchCriteriaModel();
        }
    }
}