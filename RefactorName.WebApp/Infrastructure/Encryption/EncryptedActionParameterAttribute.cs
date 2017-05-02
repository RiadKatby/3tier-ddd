using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using RefactorName.WebApp.Infrastructure.Encryption;

namespace RefactorName.WebApp
{
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
        //    try
        //    {
        //        //decrypt query string values
        //        if (HttpContext.Current.Request.QueryString.Get("q") != null)
        //        {
        //            string encryptedQueryString = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.Get("q"));
        //            decryptedParameters = Util.RouteValuesFromEncryptedQueryString(encryptedQueryString);
        //        }

        //        if (filterContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            //decrypt ajax query string parameters
        //            var encryptedKeys = new List<string>();
        //            foreach (var key in HttpContext.Current.Request.QueryString.AllKeys)
        //            {
        //                if (key != null)
        //                    if (key.StartsWith(StringEncrypter.ControlsEncrypter.Prefix))
        //                        encryptedKeys.Add(key);
        //            }
        //            foreach (var key in encryptedKeys)
        //            {
        //                var oldValue = filterContext.HttpContext.Request.QueryString[key];
        //                decryptedParameters.Add(key.Replace(StringEncrypter.ControlsEncrypter.Prefix, "").ToLower(), StringEncrypter.ControlsEncrypter.Decrypt(oldValue));
        //            }

        //            //decrypt json data
        //            System.IO.Stream reqStream = filterContext.HttpContext.Request.InputStream;
        //            if (reqStream.Length > 0)
        //            {
        //                if (reqStream.CanSeek)
        //                    reqStream.Position = 0;

        //                //now try to read the content as string                    
        //                String data = new System.IO.StreamReader(filterContext.HttpContext.Request.InputStream).ReadToEnd();
        //                JObject jObject = new JObject();
        //                try
        //                {
        //                    jObject = JObject.Parse(data);
        //                }
        //                catch
        //                {
        //                }



        //                decryptedParameters = decryptedParameters.Concat(ExtractEncryptedParametersFromJObject(jObject)).ToDictionary(x => x.Key, x => x.Value);
        //            }
        //        }

        //        MapDecryptedParametersToAction(filterContext, decryptedParameters);
        //    }
        //    catch
        //    {
        //        filterContext.Result = RedirectToPageNotFoundResult(filterContext);
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        //private void MapDecryptedParametersToAction(ActionExecutingContext filterContext, Dictionary<string, object> decryptedParameters)
        //{
        //    if (decryptedParameters.Any())
        //    {
        //        ParameterDescriptor[] parameterDescriptors = filterContext.ActionDescriptor.GetParameters();

        //        for (int i = 0; i < parameterDescriptors.Length; i++)
        //        {
        //            string parameterName = parameterDescriptors[i].ParameterName.ToLower();
        //            if (!decryptedParameters.ContainsKey(parameterName)) continue;
        //            TypeConverter converter = TypeDescriptor.GetConverter(parameterDescriptors[i].ParameterType);
        //            object obj = null;
        //            if (converter is EnumConverter)
        //            {
        //                obj = Enum.Parse(parameterDescriptors[i].ParameterType, decryptedParameters[parameterName].ToString());
        //            }
        //            else if (converter is Int32Converter || converter is StringConverter)
        //            {
        //                obj = converter.ConvertTo(decryptedParameters[parameterName], parameterDescriptors[i].ParameterType);
        //            }
        //            else if (converter is ArrayConverter)
        //            {
        //                var innerType = parameterDescriptors[i].ParameterType.GetElementType();
        //                var ddd = decryptedParameters[parameterName] as string[];

        //                System.Reflection.MethodInfo targetMethod = typeof(dummy).GetMethod("GetGenericListImpl").MakeGenericMethod(innerType);
        //                IList theList = (IList)targetMethod.Invoke(null, new object[] { ddd });
        //                Array array = Array.CreateInstance(innerType, ddd.Length);
        //                theList.CopyTo(array, 0);
        //                obj = array;
        //            }
        //            else if (converter is CollectionConverter)
        //            {
        //                var innerType = parameterDescriptors[i].ParameterType.GetGenericArguments()[0];
        //                var ddd = decryptedParameters[parameterName] as string[];

        //                System.Reflection.MethodInfo targetMethod = typeof(dummy).GetMethod("GetGenericListImpl").MakeGenericMethod(innerType);
        //                obj = targetMethod.Invoke(null, new object[] { ddd });
        //            }
        //            else if (converter is TypeConverter)
        //            {
        //                obj = filterContext.ActionParameters[parameterName];

        //                foreach (var property in parameterDescriptors[i].ParameterType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        //                {
        //                    BindAliasAttribute attribute = Attribute.GetCustomAttribute(property, typeof(BindAliasAttribute)) as BindAliasAttribute;
        //                    if (attribute != null)
        //                    {
        //                        object propValue = decryptedParameters[attribute.Alias.ToLower()];

        //                        var conv = TypeDescriptor.GetConverter(property.PropertyType);
        //                        object obj1 = conv.ConvertFrom(propValue);
        //                        property.SetValue(obj, obj1);
        //                    }
        //                }

        //            }


        //            filterContext.ActionParameters[parameterName] = obj;
        //        }
        //    }
        //}


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

        private Dictionary<string, object> ExtractEncryptedParametersFromJObject(JObject jObject)
        {
            Dictionary<string, object> encryptedParameters = new Dictionary<string, object>();

            foreach (var jproperty in jObject.Properties().Where(p => p.Name.StartsWith(StringEncrypter.ControlsEncrypter.Prefix)))
            {
                var oldName = jproperty.Name.Replace(StringEncrypter.ControlsEncrypter.Prefix, "").ToLower();
                object value = CastJPropertyValue(jproperty);
                if (value != null)
                    encryptedParameters.Add(oldName, value);
            }
            return encryptedParameters;
        }

        private object CastJPropertyValue(JProperty jproperty)
        {
            object result = null;
            switch (jproperty.Value.Type)
            {
                case JTokenType.String:
                    result = StringEncrypter.ControlsEncrypter.Decrypt(jproperty.Value.ToString());
                    break;
                case JTokenType.Array:
                    var jArray = jproperty.Value as JArray;
                    var array = new string[jArray.Count];
                    for (int i = 0; i < jArray.Count; i++)
                    {
                        var jToken = jArray[i];
                        if (jToken.Type == JTokenType.String)
                            array[i] = StringEncrypter.ControlsEncrypter.Decrypt(jToken.Value<string>());
                    }

                    result = array;
                    break;

            }
            return result;
        }
    }

    internal static class dummy
    {
        public static List<T> GetGenericListImpl<T>(IEnumerable<string> strings)
        {
            TypeConverter innerConverter = TypeDescriptor.GetConverter(typeof(T));
            Array converted = Array.ConvertAll(strings.ToArray(), s => innerConverter.ConvertFromString(s));

            List<T> result = new List<T>();
            if (converted != null)
            {
                foreach (object item in converted)
                {
                    T castItem = (item is T) ? (T)item : default(T);
                    result.Add(castItem);
                }
            }
            return result;
        }

    }
}