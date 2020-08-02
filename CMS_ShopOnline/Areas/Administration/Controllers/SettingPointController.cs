using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    public class SettingPointController : Controller
    {
        private readonly IDoiDiem DoiDiem;
        public SettingPointController()
        {
            DoiDiem = new DoiDiemRepository();
        }
        public SettingPointController(IDoiDiem _DoiDiem)
        {
            DoiDiem = _DoiDiem;
        }
        public ActionResult Create()
        {
            var model = new DoiDiemViewModel();
            var _doidiem = DoiDiem.SelectById(1);
            AutoMapper.Mapper.Map(_doidiem, model);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(DoiDiemViewModel model)
        {
            try
            {
                var _doidiem = DoiDiem.SelectById(1);
                AutoMapper.Mapper.Map(model, _doidiem);
                DoiDiem.Update(_doidiem);
                DoiDiem.Save();
                TempData["SuccessMessage"] = "Create";
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }
    }
}