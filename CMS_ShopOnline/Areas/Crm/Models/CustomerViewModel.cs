using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Crm.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenKH { get; set; }
        public string IdNVTao { get; set; }
        public string SDT { get; set; }
        public string TongTien { get; set; }
    }
}