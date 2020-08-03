using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class KhuyenMaiViewModel
    {
        public int? Id { get; set; }
        public string Ten { get; set; }
        public DateTime NgayBD { get; set; }
        public DateTime NgayKT { get; set; }
        public string GhiChu { get; set; }
        public bool? IsDelete { get; set; }
        public string IsAll { get; set; }
        public List<CTKhuyenMaiViewModel> ListCTkm { get; set; }
    }
}