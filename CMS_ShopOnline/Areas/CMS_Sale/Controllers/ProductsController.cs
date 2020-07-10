using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using CMS_ShopOnline.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IThanhPham ThanhPham;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        public ProductsController()
        {
            ThanhPham = new ThanhPhamRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
        }
        public ProductsController(IThanhPham _ThanhPham, IDonViTinh _DVT, ILoaiSP _LoaiSP)
        {
            ThanhPham = _ThanhPham;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
        }
        //
        // GET: /CMS_Sale/Products/
        public ActionResult Index(string txtCode)
        {
            try
            {
                IEnumerable<ThanhPhamViewModel> model = ThanhPham.SelectAll().Where(x => x.IsDelete != true).Select(
                    item => new ThanhPhamViewModel
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    NgayTao = item.NgayTao,
                    IdLoai = item.IdLoai,
                    TenLoai = item.LoaiSP.Ten,
                    HinhAnh = item.HinhAnh,
                    DonGia = item.DonGia,
                    IdDVT = item.IdDVT,
                    TenDVT = item.DonViTinh.Ten,
                    IsDelete = item.IsDelete
                }).OrderBy(x => x.Id);
                if (txtCode != null)
                {
                    txtCode = txtCode == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtCode);
                    model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(txtCode)));
                }
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View(model);
            }
            catch(Exception e)
            {
                
            }

            return View();
        }

        public ActionResult Create()
        {
            var model = new ThanhPhamViewModel
            {
                listDVT = DVT.SelectAll(),
                listLoaiSP = LoaiSP.SelectAll()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ThanhPhamViewModel model, HttpPostedFileBase File)
        {
            try
            {
                var _tp = new ThanhPham();
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
                            path = Path.Combine(Server.MapPath("~/Areas/CMS_Sale/Image/ThanhPham/"), name);
                            model.HinhAnh = name;
                            File.SaveAs(path);
                        }
                    }
                }

                float vOut = Convert.ToSingle(model.DonGia);
                _tp.DonGia = vOut;
                AutoMapper.Mapper.Map(model, _tp);
                _tp.NgayTao = DateTime.Now;
                _tp.IsDelete = false;
                ThanhPham.Insert(_tp);
                ThanhPham.Save();
                TempData["SuccessMessage"] = "Create";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

            }

            return View();
        }
        public ActionResult Edit(int Id)
        {
            var _tp = ThanhPham.GetbyId(Id);
            if (_tp != null)
            {
                var model = new ThanhPhamViewModel();
                AutoMapper.Mapper.Map(_tp, model);
                model.listLoaiSP = LoaiSP.SelectAll();
                model.listDVT = DVT.SelectAll();
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ThanhPhamViewModel model, HttpPostedFileBase File)
        {
            try
            {
                var _tp = ThanhPham.SelectById(model.Id);
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
                            path = Path.Combine(Server.MapPath("~/Areas/CMS_Sale/Image/ThanhPham/"), name);
                            model.HinhAnh = name;
                            File.SaveAs(path);
                        }
                    }
                }
                else
                {
                    model.HinhAnh = _tp.HinhAnh;
                }
                float vOut = Convert.ToSingle(model.DonGia);
                _tp.DonGia = vOut;
                AutoMapper.Mapper.Map(model, _tp);
                _tp.NgayTao = DateTime.Now;
                _tp.IsDelete = false;
                ThanhPham.Update(_tp);
                ThanhPham.Save();
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
            var tp = ThanhPham.SelectById(int.Parse(IdDelete));
            if (tp != null)
            {
                tp.IsDelete = true;
                ThanhPham.Update(tp);
                ThanhPham.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
	}
}