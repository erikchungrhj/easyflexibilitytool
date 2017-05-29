namespace EasyFlexibilityTool.Web.Controllers.MVC.Base
{
    using System.Web.Mvc;
    using Data;

    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext DbContext = new ApplicationDbContext();

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