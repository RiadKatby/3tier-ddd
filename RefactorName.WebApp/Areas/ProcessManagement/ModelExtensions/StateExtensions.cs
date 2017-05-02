using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class StateExtensions
    {
        public static StateAddModel ToModel(this State entity)
        {
            return new StateAddModel
            {
                Name = entity.Name,
                Description = entity.Description,
                //StateTypeName = entity.StateType.Name,
                ProcessName = entity.Process.Name,
                ProcessId = entity.ProcessId,
                StateTypeId = entity.StateTypeId,
                Activities = (from c in entity.Activities.ToList() select c.ToModel()).ToList(),
                StateId = entity.StateId
            };
        }

        public static List<StateAddModel> ToModels(this IQueryResult<State> states)
        {
            var result = from c in states.Items select c.ToModel();
            return result.ToList();
        }
        public static List<StateAddModel> ToModels(this List<State> states)
        {
            var result = from c in states select c.ToModel();
            return result.ToList();
        }
        public static WebGridList<StateModel> ToWebGridListModel(this IQueryResult<State> states)
        {
            //var result = new WebGridList<StateModel>
            //{
            //    List = states.ToModels(),
            //    PageSize = states.PageSize,
            //    RowCount = states.TotalCount,
            //    PageIndex = states.PageNumber
            //};

            //return result;
            return null;
        }
    }
}