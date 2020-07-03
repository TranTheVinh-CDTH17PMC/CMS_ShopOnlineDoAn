using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class PhieuNhapViewModel
    {
        public int Id { get; set; }
        public int? IdNhaCungCap { get; set; }
        public DateTime NgayTao { get; set; }
        public int? IdNhanVien { get; set; }
        public double TongTien { get; set; }
        public string GhiChu { get; set; }
        public bool? IsDelete { get; set; }
    }
}