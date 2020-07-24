using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Staff.Models
{
    public class LoainvViewModel
    {
        public int Id { get; set; }

        public string TenLoai { get; set; }

        public bool? IsDelete { get; set; }
    }
}