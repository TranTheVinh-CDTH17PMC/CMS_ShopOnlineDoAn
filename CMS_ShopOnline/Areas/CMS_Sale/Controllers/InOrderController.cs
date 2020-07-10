using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class InOrderController : Controller
    {
        //
        // GET: /CMS_Sale/InOrder/
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