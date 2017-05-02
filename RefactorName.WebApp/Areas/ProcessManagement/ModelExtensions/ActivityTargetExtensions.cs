using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public  static class ActivityTargetExtensions
    {
        public static ActivityTargetAddModel ToModel(this ActivityTarget entity)
        {
            return new ActivityTargetAddModel
            {
                GroupId = entity.GroupId,
                GroupName=entity.Group.Name,
                ActivityId = entity.ActivityId,
                ActivityName = entity.Activity.Name,
                TargetId = entity.TargetId,
                TargetName = entity.Target.Name,
                ActivityTargetId = entity.ActivityTargetId,
                ProcessId = entity.Activity.ProcessId
            };
        }

        public static List<ActivityTargetAddModel> ToModels(this IQueryResult<ActivityTarget> activityTargets)
        {
            var result = from c in activityTargets.Items select c.ToModel();
            return result.ToList();
        }
    }
}