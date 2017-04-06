using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.Web.Models
{
    public class ErrorModel : HandleErrorInfo
    {
        public string ErrorTitle { get; private set; }

        public ErrorModel(Exception exception, string controllerName, string actionName, string errorTitle)
            : base(exception, controllerName, actionName)
        {
            this.ErrorTitle = errorTitle;
        }
    }
}