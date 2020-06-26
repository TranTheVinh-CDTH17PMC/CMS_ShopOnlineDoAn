using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Staff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CMS_ShopOnline.Areas.CMS_Staff.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly INhanVien NhanVien;
        private readonly ILoaiNV LoaiNV;
        public StaffController()
        {
            NhanVien = new NhanVienRepository();
            LoaiNV = new LoaiNVRepository();
        }
        public StaffController(INhanVien _NhanVien,ILoaiNV _LoaiNV)
        {
            NhanVien = _NhanVien;
            LoaiNV = _LoaiNV;
        }
        //
        // GET: /Staff/Staff/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new NhanVienViewModel
            {
                listLoaiNV = LoaiNV.SelectAll()
            };
            return View(model);
        }
        // POST: Crm/Supplier/Create
        [HttpPost]
        public ActionResult Create(NhanVienViewModel model)
        {
            if(CheckName(model.TenTaiKhoan)==true)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.TenTaiKhoan, model.MatKhau);
                    var Idnvnew = WebSecurity.GetUserId(model.TenTaiKhoan);
                    NhanVien _nhanvien = NhanVien.GetbyId(Idnvnew);
                    model.Id = Idnvnew;
                    AutoMapper.Mapper.Map(model, _nhanvien);
                    _nhanvien.SLDNSai = 0;
                    _nhanvien.IsDelete = false;
                    _nhanvien.NgayTao = DateTime.Now;
                    NhanVien.Update(_nhanvien);
                    NhanVien.Save();
                    TempData["SuccessMessage"] = "Create";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                }
            }
            else
            {
                model.listLoaiNV = LoaiNV.SelectAll();
                TempData["FailMessage"] = "FailCreate";
                return View(model);
            }
            return View();
        }
        public ActionResult Edit(int Id)
        {
            var user = NhanVien.GetbyId(Id);
            if(user!=null)
            {
                var model = new NhanVienViewModel();
                AutoMapper.Mapper.Map(user, model);
                model.listLoaiNV = LoaiNV.SelectAll();
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(NhanVienViewModel model)
        {
            var user = NhanVien.GetbyId(model.Id);
            if(user!=null)
            {
                if(user.TenTaiKhoan==model.TenTaiKhoan)
                {
                    AutoMapper.Mapper.Map(model, user);
                    NhanVien.Update(user);
                    NhanVien.Save();
                    TempData["SuccessMessage"] = "Create";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (CheckName(model.TenTaiKhoan) == true)
                    {
                        AutoMapper.Mapper.Map(model, user);
                        NhanVien.Update(user);
                        NhanVien.Save();
                        TempData["SuccessMessage"] = "Create";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        model.listLoaiNV = LoaiNV.SelectAll();
                        TempData["FailMessage"] = "FailCreate";
                        return View(model);
                    }
                }   
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public bool CheckName(string Name)
        {
            var check = WebSecurity.GetUserId(Name);
            if(check<=0)
            {
                return true;
            }
            return false;
        }
        public ActionResult Delete(int Id)
        {
            var _nhanvien = NhanVien.GetbyId(Id);
            if (_nhanvien != null)
            {
                _nhanvien.IsDelete = true;
                NhanVien.Update(_nhanvien);
                NhanVien.Save();
            }
            return View();
        }

    }
}