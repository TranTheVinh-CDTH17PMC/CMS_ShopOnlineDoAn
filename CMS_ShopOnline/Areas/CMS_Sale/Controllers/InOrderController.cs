
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
    public class InOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly INhaCungCap NhaCungCap;
        private readonly IPhieuNhap PhieuNhap;
        private readonly ICTPhieuNhap CTPhieuNhap;
        private readonly ILoaiSP LoaiSP;
        public InOrderController()
        {
            LoaiSP = new LoaiSPRepository();
            NguyenLieu = new NguyenLieuRepository();
            NhaCungCap = new NhaCungCapRepository();
            PhieuNhap = new PhieuNhapRepository();
            CTPhieuNhap = new ICTPhieuPhapRepository();
        }
        public InOrderController(INguyenLieu _nl, INhaCungCap _ncc, IPhieuNhap _pn, ICTPhieuNhap _ctpn,ILoaiSP _loaisp)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            NhaCungCap = _ncc;
            PhieuNhap = _pn;
            CTPhieuNhap = _ctpn;
        }
        //
        // GET: /CMS_Sale/InOrder/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
            IEnumerable<PhieuNhapViewModel> model = PhieuNhap.SelectAll().Where(x => x.IsDetele != true).Select(
                item => new PhieuNhapViewModel
                {
                    Id = item.Id,
                    IdNhanVien = item.IdNhanVien,
                    IdNhaCungCap = item.IdNhaCungCap,
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    Ten = item.NhaCungCap.Ten,
                    GhiChu = item.GhiChu,
                    TongTien = item.TongTien,
                    IsDelete = item.IsDetele
                }
            ).ToList().OrderByDescending(x => x.NgayTao);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int id)
        {
            var px = PhieuNhap.SelectById(id);
            PhieuNhapViewModel model = new PhieuNhapViewModel();
            AutoMapper.Mapper.Map(px, model);
            model.Ten = px.NhaCungCap.Ten;
            model.TenNV = px.NhanVien.TenNV;
            var details = CTPhieuNhap.GetById(px.Id).Select(
                item => new CTPhieuNhapViewModel
                {
                    Id = item.Id,
                    IdNguyenLieu = item.IdNguyenLieu,
                    Ten = item.NguyenLieu.Ten,
                    IdPhieuNhap = item.IdPhieuNhap,
                    Soluong = item.SoLuong,
                    DonGia = item.DonGia,
                }).ToList();
            model.ListCTPhieuNhap = details;
            return View(model);
        }
        public ActionResult Create()
        {
            var model = new PhieuNhapViewModel
            {
                listNguyenLieu = NguyenLieu.SelectAll().Where(x => x.IsDelete != true),
                listNhaCungCap = NhaCungCap.SelectAll().Where(x => x.IsDelete != true)
            };
            ViewBag.listloai = LoaiSP.SelectAll().Where(x => x.IsDelete != true).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PhieuNhapViewModel model)
        {
            try
            {
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string Action = this.ControllerContext.RouteData.Values["action"].ToString();
                string Areas = "CMS_Sale";
                var _pn = new PhieuNhap();
                AutoMapper.Mapper.Map(model, _pn);
                _pn.IdNhanVien = Helper.CurrentUser.Id;
                _pn.NgayTao = DateTime.Now;
                _pn.IsDetele = false;
                PhieuNhap.Insert(_pn);
                PhieuNhap.Save();
                var idpn = _pn.Id;
                foreach (var item in model.ListCTPhieuNhap)
                {
                    var _ctpn = new CTPhieuNhap();
                    _ctpn.IdNguyenLieu = item.IdNguyenLieu;
                    _ctpn.SoLuong = item.Soluong;
                    _ctpn.DonGia = item.DonGia;
                    _ctpn.IdPhieuNhap = idpn;
                    CTPhieuNhap.Insert(_ctpn);
                    CTPhieuNhap.Save();
                    var _nl = NguyenLieu.SelectById(item.IdNguyenLieu);
                    var tong = _nl.SoLuongKho + item.Soluong;
                    _nl.SoLuongKho = tong;
                    _nl.NgayNhap = DateTime.Now;
                    NguyenLieu.Update(_nl);
                    NguyenLieu.Save();
                }
                TaskController.CreateTask("Tạo phiếu nhập", ControllerName, Action, Areas, Helper.CurrentUser.Id, idpn);
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
            if(Idloai!=null)
            {
                q = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.Ten).Contains(name) && x.IdLoai==Idloai).ToList();
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