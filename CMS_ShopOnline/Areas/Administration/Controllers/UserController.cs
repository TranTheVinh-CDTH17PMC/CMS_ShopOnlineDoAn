using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    public class UserController : Controller
    {
        private readonly INhanVien _NhanVien = new NhanVienRepository();
        // GET: CMS_Sale/NhanVien
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                //int Id = WebSecurity.GetUserId(login.Username); //để update tạo tài khoản
                bool IsAu = WebSecurity.Login(login.Username, login.Password, login.Remeberme);
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
            WebSecurity.Logout();

            HttpContext.Application.Contents.Remove("TaiKhoanLogin");

            return RedirectToAction("Login", "User", new { @area = "Administration" });
        }
    }
}