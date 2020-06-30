using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_ShopOnline.Areas.CMS_Sale.Models
{
    public class UnitViewModel
    {
        public int Id { get; set; }

        public string Ten { get; set; }

        public bool? IsDelete { get; set; }
    }
}