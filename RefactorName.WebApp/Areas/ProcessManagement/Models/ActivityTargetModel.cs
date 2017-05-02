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
    public class ActivityTargetModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        public Dictionary<string, string> ActivityNames { get; set; }

        [Display(Name = "Activities")]
        public string[] Activities { get; set; }

        public Dictionary<string, string> GroupNames { get; set; }

        [Display(Name = "Groups")]
        public string[] Groups { get; set; }

        public string GroupName { get; set; }
        public string ActivityName { get; set; }
        public string TargetName { get; set; }
        public int ActivityId { get; set; }
        public int TargetId { get; set; }
        public int GroupId { get; set; }
        public int ActivityTargetId { get; set; }
    }
    public class ActivityTargetAddModel : ActivityTargetModel
    {
        public ActivityTargetAddModel FillDDLsWithActivities()
        {
            ActivityNames = ActivityService.Obj.GetAllActivitiesDictionary(this.ProcessId);
            return this;
        }
        public ActivityTargetAddModel FillDDLsWithGroups()
        {
            ActivityNames = GroupService.Obj.GetAllGroupsDictionary(this.ProcessId);
            return this;
        }
    }

}