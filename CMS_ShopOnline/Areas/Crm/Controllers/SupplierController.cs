﻿using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Crm.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly INhaCungCap nhacungcap;
        List<SelectListItem> Tpho = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Tp.HCM", Value = "TPHCM"},
                //new SelectListItem { Text = "Tây Ninh", Value = "2"}
            };
        List<SelectListItem> QuanHuyen = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Q1", Value = "Q1"},
                new SelectListItem { Text = "Q2", Value = "Q2"},
                new SelectListItem { Text = "Q3", Value = "Q3"},
                new SelectListItem { Text = "Q4", Value = "Q4"},
                new SelectListItem { Text = "Q5", Value = "Q5"},
                new SelectListItem { Text = "Q6", Value = "Q6"},
                new SelectListItem { Text = "Q7", Value = "Q7"},
                new SelectListItem { Text = "Q8", Value = "Q8"},
                new SelectListItem { Text = "Q9", Value = "Q9"},
                new SelectListItem { Text = "Q10", Value = "Q10"},
                new SelectListItem { Text = "Q11", Value = "Q11"},
                new SelectListItem { Text = "Q12", Value = "Q12"},
                new SelectListItem { Text = "Q.Bình Thạnh", Value = "Q.Bình Thạnh"},
                new SelectListItem { Text = "Q.Tân Bình", Value = "Q.Tân Bình"},
                new SelectListItem { Text = "Q.Bình Tân", Value = "Q.Bình Tân"},
                new SelectListItem { Text = "Q.Bình Chánh", Value = "Q.Bình Chánh"},
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
        public ActionResult Index(string txtCode)
        {

            IEnumerable<NhaCungCapViewModel> model = nhacungcap.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new NhaCungCapViewModel
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    DiaChi = item.DiaChi,
                    Quan = item.Quan,
                    ThanhPho = item.ThanhPho,
                    SDT = item.SDT,
                    Email = item.Email,
                    IsDelete = item.IsDelete
                }).OrderBy(x=>x.Id);
            if (txtCode != null)
            {
                model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode))
                    || x.Id.ToString().Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode)))
                    || (Helpers.Helper.ChuyenThanhKhongDau(x.SDT).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode))));
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(model);
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
                TempData["SuccessMessage"] = "Create";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
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
                _ncc.Ten = model.Ten;
                _ncc.DiaChi = model.DiaChi;
                _ncc.Quan = model.Quan;
                _ncc.ThanhPho = model.ThanhPho;
                _ncc.SDT = model.SDT;
                _ncc.Email = model.Email;
                nhacungcap.Update(_ncc);
                nhacungcap.Save();
                TempData["SuccessMessage"] = "Edit";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(string IdDelete)
        {
            try
            {
                var _ncc = nhacungcap.SelectById(int.Parse(IdDelete));
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
