namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuNhap")]
    public partial class PhieuNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhap()
        {
            CTPhieuNhap = new HashSet<CTPhieuNhap>();
        }

        public int Id { get; set; }

        public int? IdNhaCungCap { get; set; }

        public DateTime? NgayTao { get; set; }

        public int? IdNhanVien { get; set; }

        public double? TongTien { get; set; }

        [StringLength(50)]
        public string GhiChu { get; set; }

        public bool? IsDetele { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuNhap> CTPhieuNhap { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
