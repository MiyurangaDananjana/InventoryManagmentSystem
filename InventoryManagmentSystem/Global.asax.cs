using InventoryManagmentSystem.Models;
using Serilog;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;


namespace InventoryManagmentSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Configure Serilog here
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.File(@"C:\Users\dananjanaA\Desktop\InventoryManagmentSystem\InventoryManagmentSystem\App_Data\mylog.txt")
             .CreateLogger();

            // Log an initial message
            Log.Information("Starting MVC application...");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            container.RegisterType<InventorySystemEntities1>();
            // Other registrations...
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
