namespace CMS_Database.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThanhPham")]
    public partial class ThanhPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThanhPham()
        {
            CTPhieuXuat = new HashSet<CTPhieuXuat>();
        }

        public int Id { get; set; }

        public DateTime? NgayTao { get; set; }

        public int? IdLoai { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        public string HinhAnh { get; set; }

        public double? DonGia { get; set; }

        public bool? IsDelete { get; set; }

        public int? IdDVT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuXuat> CTPhieuXuat { get; set; }

        public virtual DonViTinh DonViTinh { get; set; }

        public virtual LoaiSP LoaiSP { get; set; }
    }
}
