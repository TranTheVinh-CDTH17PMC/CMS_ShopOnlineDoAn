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

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
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
        }
        public HomeController(INguyenLieu _nl, INhaCungCap _ncc, IPhieuNhap _pn, ICTPhieuNhap _ctpn, ILoaiSP _loaisp, IPhieuXuat _px, ICTPhieuXuat _ctpx,IHoaDon _HoaDon)
        {
            LoaiSP = _loaisp;
            NguyenLieu = _nl;
            NhaCungCap = _ncc;
            PhieuNhap = _pn;
            CTPhieuNhap = _ctpn;
            PhieuXuat = _px;
            CTPhieuXuat = _ctpx;
            HoaDon = _HoaDon;
        }
        //
        // GET: /CMS_Sale/Home/
        public ActionResult Index()
        {
            double? tongtienthu = 0;
            double? tongtienchi = 0;
            double? tongtienthuhn = 0;
            double? tongtienchihn = 0;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            // Cộng thêm 1 tuần và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddDays(-7).AddDays(1);
            var startDate = Convert.ToDateTime(aDateTime.ToString("yyyy/MM/dd"));
            var endDate = Convert.ToDateTime(retDateTime.ToString("yyyy/MM/dd"));
            var datenow = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            var tongthuhn = HoaDon.SelectAll().Where(x => x.NgayTao >= datenow);
            foreach (var item in tongthuhn)
            {
                tongtienthuhn += +item.TongTien;
            }
            var tongchihn = PhieuNhap.SelectAll().Where(x => x.NgayTao >= datenow);
            foreach (var item in tongchihn)
            {
                tongtienchihn += +item.TongTien;
            }
            var tongthu = HoaDon.SelectAll().Where(x => x.NgayTao >= endDate && x.NgayTao <= startDate);
            
            foreach (var item in tongthu)
            {
                tongtienthu += + item.TongTien;
            }
            var tongchi = PhieuNhap.SelectAll().Where(x => x.NgayTao >= endDate && x.NgayTao <= startDate);
            foreach (var item in tongchi)
            {
                tongtienchi += +item.TongTien;
            }
            ViewBag.tongdonhang = HoaDon.SelectAll().Where(x => x.NgayTao >= endDate && x.NgayTao <= startDate).Count();
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
        public ActionResult BarchatReal(int? year)
        {
            var result = _db.Database.SqlQuery<DTTT>("exec DoanhThuTheoThang1 @Year", new SqlParameter("@Year", year)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}