using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class POSViewModel
    {
        public int Id { get; set; }

        public int? IdNhanVien { get; set; }

        public int? IdKhachHang { get; set; }

        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
        public double? TongTien { get; set; }
        public List<POSDetailsViewModel> ListPOSDetails { get; set; }
    }
}