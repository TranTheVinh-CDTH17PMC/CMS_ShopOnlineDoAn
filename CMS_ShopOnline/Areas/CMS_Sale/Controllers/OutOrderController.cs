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
    public class OutOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly ILoaiSP LoaiSP;
        private readonly ITemplatePrint TemplatePrint;
        private readonly INhanVien NhanVien;
        public OutOrderController()
        {
            LoaiSP = new LoaiSPRepository();
            NguyenLieu = new NguyenLieuRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
            TemplatePrint = new TemplatePrintRepository();
            NhanVien = new NhanVienRepository();
        }
        public OutOrderController(ITemplatePrint _TemplatePrint,INguyenLieu _nl, IPhieuXuat _px, ICTPhieuXuat _ctpx, ILoaiSP _loaisp, INhanVien _NhanVien)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            PhieuXuat = _px;
            CTPhieuXuat = _ctpx;
            TemplatePrint = _TemplatePrint;
            NhanVien = _NhanVien;
        }
        // GET: CMS_Sale/OutOrder
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? idnv)
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
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    GhiChu = item.GhiChu,
                    TongTien = item.TongTien,
                    IsDelete = item.IsDelete
                }
            ).ToList().OrderByDescending(x => x.NgayTao);
            ViewBag.nhanvien = NhanVien.SelectAll();
            if (idnv != null && idnv != 0)
            {
                model = model.Where(x => x.IdNhanVien == idnv);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int id)
        {
            var px = PhieuXuat.SelectById(id);
            PhieuXuatViewModel model = new PhieuXuatViewModel();
            AutoMapper.Mapper.Map(px, model);
            model.TenNV = px.NhanVien.TenNV;
            var details = CTPhieuXuat.GetById(px.Id).Select(
                item => new CTPhieuxuatViewModel
                {
                    Id = item.Id,
                    IdNguyenLieu = item.IdNguyenLieu,
                    Ten = item.NguyenLieu.Ten,
                    IdPhieuXuat = item.IdPhieuXuat,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                }).ToList();
            model.ListCTPhieuXuat = details;
            return View(model);
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
            catch(Exception e)
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
        public ActionResult Checksl(int id)
        {
            var q = NguyenLieu.GetSLbyId(id);
            return Json(q, JsonRequestBehavior.AllowGet);
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
        public ActionResult Print(string name, bool ExportExcel, int? idnv)
        {
            var model = TemplatePrint.SelectById(1);
            IEnumerable<PhieuXuatViewModel> modellist = PhieuXuat.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new PhieuXuatViewModel
                {
                    Id = item.Id,
                    IdNhanVien = item.IdNhanVien,
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    GhiChu = item.GhiChu,
                    TongTien = item.TongTien,
                    IsDelete = item.IsDelete
                }
            ).OrderByDescending(x => x.NgayTao);
            if (idnv != null && idnv != 0)
            {
                modellist = modellist.Where(x => x.IdNhanVien == idnv);
            }
            model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
            model.Content = model.Content.Replace("{NamePrint}", "Danh sách phiếu xuất");
            model.Content = model.Content.Replace("{NameStaff}", Helpers.Helper.CurrentUser.TenNV);
            model.Content = model.Content.Replace("{Datetime}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "PhieuXuat" + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title><style>body{font-size: 25pt;}table{border-collapse: collapse;width: 70%;}table, th, td {border: 1px solid black;}th{ text-align: center;}td{ text-align: center;}th, td {padding: 15px;}</style></head><body>";
                Response.Write(html);
                Response.Write(model.Content);
                Response.Write("</body></html>");
                Response.End();
            }
            return View(model);
        }
        string BuildHtml(IEnumerable<PhieuXuatViewModel> ls)
        {
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += " <th>Ngày tạo</th><th>Tên NV</th><th>Tổng tiền</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"qty\">" + item.NgayTao + "</td>\r\n";
                list += " <td class=\"qty\">" + item.TenNV + "</td>\r\n";
                list += "<td class=\"total\">" + item.TongTien + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            list += "<tr><td colspan=\"5\"class=\"grand total\"></td>\r\n";
            return list;
        }
    }
}