namespace EasyFlexibilityTool.Web
{
    #region Usings

    using System.Web.Configuration;

    #endregion

    public static class AppSettings
    {
        public static string TelemetryInstrumentationKey => WebConfigurationManager.AppSettings["TelemetryInstrumentationKey"];

        public static string SendGridUsername => WebConfigurationManager.AppSettings["SendGridUsername"];

        public static string SendGridPassword => WebConfigurationManager.AppSettings["SendGridPassword"];

        public static string ServiceEmailAddress => WebConfigurationManager.AppSettings["ServiceEmailAddress"];

        public static string StorageConnectionString => WebConfigurationManager.AppSettings["StorageConnectionString"];

        public static string StoragePhotoContainerName => WebConfigurationManager.AppSettings["StoragePhotoContainerName"];

        public static string StorageBlobExtension => WebConfigurationManager.AppSettings["StorageBlobExtension"];

        public static string StorageBaseUrl => WebConfigurationManager.AppSettings["StorageBaseUrl"];

        public static string FacebookAppId => WebConfigurationManager.AppSettings["FacebookAppId"];

        public static string InternalApiKey => WebConfigurationManager.AppSettings["InternalApiKey"];
    }
}