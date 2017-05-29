namespace EasyFlexibilityTool.Web.Controllers.WebAPI
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base;
    using Data;
    using Models;
    using RazorEngine;
    using RazorEngine.Templating;
    using Services;

    public class TrainingReminderController : BaseApiController
    {
        [AllowAnonymous, HttpPost, Route("api/TrainingReminder/SendEmails")]
        public async Task<IHttpActionResult> SendEmailsAsync(string apiKey)
        {
            if (apiKey.Equals(AppSettings.InternalApiKey, StringComparison.OrdinalIgnoreCase))
            {
                await EmailService.SendAsync("raychangrhj@gmail.com", "EasyFlexibility Daily Email Test", "Daily Email Test", "Daily Email Test").ConfigureAwait(false);
                await sendMail_GoalAchieve();
                await sendMail_TrainingCheck();
                await sendMail_SpecialOffers();
                /*===============================================================================
                using (var context = new ApplicationDbContext())
                {
                    //todo: possible performance issues in the future - get all the measurements from DB
                    var measurements = await context.AngleMeasurements
                        .Include(am => am.User)
                        .Include(am => am.Category)
                        .Where(am => am.User.EmailConfirmed)
                        .ToListAsync();

                    var usersMeasurements = measurements.GroupBy(am => am.UserId);

                    foreach (var userMeasurement in usersMeasurements)
                    {
                        var categoriesMeasurements = userMeasurement.GroupBy(am => am.AngleMeasurementCategoryId);

                        var models = categoriesMeasurements
                            .Select(cam => new TrainingReminderModel
                            {
                                UserName = cam.First().User.UserName,
                                Email = cam.First().User.Email,
                                AngleMeasurementsCount = cam.Count(),
                                FirstAngleMeasurement = cam.OrderBy(m => m.DateTimeStamp).FirstOrDefault(),
                                LastAngleMeasurement = cam.OrderByDescending(m => m.DateTimeStamp).FirstOrDefault(),
                                CategoryName = cam.First().Category.Name
                            })
                            .ToList();

                        var nextModels = models.Where(m => (DateTime.UtcNow.Date - m.LastAngleMeasurement.DateTimeStamp.Date).Days == 2).ToList();
                        foreach (var model in nextModels)
                        {
                            var body = Engine.Razor.RunCompile("NextTrainingReminderMessage", typeof(TrainingReminderModel), model);

                            await EmailService.SendAsync(model.Email, "EasyFlexibility next training reminder", body, body).ConfigureAwait(false);
                            Trace.TraceInformation($"EMAIL TRAINING REMINDER: Next Training Reminder Email has been sent to {model.Email}");
                        }

                        var missedModels = models.Where(m => (DateTime.UtcNow.Date - m.LastAngleMeasurement.DateTimeStamp.Date).Days > 2).ToList();
                        foreach (var model in missedModels)
                        {
                            var body = Engine.Razor.RunCompile("MissedTrainingReminderMessage", typeof(TrainingReminderModel), model);
                            await EmailService.SendAsync(model.Email, "EasyFlexibility missed training reminder", body, body).ConfigureAwait(false);
                            Trace.TraceInformation($"EMAIL TRAINING REMINDER: Missed Training Reminder Email has been sent to {model.Email}");
                        }
                    }
                }
                ===============================================================================*/
            }
            else
            {
                Trace.TraceError("EMAIL TRAINING REMINDER: incorrect secret");
            }
            return Ok();
        }

        async Task sendMail_GoalAchieve()
        {
            var templateName = "Goal Achieved";
            var emailTemplate = DbContext.EmailTemplates.Where(et => et.TemplateName == templateName).FirstOrDefault();
            if (emailTemplate == null) return;

            foreach (var user in DbContext.Users)
            {
                if (!user.Email.StartsWith("raychangrhj") && !user.Email.StartsWith("chung.le.rhj")) continue;
                //============================================================================================
                UserSummaryInfoModel userInfo = new UserSummaryInfoModel(DbContext, user);
                if (userInfo.CurrentDegrees < 180) continue;
                var body = userInfo.getAdjustedTemplate(emailTemplate.TemplateContent);
                await EmailService.SendAsync(user.Email, "EasyFlexibility Goal Achieve", body, body).ConfigureAwait(false);
            }
        }

        async Task sendMail_TrainingCheck()
        {
            var templateName = "Training Check";
            var emailTemplate = DbContext.EmailTemplates.Where(et => et.TemplateName == templateName).FirstOrDefault();
            if (emailTemplate == null) return;

            var sendConditionItems = emailTemplate.SendCondition.Split('#');
            int periodicDays = 7;
            if (sendConditionItems[0] == "true")
            {
                periodicDays = int.Parse(sendConditionItems[1]);
            }
            if (DateTime.Now.Day % periodicDays > 0) return;

            int minProgress = 0;
            if (sendConditionItems[2] == "true")
            {
                minProgress = int.Parse(sendConditionItems[3]);
            }

            foreach (var user in DbContext.Users)
            {
                if (!user.Email.StartsWith("raychangrhj") && !user.Email.StartsWith("chung.le.rhj")) continue;
                //============================================================================================
                UserSummaryInfoModel userInfo = new UserSummaryInfoModel(DbContext, user);
                if (userInfo.ProgressRate >= minProgress) continue;
                var body = userInfo.getAdjustedTemplate(emailTemplate.TemplateContent);
                await EmailService.SendAsync(user.Email, "EasyFlexibility Training Check", body, body).ConfigureAwait(false);
            }
        }

        async Task sendMail_SpecialOffers()
        {
            var templateName = "Special Offers";
            var emailTemplates = DbContext.EmailTemplates.Where(et => et.TemplateName == templateName).ToList();
            foreach(var emailTemplate in emailTemplates)
            {
                if (DateTime.Now.ToString("mm/dd/yy") != emailTemplate.SendCondition) return;

                foreach (var user in DbContext.Users)
                {
                    if (!user.Email.StartsWith("raychangrhj") && !user.Email.StartsWith("chung.le.rhj")) continue;
                    //============================================================================================
                    UserSummaryInfoModel userInfo = new UserSummaryInfoModel(DbContext, user);
                    var body = userInfo.getAdjustedTemplate(emailTemplate.TemplateContent);
                    await EmailService.SendAsync(user.Email, "EasyFlexibility Special Offer", body, body).ConfigureAwait(false);
                }
            }
        }
    }
}
