using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Staff.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        //
        // GET: /Staff/Staff/
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