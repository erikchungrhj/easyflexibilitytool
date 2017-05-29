namespace EasyFlexibilityTool.Web.Controllers.WebAPI.Base
{
    using System.Web.Http;
    using Data;
    using Infrastructure;
    using Storage;

    [Authorize]
    public abstract class BaseApiController : ApiController
    {
        protected readonly ApplicationDbContext DbContext = new ApplicationDbContext();

        protected AsyncLazy<AzureStorageService> AzureStorageService
        {
            get { return new AsyncLazy<AzureStorageService>(async () => await Storage.AzureStorageService.GetInstanceAsync(AppSettings.StorageConnectionString, AppSettings.StoragePhotoContainerName)); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}