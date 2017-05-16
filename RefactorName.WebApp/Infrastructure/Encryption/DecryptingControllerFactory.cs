using RefactorName.Core;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace RefactorName.WebApp.Infrastructure
{
    public class DecryptingControllerFactory : DefaultControllerFactory
    {
        // Used to provide the action descriptors to consider for attribute routing
        public const string Action = "action";

        // Used to indicate that a route is a controller-level attribute route.
        public const string Controller = "controller";

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var keyExists = StringEncrypter.ControlsEncrypter.IsEncryptionKeyExists;

            try
            {

                NameValueCollection parameters = requestContext.HttpContext.Request.Params;
                string[] encryptedParamKeys = parameters.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.StartsWith(StringEncrypter.ControlsEncrypter.Prefix)).ToArray();

                foreach (string key in encryptedParamKeys)
                {
                    string oldKey = key.Replace(StringEncrypter.ControlsEncrypter.Prefix, string.Empty);
                    string[] value = parameters.GetValues(key);
                    if (value.Length > 1)
                    {
                        var result = value.Select(s => string.IsNullOrEmpty(s) ? "" : StringEncrypter.ControlsEncrypter.Decrypt(s)).ToArray();
                        requestContext.RouteData.Values[oldKey] = result;
                    }
                    else if (value.Length == 1)
                    {
                        requestContext.RouteData.Values[oldKey] = string.IsNullOrEmpty(value[0]) ? "" : StringEncrypter.ControlsEncrypter.Decrypt(value[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                if (keyExists)
                {
                    Tracer.Log.Failure($"Client Form data Decryption failed: {DateTime.Now.ToString()} {ex.ToString()}");
                    return RedirectTo(requestContext, "Shared", "Error");
                }
            }

            var controller = base.CreateController(requestContext, controllerName);
            requestContext.HttpContext.Items[BaseController.CurrentControllerInstanceKey] = controller;

            return controller;
        }

        private IController RedirectTo(RequestContext requestContext, string controller, string action)
        {
            requestContext.RouteData.Values[Action] = action;
            requestContext.RouteData.Values[Controller] = controller;

            return base.CreateController(requestContext, controller);
        }
    }
}