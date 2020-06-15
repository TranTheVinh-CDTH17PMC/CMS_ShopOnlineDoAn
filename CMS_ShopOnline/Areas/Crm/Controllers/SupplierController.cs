using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Crm.Controllers
{
    public class SupplierController : Controller
    {
        //
        // GET: /Crm/Supplier/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
	}
}