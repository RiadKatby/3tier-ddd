using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class ProcessSearchCriteriaExtensions
    {
        public static ProcessSearchCriteria ToEntity(this ProcessSearchCriteriaModel model)
        {
            return new ProcessSearchCriteria
            {
                Name = model.Name,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection
            };
        }
    }
}