using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.Administration.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }


        public string Ten { get; set; }

        public int? IdNhanVien { get; set; }
        public int? IdPhieuXuat { get; set; }


        public DateTime? NgayTao { get; set; }


        public string Action { get; set; }

        public string Controller { get; set; }


        public string Area { get; set; }
        public string Color { get; set; }

        public bool? IsDelete { get; set; }
        public bool? Seen { get; set; }

    }
}