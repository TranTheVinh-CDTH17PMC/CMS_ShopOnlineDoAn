
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
    public class InOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly INhaCungCap NhaCungCap;
        private readonly IPhieuNhap PhieuNhap;
        private readonly ICTPhieuNhap CTPhieuNhap;
        private readonly ILoaiSP LoaiSP;
        private readonly ITemplatePrint TemplatePrint;
        private readonly INhanVien NhanVien;
        public InOrderController()
        {
            LoaiSP = new LoaiSPRepository();
            NguyenLieu = new NguyenLieuRepository();
            NhaCungCap = new NhaCungCapRepository();
            PhieuNhap = new PhieuNhapRepository();
            CTPhieuNhap = new ICTPhieuPhapRepository();
            TemplatePrint = new TemplatePrintRepository();
            NhanVien = new NhanVienRepository();
        }
        public InOrderController(INhanVien _NhanVien, ITemplatePrint _TemplatePrint,INguyenLieu _nl, INhaCungCap _ncc, IPhieuNhap _pn, ICTPhieuNhap _ctpn,ILoaiSP _loaisp)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            NhaCungCap = _ncc;
            PhieuNhap = _pn;
            CTPhieuNhap = _ctpn;
            TemplatePrint = _TemplatePrint;
            NhanVien = _NhanVien;
        }
        //
        // GET: /CMS_Sale/InOrder/
        public ActionResult Index(int? name,int? idncc,int? idnv)
        {
            IEnumerable<PhieuNhapViewModel> model = PhieuNhap.SelectAll().Select(
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
                    IsDelete = item.IsDetele,
                    IsPrint = item.IsPrint
                }
            ).ToList().OrderByDescending(x => x.NgayTao);
            ViewBag.ncc = NhaCungCap.SelectAll().Where(x => x.IsDelete != true);
            ViewBag.nhanvien = NhanVien.SelectAll();
            if(name!=null && name!=0)
            {
                model = model.Where(x => x.Id == name);
            }
            if (idncc != null && idncc != 0)
            {
                model = model.Where(x => x.IdNhaCungCap== idncc);
            }
            if (idnv != null && idnv != 0)
            {
                model = model.Where(x => x.IdNhanVien == idnv);
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(model);
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
        public ActionResult Edit(int id)
        {
            ViewBag.listloai = LoaiSP.SelectAll().Where(x => x.IsDelete != true).ToList();
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
        [HttpPost]
        public ActionResult Edit(PhieuNhapViewModel model)
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
                _pn.IsPrint = false;
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
                TempData["SuccessMessage"] = "Create";
                TaskController.CreateTask("Tạo phiếu nhập", ControllerName, Action, Areas, Helper.CurrentUser.Id, idpn);
                return RedirectToAction("Index");
            }
            catch
            {

            }
            return View();
        }
        public ActionResult Delete(string IdDelete)
        {
            var pn = PhieuNhap.SelectById(Int32.Parse(IdDelete));
            pn.IsDetele = true;
            PhieuNhap.Update(pn);
            PhieuNhap.Save();
            TempData["SuccessMessage"] = "Create";
            return RedirectToAction("Index");
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
        public ActionResult Print(bool ExportExcel,int? name, int? idncc, int? idnv)
        {
            var model = TemplatePrint.SelectById(1);
            IEnumerable<PhieuNhapViewModel> modellist = PhieuNhap.SelectAll().Select(
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
            ).OrderByDescending(x => x.NgayTao);
            if (name != null && name != 0)
            {
                modellist = modellist.Where(x => x.Id == name);
            }
            if (idncc != null && idncc != 0)
            {
                modellist = modellist.Where(x => x.IdNhaCungCap == idncc);
            }
            if (idnv != null && idnv != 0)
            {
                modellist = modellist.Where(x => x.IdNhanVien == idnv);
            }
            model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
            model.Content = model.Content.Replace("{NamePrint}", "Danh sách phiếu nhập");
            model.Content = model.Content.Replace("{NameStaff}", Helpers.Helper.CurrentUser.TenNV);
            model.Content = model.Content.Replace("{Datetime}", DateTime.Now.Date.ToString("dd/MM/yyyy"));

            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "PhieuNhap" + ".xls");
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
        string BuildHtml(IEnumerable<PhieuNhapViewModel> ls)
        {
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += " <th>Ngày tạo</th><th>Tên NCC</th><th>Tên NV</th><th>Tổng tiền</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"qty\">" + item.NgayTao + "</td>\r\n";
                list += " <td class=\"qty\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"qty\">" + item.TenNV + "</td>\r\n";
                list += "<td class=\"total\">" + item.TongTien + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            list += "<tr><td colspan=\"6\"class=\"grand total\"></td>\r\n";
            return list;
        }

        public ActionResult PrintNT(int Id)
        {
            var model = TemplatePrint.SelectById(3);
            var modellist = PhieuNhap.SelectById(Id);
            var diachi = modellist.NhaCungCap.Ten;
            model.Content = model.Content.Replace("{DataTable}", BuildHtmlNT(modellist.Id));
            model.Content = model.Content.Replace("{Ten}", "Hóa đơn nhập");
            model.Content = model.Content.Replace("{Nhacc}",modellist.NhaCungCap.Ten);
            model.Content = model.Content.Replace("{DiaChi}", diachi);
            model.Content = model.Content.Replace("{TongTien}", modellist.TongTien.ToString());
            model.Content = model.Content.Replace("{NguoiLap}", Helpers.Helper.CurrentUser.TenNV);
            model.Content = model.Content.Replace("{NgayLap}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            modellist.IsPrint = true;
            PhieuNhap.Update(modellist);
            PhieuNhap.Save();
            return View(model);
        }
        string BuildHtmlNT(int? id)
        {
            var ls = CTPhieuNhap.SelectAll().Where(x=>x.IdPhieuNhap == id);
            var i = 1;
            string list = "";
            foreach (var item in ls)
            {
                list += "<tr><td>" + i + "</td>\r\n";
                list += " <td>" + item.NguyenLieu.Ten + "</td>\r\n";
                list += " <td>" + item.SoLuong + "</td>\r\n";
                list += " <td>" + item.DonGia + "</td>\r\n";
                list += "<td>" + item.DonGia*item.SoLuong + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            return list;
        }
    }
}