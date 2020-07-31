using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Crm.Models;
using CMS_ShopOnline.Helpers;
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
        public ActionResult Index(string txtCode)
        {
            try
            {
                IEnumerable<CustomerViewModel> model = khachhang.SelectAll().Where(x => x.IsDelete != true).Select(
                    item => new CustomerViewModel
                    {
                        Id = item.Id,
                        TenKH = item.TenKH,
                        NgayTao = item.NgayTao,
                        SDT = item.SDT,
                        IdNVTao = item.IdNVTao,
                        TenNV = item.NhanVien.TenNV,
                        TongTien = item.TongTien,
                        IsDelete = item.IsDelete
                    }).OrderBy(x => x.Id);
                if (txtCode != null)
                {
                    model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.TenKH).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode))
                    || x.Id.ToString().Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode)))
                    || (Helpers.Helper.ChuyenThanhKhongDau(x.SDT).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtCode))));
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View(model);
            }
            catch (Exception e)
            {

            }
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
                _kh.IdNVTao= Helper.CurrentUser.Id;
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
        public ActionResult Edit(int id)
        {
            var _kh = khachhang.SelectById(id);
            CustomerViewModel model = new CustomerViewModel();
            AutoMapper.Mapper.Map(_kh, model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CustomerViewModel model)
        {
            try
            {
                var _kh = khachhang.SelectById(model.Id);
                _kh.TenKH = model.TenKH;
                _kh.SDT = model.SDT;
                khachhang.Update(_kh);
                khachhang.Save();
                TempData["SuccessMessage"] = "Edit";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

            }
            return View();
        }
        public ActionResult Delete(string IdDelete)
        {
            var _kh = khachhang.SelectById(int.Parse(IdDelete));
            if (_kh != null)
            {
                _kh.IsDelete = true;
                khachhang.Update(_kh);
                khachhang.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
	}
}