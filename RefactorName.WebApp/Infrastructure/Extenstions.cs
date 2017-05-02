using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace RefactorName.WebApp
{
    public static class Extenstions
    {
        public static void PopulateIn(this ValidationException valEx, ModelStateDictionary modelState)
        {
            foreach (var item in valEx.ValidationResults)
                modelState.AddModelError(item.MemberNames.First(), item.ErrorMessage);
        }
    }
}