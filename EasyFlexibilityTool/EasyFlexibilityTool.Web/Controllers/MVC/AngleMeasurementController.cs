namespace EasyFlexibilityTool.Web.Controllers.MVC
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using Base;
    using Models;

    [AllowAnonymous]
    public class AngleMeasurementController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var categories = await DbContext.AngleMeasurementCategories.ToListAsync();
            ViewBag.Categories = Mapper.Map<List<AngleMeasurementCategoryModel>>(
                categories
                    .OrderBy(c => c.Name)
                    .OrderBy(c => !c.Name.Equals("Side Split", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(c => c.Name.Contains("Other")));

            return View();
        }
    }
}
