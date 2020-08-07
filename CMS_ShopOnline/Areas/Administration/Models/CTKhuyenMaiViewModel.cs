using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class CTKhuyenMaiViewModel
    {
        public int? Id { get; set; }
        public int? IdKhuyenMai { get; set; }
        public bool? IsPhanTram { get; set; }
        public bool? IsTienMat { get; set; }
        public int? IdThanhPham { get; set; }
        public string Ten { get; set; }
        public int? SLToithieu { get; set; }
        public int? IdLoaiSP { get; set; }
        public double? TienGiam { get; set; }
        public string Loaithanhtoan { get; set; }
        public string Loaikhuyenmai { get; set; }
    }
}