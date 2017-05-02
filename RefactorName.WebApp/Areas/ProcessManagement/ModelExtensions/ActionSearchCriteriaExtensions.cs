using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ActionSearchCriteriaExtensions
    {
        public static ActionSearchCriteria ToEntity(this ActionSearchCriteriaModel model)
        {
            return new ActionSearchCriteria
            {
                ProcessId = model.ProcessId,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection,
                TransitionId=model.TransitionId
            };
        }
    }
}