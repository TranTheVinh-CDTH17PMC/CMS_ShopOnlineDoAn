using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Crm.Controllers
{
    public class CustomerController : Controller
    
    {
        private readonly IKhachHang khachhang;
         public CustomerController()
        {
            khachhang = new KhachHangRepository();
        }
         public CustomerController(IKhachHang _kh)
        {
            khachhang = _kh;
        }
        //
        // GET: /Crm/Customer/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        // POST: Crm/Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerViewModel model)
        {
            try
            {
                var _kh = new KhachHang();
                AutoMapper.Mapper.Map(model, _kh);
                _kh.NgayTao = DateTime.Now;
                _kh.IsDelete = false;
                khachhang.Insert(_kh);
                khachhang.Save();
                ViewBag.Message = String.Format("Thêm mới thành công!!!");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return View();
        }
	}
}