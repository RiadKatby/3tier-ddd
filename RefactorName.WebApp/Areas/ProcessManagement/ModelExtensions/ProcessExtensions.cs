using RefactorName.Core;
using RefactorName.WebApp.Areas.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RefactorName.Core.Enum;

namespace RefactorName.WebApp
{
    public static class ProcessExtensions
    {
        public static ProcessAddModel ToModel(this Process entity)
        {
            return new ProcessAddModel
            {
                Name = entity.Name,
                ProcessId = entity.ProcessId,
                Admins = (from c in entity.Users select c.ToModel()).ToList(),
                ProcessStatus = Status.Exist
            };
        }

        public static List<ProcessAddModel> ToModels(this IQueryResult<Process> processes)
        {
            var result = from c in processes.Items select c.ToModel();
            return result.ToList();
        }

        public static WebGridList<ProcessAddModel> ToWebGridListModel(this IQueryResult<Process> processes)
        {
            var result = new WebGridList<ProcessAddModel>
            {
                List = processes.ToModels(),
                PageSize = processes.PageSize,
                RowCount = processes.TotalCount,
                PageIndex = processes.PageNumber
            };

            return result;
            //return null;
        }
    }
}