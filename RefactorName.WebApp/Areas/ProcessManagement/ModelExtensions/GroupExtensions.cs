using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public static class GroupExtensions
    {
        public static GroupAddModel ToModel(this Group entity)
        {
            return new GroupAddModel
            {
                Name = entity.Name,
                ProcessName = entity.Process.Name,
                GroupId = entity.GroupId,
                ProcessId = entity.ProcessId,
                Memebers = (from c in entity.Users.ToList() select c.ToModel()).ToList()
            };
        }

        public static List<GroupAddModel> ToModels(this IQueryResult<Group> groups)
        {
            var result = from c in groups.Items select c.ToModel();
            return result.ToList();
        }

        public static List<GroupAddModel> ToModels(this List<Group> groups)
        {
            var result = from c in groups select c.ToModel();
            return result.ToList();
        }
        public static WebGridList<GroupAddModel> ToWebGridListModel(this IQueryResult<Group> groups)
        {
            var result = new WebGridList<GroupAddModel>
            {
                List = groups.ToModels(),
                PageSize = groups.PageSize,
                RowCount = groups.TotalCount,
                PageIndex = groups.PageNumber
            };

            return result;
        }
    }
}