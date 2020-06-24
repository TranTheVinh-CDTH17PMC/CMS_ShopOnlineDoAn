using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Crm.Controllers
{
    public class SupplierController : Controller
    {
        private readonly INhaCungCap nhacungcap;
        List<SelectListItem> Tpho = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Tp.HCM", Value = "1"},
                //new SelectListItem { Text = "Tây Ninh", Value = "2"}
            };
        List<SelectListItem> QuanHuyen = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Q1", Value = "1"},
                new SelectListItem { Text = "Q2", Value = "2"},
                new SelectListItem { Text = "Q3", Value = "3"},
                new SelectListItem { Text = "Q4", Value = "4"},
                new SelectListItem { Text = "Q5", Value = "5"},
                new SelectListItem { Text = "Q6", Value = "6"}
            };
        public SupplierController()
        {
            nhacungcap = new NhaCungCapRepository();
        }
        public SupplierController(INhaCungCap _ncc)
        {
            nhacungcap = _ncc;
        }
        // GET: Crm/Supplier
        public ActionResult Index()
        {
            return View();
        }

        // GET: Crm/Supplier/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Crm/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.ThanhPho = Tpho;
            ViewBag.QuanHuyen = QuanHuyen;
            return View();
        }
        // POST: Crm/Supplier/Create
        [HttpPost]
        public ActionResult Create(NhaCungCapViewModel model)
        {
            try
            {
                var _ncc = new NhaCungCap();
                AutoMapper.Mapper.Map(model, _ncc);
                _ncc.IsDelete = false;
                nhacungcap.Insert(_ncc);
                nhacungcap.Save();
                ViewBag.Message = String.Format("Thêm mới thành công!!!");
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

            }
            return View();
        }

        // GET: Crm/Supplier/Edit/5
        public ActionResult Edit(int id)
        {
            var _ncc = nhacungcap.SelectById(id);
            NhaCungCapViewModel model = new NhaCungCapViewModel();
            AutoMapper.Mapper.Map(_ncc,model);
            foreach (var x in Tpho)
            {
                if (x.Value == _ncc.ThanhPho)
                {
                    x.Selected = true;
                }
            }
            foreach (var y in QuanHuyen)
            {
                if (y.Value == _ncc.Quan)
                {
                    y.Selected = true;      
                }
            }
            ViewBag.ThanhPho = Tpho;
            ViewBag.QuanHuyen = QuanHuyen;
            return View(model);
        }

        // POST: Crm/Supplier/Edit/5
        [HttpPost]
        public ActionResult Edit(NhaCungCapViewModel model)
        {
            try
            {
                var _ncc = nhacungcap.SelectById(model.Id);
                AutoMapper.Mapper.Map(model, _ncc);
                nhacungcap.Update(_ncc);
                nhacungcap.Save();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

            }
            return View();
        }

        #region Delete
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var _ncc = nhacungcap.SelectById(Id);
                _ncc.IsDelete = true;
                nhacungcap.Update(_ncc);
                nhacungcap.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
