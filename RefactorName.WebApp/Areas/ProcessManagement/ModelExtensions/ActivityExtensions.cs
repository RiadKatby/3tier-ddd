using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ActivityAddExtensions
    {
        public static ActivityAddModel ToModel(this Activity entity)
        {
            return new ActivityAddModel
            {
                Name = entity.Name,
                Description = entity.Description,
                ActivityTypeName = entity.ActivityType.Name,
                ActivityTypeID=entity.ActivityType.ActivityTypeId,
                ProcessId = entity.ProcessId,
                //ProcessName = entity.Process.Name,
                ActivityId = entity.ActivityId
            };
        }

        public static List<ActivityAddModel> ToModels(this IQueryResult<RefactorName.Core.Activity> activitys)
        {
            var result = from c in activitys.Items select c.ToModel();
            return result.ToList();
        }
        public static List<ActivityAddModel> ToModels(this List<Activity> activities)
        {
            var result = from c in activities select c.ToModel();
            return result.ToList();
        }

        public static WebGridList<ActivityAddModel> ToWebGridListModel(this IQueryResult<RefactorName.Core.Activity> activitys)
        {
            var result = new WebGridList<ActivityAddModel>
            {
                List = activitys.ToModels(),
                PageSize = activitys.PageSize,
                RowCount = activitys.TotalCount,
                PageIndex = activitys.PageNumber
            };

            return result;
        }
    }
}