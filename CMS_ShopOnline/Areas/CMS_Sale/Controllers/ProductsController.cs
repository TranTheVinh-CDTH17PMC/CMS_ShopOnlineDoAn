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
        private readonly ITemplatePrint TemplatePrint;
        public ProductsController()
        {
            ThanhPham = new ThanhPhamRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            TemplatePrint = new TemplatePrintRepository();
        }
        public ProductsController(IThanhPham _ThanhPham, IDonViTinh _DVT, ILoaiSP _LoaiSP, ITemplatePrint _TemplatePrint)
        {
            ThanhPham = _ThanhPham;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            TemplatePrint = _TemplatePrint;
        }
        //
        // GET: /CMS_Sale/Products/
        public ActionResult Index(string txtInfo,int? IdLoaiSP, string txtName, string txtTenLoai)
        {
            try
            {
                IEnumerable<ThanhPhamViewModel> model = ThanhPham.SelectAll().Select(
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
                }).OrderByDescending(x=>x.NgayTao);
                if(Helpers.Helper.IsManager()!=true)
                {
                    model = model.Where(x => x.IsDelete != true);
                }
                ViewBag.LoaiSP = LoaiSP.SelectAll().Where(x=>x.IsDelete!=true);
                if (IdLoaiSP != null)
                {
                    model = model.Where(x => x.IdLoai == IdLoaiSP).ToList();
                }
                if (txtName != null)
                {
                    txtName = txtName == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtName);
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).ToString().Contains(txtName.ToString())).ToList();
                }
                if (txtTenLoai != null)
                {
                    txtTenLoai = txtTenLoai == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtTenLoai);
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.TenLoai).ToString().Contains(txtTenLoai.ToString())).ToList();
                }
                if (!string.IsNullOrEmpty(txtInfo))
                {
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtInfo)) || Helpers.Helper.ChuyenThanhKhongDau(x.TenLoai).Contains(txtInfo)).ToList();
                    //txtCode = txtCode == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtCode);
                    //model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(txtCode)));
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
                if(tp.IsDelete == true)
                {
                    tp.IsDelete = false;
                    ThanhPham.Update(tp);
                    ThanhPham.Save();
                    return RedirectToAction("Index");
                }
                if (tp.IsDelete == false)
                {
                    tp.IsDelete = true;
                    ThanhPham.Update(tp);
                    ThanhPham.Save();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public ActionResult Print(int? IdLoaiSP, string txtInfo, string txtName, string txtTenLoai, bool ExportExcel)
        {
            var model = TemplatePrint.SelectById(1);
            IEnumerable<ThanhPhamViewModel> modellist = ThanhPham.SelectAll().Select(
               item => new ThanhPhamViewModel
               {
                   Id = item.Id,
                   TenLoai = item.LoaiSP.Ten,
                   IdLoai = item.IdLoai,
                   Ten = item.Ten,
                   HinhAnh = item.HinhAnh,
                   TenDVT = item.DonViTinh.Ten,
                   IdDVT = item.IdDVT,
                   DonGia = item.DonGia,
                   IsDelete = item.IsDelete
               }).OrderByDescending(x => x.NgayTao);
            if (Helpers.Helper.IsManager() != true)
            {
                modellist = modellist.Where(x=>x.IsDelete != true);
            }
            if (IdLoaiSP != null)
            {
                modellist = modellist.Where(x => x.IdLoai == IdLoaiSP).ToList();
            }
            if (txtName != null)
            {
                txtName = txtName == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtName);
                modellist = modellist.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).ToString().Contains(txtName.ToString())).ToList();
            }
            if (txtTenLoai != null)
            {
                txtTenLoai = txtTenLoai == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtTenLoai);
                modellist = modellist.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.TenLoai).ToString().Contains(txtTenLoai.ToString())).ToList();
            }
            if (!string.IsNullOrEmpty(txtInfo) && txtInfo!= "undefined")
            {
                modellist = modellist.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtInfo)) || Helpers.Helper.ChuyenThanhKhongDau(x.TenLoai).Contains(txtInfo)).ToList();
                //txtCode = txtCode == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtCode);
                //model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(txtCode)));
            }
            model.Content = model.Content.Replace("{Table}", BuildHtml(modellist));
            model.Content = model.Content.Replace("{NamePrint}", "Danh Sách Sản Phẩm");
            model.Content = model.Content.Replace("{NameStaff}", Helpers.Helper.CurrentUser.TenNV);
            model.Content = model.Content.Replace("{Datetime}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "ThanhPham" + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var html = "<!DOCTYPE html><html lang='en'><head><metacharset='utf-8'><title>Print</title><style>body{font-size: 25pt;}table{border-collapse: collapse;width: 70%;}table, th, td {border: 1px solid black;}th{ text-align: center;}td{ text-align: center;}th, td {padding: 15px;}</style></head><body>";
                Response.Write(html);
                Response.Write(model.Content);
                Response.Write("</body></html>");
                Response.End();
            }
            return View(model);
        }
        string BuildHtml(IEnumerable<ThanhPhamViewModel> ls)
        {
            var i = 1;
            string list = "<thead><tr>";
            list = "<th>STT</th><th class=\"desc\">Mã</th>\r\n";
            list += "<th>Tên</th><th>Tên loại</th><th>ĐVT</th><th>Đơn giá</th></thead><tbody>\r\n";
            foreach (var item in ls)
            {
                list += "<tr><td class=\"service\">" + i + "</td>\r\n";
                list += "<td class=\"desc\">" + item.Id + "</td>\r\n";
                list += " <td class=\"qty\">" + item.Ten + "</td>\r\n";
                list += " <td class=\"qty\">" + item.TenLoai + "</td>\r\n";
                list += "<td class=\"total\">" + item.TenDVT + "</td>\r\n";
                list += "<td class=\"total\">" + item.DonGia + "</td>\r\n";
                list += "</tr>\r\n";
                i++;
            }
            list += "<tr><td colspan=\"6\"class=\"grand total\"></td>\r\n";
            return list;
        }
    }
}