namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguyenLieu")]
    public partial class NguyenLieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguyenLieu()
        {
            CTPhieuNhap = new HashSet<CTPhieuNhap>();
        }

        public int Id { get; set; }

        public DateTime NgayTao { get; set; }
        public DateTime NgayNhap { get; set; }

        public int? IdLoai { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        public string HinhAnh { get; set; }

        public int? IdDVT { get; set; }

        public double? DonGia { get; set; }

        public int? SoLuongKho { get; set; }

        public bool? IsDelete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuNhap> CTPhieuNhap { get; set; }

        public virtual DonViTinh DonViTinh { get; set; }

        public virtual LoaiSP LoaiSP { get; set; }
    }
}
