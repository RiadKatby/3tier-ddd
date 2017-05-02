using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.Mvc;

namespace MvcHtmlHelpers
{
    public static class HtmlHelperExt
    {
        internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        internal static bool EvalBoolean(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToBoolean(htmlHelper.ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key, string format)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key, format), CultureInfo.CurrentCulture);
        }

        internal static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            Debug.Assert(tagBuilder != null);
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }
    }
}