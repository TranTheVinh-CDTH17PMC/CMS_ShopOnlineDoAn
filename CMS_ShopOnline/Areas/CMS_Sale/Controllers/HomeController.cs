using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Database.Entities;
using CMS_Database.Repositories;
using CMS_Database.Interfaces;
using CMS_ShopOnline.Areas.Administration.Models;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        DBConnection _db = new DBConnection();
        private readonly INguyenLieu NguyenLieu;
        private readonly INhaCungCap NhaCungCap;
        private readonly IPhieuNhap PhieuNhap;
        private readonly ICTPhieuNhap CTPhieuNhap;
        private readonly ILoaiSP LoaiSP;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly IHoaDon HoaDon;
        private readonly IKhuyenMai KhuyenMai;
        private DateTime datetimesetting = new DateTime(2010, 10, 10, 1, 1, 1);
        public HomeController()
        {
            LoaiSP = new LoaiSPRepository();
            NguyenLieu = new NguyenLieuRepository();
            NhaCungCap = new NhaCungCapRepository();
            PhieuNhap = new PhieuNhapRepository();
            CTPhieuNhap = new ICTPhieuPhapRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
            HoaDon = new HoaDonRepository();
            KhuyenMai = new KhuyenMaiRepository();
        }
        public HomeController(INguyenLieu _nl, INhaCungCap _ncc, IPhieuNhap _pn, ICTPhieuNhap _ctpn, ILoaiSP _loaisp, IPhieuXuat _px, ICTPhieuXuat _ctpx,IHoaDon _HoaDon, IKhuyenMai _KhuyenMai)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            NhaCungCap = _ncc;
            PhieuNhap = _pn;
            CTPhieuNhap = _ctpn;
            PhieuXuat = _px;
            CTPhieuXuat = _ctpx;
            HoaDon = _HoaDon;
            KhuyenMai = _KhuyenMai;
        }
        //
        // GET: /CMS_Sale/Home/
        public ActionResult Index()
        {
            double? tongtienthu = 0;
            double? tongtienchi = 0;
            double? tongtienthuhn = 0;
            double? tongtienchihn = 0;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            DateTime aDateTime = GetFistDayInMonth(DateTime.Now.Year, DateTime.Now.Month);
            // Cộng thêm 1 tuần và trừ đi một ngày.
            DateTime retDateTime = GetLastDayInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var startDate = Convert.ToDateTime(aDateTime.ToString("yyyy/MM/dd"));
            var endDate = Convert.ToDateTime(retDateTime.ToString("yyyy/MM/dd"));
            var datenow = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            var tongthuhn = HoaDon.SelectAll().Where(x => x.NgayTao >= datenow);
            IEnumerable<KhuyenMaiViewModel> model = KhuyenMai.SelectAll().Where(x => x.IsDelete != true).Select(
                    item => new KhuyenMaiViewModel
                    {
                        Id = item.Id,
                        Ten = item.Ten,
                        NgayBD = item.NgayBD,
                        NgayKT = item.NgayKT,
                        IsDelete = item.IsDelete
                    }).OrderByDescending(x => x.NgayBD);
            ViewBag.Ten = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
            foreach (var item in tongthuhn)
            {
                tongtienthuhn += item.TongTien;
            }
            var tongchihn = PhieuNhap.SelectAll().Where(x => x.NgayTao >= datenow);
            foreach (var item in tongchihn)
            {
                tongtienchihn += item.TongTien;
            }
            var tongthu = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", month), new SqlParameter("@Year", year)).ToList();

            foreach (var item in tongthu)
            {
                tongtienthu +=  item.Tongthu;
            }
            var tongchi = _db.Database.SqlQuery<DoanhThuTheoTungNgay>("exec DoanhThuTheoNgay @Month,@Year", new SqlParameter("@Month", month), new SqlParameter("@Year", year)).ToList();
            foreach (var item in tongchi)
            {
                tongtienchi += item.Tongchi;
            }
            ViewBag.tongdonhang = HoaDon.SelectAll().Where(x => x.NgayTao >= aDateTime && x.NgayTao <= retDateTime || x.NgayTao >= datenow).Count();
            ViewBag.countHangCon = NguyenLieu.SelectAll().Where(x => x.SoLuongKho > 0).Count();
            ViewBag.tonghang = NguyenLieu.SelectAll().Count();
            ViewBag.tongthu = tongtienthu;
            ViewBag.tongchi = tongtienchi;
            ViewBag.tongthuhn = tongtienthuhn;
            ViewBag.tongchihn = tongtienchihn;
            ViewBag.thuchi = tongtienthuhn - tongtienchihn;
            ViewBag.countSapHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho >= 1 && x.SoLuongKho <= 5).Count();
            ViewBag.countHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho == 0 && x.NgayNhap != datetimesetting).Count();
            return View();
        }
        public ActionResult BarchatReal()
        {
            var year = DateTime.Now.Year;
            var result = _db.Database.SqlQuery<DTTT>("exec DoanhThuTheoThang1 @Year", new SqlParameter("@Year", year)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PieChartReal()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            DateTime startDate = GetFistDayInMonth(year, month);
            DateTime endDate = GetLastDayInMonth(year, month);
            var model = _db.Database.SqlQuery<TopSanPhamBanChay>("exec topspbanchay @batdau,@ketthuc", new SqlParameter("@batdau", startDate), new SqlParameter("@ketthuc", endDate)).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public static DateTime GetFistDayInMonth(int year, int month)
        {
            DateTime aDateTime = new DateTime(year, month, 1);

            return aDateTime;
        }

        // Trả về ngày cuối cùng của tháng.
        public static DateTime GetLastDayInMonth(int year, int month)
        {
            DateTime aDateTime = new DateTime(year, month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            return retDateTime;
        }
    }
}