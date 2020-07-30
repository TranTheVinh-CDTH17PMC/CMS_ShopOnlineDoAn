using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    public class ProductTypeController : Controller
    {
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        public ProductTypeController()
        {
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
        }
        public ProductTypeController(IDonViTinh _DVT, ILoaiSP _LoaiSP)
        {
            DVT = _DVT;
            LoaiSP = _LoaiSP;
        }
        // GET: CMS_Sale/ProductType
        public ActionResult Index()
        {
            IEnumerable<TypeViewModel> ListType = LoaiSP.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new TypeViewModel
                {
                    Id= item.Id,
                    Ten = item.Ten,
                    IsDelete = item.IsDelete,
                });
            ViewBag.ListType = ListType;
            IEnumerable<UnitViewModel> ListUnit = DVT.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new UnitViewModel
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    IsDelete = item.IsDelete,
                });
            ViewBag.ListUnit = ListUnit;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        // GET: CMS_Sale/ProductType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CMS_Sale/ProductType/Create
        public ActionResult CreateType()
        {
            return View();
        }

        // POST: CMS_Sale/ProductType/Create
        [HttpPost]
        public ActionResult CreateType(UnitViewModel model)
        {
            try
            {
                var _Type = new LoaiSP();
                _Type.Ten = model.Ten;
                _Type.IsDelete = false;
                LoaiSP.Insert(_Type);
                LoaiSP.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult CreateUnit()
        {
            return View();
        }

        // POST: CMS_Sale/ProductType/Create
        [HttpPost]
        public ActionResult CreateUnit(UnitViewModel model)
        {
            try
            {
                var _Unit = new DonViTinh();
                _Unit.Ten = model.Ten;
                _Unit.IsDelete = false;
                DVT.Insert(_Unit);
                DVT.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CMS_Sale/ProductType/Edit/5
        // POST: CMS_Sale/ProductType/Edit/5
        [HttpPost]
        public ActionResult EditType(string SubmitType, string NameUnit)
        {

            try
            {
                    var _Type = LoaiSP.SelectById(int.Parse(SubmitType));
                    _Type.Ten = NameUnit;
                    _Type.IsDelete = false;
                    LoaiSP.Update(_Type);
                    LoaiSP.Save();
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditUnit(string SubmitUnit, string NameUnit)
        {

            try
            {

                    var _Unit = DVT.SelectById(int.Parse(SubmitUnit));
                    _Unit.Ten = NameUnit;
                    _Unit.IsDelete = false;
                    DVT.Update(_Unit);
                    DVT.Save();
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: CMS_Sale/ProductType/Delete/5

        // POST: CMS_Sale/ProductType/Delete/5
        [HttpPost]
        public ActionResult DeleteUnit(string IdDelete)
        {
            try
            {
                var _Unit = DVT.SelectById(int.Parse(IdDelete));
                _Unit.IsDelete = true;
                DVT.Update(_Unit);
                DVT.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult DeleteType(string IdDelete)
        {
            try
            {
                var _Type = LoaiSP.SelectById(int.Parse(IdDelete));
                _Type.IsDelete = true;
                LoaiSP.Update(_Type);
                LoaiSP.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
