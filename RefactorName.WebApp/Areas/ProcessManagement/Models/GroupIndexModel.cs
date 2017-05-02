using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.Models
{
    public class GroupIndexModel
    {
        public GroupSearchCriteriaModel groupSearchCriteriaModel { get; set; }

        public WebGridList<GroupModel> Items { get; set; }

        //public Dictionary<string, string> RoleNames { get; set; }

        public GroupIndexModel()
        {
            groupSearchCriteriaModel = new GroupSearchCriteriaModel();
        }
    }
}