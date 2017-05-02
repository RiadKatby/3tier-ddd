using System.Web.Mvc;

namespace RefactorName.WebApp.Areas.ProcessManagement
{
    public class ProcessManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ProcessManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
                "ProcessManagement_default",
                "ProcessManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}