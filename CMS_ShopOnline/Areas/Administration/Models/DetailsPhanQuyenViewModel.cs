using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class DetailsPhanQuyenViewModel
    {
        public int? Id { get; set; }
        public int? IdControllerName { get; set; }
        public int? IdRole { get; set; }
        public bool IsDelete { get; set; }
    }
}