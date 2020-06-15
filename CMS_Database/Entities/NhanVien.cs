namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        public int Id { get; set; }

        public DateTime? NgayTao { get; set; }

        public int? IdLoaiNV { get; set; }

        [StringLength(50)]
        public string TenNV { get; set; }

        public int? SLDNSai { get; set; }

        [StringLength(50)]
        public string TenTaiKhoan { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        public bool? IsDelete { get; set; }
    }
}
