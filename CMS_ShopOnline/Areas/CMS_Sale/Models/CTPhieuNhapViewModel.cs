using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class CTPhieuNhapViewModel
    {
        public int Id { get; set; }
        public int? IdNguyenLieu { get; set; }
        public string Ten { get; set; }
        public int? IdPhieuNhap { get; set; }
        public int? Soluong { get; set; }
        public double? DonGia { get; set; }

    }
}