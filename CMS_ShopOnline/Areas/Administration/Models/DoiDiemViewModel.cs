using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class DoiDiemViewModel
    {
        public double? Tienhang { get; set; }
        public int? Diemhang { get; set; }
        public double? Tiendoi { get; set; }
        public int? Diemdoi { get; set; }
        public bool? IsDelete { get; set; }
    }
}