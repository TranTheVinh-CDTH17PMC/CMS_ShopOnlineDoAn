using CMS_Database.Entities;
using CMS_Database.Interfaces;
using CMS_Database.Repositories;
using CMS_ShopOnline.Areas.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.Administration.Controllers
{
    [Authorize]
    public class PromotionsController : Controller
    {
        private readonly IThanhPham ThanhPham;
        private readonly IDonViTinh DVT;
        private readonly ILoaiSP LoaiSP;
        private readonly IKhuyenMai KhuyenMai;
        private readonly ICTKhuyenMai CTKhuyenMai;
        private readonly IDoiDiem DoiDiem;
        public PromotionsController()
        {
            ThanhPham = new ThanhPhamRepository();
            DVT = new DonViTinhRepository();
            LoaiSP = new LoaiSPRepository();
            KhuyenMai = new KhuyenMaiRepository();
            CTKhuyenMai = new CTKhuyenMaiRepository();
            DoiDiem = new DoiDiemRepository();
        }
        public PromotionsController(IDoiDiem _DoiDiem,ICTKhuyenMai _CTKhuyenMai, IKhuyenMai _KhuyenMai, IThanhPham _ThanhPham, IDonViTinh _DVT, ILoaiSP _LoaiSP, ITemplatePrint _TemplatePrint)
        {
            ThanhPham = _ThanhPham;
            DVT = _DVT;
            LoaiSP = _LoaiSP;
            KhuyenMai = _KhuyenMai;
            CTKhuyenMai = _CTKhuyenMai;
            DoiDiem = _DoiDiem;
        }
        // GET: Administration/Promotions
        public ActionResult Index(string ghichu, string txtName, string txtInfo)
        {
            try
            {
                IEnumerable<KhuyenMaiViewModel> model = KhuyenMai.SelectAll().Select(
                item => new KhuyenMaiViewModel
                {
                    Id = item.Id,
                    NgayBD = item.NgayBD,
                    NgayKT = item.NgayKT,
                    Ten = item.Ten,
                    GhiChu = item.GhiChu,
                    IsDelete = item.IsDelete
                }
                ).ToList().OrderByDescending(x => x.NgayBD);
                if (ghichu != null)
                {
                    ghichu = ghichu == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(ghichu);
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.GhiChu).ToString().Contains(ghichu.ToString())).ToList();
                }
                if (txtName != null)
                {
                txtName = txtName == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtName);
                model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).ToString().Contains(txtName.ToString())).ToList();
                }
                if (!string.IsNullOrEmpty(txtInfo))
                {
                    model = model.Where(x => Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(Helpers.Helper.ChuyenThanhKhongDau(txtInfo)) || Helpers.Helper.ChuyenThanhKhongDau(x.GhiChu).Contains(txtInfo)).ToList();
                    //txtCode = txtCode == "" ? "~" : Helpers.Helper.ChuyenThanhKhongDau(txtCode);
                    //model = model.Where(x => x.IsDelete != true && (Helpers.Helper.ChuyenThanhKhongDau(x.Ten).Contains(txtCode)));
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
            var km = KhuyenMai.SelectById(id);
            KhuyenMaiViewModel model = new KhuyenMaiViewModel();
            model.Id = km.Id;
            model.IsDelete = km.IsDelete;
            model.Ten = km.Ten;
            model.NgayBD = km.NgayBD;
            model.NgayKT = km.NgayKT;
            model.GhiChu = km.GhiChu;
            var details = CTKhuyenMai.GetById(km.Id).Select(
                item => new CTKhuyenMaiViewModel
                {
                    Id = item.Id,
                    IdKhuyenMai = item.IdKhuyenMai,
                    IdThanhPham = item.IdThanhPham,
                    IsPhanTram = item.IsPhanTram,
                    IsTienMat = item.IsTienMat,
                    TienGiam = item.TienGiam,
                    SLToithieu = item.SLToithieu,
                    IdLoaiSP = item.IdLoaiSP
                }).ToList();
            model.ListCTkm = details;
            return View(model);
        }
        public ActionResult Delete(string IdDelete)
        {
            var km = KhuyenMai.SelectById(Int32.Parse(IdDelete));
            km.IsDelete = true;
            KhuyenMai.Update(km);
            KhuyenMai.Save();
            TempData["SuccessMessage"] = "Create";
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            var daynow = DateTime.Now.Date;
            var abc = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
            foreach (var item in abc)
            {
                if (daynow > item.NgayKT)
                {
                    item.IsDelete = true;
                    KhuyenMai.Update(item);
                    KhuyenMai.Save();
                }
            }
            bool check = false;
            var model = KhuyenMai.SelectAll().Where(x => x.IsDelete != true && x.IsAll == true).Count();
            if (model > 0)
            {
                check = true;
            }
            ViewBag.CheckKM = check;
            ViewBag.LoaiSp = LoaiSP.SelectAll().Where(x => x.IsDelete != true);
            return View();
        }
        [HttpPost]
        public ActionResult Create(KhuyenMaiViewModel model)
        {
            try
            {
                var km = new KhuyenMai();
                km.Ten = model.Ten;
                km.GhiChu = model.GhiChu;
                km.NgayBD = model.NgayBD;
                km.NgayKT = model.NgayKT;
                km.IsDelete = false;
                if(model.IsAll == "all")
                {
                    var abc = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
                    foreach (var item in abc)
                    {
                        item.IsDelete = true;
                        KhuyenMai.Update(item);
                        KhuyenMai.Save();
                    }
                    km.IsAll = true;
                }
                else
                {
                    km.IsAll = false;
                }
                KhuyenMai.Insert(km);
                KhuyenMai.Save();
                var _doidiem = DoiDiem.SelectById(1);
                _doidiem.IsDelete = true;
                DoiDiem.Update(_doidiem);
                DoiDiem.Save();
                var id = km.Id;
                if (model.IsAll == "all")
                {
                    var ctkm = new CTKhuyenmai();
                    ctkm.IdKhuyenMai = km.Id;
                    ctkm.IdLoaiSP = model.ListCTkm[0].IdLoaiSP;
                    ctkm.IdThanhPham = model.ListCTkm[0].IdThanhPham;
                    ctkm.SLToithieu = model.ListCTkm[0].SLToithieu;
                    ctkm.TienGiam = model.ListCTkm[0].TienGiam;
                    if (model.ListCTkm[0].IsPhanTram == true)
                    {
                        ctkm.IsPhanTram = true;
                        ctkm.IsTienMat = false;
                    }
                    else
                    {
                        ctkm.IsPhanTram = false;
                        ctkm.IsTienMat = true;
                    }
                    CTKhuyenMai.Insert(ctkm);
                    CTKhuyenMai.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    for (var i = 1; i < model.ListCTkm.Count; i++)
                    {
                        var ctkm = new CTKhuyenmai();
                        ctkm.IdKhuyenMai = km.Id;
                        ctkm.IdLoaiSP = model.ListCTkm[i].IdLoaiSP;
                        ctkm.IdThanhPham = model.ListCTkm[i].IdThanhPham;
                        ctkm.SLToithieu = model.ListCTkm[i].SLToithieu;
                        ctkm.TienGiam = model.ListCTkm[i].TienGiam;
                        if (model.ListCTkm[i].IsPhanTram == true)
                        {
                            ctkm.IsPhanTram = true;
                            ctkm.IsTienMat = false;
                        }
                        else
                        {
                            ctkm.IsPhanTram = false;
                            ctkm.IsTienMat = true;
                        }
                        CTKhuyenMai.Insert(ctkm);
                        CTKhuyenMai.Save();
                    }
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {

            }
            
            return View();
        }
        public ActionResult Checktontai(int? idtp,string idloai)
        {
            var checksl = 0;
            if (idloai == "thanhpham")
            {
                var q = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
                var a = q.Count();
                foreach (var item in q)
                {
                    var x = CTKhuyenMai.SelectAll().Where(y => y.IdKhuyenMai == item.Id && y.IdThanhPham == idtp).Count();
                    if (x == 0)
                    {
                        checksl = 1;
                    }
                    else
                    {
                        checksl = 0;
                    }
                }
                if (a == 0)
                {
                    checksl = 1;
                }
            }
            if (idloai == "loaisp")
            {
                var q = KhuyenMai.SelectAll().Where(x => x.IsDelete != true);
                var a = q.Count();
                foreach (var item in q)
                {
                    var x = CTKhuyenMai.SelectAll().Where(y => y.IdKhuyenMai == item.Id && y.IdLoaiSP == idtp).Count();
                    if (x == 0)
                    {
                        checksl = 1;
                    }
                    else
                    {
                        checksl = 0;
                    }
                }
                if(a  == 0)
                {
                    checksl = 1;
                }
            }
            return Json(checksl, JsonRequestBehavior.AllowGet);
        }

    }
}