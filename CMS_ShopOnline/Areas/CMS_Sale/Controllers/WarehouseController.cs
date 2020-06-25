using CMS_Database.Interfaces;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly INgyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        public WarehouseController()
        {

        }
        public WarehouseController(INgyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP)
        {

        }
        //
        // GET: /CMS_Sale/Warehouse/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(NguyenLieuViewModel model)
        {
            return View();
        }
    }
}