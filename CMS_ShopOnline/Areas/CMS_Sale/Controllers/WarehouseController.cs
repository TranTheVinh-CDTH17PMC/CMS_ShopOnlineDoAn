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
        private readonly ITemplatePrint TemplatePrint;
        public WarehouseController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            TemplatePrint = new TemplatePrintRepository();
        }
        public WarehouseController(INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP, ITemplatePrint _TemplatePrint)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            TemplatePrint = _TemplatePrint;
        }
        //
        // GET: /CMS_Sale/Warehouse/
        public ActionResult Index(string name,int? iddvt,int ? idloai, string hangcon, string tonkho, string saphethang,string hethang,string all)
        {
            var idten = Helpers.Helper.ChuyenThanhKhongDau(name);
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
            ViewBag.loai = LoaiSP.SelectAll().Where(x => x.IsDelete != true);
            ViewBag.dvt = DVT.SelectAll().Where(x => x.IsDelete != true);
            if (hangcon!=null)
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
            if(iddvt!=null)
            {
                model = model.Where(x => x.IsDelete != true && x.IdDVT == iddvt);
            }
            if (idloai != null)
            {
                model = model.Where(x => x.IsDelete != true && x.IdLoai == idloai);
            }
            if(idten!=null && idten!="")
            {
                model = model.Where(x => x.IsDelete != true && x.Id.ToString().Contains(idten) || Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(idten));
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
                _nguyenlieu.Ten = model.Ten;
                _nguyenlieu.IdLoai = model.IdLoai;
                _nguyenlieu.IdDVT = model.IdDVT;
                _nguyenlieu.HinhAnh = model.HinhAnh;
                _nguyenlieu.IsDelete = false;
                _nguyenlieu.NgayTao = DateTime.Now;
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
        public ActionResult Print(string name,bool ExportExcel, int? iddvt, int? idloai)
        {
            var idten = Helpers.Helper.ChuyenThanhKhongDau(name);
            var model = TemplatePrint.SelectById(1);
            IEnumerable<NguyenLieuViewModel> modellist = NguyenLieu.SelectAll().Where(x => x.IsDelete != true).Select(
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
               }).OrderByDescending(x => x.NgayTao);
            if (iddvt != null)
            {
                modellist = modellist.Where(x => x.IsDelete != true && x.IdDVT == iddvt);
            }
            if (idloai != null)
            {
                modellist = modellist.Where(x => x.IsDelete != true && x.IdLoai == idloai);
            }
            if (idten != null && idten != "")
            {
                modellist = modellist.Where(x => x.IsDelete != true && x.Id.ToString().Contains(idten) || Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(idten));
            }
            model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
            model.Content = model.Content.Replace("{NamePrint}", "Danh sach kho hang");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "Nguyenlieu" + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string BuildHtml(IEnumerable<NguyenLieuViewModel> ls)
        {
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += " <th>Ngày nhập</th><th>Tên</th><th>Tên loại</th><th>ĐVT</th><th>Số lượng kho</th><th>Đơn giá</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"unit\">" + item.NgayNhap + "</td>\r\n";
                list += " <td class=\"qty\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"qty\">" + item.TenLoai + "</td>\r\n";
                list += "<td class=\"total\">" + item.TenDVT + "</td>\r\n";
                list += "<td class=\"total\">" + item.SoLuongKho + "</td>\r\n";
                list += "<td class=\"total\">" + item.DonGia + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            list += "<tr><td colspan=\"8\"class=\"grand total\"></td>\r\n";
            return list;
        }
    }
}