using RefactorName.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp
{
    public static class EncryptStringExtentions
    {
        public static RouteValueDictionary EncryptObject(this IEncryptString encrypter, object routeValues)
        {
            RouteValueDictionary parameters = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            return encrypter.EncryptRouteValueDictionary(parameters);
        }

        public static RouteValueDictionary EncryptRouteValueDictionary(this IEncryptString encrypter, RouteValueDictionary routeValues)
        {
            string encryptedParameters = encrypter.Encrypt(routeValues.ToQueryString());
            RouteValueDictionary result = new RouteValueDictionary();
            result.Add("q", encryptedParameters);

            return result;
        }

        public static string ToQueryString(this RouteValueDictionary routeValue)
        {
            List<string> items = new List<string>();

            foreach (string name in routeValue.Keys)
                items.Add(string.Concat(name, "=", System.Web.HttpUtility.UrlEncode(Convert.ToString(routeValue[name]))));

            return string.Join("&", items.ToArray());
        }
    }
}