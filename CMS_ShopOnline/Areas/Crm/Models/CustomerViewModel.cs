using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Crm.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public DateTime? NgayTao { get; set; }
        public string TenKH { get; set; }
        public int? IdNVTao { get; set; }
        public string TenNV { get; set; }
        public string SDT { get; set; }
        public double? TongTien { get; set; }
        public double? SoDiemToiDa { get; set; }
        public double? SoDiem{ get; set; }
        public double? SoTien { get; set; }
        public bool? IsDelete { get; set; }
    }
}