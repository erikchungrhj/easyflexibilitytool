namespace EasyFlexibilityTool.Web
{
    using System.Web.Hosting;
    using RazorEngine.Configuration;
    using RazorEngine.Templating;

    public static class RazorEngineConfig
    {
        #region Public Methods

        public static IRazorEngineService GetConfiguredEngine()
        {
            var configuration = new TemplateServiceConfiguration
            {
                TemplateManager = new ResolvePathTemplateManager(new[]
                {
                    HostingEnvironment.MapPath("~/Views/Shared/EmailTemplates")
                })
            };

            return RazorEngineService.Create(configuration);
        }

        #endregion
    }
}