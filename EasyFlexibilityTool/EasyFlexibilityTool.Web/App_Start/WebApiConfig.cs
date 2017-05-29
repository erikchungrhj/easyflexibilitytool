namespace EasyFlexibilityTool.Web
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Telemetry;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());

            // Web API routes - Enabling Attribute Routing
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
