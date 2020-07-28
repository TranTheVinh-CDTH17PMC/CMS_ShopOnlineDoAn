using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using CMS_ShopOnline.Filter;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class SaleReportController : Controller
    {
        DBConnection _db = new DBConnection();
        private readonly INguyenLieu NguyenLieu;
        private DateTime datetimesetting = new DateTime(2010, 10, 10, 1, 1, 1);
        private readonly ITemplatePrint TemplatePrint;

        public SaleReportController()
        {
            NguyenLieu = new NguyenLieuRepository();
            TemplatePrint = new TemplatePrintRepository();
        }
        public SaleReportController(INguyenLieu _NguyenLieu, ITemplatePrint _TemplatePrint)
        {
            NguyenLieu = _NguyenLieu;
            TemplatePrint = _TemplatePrint;
        }
        // GET: CMS_Sale/SaleReport
        public ActionResult TopSanPhamBanChay(string startDate, string endDate, string sortOrder, string currentFilter, string searchString, int? page)
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
            if (startDate == null && endDate == null && startDate != "" && endDate != "")
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                // Cộng thêm 1 tuần và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddDays(7).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    var model = _db.Database.SqlQuery<TopSanPhamBanChay>("exec topspbanchay @batdau,@ketthuc", new SqlParameter("@batdau", d_startDate), new SqlParameter("@ketthuc", d_endDate)).ToList();
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    return View(model.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var model = _db.Database.SqlQuery<TopSanPhamBanChay>("exec topspbanchay @batdau,@ketthuc", new SqlParameter("@batdau", startDate), new SqlParameter("@ketthuc", endDate)).ToList();
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }
        public ActionResult TopSanPhamKhongBanChay(string startDate, string endDate, string sortOrder, string currentFilter, string searchString, int? page)
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
            if (startDate == null && endDate == null && startDate != "" && endDate != "")
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                // Cộng thêm 1 tuần và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddDays(7).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    var model = _db.Database.SqlQuery<TopSanPhamBanCham>("exec topsanphambancham @batdau,@ketthuc", new SqlParameter("@batdau", d_startDate), new SqlParameter("@ketthuc", d_endDate)).ToList();
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    return View(model.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                var model = _db.Database.SqlQuery<TopSanPhamBanCham>("exec topsanphambancham @batdau,@ketthuc", new SqlParameter("@batdau", startDate), new SqlParameter("@ketthuc", endDate)).ToList();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }
        public ActionResult BaoCaoTonKho(string startDate, string endDate, string sortOrder, string currentFilter, string searchString, int? page)
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
            DateTime d_startDate, d_endDate;
            double? tongtien = 0;
            IEnumerable<NguyenLieuViewModel> model = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20 && x.NgayNhap != datetimesetting).Select(
                item => new NguyenLieuViewModel
                {
                    Id = item.Id,
                    TenLoai = item.LoaiSP.Ten,
                    IdLoai = item.IdLoai,
                    Ten = item.Ten,
                    HinhAnh = item.HinhAnh,
                    TenDVT = item.DonViTinh.Ten,
                    IdDVT = item.IdDVT,
                    DonGia = item.DonGia,
                    SoLuongKho = item.SoLuongKho,
                    IsDelete = item.IsDelete,
                    NgayNhap = item.NgayNhap
                }).ToList();
            if (startDate != null && endDate != null && startDate != "" && endDate != "")
            {
                d_startDate = Convert.ToDateTime(startDate);
                d_endDate = Convert.ToDateTime(endDate);
                model = model.Where(x => x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20 && x.NgayNhap != datetimesetting && x.NgayNhap >= d_startDate && x.NgayNhap <= d_endDate);
            }
            foreach (var item in model)
            {
                tongtien += item.DonGia * item.SoLuongKho;
            }
            ViewBag.tongtien = tongtien;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BaoCaoDoanhThuTheoThang(string s_year)
        {
            List<SelectListItem> Year = new List<SelectListItem>();
            DateTime d_year;
            double? tongtien = 0;
            var year = DateTime.Now.Year;
            for (int i = year - 3; i <= year + 3; i++)
            {
                Year.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            var model = _db.Database.SqlQuery<DoanhThuTheoTungThang>("exec DoanhThuTheoThang @Year", new SqlParameter("@Year", year)).ToList();
            if (s_year!=null && s_year!="")
            {
                model = _db.Database.SqlQuery<DoanhThuTheoTungThang>("exec DoanhThuTheoThang @Year", new SqlParameter("@Year", s_year)).ToList();
                year = Int32.Parse(s_year);
            }
            foreach(var item in model)
            {
                tongtien += item.Loinhuan;
            }
            ViewBag.year = Year.ToList();
            ViewBag.isyear = year.ToString();
            ViewBag.tongtien = tongtien;
            return View(model);
        }
        public ActionResult BaoCaoDoanhThuTheoNgay(string s_year,string s_month)
        {
            List<SelectListItem> Year = new List<SelectListItem>();
            List<SelectListItem> Month = new List<SelectListItem>();
            DateTime d_year;
            double? tongtien = 0;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            for (int i = year - 3; i <= year + 3; i++)
            {
                Year.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 12; i++)
            {
                Month.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            var model = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", month), new SqlParameter("@Year", year)).ToList();
            if (s_year != null && s_year != "" && s_month != null && s_month != "")
            {
                model = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", s_month), new SqlParameter("@Year", s_year)).ToList();
                year = Int32.Parse(s_year);
                month = Int32.Parse(s_month);
            }
            
            foreach (var item in model)
            {
                tongtien += item.Loinhuan;
            }
            ViewBag.year = Year.ToList();
            ViewBag.moth = Month.ToList();
            ViewBag.isyear = year.ToString();
            ViewBag.ismonth = month.ToString();
            ViewBag.tongtien = tongtien;
            return View(model);
        }
        public int ngay(DateTime ngaynhap)
        {
            int i = 0;
            TimeSpan diff1 = DateTime.Now.Subtract(ngaynhap);
            i = diff1.Days;
            return i;
        }
        public ActionResult ExportToExcel(string name,string startDate, string endDate,bool ExportExcel, string s_month, string s_year)
        {
            DateTime d_startDate, d_endDate;
            Encoding encoding;
            try
            {
                var model = TemplatePrint.SelectById(1);
                if (name == "doanhthutheothang")
                {
                    List<SelectListItem> Year = new List<SelectListItem>();
                    DateTime d_year;
                    double? tongtien = 0;
                    var year = DateTime.Now.Year;
                    for (int i = year - 3; i <= year + 3; i++)
                    {
                        Year.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                    }
                    if (s_year != null && s_year != "")
                    {
                        var modellist = _db.Database.SqlQuery<DoanhThuTheoTungThang>("exec DoanhThuTheoThang @Year", new SqlParameter("@Year", year)).ToList();
                        model.Content = model.Content.Replace("{Table}", BuildHtmlDoanhthutheothang(modellist));
                        model.Content = model.Content.Replace("{NamePrint}", "Báo cáo doanh thu theo tháng");
                        if (ExportExcel)
                        {
                            encoding = Encoding.UTF8;
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "DoanhThuTheoThang" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                            Response.Write(html);
                            Response.Write(model.Content);
                            Response.Write("</body></html>");
                            Response.End();
                        }
                        return View(model);

                    }
                    else
                    {
                        var modellist = _db.Database.SqlQuery<DoanhThuTheoTungThang>("exec DoanhThuTheoThang @Year", new SqlParameter("@Year", year)).ToList();
                        model.Content = model.Content.Replace("{Table}", BuildHtmlDoanhthutheothang(modellist));
                        model.Content = model.Content.Replace("{NamePrint}", "Báo cáo doanh thu theo tháng");
                        if (ExportExcel)
                        {
                            encoding = Encoding.UTF8;
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "DoanhThuTheoThang" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                            Response.Write(html);
                            Response.Write(model.Content);
                            Response.Write("</body></html>");
                            Response.End();
                        }
                        return View(model);
                    }
                }
                if (name == "doanhthutheongay")
                {
                    List<SelectListItem> Year = new List<SelectListItem>();
                    List<SelectListItem> Month = new List<SelectListItem>();
                    DateTime d_year;
                    double? tongtien = 0;
                    var year = DateTime.Now.Year;
                    var month = DateTime.Now.Month;
                    for (int i = year - 3; i <= year + 3; i++)
                    {
                        Year.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        Month.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                    }
                    if (s_year != null && s_year != "" && s_month != null && s_month != "")
                    {

                            var modellist = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", month), new SqlParameter("@Year", year)).ToList();
                            model.Content = model.Content.Replace("{Table}", BuildHtmlDoanhthutheotungngay(modellist));
                            model.Content = model.Content.Replace("{NamePrint}", "Báo cáo doanh thu theo ngày");
                            if (ExportExcel)
                            {
                                encoding = Encoding.UTF8;
                                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "DoanhThuTheoNgay" + ".xls");
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                                Response.Write(html);
                                Response.Write(model.Content);
                                Response.Write("</body></html>");
                                Response.End();
                            }
                            return View(model);
                        
                    }
                    else
                    {
                        var modellist = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", month), new SqlParameter("@Year", year)).ToList();
                        model.Content = model.Content.Replace("{Table}", BuildHtmlDoanhthutheotungngay(modellist));
                        model.Content = model.Content.Replace("{NamePrint}", "Báo cáo doanh thu theo ngày");
                        if (ExportExcel)
                        {
                            encoding = Encoding.UTF8;
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "DoanhThuTheoNgay" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                            Response.Write(html);
                            Response.Write(model.Content);
                            Response.Write("</body></html>");
                            Response.End();
                        }
                        return View(model);
                    }

                }
                if (name=="baocaotonkho")
                {
                    IEnumerable<NguyenLieuViewModel> modellist = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20 && x.NgayNhap != datetimesetting).Select(
                     item => new NguyenLieuViewModel
                     {
                         Id = item.Id,
                         TenLoai = item.LoaiSP.Ten,
                         IdLoai = item.IdLoai,
                         Ten = item.Ten,
                         HinhAnh = item.HinhAnh,
                         TenDVT = item.DonViTinh.Ten,
                         IdDVT = item.IdDVT,
                         DonGia = item.DonGia,
                         SoLuongKho = item.SoLuongKho,
                         IsDelete = item.IsDelete,
                         NgayNhap = item.NgayNhap
                     }).ToList();
                    if (startDate != null && endDate != null && startDate != "" && endDate != "")
                    {
                        d_startDate = Convert.ToDateTime(startDate);
                        d_endDate = Convert.ToDateTime(endDate);
                        modellist = modellist.Where(x => x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20 && x.NgayNhap != datetimesetting && x.NgayNhap >= d_startDate && x.NgayNhap <= d_endDate);
                    }
                    model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
                    model.Content = model.Content.Replace("{NamePrint}", "Bao cao ton kho");
                    if (ExportExcel)
                    {
                        Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BaoCaoTonKho" + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Write(model.Content);
                        Response.End();
                    }
                    return View(model);
                }
                if (name == "spbanchay")
                {
                    if (startDate == null && endDate == null || startDate == "" && endDate == "")
                    {
                        DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        // Cộng thêm 1 tuần và trừ đi một ngày.
                        DateTime retDateTime = aDateTime.AddDays(7).AddDays(-1);
                        startDate = aDateTime.ToString("dd/MM/yyyy");
                        endDate = retDateTime.ToString("dd/MM/yyyy");
                    }
                    if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
                    {
                        if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                        {
                           var modellist = _db.Database.SqlQuery<TopSanPhamBanChay>("exec topspbanchay @batdau,@ketthuc", new SqlParameter("@batdau", d_startDate), new SqlParameter("@ketthuc", d_endDate)).ToList();
                            model.Content = model.Content.Replace("{Table}", BuildHtmlspbanchay(modellist));
                            model.Content = model.Content.Replace("{NamePrint}", "Top san pham ban chay");
                            if (ExportExcel)
                            {
                                encoding = Encoding.UTF8;
                                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BaoCaoBanChay" + ".xls");
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                                Response.Write(html);
                                Response.Write(model.Content);
                                Response.Write("</body></html>");
                                Response.End();
                            }
                            return View(model);
                        }
                    }
                    else
                    {
                        var modellist  = _db.Database.SqlQuery<TopSanPhamBanChay>("exec topspbanchay @batdau,@ketthuc", new SqlParameter("@batdau", startDate), new SqlParameter("@ketthuc", endDate)).ToList();
                        model.Content = model.Content.Replace("{Table}", BuildHtmlspbanchay(modellist));
                        model.Content = model.Content.Replace("{NamePrint}", "Top san pham ban chay");
                        if (ExportExcel)
                        {
                            encoding = Encoding.UTF8;
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BaoCaoBanChay" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                            Response.Write(html);
                            Response.Write(model.Content);
                            Response.Write("</body></html>");
                            Response.End();
                        }
                        return View(model);
                    }
                   
                }                
                if (name == "spbankhongchay")
                {
                    if (startDate == null && endDate == null || startDate == "" && endDate == "")
                    {
                        DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        // Cộng thêm 1 tuần và trừ đi một ngày.
                        DateTime retDateTime = aDateTime.AddDays(7).AddDays(-1);
                        startDate = aDateTime.ToString("dd/MM/yyyy");
                        endDate = retDateTime.ToString("dd/MM/yyyy");
                    }
                    if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
                    {
                        if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                        {
                            encoding = Encoding.UTF8;
                            var modellist = _db.Database.SqlQuery<TopSanPhamBanCham>("exec topsanphambancham @batdau,@ketthuc", new SqlParameter("@batdau", d_startDate), new SqlParameter("@ketthuc", d_endDate)).ToList();
                            model.Content = model.Content.Replace("{Table}", BuildHtmlspbancham(modellist));
                            model.Content = model.Content.Replace("{NamePrint}", "Top san pham ban cham");
                            if (ExportExcel)
                            {
                                encoding = Encoding.UTF8;
                                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BaoCaoBanCham" + ".xls");
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                                Response.Write(html);
                                Response.Write(model.Content);
                                Response.Write("</body></html>");
                                Response.End();
                            }
                            return View(model);
                        }
                    }
                    else
                    {
                        var modellist = _db.Database.SqlQuery<TopSanPhamBanCham>("exec topsanphambancham @batdau,@ketthuc", new SqlParameter("@batdau", startDate), new SqlParameter("@ketthuc", endDate)).ToList();
                        model.Content = model.Content.Replace("{Table}", BuildHtmlspbancham(modellist));
                        model.Content = model.Content.Replace("{NamePrint}", "Top san pham ban cham");
                        if (ExportExcel)
                        {
                            encoding = Encoding.UTF8;
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BaoCaoBanCham" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title></head ><body>";
                            Response.Write(html);
                            Response.Write(model.Content);
                            Response.Write("</body></html>");
                            Response.End();
                        }
                        return View(model);
                    }

                }

            }
            catch (Exception e)
            {

            }
            return View();
        }
        string BuildHtml(IEnumerable<NguyenLieuViewModel> ls)
        {
            double? tongbill = 0;
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += " <th>Ngày nhập</th><th>Tên</th><th>Tên loại</th><th>ĐVT</th><th>Số lượng kho</th><th>Đơn giá</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">"+i+"</td>\r\n";
                list += "<td class=\"desc\">"+item.Id+"</td>\r\n";
                list += " <td class=\"unit\">"+item.NgayNhap+"</td>\r\n";
                list += " <td class=\"qty\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"qty\">"+item.TenLoai+"</td>\r\n";
                list += "<td class=\"total\">"+item.TenDVT+"</td>\r\n";
                list += "<td class=\"total\">" + item.SoLuongKho + "</td>\r\n";
                list += "<td class=\"total\">" + item.DonGia + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
                tongbill += item.DonGia * item.SoLuongKho;
            }
            list += "<tr><td colspan=\"7\"class=\"grand total\">Tổng tiền</td>\r\n";
            list += " <td class=\"grand total\">"+ tongbill + "</td></tr></tbody>\r\n";
            return list;
        }
        string BuildHtmlspbanchay(List<TopSanPhamBanChay> ls)
        {
            double? tongbill = 0;
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += "<th>Tên</th> <th>Đơn giá</th><th>ĐVT</th><th>Loại</th><th>Tổng số lượng</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"unit\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"unit\">" + item.DonGia + "</td>\r\n";
                list += "<td class=\"total\">" + item.IdDVT + "</td>\r\n";
                list += " <td class=\"qty\">" + item.IdLoai + "</td>\r\n";
                list += "<td class=\"total\">" + item.Tongsl + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            return list;
        }
        string BuildHtmlspbancham(List<TopSanPhamBanCham> ls)
        {
            double? tongbill = 0;
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += "<th>Tên</th> <th>Đơn giá</th><th>ĐVT</th><th>Loại</th><th>Tổng số lượng</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"unit\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"unit\">" + item.DonGia + "</td>\r\n";
                list += "<td class=\"total\">" + item.IdDVT + "</td>\r\n";
                list += " <td class=\"qty\">" + item.IdLoai + "</td>\r\n";
                list += "<td class=\"total\">" + item.Tongsl + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            return list;
        }
        string BuildHtmlDoanhthutheotungngay(List<DoanhThuTheoTungNgay> ls)
        {
            double? tongbill = 0;
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th>";
            list += "<th>Ngày</th><th>Tổng tiền thu</th><th>Tổng tiền chi</th><th>Tiền lãi</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Ngay+ "</td>\r\n";
                list += " <td class=\"unit\">" + item.Tongthu + "</td>\r\n";
                list += " <td class=\"unit\">" + item.Tongchi + "</td>\r\n";
                list += "<td class=\"total\">" + item.Loinhuan + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            return list;
        }
        string BuildHtmlDoanhthutheothang(List<DoanhThuTheoTungThang> ls)
        {
            double? tongbill = 0;
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th>";
            list += "<th>Tháng</th><th>Tổng tiền thu</th><th>Tổng tiền chi</th><th>Tiền lãi</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Thang + "</td>\r\n";
                list += " <td class=\"unit\">" + item.Tongthu + "</td>\r\n";
                list += " <td class=\"unit\">" + item.Tongchi + "</td>\r\n";
                list += "<td class=\"total\">" + item.Loinhuan + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            return list;
        }
    }
}