using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("CTKhuyenmai")]
    public partial class CTKhuyenmai
    {
        public int Id { get; set; }
        public int? IdKhuyenMai { get; set; }
        public bool? IsPhanTram { get; set; }
        public bool? IsTienMat { get; set; }
        public int? IdThanhPham { get; set; }
        public int? SLToithieu { get; set; }
        public int? IdLoaiSP { get; set; }
        public double? TienGiam { get; set; }
        public bool? IsDelete { get; set; }
    }
}
