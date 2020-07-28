using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Database.Entities
{
    public class DoanhThuTheoTungNgay
    {
        public DateTime Ngay { set; get; }
        public double? Tongthu { set; get; }
        public double? Tongchi { set; get; }
        public double? Loinhuan { set; get; }
    }
}
