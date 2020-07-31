namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuXuat")]
    public partial class PhieuXuat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuXuat()
        {
            CTPhieuXuat = new HashSet<CTPhieuXuat>();
        }

        public int Id { get; set; }

        public int? IdNhanVien { get; set; }

        //public int? IdKhachHang { get; set; }

        [StringLength(50)]
        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; }

        public double? TongTien { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsPrint { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuXuat> CTPhieuXuat { get; set; }

        //public virtual KhachHang KhachHang { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
