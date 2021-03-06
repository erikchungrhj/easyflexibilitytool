﻿namespace EasyFlexibilityTool.Web.Telemetry
{
    using System.Web.Http.ExceptionHandling;
    using Microsoft.ApplicationInsights;

    public class AiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient _telemetryClient;

        public AiExceptionLogger()
        {
            _telemetryClient = new TelemetryClient();
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception != null)
            {
                _telemetryClient.TrackException(context.Exception);
            }

            base.Log(context);
        }
    }
}