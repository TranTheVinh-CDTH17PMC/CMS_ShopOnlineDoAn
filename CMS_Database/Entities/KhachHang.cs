namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            PhieuXuat = new HashSet<PhieuXuat>();
        }

        public int Id { get; set; }

        public DateTime? NgayTao { get; set; }

        [StringLength(50)]
        public string TenKH { get; set; }

        public int? IdNVTao { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        public double? TongTien { get; set; }

        public bool? IsDelete { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
    }
}
