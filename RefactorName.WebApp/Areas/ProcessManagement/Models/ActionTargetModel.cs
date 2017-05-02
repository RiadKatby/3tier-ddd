using RefactorName.Domain.Workflow;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class ActionTargetModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        public Dictionary<string, string> ActionNames { get; set; }

        [Display(Name = "Actions")]
        public string[] Actions { get; set; }

        public Dictionary<string, string> GroupNames { get; set; }

        [Display(Name = "Groups")]
        public string[] Groups { get; set; }

        public int ActionId { get; set; }
        public int TargetId { get; set; }
        public int GroupId { get; set; }
        public string ActionName { get; set; }
        public int ActionTargetId { get; set; }
        public string GroupName { get; set; }
        public string TargetName { get; set; }
    }
    public class ActionTargetAddModel : ActionTargetModel
    {
        

        public ActionTargetAddModel FillDDLsWithActions()
        {
            ActionNames = ActionService.Obj.GetAllActionsDictionary(this.ProcessId);
            return this;
        }
        public ActionTargetAddModel FillDDLsWithGroups()
        {
            ActionNames = GroupService.Obj.GetAllGroupsDictionary(this.ProcessId);
            return this;
        }
    }

}