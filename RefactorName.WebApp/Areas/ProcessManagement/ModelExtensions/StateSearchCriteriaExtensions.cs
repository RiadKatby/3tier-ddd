using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Areas.Backend.ModelExtensions
{
    public static class StateSearchCriteriaExtensions
    {
        public static StateSearchCriteria ToEntity(this StateSearchCriteriaModel model)
        {
            return new StateSearchCriteria
            {
                ProcessId = model.ProcessId,
                Name = model.Name,
                Description = model.Description,
                StateTypeId = model.StateTypeId,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection
            };
        }
    }
}