using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly INhanVien _NhanVien = new NhanVienRepository();
        // GET: CMS_Sale/NhanVien
        public ActionResult Login()
        {
            WebSecurity.CreateUserAndAccount("admin", "admin", false);
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                int id = WebSecurity.GetUserId(login.Username);
                bool isAu = WebSecurity.Login(login.Username, login.Password, login.Remeberme);
                if (isAu)
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
                        return RedirectToAction("Index", "Home");
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
    }
}