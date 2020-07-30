using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class HoaDonViewModel
    {
        public int Id { get; set; }

        public int? IdNhanVien { get; set; }

        public int? IdKhachHang { get; set; }

        public string GhiChu { get; set; }
        public string TenKH { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenNV { get; set; }
        public string TrangThai { get; set; }
        public double? TongTien { get; set; }
        public double? TongKM { get; set; }
        public bool? IsDelete { get; set; }
        public List<CTHoaDonViewModel> ListPOSDetails { get; set; }
    }
}