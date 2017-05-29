namespace EasyFlexibilityTool.Web.Controllers.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AutoMapper;
    using Models;
    using Base;
    using Data;
    using Data.Model;
    using EasyFlexibilityTool.Web.Services;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    public class LeaderboardController : BaseApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var dictionary = await DbContext.AngleMeasurements
                    .Where(am => am.User.IsAgreeToLeaderboard)
                    .GroupBy(am => am.User.UserName)
                    .ToDictionaryAsync(g => g.Key, g => new List<AngleMeasurement> {
                        g.OrderBy(am => am.DateTimeStamp).FirstOrDefault(),
                        g.OrderByDescending(am => am.Angle).FirstOrDefault()
                    });
                var models = Mapper.Map<List<LeaderboardItemModel>>(dictionary);
                foreach (var model in models)
                {
                    var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Id == model.FirstMeasurement.UserId);
                    model.UserId = user.Id;
                    model.RewardPoints = user.Redeemed ? 0 : Math.Round(model.BestMeasurement.Angle - model.FirstMeasurement.Angle);
                }
                return Ok(models.OrderByDescending(i => i.Progress));
            }
            catch
            {
                throw;
            }
        }

        [HttpPost, Route("api/leaderboard/enable")]
        public async Task<IHttpActionResult> Enable(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                try
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var existingCount = await context.Users.CountAsync(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
                        if (existingCount == 0)
                        {
                            var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                            var user = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId, StringComparison.OrdinalIgnoreCase));
                            if (user != null)
                            {
                                if (!user.IsAgreeToLeaderboard)
                                {
                                    user.IsAgreeToLeaderboard = true;
                                    user.Redeemed = false;
                                    user.UserName = userName;
                                    await context.SaveChangesAsync();
                                    return Ok("Special15Off");
                                }
                                return BadRequest("User is already agreed");
                            }
                            return NotFound();
                        }
                        return BadRequest("User Name is already taken");
                    }
                }
                catch
                {
                    throw;
                }
            }
            return BadRequest("UserName is empty");
        }

        [HttpGet, Route("api/leaderboard/redeempoints")]
        public async Task<IHttpActionResult> RedeemPoints(string RedeemPoints)
        {
            try
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUserManager applicationUserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var emailFrom = await applicationUserManager.GetEmailAsync(userId);
                //var emailTo = AppSettings.ServiceEmailAddress;
                var emailTo = "dtkach@elasticsteel.com";
                //var emailTo = "chung.le.rhj@gmx.com";
                var message = string.Format("<strong>EMail</strong>: {0}<br/><strong>RedeemPoints</strong>: {1}", emailFrom, RedeemPoints);
                await EmailService.SendAsync(emailFrom, emailTo, "Redeem Points", "", message).ConfigureAwait(false);

                var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    if (!user.Redeemed)
                    {
                        user.Redeemed = true;
                        await DbContext.SaveChangesAsync();
                    }
                }

                return Ok("Success");
            }
            catch { }
            return BadRequest();
        }

        [HttpPost, Route("api/leaderboard/delete")]
        public IHttpActionResult Delete(string userId)
        {
            var user = DbContext.Users.SingleOrDefault(u => u.Id == userId);
            user.IsAgreeToLeaderboard = false;
            user.UserName = user.Email;
            DbContext.SaveChanges();
            return Ok(new { UserName = user.UserName });
        }

        [HttpGet, Route("api/leaderboard/getcomment")]
        public IHttpActionResult GetComment(string imageId)
        {
            var comments = DbContext.Comments.Where(c => c.ImageId == imageId);
            try
            {
                var models = new List<CommentModel>();
                foreach (var comment in comments)
                {
                    models.Add(new CommentModel()
                    {
                        Id = comment.Id,
                        ImageId = comment.ImageId,
                        UserId = comment.UserId,
                        UserName = comment.UserName,
                        DateTimeStamp = comment.DateTimeStamp,
                        CommentContent = comment.CommentContent
                    });
                }
                return Ok(models);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost, Route("api/leaderboard/addcomment")]
        public async Task<IHttpActionResult> AddCommentAsync(CommentModel commentModel)
        {
            var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var dateTime = DateTime.Now;
            DbContext.Comments.Add(new Comment()
            {
                ImageId = commentModel.ImageId,
                UserId = userId,
                UserName = user.UserName,
                DateTimeStamp = dateTime,
                CommentContent = commentModel.CommentContent
            });
            DbContext.SaveChanges();
            return Ok(commentModel);
        }

        [HttpPost, Route("api/leaderboard/deletecomment")]
        public IHttpActionResult DeleteComment(Guid commentId)
        {
            var commentsOfImageId = DbContext.Comments.Where(c => c.Id == commentId);
            foreach(var comment in commentsOfImageId)
            {
                DbContext.Comments.Remove(comment);
            }
            DbContext.SaveChanges();
            return Ok("OK");
        }
    }
}
