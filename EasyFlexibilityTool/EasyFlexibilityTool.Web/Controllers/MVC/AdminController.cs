using AutoMapper;
using EasyFlexibilityTool.Web.Controllers.MVC.Base;
using EasyFlexibilityTool.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EasyFlexibilityTool.Web.Controllers.MVC
{
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUser()
        {
            return View();
        }

        public ActionResult UserProfile(string userId)
        {
            ViewBag.User = DbContext.Users.Find(userId);
            ViewBag.AngleMeasurementCategories = Mapper.Map<List<AngleMeasurementCategoryModel>>(DbContext.AngleMeasurementCategories.ToList());
            return View();
        }

        public ActionResult MessageUsers()
        {
            return View();
        }

        public ActionResult EmailTemplate()
        {
            return View();
        }

        public FileContentResult ExportEmail()
        {
            string emailCsvString = "";
            foreach (var user in DbContext.Users)
            {
                emailCsvString += "," + user.Email;
            }
            return File(new System.Text.UTF8Encoding().GetBytes(emailCsvString.Substring(1)), "text/csv", "emails.csv");
        }
    }
}
