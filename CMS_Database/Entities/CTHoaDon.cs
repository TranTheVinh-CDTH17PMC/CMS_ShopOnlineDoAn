using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    [Table("CTHoaDon")]
    public partial class CTHoaDon
    {
        public int Id { get; set; }

        public int? IdThanhPham { get; set; }

        public int? IdHoaDon { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }

        public virtual HoaDon HoaDon { get; set; }

        public virtual ThanhPham ThanhPham { get; set; }
    }
}
