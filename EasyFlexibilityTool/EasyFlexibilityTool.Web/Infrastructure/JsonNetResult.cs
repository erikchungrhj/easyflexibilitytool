namespace EasyFlexibilityTool.Web.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                //Unspecified for showing the DateTime as it is in the DB regardless of the DB, WebServer and Browser TZs
                //Utc for showing the DateTime is in the UTC
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context != null)
            {
                HttpResponseBase response = context.HttpContext.Response;

                response.ContentType = !string.IsNullOrEmpty(ContentType)
                    ? ContentType
                    : "application/json";

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                if (Data != null)
                {
                    var serializedObject = JsonConvert.SerializeObject(Data, Formatting, SerializerSettings);
                    response.Write(serializedObject);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}