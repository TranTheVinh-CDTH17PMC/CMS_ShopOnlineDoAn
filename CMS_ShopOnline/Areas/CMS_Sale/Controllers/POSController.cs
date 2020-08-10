using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_ShopOnline.Helpers;
using CMS_ShopOnline.Areas.Crm.Models;
using CMS_ShopOnline.Areas.Administration.Controllers;
using CMS_ShopOnline.Filter;
using CMS_ShopOnline.Hubs;
using SelectPdf;

namespace CMS_ShopOnline.Areas.CMS_Sale.Controllers
{
    [Authorize]
    public class POSController : Controller
    {
        private readonly INguyenLieu NguyenLieu;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private readonly IThanhPham ThanhPham;
        private readonly IPhieuXuat PhieuXuat;
        private readonly ICTPhieuXuat CTPhieuXuat;
        private readonly IKhachHang KhachHang;
        private readonly IHoaDon HoaDon;
        private readonly ICTHoaDon CTHoaDon;
        private readonly ITemplatePrint TemplatePrint;
        private readonly IDoiDiem DoiDiem;
        private readonly IKhuyenMai KhuyenMai;
        private readonly ICTKhuyenMai CTKhuyenMai;

        public POSController()
        {
            NguyenLieu = new NguyenLieuRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            ThanhPham = new ThanhPhamRepository();
            PhieuXuat = new PhieuXuatRepository();
            CTPhieuXuat = new ICTPhieuXuatRepository();
            KhachHang = new KhachHangRepository();
            HoaDon = new HoaDonRepository();
            CTHoaDon = new CTHoaDonRepository();
            TemplatePrint = new TemplatePrintRepository();
            DoiDiem = new DoiDiemRepository();
            KhuyenMai = new KhuyenMaiRepository();
            CTKhuyenMai = new CTKhuyenMaiRepository();
        }
        public POSController(ICTKhuyenMai _CTKhuyenMai, IKhuyenMai _KhuyenMai, IDoiDiem _DoiDiem, ITemplatePrint _TemplatePrint, ICTHoaDon _CTHoaDon, IKhachHang _KhachHang, IPhieuXuat _hoadon,ICTPhieuXuat _CTPhieuXuat,INguyenLieu _NguyenLieu, IDonViTinh _DVT, ILoaiSP _LoaiSP,IThanhPham _ThanhPham, IHoaDon _HoaDon)
        {
            NguyenLieu = _NguyenLieu;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            ThanhPham = _ThanhPham;
            PhieuXuat = _hoadon;
            CTPhieuXuat = _CTPhieuXuat;
            KhachHang = _KhachHang;
            HoaDon = _HoaDon;
            CTHoaDon = _CTHoaDon;
            TemplatePrint = _TemplatePrint;
            DoiDiem = _DoiDiem;
            KhuyenMai = _KhuyenMai;
            CTKhuyenMai = _CTKhuyenMai;
        }
        public string checknullkh(string tenkh)
        {
            string name = "Rỗng";
            if (tenkh != null && tenkh != "")
            {
                name = tenkh;
            }
            return name;
        }
        public string Checkhuyenmailoai(int? id)
        {
            var check = "";
            var model = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
            foreach(var item in model)
            {
                var details = CTKhuyenMai.SelectAll().Where(x => x.IdKhuyenMai == item.Id);
                foreach(var item2 in details)
                {
                    if (item2.IdLoaiSP == id)
                    {
                        if(item2.IsPhanTram == true)
                        {
                            check = item2.TienGiam.ToString()+"%";
                        }
                        if (item2.IsTienMat == true)
                        {
                            check = item2.TienGiam.ToString() + "Đ";
                        }
                        break;
                    }
                }
            }
            return check;
        }
        public string Checkhuyenmaisp(int? id)
        {
            var check = "";
            var model = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
            foreach (var item in model)
            {
                var details = CTKhuyenMai.SelectAll().Where(x => x.IdKhuyenMai == item.Id);
                foreach (var item2 in details)
                {
                    if (item2.IdThanhPham == id)
                    {
                        if (item2.IsPhanTram == true)
                        {
                            check = item2.TienGiam.ToString() + "%";
                        }
                        if (item2.IsTienMat == true)
                        {
                            check = item2.TienGiam.ToString() + "Đ";
                        }
                        break;
                    }
                }
            }
            return check;
        }
        //
        //
        // GET: /CMS_Sale/POS/
        public ActionResult Index()
        {
            IEnumerable<TypeViewModel> ListType = LoaiSP.SelectAll().Where(x => x.IsDelete != true && x.IsProducts == true).Select(
                 item => new TypeViewModel
                 {
                     Id = item.Id,
                     Ten = item.Ten,
                     IsDelete = item.IsDelete,
                     IsProducts = item.IsProducts,
                     Khuyenmai = Checkhuyenmailoai(item.Id)
                 });
            ViewBag.ListType = ListType;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }
        [HttpPost]
        public ActionResult Index(POSViewModel model)
        {
            try
            {
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string Action = this.ControllerContext.RouteData.Values["action"].ToString();
                string Areas = "CMS_Sale";
                var _hoadon = new HoaDon();
                AutoMapper.Mapper.Map(model, _hoadon);
                _hoadon.IdNhanVien = Helper.CurrentUser.Id;
                _hoadon.TrangThai = "Create";
                _hoadon.NgayTao = DateTime.Now;
                _hoadon.IsDelete = false;
                HoaDon.Insert(_hoadon);
                HoaDon.Save();
                var idhd = _hoadon.Id;
                foreach (var item in model.ListPOSDetails)
                {
                    var _cthoadon = new CTHoaDon();
                    _cthoadon.IdThanhPham = item.IdThanhPham;
                    _cthoadon.SoLuong = item.SoLuong;
                    _cthoadon.DonGia = item.DonGia;
                    _cthoadon.IdHoaDon = idhd;
                    CTHoaDon.Insert(_cthoadon);
                    CTHoaDon.Save();
                }
                if(_hoadon.IdKhachHang!=null && _hoadon.IdKhachHang!=0)
                {
                    var _kh = KhachHang.SelectById(_hoadon.IdKhachHang);
                    _kh.TongTien = (_kh.TongTien - _hoadon.TongKM) + _hoadon.TongTien;
                    KhachHang.Update(_kh);
                    KhachHang.Save();
                }
                TaskController.CreateTask("Tạo hóa đơn", ControllerName, Action,Areas,Helper.CurrentUser.Id, idhd);
                var _px = HoaDon.SelectById(idhd);
                string NameNV = Helper.CurrentUser.TenNV;
                MyHub.PurchaseOder(idhd, _px.NgayTao, _px.IdNhanVien, _px.IdKhachHang, NameNV, model.TenKH , _px.GhiChu, _px.TrangThai, _px.TongTien,_px.TongKM);
                TempData["SuccessMessage"] = idhd;
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }
        public ActionResult Print(int? id)
        {
            var model = TemplatePrint.SelectById(2);
            var px = HoaDon.SelectById(id);
            HoaDonViewModel modellist = new HoaDonViewModel();
            AutoMapper.Mapper.Map(px, modellist);
            if(px.IdKhachHang==null)
            {
                modellist.TenKH = "Rỗng";
            }
            //else
            //{
            //    modellist.TenKH =px.KhachHang.TenKH;
            //}
            modellist.TenNV = px.NhanVien.TenNV;
            var details = CTHoaDon.GetById(px.Id).Select(
                item => new CTHoaDonViewModel
                {
                    Id = item.Id,
                    IdThanhPham = item.IdThanhPham,
                    Ten = item.ThanhPham.Ten,
                    IdHoaDon = item.IdHoaDon,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                }).ToList();
            modellist.ListPOSDetails = details;
            model.Content = model.Content.Replace("{DataTable}", BuildHtml(details));
            model.Content = model.Content.Replace("{MAHD}", id.ToString());
            model.Content = model.Content.Replace("{Tongtam}", modellist.TongTien.ToString());
            model.Content = model.Content.Replace("{Khuyenmai}",px.TongKM.ToString());
            model.Content = model.Content.Replace("{Tongtien}", modellist.TongTien.ToString());
            return View(model);
        }
        string BuildHtml(List<CTHoaDonViewModel> model)
        {
            string list = null;
            var i = 1;
            foreach(var item in model)
            {
                list += "<tr><td style=\"font-size: 12px; font-family:'Open Sans',sans-serif;color:#646a6e; line-height:18px; vertical-align:top;padding:10px0;\">"+i+"</td>\r\n";
                list += "<td style=\"font-size: 12px; font-family:'Open Sans',sans-serif;color:#646a6e; line-height:18px; vertical-align:top;padding:10px0;\" class=\"article\">" + item.Ten + "</td>\r\n";
                list += "<td style=\"font-size: 14px; font-family:'Open Sans',sans-serif;color:#646a6e; line-height:18px; vertical-align:top;padding:10px0;\"><small>" + item.DonGia + "</small></td>\r\n";
                list += "<td style=\"font-size: 12px; font-family:'Open Sans',sans-serif;color:#646a6e; line-height:18px; vertical-align:top;padding:10px0;\" align=\"center\">" + item.SoLuong + "</td>\r\n";
                list += "<td style=\"font-size: 12px; font-family:'Open Sans',sans-serif;color:#1e2b33; line-height:18px; vertical-align:top;padding:10px0;\" align=\"right\">" + item.SoLuong * item.DonGia + "</td></tr>\r\n";
                i++;
            }
            return list;
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CMS_Sale/Default/Create
        public ActionResult IndexList()
        {
            return View();
        }

        // POST: CMS_Sale/Default/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CMS_Sale/Default/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CMS_Sale/Default/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CMS_Sale/Default/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CMS_Sale/Default/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult ListProducts(int Id)
        {
            var q = ThanhPham.SelectAll().Where(x => x.IsDelete != true);
            if (Id > 0)
            {
                q = ThanhPham.SelectAll().Where(x => x.IsDelete != true && x.IdLoai == Id);
            }
            var model = q.Select(item => new ThanhPhamViewModel
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
                IsDelete = item.IsDelete,
                Khuyenmai = Checkhuyenmaisp(item.Id)
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Checksoluong(int Id,int soluong)
        {
            var q = ThanhPham.SelectById(Id);
            q.DonGia = Checkkm(q.DonGia, q.IdLoai, soluong,q.Id);
            return Json(q.DonGia, JsonRequestBehavior.AllowGet);
        }
        public double? Checkkm(double? dongia,int? loaisp,int sl,int idtp)
        {
            if (Helper.CheckKhuyenMai() == true)
            {
                var listkm = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
                foreach (var item in listkm)
                {
                    var listctkm = CTKhuyenMai.SelectAll().Where(x => x.IdKhuyenMai == item.Id);
                    foreach (var ctitem in listctkm)
                    {
                        if (item.IsAll == true && sl >= ctitem.SLToithieu)
                        {
                            if (ctitem.IsPhanTram == true)
                            {
                                dongia = dongia - (dongia / 100) * ctitem.TienGiam;
                                if (dongia < 0)
                                {
                                    dongia = 0;
                                }
                            }
                            else
                            {
                                dongia = dongia - ctitem.TienGiam;
                                if (dongia < 0)
                                {
                                    dongia = 0;
                                }
                            }
                        }
                        else
                        {
                            if (ctitem.IdLoaiSP != null)
                            {
                                if (loaisp == ctitem.IdLoaiSP && sl >= ctitem.SLToithieu)
                                {
                                    if(ctitem.IsPhanTram == true)
                                    {
                                        dongia = dongia - (dongia / 100) * ctitem.TienGiam;
                                        if (dongia < 0)
                                        {
                                            dongia = 0;
                                        }
                                    }
                                    else
                                    {
                                        dongia = dongia - ctitem.TienGiam;
                                        if (dongia < 0)
                                        {
                                            dongia = 0;
                                        }
                                    }
                                    
                                }
                            }
                            if (ctitem.IdThanhPham != null)
                            {
                                if (idtp == ctitem.IdThanhPham && sl >= ctitem.SLToithieu)
                                {
                                    if (ctitem.IsPhanTram == true)
                                    {
                                        dongia = dongia - (dongia / 100) * ctitem.TienGiam;
                                        if (dongia < 0)
                                        {
                                            dongia = 0;
                                        }
                                    }
                                    else
                                    {
                                        dongia = dongia - ctitem.TienGiam;
                                        if (dongia < 0)
                                        {
                                            dongia = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dongia;
        }
        public ActionResult ListProductsById(int Id)
        {
            var q =  ThanhPham.SelectAll().Where(x => x.IsDelete != true && x.Id == Id);
            var model = q.Select(item => new ThanhPhamViewModel
            {
                Id = item.Id,
                Ten = item.Ten,
                NgayTao = item.NgayTao,
                IdLoai = item.IdLoai,
                TenLoai = item.LoaiSP.Ten,
                HinhAnh = item.HinhAnh,
                DonGia = Checkkm(item.DonGia,item.IdLoai,1,item.Id),
                IdDVT = item.IdDVT,
                TenDVT = item.DonViTinh.Ten,
                IsDelete = item.IsDelete
            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string name)
        {
            name = Helper.ChuyenThanhKhongDau(name);
            var q = ThanhPham.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.Ten).Contains(name)).ToList();
            var model = q.Select(item => new ThanhPhamViewModel
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
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNameCustomer(int Id)
        {
            var q = KhachHang.SelectAll().Where(x => x.IsDelete != true && x.Id==Id).ToList();
            var model = q.Select(item => new CustomerViewModel
            {
                Id = item.Id,
                TenKH = item.TenKH,
                NgayTao = item.NgayTao,
                SDT = item.SDT,
                IdNVTao = item.IdNVTao,
                TenNV = item.NhanVien.TenNV,
                TongTien = item.TongTien,
                SoDiemToiDa = Tinhdiemtoida(item.TongTien),
                SoDiem = TinhDiem(),
                SoTien = Tinhtien(),
                IsDelete = item.IsDelete
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public double? Tinhdiemtoida(double? tongtien)
        {
            double diem = 0; ;
            var _dd = DoiDiem.SelectAll();
            foreach(var item in _dd)
            {
                diem = Convert.ToDouble((tongtien / item.Tienhang) * item.Diemhang);
            }
            return Math.Floor(diem);
        }
        public double? Tinhtien()
        {
            double? tien = 0; ;
            var _dd = DoiDiem.SelectAll();
            foreach (var item in _dd)
            {
                tien = item.Tiendoi;
            }
            return tien;
        }
        public double? TinhDiem()
        {
            double? tien = 0; ;
            var _dd = DoiDiem.SelectAll();
            foreach (var item in _dd)
            {
                tien = item.Diemdoi;
            }
            return tien;
        }
        public ActionResult SearchCustomer(string name)
        {
            name = Helper.ChuyenThanhKhongDau(name);
            var q = KhachHang.SelectAll().Where(x => x.IsDelete != true && Helper.ChuyenThanhKhongDau(x.TenKH).Contains(name) || x.SDT.Contains(name)).ToList();
            var model = q.Select(item => new CustomerViewModel
            {
                Id = item.Id,
                TenKH = item.TenKH,
                NgayTao = item.NgayTao,
                SDT = item.SDT,
                IdNVTao = item.IdNVTao,
                TenNV = item.NhanVien.TenNV,
                TongTien = item.TongTien,
                IsDelete = item.IsDelete
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}