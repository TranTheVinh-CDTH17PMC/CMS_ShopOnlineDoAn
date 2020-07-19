using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using CMS_ShopOnline.Filter;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_ShopOnline.Helpers;
using CMS_ShopOnline.Areas.Administration.Controllers;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class OutOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly ILoaiSP LoaiSP;
        public OutOrderController()
        {
            LoaiSP = new LoaiSPRepository();
            NguyenLieu = new NguyenLieuRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
        }
        public OutOrderController(INguyenLieu _nl, IPhieuXuat _px, ICTPhieuXuat _ctpx, ILoaiSP _loaisp)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            PhieuXuat = _px;
            CTPhieuXuat = _ctpx;
        }
        // GET: CMS_Sale/OutOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new PhieuXuatViewModel
            {
                listNguyenLieu = NguyenLieu.SelectAll().Where(x => x.IsDelete != true)
            };
            ViewBag.listloai = LoaiSP.SelectAll().Where(x => x.IsDelete != true).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PhieuXuatViewModel model)
        {
            try
            {
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string Action = this.ControllerContext.RouteData.Values["action"].ToString();
                string Areas = "CMS_Sale";
                var _px = new PhieuXuat();
                AutoMapper.Mapper.Map(model, _px);
                _px.IdNhanVien = Helper.CurrentUser.Id;
                _px.NgayTao = DateTime.Now;
                _px.IsDelete = false;
                PhieuXuat.Insert(_px);
                PhieuXuat.Save();
                var idpx = _px.Id;
                foreach (var item in model.ListCTPhieuXuat)
                {
                    var _ctpx = new CTPhieuXuat();
                    _ctpx.IdNguyenLieu = item.IdNguyenLieu;
                    _ctpx.SoLuong = item.SoLuong;
                    _ctpx.DonGia = item.DonGia;
                    _ctpx.IdPhieuXuat = idpx;
                    CTPhieuXuat.Insert(_ctpx);
                    CTPhieuXuat.Save();
                    var _nl = NguyenLieu.SelectById(item.IdNguyenLieu);
                    var tong = _nl.SoLuongKho - item.SoLuong;
                    _nl.SoLuongKho = tong;
                    _nl.NgayNhap = DateTime.Now;
                    NguyenLieu.Update(_nl);
                    NguyenLieu.Save();
                }
                TaskController.CreateTask("Tạo phiếu xuất", ControllerName, Action, Areas, Helper.CurrentUser.Id, idpx);
                return RedirectToAction("Index");
            }
            catch
            {

            }
            return View();
        }
        public ActionResult Search(string name, int? Idloai)
        {
            name = Helper.ChuyenThanhKhongDau(name);
            var q = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.Ten).Contains(name)).ToList();
            if (Idloai != null)
            {
                q = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.Ten).Contains(name) && x.IdLoai == Idloai).ToList();
            }
            var model = q.Select(item => new NguyenLieuViewModel
            {
                Id = item.Id,
                Ten = item.Ten,
                NgayNhap = item.NgayNhap,
                IdLoai = item.IdLoai,
                TenLoai = item.LoaiSP.Ten,
                HinhAnh = item.HinhAnh,
                DonGia = item.DonGia,
                IdDVT = item.IdDVT,
                TenDVT = item.DonViTinh.Ten,
                SoLuongKho = item.SoLuongKho,
                IsDelete = item.IsDelete
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListProductsById(int Id)
        {
            var q = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && x.Id == Id).ToList();
            var model = q.Select(item => new NguyenLieuViewModel
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