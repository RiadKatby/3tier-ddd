using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Models
{
    public class ErrorModel : HandleErrorInfo
    {
        public string ErrorTitle { get; internal set; }
        public string Description { get; internal set; }
        public string EntityName { get; internal set; }
        public object EntityId { get; internal set; }
        public string PermissionCode { get; internal set; }
        public string UserName { get; internal set; }
        public string ErrorType { get; internal set; }
        public string ParamName { get; internal set; }
        public object ParamValue { get; internal set; }

        public ErrorModel(Exception exception, string controllerName, string actionName)
            : base(exception, controllerName, actionName)
        {

        }
    }
}