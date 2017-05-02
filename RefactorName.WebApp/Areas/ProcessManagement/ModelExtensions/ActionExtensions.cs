using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ActionAddExtensions
    {
        public static ActionAddModel ToModel(this RefactorName.Core.Action entity)
        {
            return new ActionAddModel
            {
                Name = entity.Name,
                Description = entity.Description,
                ActionTypeName = entity.ActionType.Name,
                //ProcessName = entity.Process.Name,
                ActionId = entity.ActionId,
                ProcessId = entity.ProcessId,
                ActionTypeID = entity.ActionTypeId
            };
        }

        public static List<ActionAddModel> ToModels(this IQueryResult<RefactorName.Core.Action> actions)
        {
            var result = from c in actions.Items select c.ToModel();
            return result.ToList();
        }
        public static List<ActionAddModel> ToModels(this List<RefactorName.Core.Action> actions)
        {
            var result = from c in actions select c.ToModel();
            return result.ToList();
        }
        public static WebGridList<ActionAddModel> ToWebGridListModel(this IQueryResult<RefactorName.Core.Action> actions)
        {
            var result = new WebGridList<ActionAddModel>
            {
                List = actions.ToModels(),
                PageSize = actions.PageSize,
                RowCount = actions.TotalCount,
                PageIndex = actions.PageNumber
            };

            return result;
        }
    }
}