using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace RefactorName.Web.Infrastructure.Encryption
{
    public class DecryptingControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            NameValueCollection parameters = requestContext.HttpContext.Request.Params;
            string[] encryptedParamKeys = parameters.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.StartsWith(StringEncrypter.Obj.Prefix)).ToArray();

            foreach (string key in encryptedParamKeys)
            {
                string oldKey = key.Replace(StringEncrypter.Obj.Prefix, string.Empty);
                string oldValue = StringEncrypter.Obj.Decrypt(parameters[key]);
                requestContext.RouteData.Values[oldKey] = oldValue;
            }

            return base.CreateController(requestContext, controllerName);
        }
    }

}