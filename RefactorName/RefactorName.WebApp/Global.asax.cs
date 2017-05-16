using RefactorName.Domain;
using RefactorName.Core;
using RefactorName.WebApp.Binders;
using System;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RefactorName.WebApp.Infrastructure.Encryption;
using RefactorName.WebApp.Infrastructure;
using RefactorName.Core.Basis;

namespace RefactorName.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Factory.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var binder = new DateTimeModelBinder(new string[] { "dd/MM/yyyy", "d/M/yyyy" });
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);

            //Important for localize default validation messages
            DefaultModelBinder.ResourceClassKey = "ValidationMessages";
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ValidationMessages";

            //register our decrypter controller factory
            ControllerBuilder.Current.SetControllerFactory(typeof(DecryptingControllerFactory));

            Thread cleanThread = new Thread(() => Util.CleanTempFolder());
            cleanThread.Start();


        }

        protected void Session_End(Object sender, EventArgs e)
        {

            string tempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp");
            DirectoryInfo dir = new DirectoryInfo(tempPath);
            try
            {
                foreach (FileInfo fInfo in dir.GetFiles())
                {
                    if (fInfo.Name.Contains(Session.SessionID + "__"))
                        fInfo.Delete();
                }
            }
            catch { }

            //clear the session
            Session.Clear();
        }

        protected static void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Add("X-Frame-Options", "DENY");
            HttpContext.Current.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            HttpContext.Current.Response.Headers.Remove("Server");

            //Uncommet this line if you are sure you do not use external scripts (like google maps)
            //HttpContext.Current.Response.Headers.Add("content-security-policy", "script-src 'self' 'unsafe-inline' 'unsafe-eval'");
        }

        protected static void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var oldPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (oldPrincipal != null)
            {
                var oldIdentity = oldPrincipal.Identity as ClaimsIdentity;

                if (oldIdentity != null && oldIdentity.IsAuthenticated)
                {
                    User userProfile = new User(int.Parse(oldIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));  // UserService.Obj.FindByName(oldIdentity.Name); //we will store things in claims so we will not go to db everytime
                    if (userProfile != null)
                        Thread.CurrentPrincipal = HttpContext.Current.User = new UserProfilePrincipal(oldPrincipal, oldIdentity, userProfile);
                }
            }

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError().GetBaseException();
        }
    }
}
