using AutoMapper;
using EasyFlexibilityTool.Data.Model;
using EasyFlexibilityTool.Web.Controllers.WebAPI.Base;
using EasyFlexibilityTool.Web.Models;
using EasyFlexibilityTool.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyFlexibilityTool.Web.Controllers.WebAPI
{
    public class AdminController : BaseApiController
    {
        public async Task<IHttpActionResult> GetAsync()
        {
            var angleMeasurementDictionary = await DbContext.AngleMeasurements
                .GroupBy(am => am.User.Id)
                .ToDictionaryAsync(g => g.Key, g => new List<AngleMeasurement> {
                    g.OrderBy(am => am.DateTimeStamp).FirstOrDefault(),
                    g.OrderByDescending(am => am.Angle).FirstOrDefault(),
                    g.OrderBy(am => am.DateTimeStamp).LastOrDefault()
                });
            try
            {
                var models = new List<LeaderboardItemModel>();
                foreach (var user in DbContext.Users)
                {
                    var angleMeasurementOfUser = new List<AngleMeasurementModel>() { new AngleMeasurementModel(), new AngleMeasurementModel() };
                    if (angleMeasurementDictionary.ContainsKey(user.Id))
                    {
                        angleMeasurementOfUser = Mapper.Map<List<AngleMeasurementModel>>(angleMeasurementDictionary[user.Id]);
                    }
                    models.Add(new LeaderboardItemModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstMeasurement = angleMeasurementOfUser[0],
                        BestMeasurement = angleMeasurementOfUser[1],
                        LastMeasurementDate = (angleMeasurementOfUser.Count < 3 ? angleMeasurementOfUser[0] : angleMeasurementOfUser[2]).DateTimeStamp,
                        RewardPoints = user.Redeemed ? 0 : Math.Round(angleMeasurementOfUser[1].Angle - angleMeasurementOfUser[0].Angle)
                    });
                }
                return Ok(models.OrderByDescending(i => i.Progress));
            }
            catch
            {
                throw;
            }
        }

        [HttpGet, Route("api/admin/userrole")]
        public IHttpActionResult UserRole()
        {
            var roleInfo = "User";
            try
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = DbContext.Users.Find(userId);
                var email = user.Email;
                var adminEmails = new List<string>()
                {
                    AppSettings.ServiceEmailAddress,
                    "dtkach@elasticsteel.com",
                    "hokanokoto@gmail.com",
                    "anna@easyflexibility.com",
                    "raychangrhj@gmail.com"
                };
                if (adminEmails.Contains(email))
                {
                    roleInfo = "SuperAdmin";
                }
                else
                {
                    var role = DbContext.Roles.Find(userId);
                    if (role != null)
                    {
                        roleInfo = role.Name;
                    }
                }
            }
            catch { }
            return Ok(new { Role = roleInfo });
        }

        [HttpPost, Route("api/admin/delete")]
        public IHttpActionResult Delete(string email)
        {
            var user = DbContext.Users.SingleOrDefault(u => u.Email == email);
            var angleMeasurements = DbContext.AngleMeasurements.Where(am => am.UserId == user.Id);
            foreach (var angleMeasurement in angleMeasurements)
            {
                DbContext.AngleMeasurements.Remove(angleMeasurement);
            }
            DbContext.Users.Remove(user);
            DbContext.SaveChanges();
            return Ok(new { UserName = user.UserName });
        }

        [HttpGet, Route("api/admin/messageusers")]
        public async Task<IHttpActionResult> MessageUsers(string MailSubject, string MailBody)
        {
            try
            {
                var emailFrom = AppSettings.ServiceEmailAddress;
                var emailTo = new List<string>();
                //emailTo.Add("raychangrhj@gmail.com");
                foreach (var user in DbContext.Users)
                {
                    emailTo.Add(user.Email);
                }
                var message = string.Format("<strong>{0}</strong><br>{1}", MailSubject, MailBody);
                await EmailService.SendAsync(emailFrom, emailTo, MailSubject, "", MailBody).ConfigureAwait(false);
                return Ok("Success");
            }
            catch { }
            return BadRequest();
        }

        [HttpGet, Route("api/admin/getspecialofferlist")]
        public IHttpActionResult GetSpecialOfferList()
        {
            try
            {
                var emailTemplates = DbContext.EmailTemplates.Where(et => et.SendCondition.StartsWith("OFFER#")).ToList();
                var emailTemplateModels = new List<EmailTemplateModel>();
                foreach(var emailTemplate in emailTemplates)
                {
                    emailTemplateModels.Add(new EmailTemplateModel()
                    {
                        TemplateName = emailTemplate.TemplateName,
                        SendCondition = emailTemplate.SendCondition.Substring(6),
                        TemplateContent = emailTemplate.TemplateContent
                    });
                }
                return Ok(emailTemplateModels);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("api/admin/getemailtemplate")]
        public IHttpActionResult GetEmailTemplate(string templateName)
        {
            try
            {
                var emailTemplate = DbContext.EmailTemplates.Where(et => et.TemplateName == templateName).FirstOrDefault();
                return Ok(emailTemplate);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost, Route("api/admin/addemailtemplate")]
        public IHttpActionResult AddEmailTemplate(EmailTemplateModel emailTemplateModel)
        {
            var emailTemplate = DbContext.EmailTemplates.Where(et => et.TemplateName == emailTemplateModel.TemplateName).FirstOrDefault();
            if (emailTemplate == null)
            {
                DbContext.EmailTemplates.Add(new EmailTemplate()
                {
                    TemplateName = emailTemplateModel.TemplateName,
                    SendCondition = emailTemplateModel.SendCondition,
                    TemplateContent = emailTemplateModel.TemplateContent
                });
            }
            else
            {
                emailTemplate.TemplateName = emailTemplateModel.TemplateName;
                emailTemplate.SendCondition = emailTemplateModel.SendCondition;
                emailTemplate.TemplateContent = emailTemplateModel.TemplateContent;
            }
            DbContext.SaveChanges();
            return Ok("OK");
        }

        [HttpPost, Route("api/admin/deleteemailtemplate")]
        public IHttpActionResult DeleteComment(string templateName)
        {
            var emailTemplate = DbContext.EmailTemplates.Where(et => et.TemplateName == templateName).SingleOrDefault();
            DbContext.EmailTemplates.Remove(emailTemplate);
            DbContext.SaveChanges();
            return Ok("OK");
        }
    }
}
