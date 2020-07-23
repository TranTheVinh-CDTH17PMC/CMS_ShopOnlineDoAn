using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class PhanQuyenViewModel
    {
        public int? Id { get; set; }
        public string ControllerName { get; set; }
        public bool? IsDelete { get; set; }
        public IEnumerable<DetailsPhanQuyenViewModel> DetailsPhanQuyen { get; set; }
        public List<DetailsPhanQuyenViewModel> LDetailsPhanQuyen { get; set; }
    }
}