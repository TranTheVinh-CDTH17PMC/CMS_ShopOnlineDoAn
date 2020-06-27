using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Staff.Models
{
    public class NhanVienViewModel
    {
        public int Id { get; set; }


        public int? IdLoaiNV { get; set; }

        public string TenNV { get; set; }

        public int? SLDNSai { get; set; }
        public string CMND { get; set; }

        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }

        public string Email { get; set; }

        public string DiaChi { get; set; }

        public string SDT { get; set; }

        public bool? IsDelete { get; set; }
        public IEnumerable<LoaiNV> listLoaiNV { get; set; }
    }
}