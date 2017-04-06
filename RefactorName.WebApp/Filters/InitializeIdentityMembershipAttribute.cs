using RefactorName.Core;
using RefactorName.Domain;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace RefactorName.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeIdentityMembershipAttribute : ActionFilterAttribute
    {
        private static IdentityMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class IdentityMembershipInitializer
        {
            public IdentityMembershipInitializer()
            {
                try
                {
                    ApplicationUserManager manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();


                    var adminUser = manager.FindByNameAsync("Admin").Result;


                    if (adminUser == null)
                    {
                        User firstUser = new User("Admin", "Administrator", true, "0555555555", "Admin");
                        var u = manager.CreateAsync(firstUser, "P@ssw0rd");
                        //get details
                        firstUser = manager.FindByNameAsync("Admin").Result;

                        //add to role
                        manager.AddToRoleAsync(firstUser.Id, RoleNames.SuperAdministrator);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Identity Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
