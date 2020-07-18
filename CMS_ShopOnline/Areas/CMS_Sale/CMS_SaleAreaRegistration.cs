using CMS_ShopOnline.Areas.CMS_Sale.Models;
using System.Web.Mvc;

namespace CMS_ShopOnline.Areas.CMS_Sale
{
    public class CMS_SaleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CMS_Sale";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CMS_Sale_default",
                "CMS_Sale/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            CreateMap();
        }
        public void CreateMap()
        {
            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.NguyenLieu, NguyenLieuViewModel>();
            AutoMapper.Mapper.CreateMap<NguyenLieuViewModel, CMS_Database.Entities.NguyenLieu>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.ThanhPham, ThanhPhamViewModel>();
            AutoMapper.Mapper.CreateMap<ThanhPhamViewModel, CMS_Database.Entities.ThanhPham>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.HoaDon, POSViewModel>();
            AutoMapper.Mapper.CreateMap<POSViewModel, CMS_Database.Entities.HoaDon>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.HoaDon, HoaDonViewModel>();
            AutoMapper.Mapper.CreateMap<HoaDonViewModel, CMS_Database.Entities.HoaDon>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.CTHoaDon, POSDetailsViewModel>();
            AutoMapper.Mapper.CreateMap<POSDetailsViewModel, CMS_Database.Entities.CTHoaDon>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.PhieuXuat, PhieuXuatViewModel>();
            AutoMapper.Mapper.CreateMap<PhieuXuatViewModel, CMS_Database.Entities.PhieuXuat>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.PhieuNhap, PhieuNhapViewModel>();
            AutoMapper.Mapper.CreateMap<PhieuNhapViewModel, CMS_Database.Entities.PhieuNhap>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.CTPhieuNhap, CTPhieuNhapViewModel>();
            AutoMapper.Mapper.CreateMap<CTPhieuNhapViewModel, CMS_Database.Entities.CTPhieuNhap>();

            AutoMapper.Mapper.CreateMap<CMS_Database.Entities.CTHoaDon, CTHoaDonViewModel>();
            AutoMapper.Mapper.CreateMap<CTHoaDonViewModel, CMS_Database.Entities.CTHoaDon>();
        }
    }
}