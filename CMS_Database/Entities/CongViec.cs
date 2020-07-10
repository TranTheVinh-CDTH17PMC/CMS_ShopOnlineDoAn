namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CongViec")]
    public partial class CongViec
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        public int? IdNhanVien { get; set; }
        public int? IdPhieuXuat { get; set; }
        
        public DateTime NgayTao { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        public bool? IsDelete { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
