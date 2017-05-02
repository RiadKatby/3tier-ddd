using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class TransitionExtensions
    {
        public static TransitionAddModel ToModel(this Transition entity)
        {
            return new TransitionAddModel
            {
                CurrentStateName = entity.CurrentState.Name,
                CurrentState = entity.CurrentState.ToModel(),
                NextState = entity.NextState.ToModel(),
                NextStateName = entity.NextState.Name,
                ProcessName = entity.Process.Name,
                TransitionId = entity.TransitionId,
                TransitionActions = entity.Actions.ToList().ToModels(),
                TransitionActivities = entity.Activities.ToList().ToModels(),
                ProcessId = entity.ProcessId,
                ActionNames = new Dictionary<string, string>(),
                ActivityNames = new Dictionary<string, string>(),
                CurrentStateId = entity.CurrentStateId,
                NextStateId = entity.NextStateId
            };
        }

        public static List<TransitionAddModel> ToModels(this IQueryResult<Transition> transitions)
        {
            var result = from c in transitions.Items select c.ToModel();
            return result.ToList();
        }

        public static List<TransitionAddModel> ToModels(this List<Transition> transitions)
        {
            var result = from c in transitions select c.ToModel();
            return result.ToList();
        }

        public static WebGridList<TransitionModel> ToWebGridListModel(this IQueryResult<Transition> transitions)
        {
            //var result = new WebGridList<TransitionModel>
            //{
            //    List = transitions.ToModels(),
            //    PageSize = transitions.PageSize,
            //    RowCount = transitions.TotalCount,
            //    PageIndex = transitions.PageNumber
            //};

            //return result;
            return null;
        }
    }
}