using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Administration.Models;
using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly ILoaiNV LoaiNV;
        private readonly IPhanQuyen PhanQuyen;
        private readonly IListController ListController;
        public SettingController()
        {
            LoaiNV = new LoaiNVRepository();
            PhanQuyen = new PhanQuyenRepository();
            ListController = new ListControllerRepository();
        }
        public SettingController(ILoaiNV _LoaiNV,IPhanQuyen _PhanQuyen,IListController _ListController)
        {
            LoaiNV = _LoaiNV;
            PhanQuyen = _PhanQuyen;
            ListController = _ListController;
        }
        // GET: Administration/Setting
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<PhanQuyenViewModel> model = ListController.SelectAll().Select(
                item => new PhanQuyenViewModel
                {
                    Id = item.Id,
                    ControllerName = item.ControllerName,
                    IsDelete = item.IsDelete,
                    DetailsPhanQuyen = Getphanquyen(item.Id),
                    LDetailsPhanQuyen = Getphanquyen(item.Id).ToList(),
                }).ToList();
            if(Helpers.Helper.IsGD()==true)
            {
                ViewBag.LoaiNV = LoaiNV.SelectAll();
            }
            else
            {
                ViewBag.LoaiNV = LoaiNV.SelectAll().Where(x=>x.TenCode != "GD");
            }
            //ViewBag.PhanQuyen = PhanQuyen.SelectAll();
            ViewBag.ListController = ListController.SelectAll();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(PhanQuyenViewModel model)
        {
           foreach(var item in model.DetailsPhanQuyen)
            {
                var _pq = PhanQuyen.SelectById(item.Id);
                _pq.IsDelete = item.IsDelete;
                PhanQuyen.Update(_pq);
                PhanQuyen.Save();
            }
            return RedirectToAction("Index");
        }
        public IEnumerable<DetailsPhanQuyenViewModel> Getphanquyen(int? name)
        {
            if (Helpers.Helper.IsGD() != true)
            {
                var idgd = LoaiNV.Selectbyname("GD");
                var model1 = PhanQuyen.SelectAll().Where(x => x.IdControllerName == name && x.IdRole != idgd.Id).Select(
                    item => new DetailsPhanQuyenViewModel
                    {
                        Id = item.Id,
                        IdControllerName = item.IdControllerName,
                        IdRole = item.IdRole,
                        IsDelete = item.IsDelete,
                    });
                return model1;
            }
            else
            {
                var model = PhanQuyen.SelectAll().Where(x => x.IdControllerName == name).Select(
               item => new DetailsPhanQuyenViewModel
               {
                   Id = item.Id,
                   IdControllerName = item.IdControllerName,
                   IdRole = item.IdRole,
                   IsDelete = item.IsDelete,
               });
                return model;
            }
           
        }
        // GET: Administration/Setting/Details/5
        public ActionResult Details(int id,string value)
        {
            var _pq = PhanQuyen.SelectById(id);
            if(value=="true")
            {
                _pq.IsDelete = false;
            }
            if (value == "false")
            {
                _pq.IsDelete = true;
            }
            PhanQuyen.Update(_pq);
            PhanQuyen.Save();
            return Json(_pq, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotPage()
        {
            return View();
        }
        // GET: Administration/Setting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Setting/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Setting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/Setting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Setting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administration/Setting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
