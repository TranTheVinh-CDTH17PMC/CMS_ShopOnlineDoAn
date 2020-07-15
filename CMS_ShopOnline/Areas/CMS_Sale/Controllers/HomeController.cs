using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Database.Entities;
using CMS_Database.Repositories;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        DBConnection _db = new DBConnection();
        //
        // GET: /CMS_Sale/Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BarchatReal(int? year)
        {
            var result = _db.Database.SqlQuery<DTTT>(" exec DoanhThuTheoThang1 @Year", new SqlParameter("@Year", year)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}