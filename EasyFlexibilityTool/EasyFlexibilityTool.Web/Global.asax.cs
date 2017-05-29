namespace EasyFlexibilityTool.Web
{
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using AutoMapper;
    using Data;
    using RazorEngine;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            TelemetryConfig.RegisterTelemetryInstrumentationKey();

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            //DB initialization
            using (var context = new ApplicationDbContext())
            {
                context.Users.Find(Guid.NewGuid().ToString());
            }

            Mapper.Initialize(config =>
            {
                config.AddMappings();
            });

            Engine.Razor = RazorEngineConfig.GetConfiguredEngine();
        }
    }
}
