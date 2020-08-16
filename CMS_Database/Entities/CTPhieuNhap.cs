namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTPhieuNhap")]
    public partial class CTPhieuNhap
    {
        public int Id { get; set; }

        public int? IdNguyenLieu { get; set; }

        public int? IdPhieuNhap { get; set; }

        public int? SoLuong { get; set; }
        public int? IdDVT { get; set; }

        public double? DonGia { get; set; }

        public virtual NguyenLieu NguyenLieu { get; set; }

        public virtual PhieuNhap PhieuNhap { get; set; }
    }
}
