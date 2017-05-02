using RefactorName.Core;
using RefactorName.Domain;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RefactorName.WebApp.Infrastructure.Security;
using RefactorName.WebApp.Filters;
using ReafactorName.WebApp;
using RefactorName.WebApp.Controllers;
using RefactorName.Domain.Workflow;
using Microsoft.AspNet.Identity;


namespace RefactorName.WebApp.Filters
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
                    var adminUser = UserService.Obj.FindByNameAsync("Admin@Thiqah.sa").Result;


                    if (adminUser == null)
                    {
                        User firstUser = new User("Admin",true,"000000000", "Admin@Thiqah.sa");
                        var u = UserService.Obj.CreateAsync(firstUser, "P@ssw0rd");
                        //get details
                        firstUser = UserService.Obj.FindByNameAsync("Admin@Thiqah.sa").Result;

                        //add to role
                        UserService.Obj.AddToRoleAsync(firstUser.Id, RoleNames.SuperAdministrator);
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
