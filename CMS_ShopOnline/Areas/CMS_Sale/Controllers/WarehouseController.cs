using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class WarehouseController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private DateTime datetimenow = DateTime.Now;
        private DateTime datetimesetting = new DateTime(2010,10,10,1,1,1);
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
        public ActionResult Index(string hangcon, string tonkho, string saphethang,string hethang,string all)
        {
            
            int i = 0;
            ViewBag.countAll = NguyenLieu.SelectAll().Count();
            ViewBag.countHangCon = NguyenLieu.SelectAll().Where(x => x.SoLuongKho > 0).Count();
            ViewBag.countSapHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho >= 1 && x.SoLuongKho <= 5).Count(); 
            ViewBag.HangTonKho = NguyenLieu.SelectAll().Where(x => x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20).Count();
            ViewBag.countHetHang = NguyenLieu.SelectAll().Where(x => x.SoLuongKho == 0 && x.NgayNhap != datetimesetting).Count();
            IEnumerable<NguyenLieuViewModel> model = NguyenLieu.SelectAll().Where(x => x.IsDelete != true).Select(
                item => new NguyenLieuViewModel
                {
                    Id = item.Id,
                    TenLoai = item.LoaiSP.Ten,
                    IdLoai = item.IdLoai,
                    Ten = item.Ten,
                    HinhAnh = item.HinhAnh,
                    TenDVT = item.DonViTinh.Ten,
                    IdDVT = item.IdDVT,
                    DonGia = item.DonGia,
                    SoLuongKho = item.SoLuongKho,
                    IsDelete = item.IsDelete,
                    NgayNhap = item.NgayNhap
                }).OrderByDescending(x=>x.Id);
            if(hangcon!=null)
            {
                model = model.Where(x => x.SoLuongKho > 0 && x.IsDelete != true);
            }
            if (tonkho != null)
            {
                model = model.Where(x=>x.IsDelete != true && ngay(x.NgayNhap) > 30 && x.SoLuongKho > 20 && x.NgayNhap!=datetimesetting);
            }
            if (saphethang != null)
            {
                model = model.Where(x => x.SoLuongKho >= 1 && x.SoLuongKho <= 5 && x.IsDelete != true);
            }
            if (hethang != null)
            {
                model = model.Where(x => x.SoLuongKho == 0 && x.IsDelete != true && x.NgayNhap != datetimesetting);
            }
            if (all != null)
            {
                model = model.Where(x=>x.IsDelete != true);
            }
            return View(model);
        }
        public int ngay(DateTime ngaynhap)
        {
            int i = 0;
            TimeSpan diff1 = datetimenow.Subtract(ngaynhap);
            i = diff1.Days;
            return i;
        }
        public ActionResult Create()
        {
            var model = new NguyenLieuViewModel
            {
                listDVT = DVT.SelectAll().Where(x => x.IsDelete != true),
                listLoaiSP = LoaiSP.SelectAll().Where(x => x.IsDelete != true)
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
                _nguyenlieu.NgayNhap = datetimesetting;
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
        [HttpPost]
        public ActionResult Delete(string IdDelete)
        {
            var _nguyenlieu = NguyenLieu.SelectById(int.Parse(IdDelete));
            if(_nguyenlieu!=null)
            {
                _nguyenlieu.IsDelete = true;
                NguyenLieu.Update(_nguyenlieu);
                NguyenLieu.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}