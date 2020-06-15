using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /CMS_Sale/Products/
        public ActionResult Index()
        {
            return View();
        }
	}
}