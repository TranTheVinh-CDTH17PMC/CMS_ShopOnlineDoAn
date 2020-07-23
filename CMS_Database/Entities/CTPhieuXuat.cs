namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTPhieuXuat")]
    public partial class CTPhieuXuat
    {
        public int Id { get; set; }

        public int? IdNguyenLieu { get; set; }

        public int? IdPhieuXuat { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }

        public virtual PhieuXuat PhieuXuat { get; set; }

        public virtual NguyenLieu NguyenLieu { get; set; }
    }
}
