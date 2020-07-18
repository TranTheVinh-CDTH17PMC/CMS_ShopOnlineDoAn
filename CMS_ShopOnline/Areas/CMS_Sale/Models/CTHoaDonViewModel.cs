using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class CTHoaDonViewModel
    {
        public int Id { get; set; }

        public int? IdThanhPham { get; set; }

        public int? IdHoaDon { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }
        public string Ten { get; set; }
    }
}