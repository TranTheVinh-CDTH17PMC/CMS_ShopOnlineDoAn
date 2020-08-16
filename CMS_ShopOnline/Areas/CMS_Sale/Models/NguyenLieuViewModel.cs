using CMS_Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class NguyenLieuViewModel
    {
        public int Id { get; set; }

        public int? IdLoai { get; set; }
        public string TenLoai { get; set; }

        public string Ten { get; set; }

        public string HinhAnh { get; set; }

        public int? IdDVT { get; set; }
        public string TenDVT { get; set; }

        public double? DonGia { get; set; }

        public int? SoLuongKho { get; set; }

        public bool? IsDelete { get; set; }
        public DateTime NgayNhap { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime HSD { get; set; }

        public IEnumerable<LoaiSP> listLoaiSP { get; set; }
        public IEnumerable<DonViTinh> listDVT { get; set; }
    }
}