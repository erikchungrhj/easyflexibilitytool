namespace EasyFlexibilityTool.Web.Controllers.WebAPI
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Models;
    using Base;
    using Data.Extensions;

    public class AngleMeasurementController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(Guid? categoryId)
        {
            try
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                var query = DbContext.AngleMeasurements.Where(am => am.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
                if (categoryId.HasValue)
                {
                    query = query.Where(am => am.AngleMeasurementCategoryId == categoryId.Value);
                }
                var models = await query
                    .OrderBy(am => am.DateTimeStamp)
                    .ProjectTo<AngleMeasurementModel>()
                    .ToListAsync();
                return Ok(models);
            }
            catch
            {
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Post(AngleMeasurementPostModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    string blobName;
                    object result;
                    var measurementDate = model.Date.Value.ToUniversalTime();
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                        var entity = await DbContext.AngleMeasurements
                            .SingleOrDefaultAsync(am => am.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)
                                                        && am.DateTimeStamp == measurementDate
                                                        && am.AngleMeasurementCategoryId == model.CategoryId);
                        if (entity == null)
                        {
                            entity = DbContext.AngleMeasurements.Create();
                            DbContext.AngleMeasurements.Add(entity);
                            entity.UserId = userId;
                            entity.DateTimeStamp = measurementDate;
                        }
                        entity.Angle = model.Angle;
                        entity.AngleMeasurementCategoryId = model.CategoryId;
                        blobName = entity.Id.ToString();
                        result = Mapper.Map<AngleMeasurementModel>(entity);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Email))
                        {
                            var existingCount = await DbContext.Users.CountAsync(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
                            if (existingCount == 0)
                            {
                                var entity = await DbContext.AnonymAngleMeasurements.SingleOrDefaultAsync(m => m.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));
                                if (entity == null)
                                {
                                    entity = DbContext.AnonymAngleMeasurements.Create();
                                    DbContext.AnonymAngleMeasurements.Add(entity);
                                    entity.Email = model.Email;
                                }
                                entity.Angle = model.Angle;
                                entity.AngleMeasurementCategoryId = model.CategoryId;
                                entity.DateTimeStamp = measurementDate;
                                blobName = entity.Id.ToString();
                                result = entity.Email;
                            }
                            else
                            {
                                return BadRequest("Email is already used");
                            }
                        }
                        else
                        {
                            return BadRequest("Provided email is empty");
                        }
                    }
                    await (await AzureStorageService.Value).UploadBlobFromByteArrayAsync($"{blobName}.{AppSettings.StorageBlobExtension}", Convert.FromBase64String(model.PhotoData), "image/png");
                    await DbContext.SaveChangesAsync();
                    return Ok(result);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return BadRequest("Provided data is invalid");
            }
        }

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                var entity = await DbContext.AngleMeasurements
                    .SingleOrDefaultAsync(am => am.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase) && am.Id.Equals(id));
                if (entity != null)
                {
                    await (await AzureStorageService.Value).DeleteBlobAsync($"{id}.{AppSettings.StorageBlobExtension}");
                    DbContext.AngleMeasurements.Remove(entity);
                    await DbContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet, Route("api/AngleMeasurement/GetProgress")]
        public async Task<IHttpActionResult> GetProgress()
        {
            try
            {
                string[] result = new string[2];

                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                var measurements = await DbContext.AngleMeasurements
                    .Where(am => am.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(am => am.DateTimeStamp)
                    .ToListAsync();

                if (measurements.Count > 0)
                {
                    result[1] = $"{AppSettings.StorageBaseUrl}{AppSettings.StoragePhotoContainerName}/{measurements.Last().Id}.{AppSettings.StorageBlobExtension}";

                    if (measurements.Count == 1)
                    {
                        result[0] = $"I have just started my splits journey, and I'm at {measurements[0].Angle} degrees now. What about you? How many degrees are you at now? Do you dare to join me in this challenge and see how fast we can master the Splits? This program will help us reach the goal together!";
                    }
                    else
                    {
                        var lastAngle = measurements.Last().Angle;
                        if (lastAngle >= 180)
                        {
                            result[0] = measurements[measurements.Count - 2].Angle < 180
                                ? $"This is an update on the proguress of my Splits Challenge! I'm now at {lastAngle} degrees! I finally got the FULL SPLIT! Let's move further rogether!"
                                : $"This is an update on the proguress of my Splits Challenge! I'm now at {lastAngle} degrees! How about you?";
                        }
                        else
                        {
                            var angleDiff = measurements.GetAngleDiff().Where(d => d >= 0);
                            var progress = angleDiff.Average() * 0.2;
                            var daysCount = Math.Ceiling((180 - lastAngle) / progress * 2);

                            if (daysCount < 0)
                            {
                                daysCount = 0;
                            }

                            result[0] = $"This is an update on the proguress of my Splits Challenge! I am now at {lastAngle} degrees, and by my rate of progress I will get a full split in approximately {daysCount} days. Join me and let's reach this goal together!";
                        }
                    }

                    return Ok(result);
                }

                return NotFound();
            }
            catch
            {
                //let's leave it to the standlard error handling
                throw;
            }
        }

        [HttpGet, Route("api/anglemeasurement/anglemeasurementofuser")]
        public async Task<IHttpActionResult> AngleMeasurementOfUser(string userId, Guid? categoryId)
        {
            try
            {
                var query = DbContext.AngleMeasurements.Where(am => am.UserId == userId.ToString());
                if (categoryId.HasValue)
                {
                    query = query.Where(am => am.AngleMeasurementCategoryId == categoryId.Value);
                }
                var models = await query
                    .OrderBy(am => am.DateTimeStamp)
                    .ProjectTo<AngleMeasurementModel>()
                    .ToListAsync();
                return Ok(models);
            }
            catch
            {
                throw;
            }
        }
    }
}
