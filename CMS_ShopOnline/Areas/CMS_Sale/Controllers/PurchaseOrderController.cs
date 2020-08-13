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
    public class PurchaseOrderController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private readonly IThanhPham ThanhPham;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly IKhachHang KhachHang;
        private readonly IHoaDon HoaDon;
        private readonly ICTHoaDon CTHoaDon;
        private readonly ITemplatePrint TemplatePrint;
        private readonly INhanVien NhanVien;
        public PurchaseOrderController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            ThanhPham = new ThanhPhamRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
            KhachHang = new KhachHangRepository();
            HoaDon = new HoaDonRepository();
            CTHoaDon = new CTHoaDonRepository();
            TemplatePrint = new TemplatePrintRepository();
            NhanVien = new NhanVienRepository();
        }
        public PurchaseOrderController(INhanVien _NhanVien, ITemplatePrint _TemplatePrint, IHoaDon _HoaDon, ICTHoaDon _CTHoaDon, IKhachHang _KhachHang, IPhieuXuat _PhieuXuat, ICTPhieuXuat _CTPhieuXuat, INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP, IThanhPham _ThanhPham)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            ThanhPham = _ThanhPham;
            PhieuXuat = _PhieuXuat;
            CTPhieuXuat = _CTPhieuXuat;
            KhachHang = _KhachHang;
            HoaDon = _HoaDon;
            CTHoaDon = _CTHoaDon;
            TemplatePrint = _TemplatePrint;
            NhanVien = _NhanVien;
        }
        public string checknullkh(int? tenkh)
        {
            string name = "Rỗng";
            if(tenkh!=null)
            {
                var getname = KhachHang.GetNameById(tenkh);
                name = getname.TenKH.ToString();
            }
            return name;
        }
        //
        // GET: /CMS_Sale/PurchaseOrder/
        public ActionResult Index(int? idnv,int? id,string sortOrder, string currentFilter, string searchString, int? page)
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
            IEnumerable<HoaDonViewModel> model = HoaDon.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new HoaDonViewModel
                {
                    Id = item.Id,
                    IdNhanVien = item.IdNhanVien,
                    IdKhachHang = item.IdKhachHang,
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    TenKH = checknullkh(item.IdKhachHang),
                    GhiChu = item.GhiChu,
                    TrangThai = item.TrangThai,
                    TongTien = item.TongTien,
                    TongKM = item.TongKM,
                    IsDelete = item.IsDelete
                }
            ).ToList().OrderByDescending(x=>x.NgayTao);
            ViewBag.nhanvien = NhanVien.SelectAll();
            if (idnv!=null)
            {
                model = model.Where(x => x.IdNhanVien == idnv).ToList();
            }
            if (id != null)
            {
                model = model.Where(x => x.Id.ToString().Contains(id.ToString())).ToList();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete(string IdDelete)
        {
            if(IdDelete!=null && IdDelete!="")
            {
                var px = HoaDon.SelectById(Int32.Parse(IdDelete));
                px.TrangThai = "Delete";
                HoaDon.Update(px);
                HoaDon.Save();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var px = HoaDon.SelectById(id);
            HoaDonViewModel model = new HoaDonViewModel();
            AutoMapper.Mapper.Map(px, model);
            model.TenKH = checknullkh(px.IdKhachHang);
            model.TenNV = px.NhanVien.TenNV;
            var details = CTHoaDon.GetById(px.Id).Select(
                item => new CTHoaDonViewModel
                {
                    Id = item.Id,
                    IdThanhPham = item.IdThanhPham,
                    Ten = item.ThanhPham.Ten,
                    IdHoaDon = item.IdHoaDon,
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
                var px = HoaDon.SelectById(int.Parse(Submit));
                if (px.TrangThai == "Create")
                {
                    px.TrangThai = "Processing";
                    HoaDon.Update(px);
                    HoaDon.Save();
                    return RedirectToAction("Details", "PurchaseOrder", new { id = int.Parse(Submit) });
                }
                if (px.TrangThai== "Processing")
                {
                    px.TrangThai = "Success";
                    HoaDon.Update(px);
                    HoaDon.Save();
                    return RedirectToAction("Details", "PurchaseOrder", new { id = int.Parse(Submit) });
                }  
            }
            return View();
        }
        public ActionResult Print(int? idnv, int? id,string name, bool ExportExcel)
        {
            var model = TemplatePrint.SelectById(1);
            IEnumerable<HoaDonViewModel> modellist = HoaDon.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new HoaDonViewModel
                {
                    Id = item.Id,
                    IdNhanVien = item.IdNhanVien,
                    IdKhachHang = item.IdKhachHang,
                    NgayTao = item.NgayTao,
                    TenNV = item.NhanVien.TenNV,
                    TenKH = checknullkh(item.IdKhachHang),
                    GhiChu = item.GhiChu,
                    TrangThai = item.TrangThai,
                    TongTien = item.TongTien,
                    TongKM = item.TongKM,
                    IsDelete = item.IsDelete
                }
            ).OrderByDescending(x => x.NgayTao);
            if (idnv != null)
            {
                modellist = modellist.Where(x => x.IdNhanVien == idnv).ToList();
            }
            if (id != null)
            {
                modellist = modellist.Where(x => x.IdNhanVien == id).ToList();
            }
            model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
            model.Content = model.Content.Replace("{NamePrint}", "Danh sách hóa đơn");
            model.Content = model.Content.Replace("{NameStaff}", Helpers.Helper.CurrentUser.TenNV);
            model.Content = model.Content.Replace("{Datetime}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "HoaDon" + ".xls");
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
        string BuildHtml(IEnumerable<HoaDonViewModel> ls)
        {
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += " <th>Ngày tạo</th><th>Tên NV</th><th>Tên KH</th><th>Tổng tiền</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"qty\">" + item.NgayTao + "</td>\r\n";
                list += " <td class=\"qty\">" + item.TenNV + "</td>\r\n";
                list += " <td class=\"unit\">" + item.TenKH + "</td>\r\n";
                list += "<td class=\"total\">" + item.TongTien + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            list += "<tr><td colspan=\"6\"class=\"grand total\"></td>\r\n";
            return list;
        }
    }
}