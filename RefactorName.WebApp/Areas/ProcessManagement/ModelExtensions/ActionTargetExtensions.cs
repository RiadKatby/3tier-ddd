using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ActionTargetExtensions
    {
        public static ActionTargetAddModel ToModel(this ActionTarget entity)
        {
            return new ActionTargetAddModel
            {
                GroupId = entity.GroupId,
                GroupName = entity.Group.Name,
                ActionId = entity.ActionId,
                ActionName = entity.Action.Name,
                TargetId = entity.TargetId,
                TargetName = entity.Target.Name,
                ActionTargetId = entity.ActionTargetId,
                ProcessId = entity.Action.ProcessId
            };
        }

        public static List<ActionTargetAddModel> ToModels(this IQueryResult<ActionTarget> actionTargets)
        {
            var result = from c in actionTargets.Items select c.ToModel();
            return result.ToList();
        }
    }
}