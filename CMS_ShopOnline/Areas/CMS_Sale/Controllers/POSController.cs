using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_ShopOnline.Helpers;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    public class POSController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private readonly IThanhPham ThanhPham;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        public POSController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            ThanhPham = new ThanhPhamRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new CTPhieuXuatRepository();
        }
        public POSController(IPhieuXuat _PhieuXuat,ICTPhieuXuat _CTPhieuXuat,INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP,IThanhPham _ThanhPham)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            ThanhPham = _ThanhPham;
            PhieuXuat = _PhieuXuat;
            CTPhieuXuat = _CTPhieuXuat;
        }
        //
        //
        // GET: /CMS_Sale/POS/
        public ActionResult Index()
        {
            ViewBag.ListType = LoaiSP.SelectAll().Where(x => x.IsDelete != true);  
            return View();
        }
        [HttpPost]
        public ActionResult Index(POSViewModel model)
        {
            try
            {
                var _phieuxuat = new PhieuXuat();
                AutoMapper.Mapper.Map(model, _phieuxuat);
                _phieuxuat.IdNhanVien = Helper.CurrentUser.Id;
                _phieuxuat.TrangThai = "Create";
                _phieuxuat.NgayTao = DateTime.Now;
                PhieuXuat.Insert(_phieuxuat);
                PhieuXuat.Save();
                var idpx = _phieuxuat.Id;
                foreach(var item in model.ListPOSDetails)
                {
                    var _ctphieuxuat = new CTPhieuXuat();
                    _ctphieuxuat.IdThanhPham = item.IdThanhPham;
                    _ctphieuxuat.SoLuong = item.SoLuong;
                    _ctphieuxuat.DonGia = item.DonGia;
                    _ctphieuxuat.IdPhieuXuat = idpx;
                    CTPhieuXuat.Insert(_ctphieuxuat);
                    CTPhieuXuat.Save();
                }
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CMS_Sale/Default/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CMS_Sale/Default/Create
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

        // GET: CMS_Sale/Default/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CMS_Sale/Default/Edit/5
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

        // GET: CMS_Sale/Default/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CMS_Sale/Default/Delete/5
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
        [HttpGet]
        public ActionResult ListProducts(int Id)
        {
            var q = ThanhPham.SelectAll().Where(x => x.IsDelete != true);
            if (Id > 0)
            {
                q = ThanhPham.SelectAll().Where(x => x.IsDelete != true && x.IdLoai == Id);
            }
            var model = q.Select(item => new ThanhPhamViewModel
            {
                Id = item.Id,
                Ten = item.Ten,
                NgayTao = item.NgayTao,
                IdLoai = item.IdLoai,
                TenLoai = item.LoaiSP.Ten,
                HinhAnh = item.HinhAnh,
                DonGia = item.DonGia,
                IdDVT = item.IdDVT,
                TenDVT = item.DonViTinh.Ten,
                IsDelete = item.IsDelete
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListProductsById(int Id)
        {
            var q = ThanhPham.SelectAll().Where(x => x.IsDelete != true && x.Id==Id).ToList();
            var model = q.Select(item => new ThanhPhamViewModel
            {
                Id = item.Id,
                Ten = item.Ten,
                NgayTao = item.NgayTao,
                IdLoai = item.IdLoai,
                TenLoai = item.LoaiSP.Ten,
                HinhAnh = item.HinhAnh,
                DonGia = item.DonGia,
                IdDVT = item.IdDVT,
                TenDVT = item.DonViTinh.Ten,
                IsDelete = item.IsDelete
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}