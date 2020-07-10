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

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class PurchaseOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private readonly IThanhPham ThanhPham;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly IKhachHang KhachHang;
        public PurchaseOrderController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            ThanhPham = new ThanhPhamRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
            KhachHang = new KhachHangRepository();
        }
        public PurchaseOrderController(IKhachHang _KhachHang, IPhieuXuat _PhieuXuat, ICTPhieuXuat _CTPhieuXuat, INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP, IThanhPham _ThanhPham)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            ThanhPham = _ThanhPham;
            PhieuXuat = _PhieuXuat;
            CTPhieuXuat = _CTPhieuXuat;
            KhachHang = _KhachHang;
        }
        //
        // GET: /CMS_Sale/PurchaseOrder/
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
            IEnumerable<PhieuXuatViewModel> model = PhieuXuat.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new PhieuXuatViewModel
                {
                    Id = item.Id,
                    IdNhanVien = item.IdNhanVien,
                    IdKhachHang = item.IdKhachHang,
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    TenKH = item.KhachHang.TenKH,
                    GhiChu = item.GhiChu,
                    TrangThai = item.TrangThai,
                    TongTien = item.TongTien,
                }
            ).ToList().OrderByDescending(x=>x.NgayTao);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var px = PhieuXuat.SelectById(id);
            PhieuXuatViewModel model = new PhieuXuatViewModel();
            AutoMapper.Mapper.Map(px, model);
            model.TenKH = px.KhachHang.TenKH;
            model.TenNV = px.NhanVien.TenNV;
            var details = CTPhieuXuat.GetById(px.Id).Select(
                item => new POSDetailsViewModel
                {
                    Id = item.Id,
                    IdThanhPham = item.IdThanhPham,
                    Ten = item.ThanhPham.Ten,
                    IdPhieuXuat = item.IdPhieuXuat,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                }).ToList();
            model.ListPOSDetails = details;
            return View(model);
        }
        public ActionResult UpdateStatus(string Submit)
        {
            if(Submit != null)
            {
                var px = PhieuXuat.SelectById(int.Parse(Submit));
                if (px.TrangThai == "Create")
                {
                    px.TrangThai = "Processing";
                    PhieuXuat.Update(px);
                    PhieuXuat.Save();
                    return RedirectToAction("Details", "PurchaseOrder", new { id = int.Parse(Submit) });
                }
                if (px.TrangThai== "Processing")
                {
                    px.TrangThai = "Success";
                    PhieuXuat.Update(px);
                    PhieuXuat.Save();
                    return RedirectToAction("Details", "PurchaseOrder", new { id = int.Parse(Submit) });
                }  
            }
            return View();
        }
    }
}