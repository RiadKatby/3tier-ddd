using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class TransitionSearchCriteriaExtensions
    {
        public static TransitionSearchCriteria ToEntity(this TransitionSearchCriteriaModel model)
        {
            return new TransitionSearchCriteria
            {
                ProcessId = model.ProcessId,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection
            };
        }
    }
}