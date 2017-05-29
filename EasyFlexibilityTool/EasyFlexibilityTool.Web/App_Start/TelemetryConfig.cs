namespace EasyFlexibilityTool.Web
{
    #region Usings

    using Microsoft.ApplicationInsights.Extensibility;

    #endregion

    public class TelemetryConfig
    {
        #region Public Static Methods

        public static void RegisterTelemetryInstrumentationKey()
        {
            if (string.IsNullOrWhiteSpace(AppSettings.TelemetryInstrumentationKey))
            {
                // Disable the telemetry
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }
            else
            {
                // Set application insights instrumentation key
                TelemetryConfiguration.Active.InstrumentationKey = AppSettings.TelemetryInstrumentationKey;
            }
        }

        #endregion
    }
}