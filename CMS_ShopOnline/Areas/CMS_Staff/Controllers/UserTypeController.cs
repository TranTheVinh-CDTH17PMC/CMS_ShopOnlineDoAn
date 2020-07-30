using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Staff.Models;
using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Staff.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class UserTypeController : Controller
    {
        private readonly ILoaiNV LoaiNV;
        // GET: CMS_Staff/UserType
        public UserTypeController()
        {
            LoaiNV = new LoaiNVRepository();
        }
        public UserTypeController(ILoaiNV _LoaiNV)
        {
            LoaiNV = _LoaiNV;
        }
        public ActionResult Index()
        {
            IEnumerable<LoainvViewModel> ListType = LoaiNV.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new LoainvViewModel
                {
                    Id = item.Id,
                    TenLoai = item.TenLoai,
                    IsDelete = item.IsDelete,
                });
            ViewBag.ListType = ListType;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        // GET: CMS_Staff/UserType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CMS_Staff/UserType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CMS_Staff/UserType/Create
        [HttpPost]
        public ActionResult Create(LoainvViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                var _Type = new LoaiNV();
                _Type.TenLoai = model.TenLoai;
                _Type.IsDelete = false;
                LoaiNV.Insert(_Type);
                LoaiNV.Save();
                TempData["SuccessMessage"] = "Create";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        // POST: CMS_Staff/UserType/Edit/5
        [HttpPost]
        public ActionResult Edit(string SubmitType, string NameUnit)
        {
            try
            {
                var _loainv = LoaiNV.SelectById(int.Parse(SubmitType));
                _loainv.TenLoai = NameUnit;
                _loainv.IsDelete = false;
                LoaiNV.Update(_loainv);
                LoaiNV.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: CMS_Staff/UserType/Delete/5
        [HttpPost]
        public ActionResult Delete(string IdDelete)
        {
            try
            {
                // TODO: Add delete logic here
                var _Type = LoaiNV.SelectById(int.Parse(IdDelete));
                _Type.IsDelete = true;
                LoaiNV.Update(_Type);
                LoaiNV.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
