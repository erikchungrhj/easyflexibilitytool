namespace EasyFlexibilityTool.Web.Services
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using CuttingEdge.Conditions;
    using Microsoft.AspNet.Identity;
    using SendGrid;
    using System.Collections.Generic;

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            Condition.Requires(message, "message").IsNotNull();

            await SendAsync(message.Destination, message.Subject, message.Body, message.Body).ConfigureAwait(false);
        }

        public static async Task SendAsync(string destination, string subject, string textBody, string htmlBody)
        {
            Condition.Requires(destination, "destination").IsNotNullOrEmpty();
            Condition.Requires(subject, "subject").IsNotNullOrEmpty();
            var sendGridMessage = new SendGridMessage
            {
                From = new MailAddress(AppSettings.ServiceEmailAddress, AppSettings.ServiceEmailAddress),
                To = new[] { new MailAddress(destination, destination) },
                Subject = subject,
                Text = textBody,
                Html = htmlBody
            };
            var credentials = new NetworkCredential(AppSettings.SendGridUsername, AppSettings.SendGridPassword);
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(sendGridMessage).ConfigureAwait(false);
        }

        public static async Task SendAsync(string source, string destination, string subject, string textBody, string htmlBody)
        {
            Condition.Requires(source, "source").IsNotNullOrEmpty();
            Condition.Requires(destination, "destination").IsNotNullOrEmpty();
            Condition.Requires(subject, "subject").IsNotNullOrEmpty();
            var sendGridMessage = new SendGridMessage
            {
                From = new MailAddress(source, source),
                To = new[] { new MailAddress(destination, destination) },
                Subject = subject,
                Text = textBody,
                Html = htmlBody
            };
            var credentials = new NetworkCredential(AppSettings.SendGridUsername, AppSettings.SendGridPassword);
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(sendGridMessage).ConfigureAwait(false);
        }

        public static async Task SendAsync(string source, List<string> destinationList, string subject, string textBody, string htmlBody)
        {
            Condition.Requires(source, "source").IsNotNullOrEmpty();
            Condition.Requires(subject, "subject").IsNotNullOrEmpty();
            var sendGridMessage = new SendGridMessage
            {
                From = new MailAddress(source, source),
                Subject = subject,
                Text = textBody,
                Html = htmlBody
            };
            sendGridMessage.AddTo("customers@elasticsteel.com");
            foreach (var destination in destinationList)
            {
                sendGridMessage.AddBcc(destination);
            }
            var credentials = new NetworkCredential(AppSettings.SendGridUsername, AppSettings.SendGridPassword);
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(sendGridMessage).ConfigureAwait(false);
        }
    }
}
