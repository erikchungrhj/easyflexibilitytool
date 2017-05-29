namespace EasyFlexibilityTool.Web
{
    using System.Web.Mvc;
    using Telemetry;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
