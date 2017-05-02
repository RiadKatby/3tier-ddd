using RefactorName.Core;
using RefactorName.Core.SearchEntities;
using RefactorName.WebApp.Areas.Backend.Models;
using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class UserSearchCriteriaExtensions
    {
        public static UserSearchCriteria ToEntity(this UserSearchCriteriaModel model)
        {
            return new UserSearchCriteria
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                //RoleName = model.RoleName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = model.IsActive,
                //IsActiveDirectory = model.IsADUser,
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection,
                Group = model.Group,
                GroupId = model.GroupId
            };
        }
    }
}