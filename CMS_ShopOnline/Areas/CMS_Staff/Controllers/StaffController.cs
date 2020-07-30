using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Staff.Models;
using CMS_ShopOnline.Filter;
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
        public ActionResult Index(int? IdLoaiNV, string txtCode, string txtName, string txtInfo)
        {
            try
            {
                IEnumerable<NhanVienViewModel> model = NhanVien.SelectAll().Select(
                    item => new NhanVienViewModel
                    {
                        Id = item.Id,
                        TenNV = item.TenNV,
                        NgayTao = item.NgayTao,
                        IdLoaiNV = item.IdLoaiNV,
                        TenLoai = item.LoaiNV.TenLoai,
                        TenTaiKhoan = item.TenTaiKhoan,
                        Email = item.Email,
                        DiaChi = item.DiaChi,
                        SDT = item.SDT,
                        CMND = item.CMND,
                        IsDelete = item.IsDelete
                    }).OrderBy(x => x.IsDelete);
                ViewBag.LoaiNV = LoaiNV.SelectAll();
                if (IdLoaiNV != null)
                {
                    model = model.Where(x => x.IdLoaiNV == IdLoaiNV).ToList();
                }
                if (txtName != null)
                {
                    txtName = txtName == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtName);
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.TenNV).Contains(txtName));
                }
                if (txtCode != null)
                {
                    txtCode = txtCode == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtCode);
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.SDT).Contains(txtCode));
                }
                if (!string.IsNullOrEmpty(txtInfo))
                {
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.TenNV).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtInfo)) || x.SDT.Contains(txtInfo)).ToList();
                }
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View(model);
            }
            catch (Exception e)
            {

            }

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
            var user = NhanVien.SelectById(Id);
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
            var user = NhanVien.SelectById(model.Id);
            if(user!=null)
            {
                if(user.TenTaiKhoan==model.TenTaiKhoan)
                {
                    AutoMapper.Mapper.Map(model, user);
                    user.NgayTao = DateTime.Now;
                    user.IsDelete = false;
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
        public ActionResult Delete(string IdDelete)
        {
            var _nv = NhanVien.SelectById(int.Parse(IdDelete));
            if (_nv != null)
            {
                _nv.IsDelete = true;
                NhanVien.Update(_nv);
                NhanVien.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult IndexType()
        {
            IEnumerable<LoainvViewModel> model = LoaiNV.SelectAll().Select(
                    item => new LoainvViewModel
                    {
                        Id = item.Id,
                        TenLoai = item.TenLoai,
                        IsDelete = item.IsDelete
                    }).OrderBy(x => x.IsDelete);
            return View(model);
        }
        public ActionResult CreateType()
        {
            return View();
        }
        public ActionResult CreateType(LoainvViewModel model)
        {
            var lnv = new LoaiNV();
            lnv.TenLoai = model.TenLoai;
            lnv.IsDelete = false;
            LoaiNV.Insert(lnv);
            LoaiNV.Save();
            return View();
        }
        public ActionResult EditType(int ?id)
        {
            var model = new LoainvViewModel();
            var lnv = LoaiNV.SelectById(id);
            model.TenLoai = lnv.TenLoai;
            model.Id = lnv.Id;
            return View();
        }
        public ActionResult EditType(LoainvViewModel model)
        {
            var lnv = LoaiNV.SelectById(model.Id);
            lnv.TenLoai = model.TenLoai;
            lnv.IsDelete = false;
            LoaiNV.Update(lnv);
            LoaiNV.Save();
            return View();
        }
        public ActionResult DeleteType(string IdDelete)
        {
            var _nv = LoaiNV.SelectById(int.Parse(IdDelete));
            if (_nv != null)
            {
                _nv.IsDelete = true;
                LoaiNV.Update(_nv);
                LoaiNV.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}