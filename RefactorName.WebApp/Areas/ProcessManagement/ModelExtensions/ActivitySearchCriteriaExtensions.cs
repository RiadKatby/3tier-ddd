using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ActivitySearchCriteriaExtensions
    {
        public static ActivitySearchCriteria ToEntity(this ActivitySearchCriteriaModel model)
        {
            return new ActivitySearchCriteria
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