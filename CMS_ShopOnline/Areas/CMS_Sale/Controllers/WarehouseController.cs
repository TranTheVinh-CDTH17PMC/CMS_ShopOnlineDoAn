using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        public WarehouseController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
        }
        public WarehouseController(INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
        }
        //
        // GET: /CMS_Sale/Warehouse/
        public ActionResult Index()
        {
            DateTime datetimenow = DateTime.Now;
            int i = 0;
            ViewBag.countAll = NguyenLieu.SelectAll().Count();
            ViewBag.countHangCon = NguyenLieu.SelectAll().Where(x => x.SoLuongKho > 0).Count();
            ViewBag.countSapHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho >= 1 && x.SoLuongKho <= 5).Count();
            var HangTon = NguyenLieu.SelectAll();
            foreach(var item in HangTon)
            {
                TimeSpan diff1 = datetimenow.Subtract(item.NgayNhap);
                if (diff1.Days >= 30)
                {
                    i++;
                }
            }
            ViewBag.HangTonKho = i;
            ViewBag.countHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho == 0).Count();
            return View();
        }
        public ActionResult Create()
        {
            var model = new NguyenLieuViewModel
            {
                listDVT = DVT.SelectAll(),
                listLoaiSP = LoaiSP.SelectAll()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(NguyenLieuViewModel model, HttpPostedFileBase File)
        {
            try
            {
                var _nguyenlieu = new NguyenLieu();
                var path = "";
                if (File != null)
                {
                    if (File.ContentLength > 0)
                    {
                        if (Path.GetExtension(File.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(File.FileName).ToLower() == ".png"
                            || Path.GetExtension(File.FileName).ToLower() == ".gif"
                            || Path.GetExtension(File.FileName).ToLower() == ".jpeg")
                        {

                            string name = DateTime.Now.ToString()+"_"+File.FileName;
                            name = name.Replace(" ", "");
                            name = name.Replace("/", "");
                            name = name.Replace(":", "");
                            path = Path.Combine(Server.MapPath("~/Areas/CMS_Sale/Image/NguyenLieu/"),name);
                            model.HinhAnh = name;
                            File.SaveAs(path);
                        }
                    }
                }
                
                float vOut = Convert.ToSingle(model.DonGia);
                _nguyenlieu.DonGia = vOut;
                AutoMapper.Mapper.Map(model, _nguyenlieu);
                _nguyenlieu.NgayTao = DateTime.Now;
                _nguyenlieu.NgayNhap = DateTime.Now;
                _nguyenlieu.IsDelete = false;
                _nguyenlieu.SoLuongKho = 0;
                NguyenLieu.Insert(_nguyenlieu);
                NguyenLieu.Save();
                TempData["SuccessMessage"] = "Create";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {

            }
            
            return View();
        }
        public ActionResult Edit(int Id)
        {
            var model = new NguyenLieuViewModel();
            var _nguyenlieu = NguyenLieu.SelectById(Id);
            model.listDVT = DVT.SelectAll();
            model.listLoaiSP = LoaiSP.SelectAll();
            AutoMapper.Mapper.Map(_nguyenlieu, model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(NguyenLieuViewModel model, HttpPostedFileBase File)
        {
            try
            {
                var _nguyenlieu = NguyenLieu.SelectById(model.Id);
                var path = "";
                if (File != null)
                {
                    if (File.ContentLength > 0)
                    {
                        if (Path.GetExtension(File.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(File.FileName).ToLower() == ".png"
                            || Path.GetExtension(File.FileName).ToLower() == ".gif"
                            || Path.GetExtension(File.FileName).ToLower() == ".jpeg")
                        {

                            string name = DateTime.Now.ToString() + "_" + File.FileName;
                            name = name.Replace(" ", "");
                            name = name.Replace("/", "");
                            name = name.Replace(":", "");
                            path = Path.Combine(Server.MapPath("~/Areas/CMS_Sale/Image/NguyenLieu/"), name);
                            model.HinhAnh = name;
                            File.SaveAs(path);
                        }
                    }
                }
                else
                {
                    model.HinhAnh = _nguyenlieu.HinhAnh;
                }

                float vOut = Convert.ToSingle(model.DonGia);
                _nguyenlieu.DonGia = vOut;
                AutoMapper.Mapper.Map(model, _nguyenlieu);
                _nguyenlieu.IsDelete = false;
                NguyenLieu.Update(_nguyenlieu);
                NguyenLieu.Save();
                TempData["SuccessMessage"] = "Edit";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

            }

            return View();
        }
        public ActionResult Delete(int Id)
        {
            var _nguyenlieu = NguyenLieu.SelectById(Id);
            if(_nguyenlieu!=null)
            {
                _nguyenlieu.IsDelete = true;
                NguyenLieu.Update(_nguyenlieu);
                NguyenLieu.Save();
            }
            return View();
        }
    }
}