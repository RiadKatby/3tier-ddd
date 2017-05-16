using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Infrastructure.Encryption
{
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("q") != null)
            {
                string encryptedQueryString = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.Get("q"));
                try
                {
                    decryptedParameters = Util.RouteValuesFromEncryptedQueryString(encryptedQueryString);
                    
                    ParameterDescriptor[] parameterDescriptors = filterContext.ActionDescriptor.GetParameters();

                    for (int i = 0; i < parameterDescriptors.Length; i++)
                    {
                        string parameterName = parameterDescriptors[i].ParameterName.ToLower();
                        TypeConverter converter = TypeDescriptor.GetConverter(parameterDescriptors[i].ParameterType);
                        object obj = null;
                        if (converter is EnumConverter)
                        {
                            obj = Enum.Parse(parameterDescriptors[i].ParameterType, decryptedParameters[parameterName].ToString());
                        }
                        else if (converter is Int32Converter || converter is StringConverter)
                        {
                            obj = converter.ConvertTo(decryptedParameters[parameterName], parameterDescriptors[i].ParameterType);
                        }
                        else if (converter is TypeConverter)
                        {
                            obj = filterContext.ActionParameters[parameterName];

                            foreach (var property in parameterDescriptors[i].ParameterType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                            {
                                BindAliasAttribute attribute = Attribute.GetCustomAttribute(property, typeof(BindAliasAttribute)) as BindAliasAttribute;
                                if (attribute != null)
                                {
                                    object propValue = decryptedParameters[attribute.Alias.ToLower()];

                                    var conv = TypeDescriptor.GetConverter(property.PropertyType);
                                    object obj1 = conv.ConvertFrom(propValue);
                                    property.SetValue(obj, obj1);
                                }
                            }

                        }


                        filterContext.ActionParameters[parameterName] = obj;
                    }
                }
                catch
                {
                    filterContext.Result = RedirectToPageNotFoundResult(filterContext);
                }

            }
            else
            {
                filterContext.Result = RedirectToPageNotFoundResult(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }

        private ActionResult RedirectToPageNotFoundResult(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // For AJAX requests, we're overriding the returned JSON result with a simple string,
                // indicating to the calling JavaScript code that a redirect should be performed.     

                UrlHelper url = new UrlHelper(filterContext.RequestContext);

                return new JsonResult { Data = new { success = false, url = url.Action("PageNotFound", "Shared") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
                return new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Shared"},
                        { "Action", "PageNotFound" }
                       
                });
        }
    }

}