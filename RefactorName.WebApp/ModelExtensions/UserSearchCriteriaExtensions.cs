using RefactorName.Core;
using RefactorName.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.Web
{
    public static class UserSearchCriteriaExtensions
    {
        public static UserSearchCriteria ToEntity(this UserSearchCriteriaModel model)
        {
            return new UserSearchCriteria
            {
                FullName = model.FullName,
                Mobile = model.Mobile,
                RoleName = model.RoleName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = model.IsActive,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection
            };
        }
    }
}