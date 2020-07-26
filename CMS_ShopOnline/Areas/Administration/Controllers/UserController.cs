using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    public class UserController : Controller
    {
        private readonly INhanVien _NhanVien = new NhanVienRepository();
        private readonly IPhanQuyen _PhanQuyen = new PhanQuyenRepository();
        private readonly IListController _ListController = new ListControllerRepository();
        private readonly ILoaiNV _LoaiNV = new LoaiNVRepository();
        // GET: CMS_Sale/NhanVien
        public ActionResult Login()
        {
            //Assembly asm = Assembly.GetExecutingAssembly();

            //var controllerTypes = from t in asm.GetExportedTypes()
            //                      where typeof(IController).IsAssignableFrom(t)
            //                      select t;
            //foreach (var item in controllerTypes)
            //{
            //    var ctl = new ListController();
            //    ctl.ControllerName = item.Name.Replace("Controller", "");
            //    ctl.IsDelete = false;
            //    _ListController.Insert(ctl);
            //    _ListController.Save();
            //}
            //var getlistctl = _ListController.SelectAll().Where(x => x.IsDelete != true);
            //var getlistrole = _LoaiNV.SelectAll().Where(x => x.IsDelete != true);
            //foreach (var itemrole in getlistrole)
            //{
            //    foreach (var itemctl in getlistctl)
            //    {
            //        var pq = new PhanQuyen();
            //        pq.IdControllerName = itemctl.Id;
            //        pq.IdRole = itemrole.Id;
            //        pq.IsDelete = false;
            //        _PhanQuyen.Insert(pq);
            //        _PhanQuyen.Save();
            //    }
            //}
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                //int Id = WebSecurity.GetUserId(login.Username); //để update tạo tài khoản
                bool IsAu = WebSecurity.Login(login.Username, login.Password,false);
                if (IsAu)
                { 
                    var user = _NhanVien.GetByUserName(login.Username);
                    if(user.SLDNSai >= 5)
                    {
                        ModelState.AddModelError("", "Tài khoản bị khóa.Vui lòng liên hệ quản lí!!!");
                        return View(login);
                    }
                    else
                    {
                        
                        HttpContext.Application["TaiKhoanLogin"] = user.TenTaiKhoan.ToLower();
                        return RedirectToAction("Index", "Home",new { area ="CMS_Sale"});
                    }
                }
                else
                {
                    var user = _NhanVien.GetByUserName(login.Username);
                    if(user == null)
                    {
                        ModelState.AddModelError("", "User name hoặc password không chính xác.");
                        return View(login);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu không chính xác.");
                        user.SLDNSai = user.SLDNSai+1;
                        _NhanVien.Update(user);
                        return View(login);
                    }
                }
            }
            return View();
        }
        public ActionResult LogOff()
        {
            Helpers.Helper.CurrentUser = null;
            WebSecurity.Logout();

            HttpContext.Application.Contents.Remove("TaiKhoanLogin");
            HttpContext.Application.Contents.Remove("ListRequest");

            return RedirectToAction("Login", "User", new { @area = "Administration" });
        }
      
    }
}