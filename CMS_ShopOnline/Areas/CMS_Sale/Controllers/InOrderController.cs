
﻿using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
﻿using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_ShopOnline.Helpers;

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
        public InOrderController()
        {
            NguyenLieu = new NguyenLieuRepository();
            NhaCungCap = new NhaCungCapRepository();
            PhieuNhap = new PhieuNhapRepository();
            CTPhieuNhap = new CTPhieuNhapRepository();
        }
        public InOrderController(INguyenLieu _nl, INhaCungCap _ncc, IPhieuNhap _pn, ICTPhieuNhap _ctpn)
        {
            NguyenLieu = _nl;
            NhaCungCap = _ncc;
            PhieuNhap = _pn;
            CTPhieuNhap = _ctpn;
        }
        //
        // GET: /CMS_Sale/InOrder/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new PhieuNhapViewModel
            {
                listNguyenLieu = NguyenLieu.SelectAll().Where(x => x.IsDelete != true),
                listNhaCungCap = NhaCungCap.SelectAll().Where(x => x.IsDelete != true)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PhieuNhapViewModel model)
        {
            return View();
        }
        public ActionResult Search(string name)
        {
            name = Helper.ChuyenThanhKhongDau(name);
            var q = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.Ten).Contains(name)).ToList();
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
    }
}